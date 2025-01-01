using WebTool.Model;
using WebTool.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImportExcelDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly TokenServices _tokenServices;
        public TokenController(TokenServices tokenServices)
        {
            _tokenServices = tokenServices;
        }
        [HttpPost]
        [Route("generate")]
        public async Task<IActionResult> GennerateTokenctl(TokenModel model)
        {
            var extend = "my extended";
            var token = await _tokenServices.GenerateToken(model);
            var base64 = _tokenServices.EncodeBase64(token);
            var EncodeSha256 = _tokenServices.EncodeSha256(token);
            var EncodeSha512 = _tokenServices.EncodeSha512(token);
            var EncodeMd5 = _tokenServices.EncodeMd5(token); 
            return Ok(new { token, base64, EncodeSha256, EncodeSha512, EncodeMd5, extend });
        }
    }
}
