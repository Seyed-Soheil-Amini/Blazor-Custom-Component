using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using CustomComponent.Shared.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.IO;
using System.Web;

namespace CustomComponent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EditorController : ControllerBase
    {
        private readonly HttpClient _HttpClient;
        public EditorController(HttpClient httpHttpClient)
        {
            _HttpClient = httpHttpClient;
        }
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadImageAsync(IFormFile upload)
        {
            string FileName = upload.FileName;
            FileName = Path.GetFileName(FileName);
            string FileFormat = Path.GetExtension(FileName);
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//file//");
            if(!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);  
            _HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var jsonValue = JsonConvert.SerializeObject(new Model()
            {
                Filename = FileName,
                FileFormat = FileFormat,
                FolderPath = FolderPath,
                DateCreated = DateTime.Now,
            });
            var httpContent = new StringContent(jsonValue, Encoding.UTF8, "application/json");
            /*            HttpResponseMessage res = await _HttpClient.GetAsync("http://localhost:5091/api/file/test");*/
            HttpResponseMessage response = await _HttpClient.PostAsync("http://localhost:5091/api/file/Upload", httpContent);
            if (response.IsSuccessStatusCode)
            {
                var filename = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()).ToString();
                var fullPath = FolderPath + filename + FileFormat;
                var stream = new FileStream(fullPath, FileMode.Create);
                await upload.CopyToAsync(stream);
                return Ok(new { url= $"f/{filename}{FileFormat}" });
            }
            return BadRequest();
            //set parameters
            //post file &parameters to Repossitory File Api > get guid
            //save file in folder(folderpath + guid)
            //return folderpath + guid
        }
    }
}
