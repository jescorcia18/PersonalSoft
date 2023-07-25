using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeguroAPI.Dtos.InsurancePolicy;
using SeguroAPI.Interface;
using SeguroAPI.Models;
using SeguroAPI.Services;

namespace SeguroAPI.Controllers
{
    [ApiController]
    [Route("api/v1/InsurancePolicies")]
    
    public class InsurancePoliciesController : ControllerBase
    {
        private readonly IInsurancePoliciesService _insuranceService;

        public InsurancePoliciesController(IInsurancePoliciesService insuranceService)
        { 
            _insuranceService = insuranceService;
        }

        [HttpGet]
        [Route("Get/{licensePlateOrpoliciesNumber}/{isPoliciesNumber}")]
        [Authorize]
        public async Task<IActionResult> GetInfoPolicies(string licensePlateOrpoliciesNumber, bool isPoliciesNumber = false)
        {
            var result= await _insuranceService.Get(licensePlateOrpoliciesNumber, isPoliciesNumber);
            return result.Success ? Ok(result) : BadRequest(result.Message);

        }

        [HttpPost]
        [Route("Create")]
        [Authorize]
        public async Task<IActionResult> CreateInfoPolicies([FromBody] InsuranceRequest request)
        {
            var result = await _insuranceService.Create(request);
            return result.Success ? Ok(result) : BadRequest(result.Message);

        }


    }
}
