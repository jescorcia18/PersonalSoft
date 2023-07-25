using SeguroAPI.Models;

namespace SeguroAPI.Dtos.InsurancePolicy
{
    public class InsuranceResponse
    {
        public string Message { get; set; } =string.Empty;
        public bool Success { get; set; }
        public List<InsurancePolicies> listInsurancePolicies { get; set; } =new List<InsurancePolicies>();
    }
}
