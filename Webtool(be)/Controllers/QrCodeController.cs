using WebTool.Services;
using Microsoft.AspNetCore.Mvc;
using WebTool.Model;

namespace ImportExcelDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QrCodeController : ControllerBase
    {
        public readonly QrCodeServices _qrCodeServices;
        public QrCodeController(QrCodeServices qrCodeServices)
        {
            _qrCodeServices = qrCodeServices;   
        }
        [HttpPost]
        [Route("genqrcode")]
        public async Task<IActionResult> GetQrCode(QrcodeDto qrCode)
        {
            try
            {
                return Ok(await _qrCodeServices.GenQrCode(qrCode));
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
            
        }
    }
}
