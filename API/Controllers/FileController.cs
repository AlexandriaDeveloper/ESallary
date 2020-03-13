using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.Data.Interface;
using API.DTOS;
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

    [HttpGet]
    public async Task<IActionResult> GetFilesList ([FromQuery] FileParams<Models.File> param) {

      var files = await _uow.FileRepository.Get ("Daily,FileType",
        x => x.OrderByDescending (f => f.Id),
        filter => filter.Name.StartsWith (param.Name) || filter.FileNum55 == param.Name || filter.FileNum224 == param.Name);
      if (files == null)
        return NotFound ();

      var filePagedList = await PagedList<Models.File>.CreateAsync (files.AsQueryable (), param.PageNumber, param.PageSize);
      var empsToReturn = _mapper.Map<IEnumerable<FilesDTO>> (filePagedList);
      Response.AddPageination (filePagedList.CurrentPage, filePagedList.PageSize, filePagedList.TotalCount, filePagedList.TotalPages);
      return Ok (empsToReturn);
    }

    private async Task<FilesDTO> LoadFileDetails (int id) {
      var result = _uow.FileRepository.Get (x => x.Id == id, "FileDetails.Employee.Department").FirstOrDefault ();
      var resultToReturn = _mapper.Map<FilesDTO> (result);
      foreach (var item in resultToReturn.FileDetails) {
        item.State = FileState.Saved.ToString ();
        if (item.EmployeeData == null) {
          var foundEmp = result.FileDetails.SingleOrDefault (x => x.Id == item.Id);
          item.EmployeeData = new EmployeeDataListToReturnDto () {
            Name = foundEmp.EmployeeName,

          };
        }
      }

      return resultToReturn;
    }

    [HttpGet ("{id}")]
    public async Task<IActionResult> Get (int id) {

      return Ok (await LoadFileDetails (id));
    }

    [HttpPost ()]
    public async Task<IActionResult> Post ([FromBody] FilesDTO file) {
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

    [HttpGet ("getFileTypeList")]
    public async Task<IActionResult> GetFileTypeList () {
      return Ok (await _uow.FileTypeRepository.Get ());
    }

    [HttpPut ()]
    public async Task<IActionResult> PutFile ([FromBody] FilesDTO file) {
      //TODO يمكن تكرارا رقم 55 
      // if (_uow.FileRepository.CheckFile55 (file.FileNum55.Trim ().ToLower ())) {
      //   ModelState.AddModelError ("", "عفوا رقم ملف 55 مسجل من قبل ");
      // }
      if (!ModelState.IsValid) {
        return BadRequest (ModelState.Keys.SelectMany (key => ModelState[key].Errors));

      }
      var fileToDB = _mapper.Map<Models.File> (file);

      _uow.FileRepository.Update (fileToDB);
      await _uow.SaveChangesAsync ();
      return Ok ();
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
          IEnumerable<Employee> emp;

          emp = await _uow.EmployeeRepository.Get (x => x.Code == code);

          string Net = Row.ItemArray.GetValue (Row.Table.Columns.IndexOf ("Net")).ToString ();
          var firstName = emp.FirstOrDefault ().KnownAs.Normalize ().ToNormalizedString ().Split (" ");
          var firstRowName = EmpName.Normalize ().ToNormalizedString ().Split (" ");
          if (!string.IsNullOrEmpty (EmpName) &&
            !string.IsNullOrWhiteSpace (EmpName)) {
            if (!string.IsNullOrEmpty (code) ||
              !string.IsNullOrWhiteSpace (code)) {
              string defaultPaymentMethod = "";
              if (Model.PaymentType == PaymentTypeConst.ATM) {
                defaultPaymentMethod = emp.FirstOrDefault ().ATMOption;
              } else if (Model.PaymentType == PaymentTypeConst.Bank) {
                defaultPaymentMethod = emp.FirstOrDefault ().BankOption;
              }
              var FileDetailDto = new FileDetailsDto ();

              dt.Add (new FileDetailsDto () {

                EmployeeData = new DTOS.EmployeeDataListToReturnDto () {

                    Name = emp.FirstOrDefault ().KnownAs,
                      Position = emp.FirstOrDefault ().Grade,
                      Id = emp.FirstOrDefault ().Id,
                      Collage = emp.FirstOrDefault ().Collage,
                      Code = emp.FirstOrDefault ().Code.ToString (),

                      HasATM = emp.FirstOrDefault ().HasATM,
                      HasBank = emp.FirstOrDefault ().HasBank,
                      HasOrder = emp.FirstOrDefault ().HasOrder,
                      HasPost = emp.FirstOrDefault ().HasPost,
                      ATMOption = emp.FirstOrDefault ().ATMOption,
                      BankOption = emp.FirstOrDefault ().BankOption

                  },

                  Net = decimal.Parse (Net),

                  SelectedPaymentMethod = defaultPaymentMethod,
                  //PaymentMethod= EmployeePaymentOptions(emp.FirstOrDefault()),
                  State = FileState.New.ToString (),

                  Warrning = (firstName[0] == firstRowName[0] && firstName[1] == firstRowName[1]) ? null : string.Format (" تأكد من الأسم من فضلك - {0}", EmpName)

              });

            } else {

              var s = await SuggestEmployee (EmpName.ToNormalizedString (), Model.PaymentType, false);
              dt.Add (new FileDetailsDto () {

                EmployeeData = new EmployeeDataListToReturnDto () {
                    Name = EmpName
                  },
                  Net = decimal.Parse (Net),
                  State = FileState.New.ToString (),
                  SuggestedEmployee = s
              });
            }
          }

        }

      }

      return Ok (new { data = dt });

    }

    [HttpPost ("SaveSheetData")]
    public async Task<IActionResult> SaveSheetData (List<FileDetailsDto> data) {
      // List<FileDetailsDto> dataToReturn = new List<FileDetailsDto> ();
      List<FileDetail> dataToDb = new List<FileDetail> ();
      foreach (var item in data) {
        if (item.SuggestedEmployee != null) {
          return BadRequest ("من فضلك قم بأختيار من المقتراحات للموظف " + item.EmployeeData.Name);
        } else {
          if (item.State != FileState.Saved.ToString ()) {

            var saveData = _mapper.Map<FileDetail> (item);

            dataToDb.Add (saveData);
            _uow.FileDetailRepository.Add (saveData);
            await _uow.SaveChangesAsync ();
            item.State = FileState.Saved.ToString ();

          }

        }
      }
      // _uow.FileDetailRepository.AddRange(dataToDb);
      // await _uow.SaveChangesAsync();
      var dataToReturn = await LoadFileDetails (data.FirstOrDefault ().FileId);
      return Ok (dataToReturn.FileDetails);
    }

    [HttpGet ("SuggestAllEmployee")]
    public async Task<List<SuggestionsFileDetailsDto>> SuggestAllEmployee (string name, string paymentType) {

      return await SuggestEmployee (name, paymentType, true);
    }

    private async Task<List<SuggestionsFileDetailsDto>> SuggestEmployee (string name, string paymentType, bool all) {

      string[] SplitedName = name.Split (" ");

      List<SuggestionsFileDetailsDto> Suggestions = new List<SuggestionsFileDetailsDto> ();
      if (string.IsNullOrEmpty (name)) { return Suggestions; }
      Suggestions.Add (new SuggestionsFileDetailsDto () {
        Name = name,
          SelectedPaymentMethod = PaymentTypeConst.InternalPost,

      });
      Suggestions[0].PaymentMethod.Add (PaymentTypeConst.InternalPost);
      IEnumerable<Employee> emps;
      emps = await _uow.EmployeeRepository.SuggestEmployeeByName (name, paymentType, all);

      foreach (var emp in emps) {
        var suggestedEmp = _mapper.Map<SuggestionsFileDetailsDto> (emp);

        if (paymentType == PaymentTypeConst.ATM) {
          suggestedEmp.SelectedPaymentMethod = emp.ATMOption;
        }
        if (paymentType == PaymentTypeConst.Bank) {
          suggestedEmp.SelectedPaymentMethod = emp.BankOption;
        }

        Suggestions.Add (suggestedEmp);
      }
      return Suggestions;
    }

    [HttpPost ("deleteData")]
    public async Task<IActionResult> DeleteData (IEnumerable<FileDetailsDto> model) {

      foreach (var item in model) {
        var data = await _uow.FileDetailRepository.Get (item.Id);
        if (data != null) {
          _uow.FileDetailRepository.Delete (data);
        }

      }
      await _uow.SaveChangesAsync ();
      return Ok (model);

    }

    [HttpGet ("downloadFile/{id}/{fileType}")]
    public async Task<ActionResult> DownloadFile (int id, string fileType) {
      var filePaymentType = "";
      if (fileType == "atm")
        filePaymentType = PaymentTypeConst.ATM;
      if (fileType == "bank")
        filePaymentType = PaymentTypeConst.Bank;
      if (fileType == "order")
        filePaymentType = PaymentTypeConst.PaymentOrder;
      if (fileType == "personalpost")
        filePaymentType = PaymentTypeConst.PersonalPost;
      if (fileType == "post")
        filePaymentType = PaymentTypeConst.InternalPost;
      var result = await DownloadFileMethod (id, filePaymentType);
      // var file = File (result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PrintTemplate");
      var s = new FileContentResult (await System.IO.File.ReadAllBytesAsync (result),

        new MediaTypeHeaderValue ("application/xls").MediaType) { FileDownloadName = "Report.xls" };
      return s;
    }

    [HttpGet ("printFile/{id}/{fileType}")]
    public async Task<ActionResult> PrintFile (int id, string fileType) {

      var result = await DownloadFileMethod (id, fileType);

      // var file = File (result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PrintTemplate");

      return Ok (new { Path = result });
    }

    [HttpDelete ("deleteFile/{id}")]
    public async Task<ActionResult> DeleteFile (int id) {
      var deleteFile = await _uow.FileRepository.Get (id);
      if (deleteFile == null) {
        return NotFound ();
      }
      _uow.FileRepository.Delete (deleteFile);
      await _uow.SaveChangesAsync ();

      return Ok ();

    }

    private async Task<string> DownloadFileMethod (int id, string fileType) {
      Models.File file = null;
      if (fileType == PaymentTypeConst.ATM || fileType == PaymentTypeConst.Bank) {
        file = _uow.FileRepository.Get (x => x.Id == id, "FileDetails.Employee").FirstOrDefault ();
      }

      if (fileType == PaymentTypeConst.PaymentOrder) {
        file = _uow.FileRepository.Get (x => x.Id == id, "FileDetails.Employee.EmployeeOrder.BankBranch.Bank").FirstOrDefault ();
      }
      if (fileType == PaymentTypeConst.PersonalPost) {
        file = _uow.FileRepository.Get (x => x.Id == id, "FileDetails.Employee.EmployeePost").FirstOrDefault ();
      }
      if (fileType == PaymentTypeConst.InternalPost) {
        file = _uow.FileRepository.Get (x => x.Id == id, "FileDetails").FirstOrDefault ();
      }
      file.FileDetails = file.FileDetails.Where (x => x.PaymentMethod == fileType).ToList ();
      var empsAfterSum = new List<FileDetail> ();
      if (fileType != PaymentTypeConst.InternalPost) {
        empsAfterSum = file.FileDetails.GroupBy (x => x.EmployeeId.Value).Where (g => g.Count () > 0).Select (g => {

        return new FileDetail () {

        Net = file.FileDetails.Where (x => x.EmployeeId.Value == g.Key).Sum (t => t.Net),
        EmployeeName = file.FileDetails.Where (x => x.EmployeeId.Value == g.Key).FirstOrDefault ().EmployeeName,
        Code = file.FileDetails.Where (x => x.EmployeeId.Value == g.Key).FirstOrDefault ().Code,
        FileId = file.FileDetails.Where (x => x.EmployeeId.Value == g.Key).FirstOrDefault ().FileId,
        PaymentMethod = file.FileDetails.Where (x => x.EmployeeId.Value == g.Key).FirstOrDefault ().PaymentMethod,
        Id = file.FileDetails.Where (x => x.EmployeeId.Value == g.Key).FirstOrDefault ().Id,
        EmployeeId = file.FileDetails.Where (x => x.EmployeeId.Value == g.Key).FirstOrDefault ().EmployeeId,
        Employee = file.FileDetails.Where (x => x.EmployeeId.Value == g.Key).FirstOrDefault ().Employee
          };
        }).ToList ();
      } else {
        empsAfterSum = file.FileDetails.ToList ();
      }
      var result = await this._uow.FileDetailRepository.DownloadFile (empsAfterSum, fileType, file.FileNum55, file.Name, file.EntryName);
      return result;
    }
  }
  public class PersonalPostData {
    public string Name { get; set; }
    public decimal Net { get; set; }
  }
}