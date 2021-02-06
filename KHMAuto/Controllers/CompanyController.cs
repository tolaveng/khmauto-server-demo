using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Api.Services;
using Data.DTO;
using KHMAuto.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KHMAuto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {

        private ICompanyService _companyService;
        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<ActionResult<CompanyDto>> Get()
        {
            var company = await _companyService.Get();
            if (company != null)
            {
                return Json(company);
            }
            return Ok();
        }


        
        // [HttpGet("createcompany")]
        public async Task<ActionResult> CreateNewCompany()
        {
            var company = new CompanyDto()
            {
                Name = "Test",
                Abn = "test abn",
                Address = "test address",
                Phone = "0404 123 123",
                Email = "test@test.com",
                Gst = 10,
                GstNumber = "08034343",
                BankName = "bank name",
                BankBsb = "bank bsb",
                BankAccountName = "acc name",
                BankAccountNumber = "acc num",
            };
            await _companyService.Create(company);
            return Ok();
        }

        [HttpPost("updatecompany")]
        public async Task<ActionResult> Update([FromBody] CompanyDto company)
        {
            try
            {
                await _companyService.Update(company);
            }
            catch(Exception ex)
            {
                return Json(ResponseResult<string>.Fail("Failed"));
            }
            return Json(ResponseResult<string>.Success("Updated successfully"));
        }
    }
}
