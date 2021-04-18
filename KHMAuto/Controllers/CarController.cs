using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.Api.Common;
using Data.DTO;
using Data.Services;
using KHMAuto.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KHMAuto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : Controller
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet("getall")]
        public async Task<ActionResult> GetAll([FromQuery] PageRequest pageRequest, [FromQuery] string carNo, [FromQuery] string sortDir)
        {
            var pageQuery = new PaginationQuery()
            {
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize,
                SortBy = "CarNo",
                SortDir = string.IsNullOrWhiteSpace(sortDir) ? "ASC": sortDir
            };

            var response = new PaginationResponse<CarDto>();
            if (string.IsNullOrWhiteSpace(carNo))
            {
                response = await _carService.GetAllPaged(pageQuery);
            }
            else
            {
                response = await _carService.FindByCarNoPaged(carNo, pageQuery);
            }
            if (response != null)
            {
                return Json(response);
            }
            return Ok();
        }

        [HttpGet("getmakes")]
        public async Task<ActionResult> GetMakes()
        {

            var response = await _carService.getMakes();
            if (response != null)
            {
                return Json(response);
            }
            return Ok();
        }

        [HttpGet("getmodels")]
        public async Task<ActionResult> GetModels()
        {

            var response = await _carService.getModels();
            if (response != null)
            {
                return Json(response);
            }
            return Ok();
        }
    }
}
