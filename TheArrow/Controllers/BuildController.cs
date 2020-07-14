using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using TheArrow.Services;

namespace TheArrow.Controllers
{
    [Route("build")]
    [ApiController]
    public class BuildController : ControllerBase
    {
        private BuildService buildService = new BuildService();


        [HttpGet]
        public FileResult Get(string StoreID)
        {
            buildService.CloneRepo(StoreID);
            buildService.AddStoreIdtoConfigFile(StoreID);
            buildService.RenameApp(StoreID);

            IFileProvider provider = new PhysicalFileProvider(@"D:\TheArrowBuilds\" + StoreID + @"\build\app\outputs\apk\release");
            IFileInfo fileInfo = provider.GetFileInfo(@"app-release.apk");
            var readStream = fileInfo.CreateReadStream();
            var mimeType = "application/vnd.android.package-archive";
            return File(readStream, mimeType, StoreID + ".apk");
        }

        [HttpGet]
        [Route("download")]
        public FileResult Download(string StoreID)
        {
            IFileProvider provider = new PhysicalFileProvider(@"D:\TheArrowBuilds\" + StoreID + @"\build\app\outputs\apk\release");
            IFileInfo fileInfo = provider.GetFileInfo(@"app-release.apk");
            var readStream = fileInfo.CreateReadStream();
            var mimeType = "application/vnd.android.package-archive";
            return File(readStream, mimeType, StoreID + ".apk");
        }
    }
}
