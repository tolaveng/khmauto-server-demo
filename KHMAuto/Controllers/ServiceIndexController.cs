using Data.Api.Common;
using Data.Api.Services;
using Data.DTO;
using KHMAuto.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHMAuto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceIndexController : Controller
    {
        private readonly IServiceIndexService _serviceIndexService;

        public ServiceIndexController(IServiceIndexService serviceIndexService)
        {
            _serviceIndexService = serviceIndexService;
        }


        [HttpGet("findByServiceName")]
        public async Task<ActionResult> FindByServiceName([FromQuery] PageRequest pageRequest, [FromQuery] string serviceName)
        {
            var pageQuery = new PaginationQuery()
            {
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize,
            };

            if (string.IsNullOrWhiteSpace(serviceName)) return Ok();

            var response = await _serviceIndexService.FindByServiceNamePaged(serviceName, pageQuery);
            if (response == null) return Ok();

            return Json(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteServiceIndex(int id)
        {
            await _serviceIndexService.DeleteById(id);
            return Ok();
        }

        [HttpPost("updateServiceIndex")]
        public async Task<ActionResult> UpdateServiceIndex([FromBody] ServiceIndexDto ServiceIndex)
        {
            if (ServiceIndex.ServiceIndexId == 0 || string.IsNullOrWhiteSpace(ServiceIndex.ServiceName)) return Ok();

            await _serviceIndexService.Update(ServiceIndex.ServiceIndexId, ServiceIndex.ServiceName);
            return Ok();
        }
    }
}
