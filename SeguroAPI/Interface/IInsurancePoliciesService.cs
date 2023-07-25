using SeguroAPI.Dtos.InsurancePolicy;
using SeguroAPI.Dtos.Login;

namespace SeguroAPI.Interface
{
    public interface IInsurancePoliciesService
    {
        public Task<InsuranceResponse> Get(string licensePlateOrpoliciesNumber, bool isPoliciesNumber = false);
        public Task<RegisterResponse> Create(InsuranceRequest request);

    }
}
