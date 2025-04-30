using Microsoft.AspNetCore.Mvc;
using MvcNetCoreAWSS3.Services;

namespace MvcNetCoreAWSS3.Controllers
{
    public class AWSFilesController : Controller
    {
        private ServiceStorageS3 service;

        public AWSFilesController(ServiceStorageS3 service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<string> filesS3 = await this.service.GetVersionesFileAsync();
            return View(filesS3);
        }

        public IActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            using (Stream stream = file.OpenReadStream())
            {
                await this.service.UploadFileAsync(file.FileName, stream);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteFile(string filename)
        {
            await this.service.DeleteFileAsync(filename);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> PrivateFile(string filename)
        {
            Stream stream = await this.service.GetPrivateFileAsync(filename);
            return File(stream, "image/png");
        }
    }
}
