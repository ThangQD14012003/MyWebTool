using WebTool.Model;
using WebTool.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq.Expressions;

namespace ImportExcelDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly ExcelServices _import; 
        public ImportController(ExcelServices import)
        {
            _import = import; 
        }
        [HttpPost]
        [EnableCors("MyCors")]
        public async Task<IActionResult> ImportFromExcel([FromForm] ImportModel import, IFormFile file, [FromForm] string columnMappings)
        {
            var columnMappingsDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(columnMappings);
            if (file == null || file.Length == 0)
            {
                return BadRequest(); 
            }
            if(string.IsNullOrEmpty(columnMappings))
            {
                return BadRequest("No column mappings provided"); 
            }
            try
            {
                var myResult = await _import.ImportExcelToDatabaseError(import, file, columnMappingsDict);
                
                return Ok(myResult);    
                //await _import.ImportExcelToDatabase(import, file, columnMappingsDict);
            } catch (Exception ex) {
                return BadRequest($"Failed to import {ex.Message}");    
            }
        }

        [HttpPost("NoError")]
        [EnableCors("MyCors")]
        public async Task<IActionResult> ImportExcelNoError([FromForm] ImportModel model, IFormFile file, [FromForm] string columnMappings)
        {
            var columnMappingsDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(columnMappings);
            if (file == null|| file.Length == 0)
            {
                return BadRequest(); 
            }
            if (string.IsNullOrEmpty(columnMappings))
            {
                return BadRequest(); 
            }
            var myResult = await _import.ImportExcelToDatabaseNoError(model, file, columnMappingsDict);
            return Ok(myResult); 
        }
        [HttpPost("get-columnsinfo")]
        [EnableCors("MyCors")]
        public async Task<IActionResult> GetColumns(ImportModel model)
        {
            try
            {
                var columns = await _import.GetColumnsInfoFromTable(model); 
                return Ok(columns);
            }catch
            {
                return BadRequest(); 
            }
            
        }
        [HttpPost("get-table")]
        [EnableCors("MyCors")]
        public async Task<IActionResult> GetTable(ImportModel model)
        {
            try
            {
                return Ok(await _import.GetTableAsync(model));  
            }catch
            {
                return BadRequest(); 
            }
            
        }
    }
}
