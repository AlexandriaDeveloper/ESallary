using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Interface;
using API.Helper;
using API.Models;
using API.Service;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
  [Authorize ()]
  [Route ("api/[controller]")]
  [ApiController]
  public class FileController : ControllerBase {
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private object debug;

    public FileController (IUnitOfWork uow, IMapper mapper) {
      this._uow = uow;
      this._mapper = mapper;
    }

    public async Task<IActionResult> Get () {
      return Content ("hello world");
    }

    [HttpGet ("{id}")]
    public async Task<IActionResult> Get (int id) {
      var result = _uow.FileRepository.Get (x => x.Id == id, "FileDetails").FirstOrDefault ();
      var resultToReturn = _mapper.Map<FileDTO> (result);
      return Ok (resultToReturn);
    }

    [HttpPost ()]
    public async Task<IActionResult> Post ([FromBody] FileDTO file) {
      if (_uow.FileRepository.CheckFile55 (file.FileNum55.Trim ().ToLower ())) {
        ModelState.AddModelError ("", "عفوا رقم ملف 55 مسجل من قبل ");
      }

      if (!ModelState.IsValid) {
        return BadRequest (ModelState.Keys.SelectMany (key => ModelState[key].Errors));

      }
      file.Name = file.Name.Trim ().ToLower ();
      file.FileNum55 = file.FileNum55.Trim ().ToLower ();

      file.EntryName = User.Identity.Name;
      file.CreatedDate = DateTime.Now;
      try {
        var fileToReturn = _mapper.Map<Models.File> (file);
        _uow.FileRepository.Add (fileToReturn);
        await _uow.SaveChangesAsync ();
        return Ok (fileToReturn);
      } catch (Exception ex) {
        return BadRequest (ex.Message);
      }

    }

    [HttpPost ("getSheetsName")]
    public async Task<IActionResult> GetSheetsName () {
      var filePath = Path.GetTempFileName ();
      if (Request.Form.Files.Count == 0)
        return BadRequest ();
      var file = Request.Form.Files[0];
      using (var stream = new FileStream (filePath, FileMode.Create)) {
        await file.CopyToAsync (stream);
      }

      //read Table 
      var npoiHelper = new NPOIHelper (filePath);
      var sheetsName = npoiHelper.GetFileSheets ();
      return Ok (new { sheets = sheetsName, path = filePath, fileData = file });

    }

    [HttpPost ("readSheets")]
    public async Task<IActionResult> ReadSheets (ImportedSheetDto Model) {

      var npoiHelper = new NPOIHelper (Model.Path);
      List<FileDetailsDto> dt = new List<FileDetailsDto> ();

      foreach (var sheet in Model.Sheets) {

        var sheetData = npoiHelper.ReadSheet (sheet);
        //start Read Header 
        var headerIndex = npoiHelper.GetFirstRowIndex (sheet);

        sheetData = sheetData.ReplaceCoulmnName (SheetHeader.Names, "Name");
        int NameIndex = sheetData.Columns.IndexOf ("Name");
        sheetData = sheetData.ReplaceCoulmnName (SheetHeader.Codes, "Code");
        int CodeIndex = sheetData.Columns.IndexOf ("Code");
        sheetData = sheetData.ReplaceCoulmnName (SheetHeader.Net, "Net");
        int NetIndex = sheetData.Columns.IndexOf ("Net");
        var data = sheetData.DefaultView.ToTable (true, "Net", "Code", "Name");
        foreach (DataRow Row in data.Rows) {
          string EmpName = Row.ItemArray.GetValue (Row.Table.Columns.IndexOf ("Name")).ToString ();
          string code = Row.ItemArray.GetValue (Row.Table.Columns.IndexOf ("Code")).ToString ();
          var emp = await _uow.EmployeeRepository.Get (x => x.Code == code);
          string Net = Row.ItemArray.GetValue (Row.Table.Columns.IndexOf ("Net")).ToString ();

          if (!string.IsNullOrEmpty (EmpName) &&
            !string.IsNullOrWhiteSpace (EmpName)) {
            if (!string.IsNullOrEmpty (code) ||
              !string.IsNullOrWhiteSpace (code)) {

              dt.Add (new FileDetailsDto () {
                EmployeeName = emp.FirstOrDefault ().KnownAs,
                  Id = emp.FirstOrDefault ().Id,
                  Code = code.ToString (),
                  Net = decimal.Parse (Net)

              });

            } else {

              var s = await SuggestEmployee (EmpName.ToNormalizedString (), Model.PaymentType);
              dt.Add (new FileDetailsDto () {

                EmployeeName = EmpName,
                  Net = decimal.Parse (Net),
                  SuggestedEmployee = s
              });
            }
          }

        }

      }

      return Ok (new { data = dt });

    }

    private async Task<List<SuggestionsFileDetailsDto>> SuggestEmployee (string name, string paymentType) {

      string[] SplitedName = name.Split (" ");

      List<SuggestionsFileDetailsDto> Suggestions = new List<SuggestionsFileDetailsDto> ();
      if (string.IsNullOrEmpty (name)) { return Suggestions; }
      Suggestions.Add (new SuggestionsFileDetailsDto () {
        Name = name,
          DefaultPaymentMethod = PaymentTypeConst.InternalPost,

      });
      Suggestions[0].PaymentMethod.Add (PaymentTypeConst.InternalPost);
      var emps = await _uow.EmployeeRepository
        .Get (x => SplitedName.All (s => x.KnownAs.Contains (s)) && x.KnownAs.StartsWith (SplitedName[0]) && x.Deleted == false);
      foreach (var emp in emps) {
        var suggestedEmp = new SuggestionsFileDetailsDto () {

          Id = emp.Id,
          Code = emp.Code,
          Name = emp.KnownAs,
          Position = emp.Section,
          Collage = emp.Collage,
          DefaultPaymentMethod = emp.OtherOption
        };
    
        if (emp.HasATM) {
          suggestedEmp.PaymentMethod.Add (PaymentTypeConst.ATM);
        }
        if (emp.HasBank) {
          suggestedEmp.PaymentMethod.Add (PaymentTypeConst.Bank);
        }
        if (emp.HasPost) {
          suggestedEmp.PaymentMethod.Add (PaymentTypeConst.PersonalPost);
        }
        if (emp.HasOrder) {
          suggestedEmp.PaymentMethod.Add (PaymentTypeConst.PaymentOrder);
        }
        suggestedEmp.PaymentMethod.Add (PaymentTypeConst.InternalPost);
        Suggestions.Add (suggestedEmp);
      }
      return Suggestions;
    }

  }
}