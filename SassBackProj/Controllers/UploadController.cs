using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SassBackProj.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UploadController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetUpload(long id)
        {
            return CreatedAtAction("Upload", id);
        }

    }
}
