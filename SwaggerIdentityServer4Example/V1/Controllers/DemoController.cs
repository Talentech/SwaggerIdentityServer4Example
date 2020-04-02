using Microsoft.AspNetCore.Mvc;
using SwaggerIdentityServer4Example.V1.Models;

namespace SwaggerIdentityServer4Example.V1.Controllers
{ 
    [Route("v1/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    { 
        [HttpGet]
        public IActionResult GetDemoValues() 
            => Ok(new string[] { "value1", "value2" });
         
        [HttpPost]
        [Route("post")]
        public IActionResult PostDemoModel(DemoModel model) 
            => Ok(model);
    }
}