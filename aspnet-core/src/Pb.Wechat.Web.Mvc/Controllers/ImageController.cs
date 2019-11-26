using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Abp.Web.Models;

namespace Pb.Wechat.Web.Controllers
{
    public class ImageController : AbpZeroTemplateControllerBase
    {
        private readonly IHostingEnvironment _env;
        public ImageController(IHostingEnvironment env)
        {
            _env = env;
        }
        [IgnoreAntiforgeryToken]
        [DontWrapResult]
        public ActionResult GetPic(string filePath)
        {
            var tempProfilePicturePath = Path.Combine(_env.WebRootPath, filePath);
            var stream= new MemoryStream(System.IO.File.ReadAllBytes(tempProfilePicturePath));
            return File(stream, "image/jpg");
        }


    }
}