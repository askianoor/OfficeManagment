using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManagment.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class InfoController : ControllerBase
    {
        private readonly OfficeInfo _officeInfo;

        public InfoController(IOptions<OfficeInfo> officeInfoAccessor)
        {
            _officeInfo = officeInfoAccessor.Value;
        }

        [HttpGet(Name = nameof(GetInfo))]
        public IActionResult GetInfo()
        {
            _officeInfo.Href = Url.Link(nameof(GetInfo), null);

            return Ok(_officeInfo);
        }
    }
}
