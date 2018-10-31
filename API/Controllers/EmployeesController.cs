using System.IO;
using System.Threading;
using System.Threading.Tasks;
using API.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers {
    [Authorize ()]
    [Route ("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase {
        private object thread;

        [HttpGet ("get")]
        public IActionResult GetEmployees () {
            var npoiHelper = new NPOIHelper ("E:/test.xlsx");

            var result = npoiHelper.GetFileSheets ();
            return Ok (new { data = result });
        }

        [HttpPost ("UploadEmployees")]
        public async Task<IActionResult> UploadEmployeesAsync (IFormFile file) {

            // full path to file in temp location
            var filePath = Path.GetTempFileName ();

            if (file.Length > 0) {
                using (var stream = new FileStream (filePath, FileMode.Create)) {
                    await file.CopyToAsync (stream);
                }
            }
            var npoiHelper = new NPOIHelper (filePath);
             npoiHelper.GetFileSheets ();
            var result = npoiHelper.ReadSheet("Sheet1");
            return Ok (result);
        }
    }

}