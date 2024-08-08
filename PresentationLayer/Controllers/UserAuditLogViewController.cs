using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.BusinessLayer;
using UserManagementAPI.Model;

namespace UserManagementAPI.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuditLogViewController : ControllerBase
    {
        private readonly IUserAuditLogViewService _service;

        public UserAuditLogViewController(IUserAuditLogViewService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
}
