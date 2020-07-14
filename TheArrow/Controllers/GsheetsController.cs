using Microsoft.AspNetCore.Mvc;
using TheArrow.Services;

namespace TheArrow.Controllers
{
    [ApiController]
    [Route("Gsheets")]
    public class GsheetsController : ControllerBase
    {
        private GsheetsService gsheetsService = new GsheetsService();

        [HttpGet]
        public string Get(string StoreId)
        {
            return gsheetsService.CreateSheet(StoreId);
        }

    }
}
