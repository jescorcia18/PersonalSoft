using AutoMapper;
using SeguroAPI.Dtos.InsurancePolicy;
using SeguroAPI.Models;

namespace SeguroAPI.Mapper
{
    public class InsuranceProfile: Profile
    {
        public InsuranceProfile()
        {
            CreateMap<InsuranceRequest, InsurancePolicies>().ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId)).ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<InsurancePolicies,InsuranceRequest>().ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId));
        }

    }
}
