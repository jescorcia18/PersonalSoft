using AutoMapper;
using MongoDB.Driver;
using SeguroAPI.Dtos.InsurancePolicy;
using SeguroAPI.Dtos.Login;
using SeguroAPI.Interface;
using SeguroAPI.Models;

namespace SeguroAPI.Services
{
    public class InsurancePoliciesService : IInsurancePoliciesService
    {
        private IMongoCollection<InsurancePolicies> _insurancePolicies;
        private IMapper _mapper;
        const int PoliciesDays = 365;
        public InsurancePoliciesService(IMongoSettings settings, IMapper mapper)
        {
            var client = new MongoClient(settings.Server);
            var database = client.GetDatabase(settings.Database);
            _insurancePolicies = database.GetCollection<InsurancePolicies>(settings.Collection);
            _mapper = mapper;
        }

        public async Task<InsuranceResponse> Get(string licensePlateOrpoliciesNumber, bool isPoliciesNumber = false)
        {
            try
            {
                var param = isPoliciesNumber ? "Número de Poliza" : "Placa Vehiculo";
;                var result = new List<InsurancePolicies>();
                if (string.IsNullOrEmpty(licensePlateOrpoliciesNumber))
                    return new InsuranceResponse { Message = $"Parametros de Busqueda Vacíos.", Success = false, listInsurancePolicies = Enumerable.Empty<InsurancePolicies>().ToList() };

                if (!isPoliciesNumber)
                    result = await Task.Run(() => _insurancePolicies.Find(d => d.VehicleLicensePlate == licensePlateOrpoliciesNumber).ToListAsync());

                if (isPoliciesNumber)
                    result = await Task.Run(() => _insurancePolicies.Find(d => d.PoliciesNumber == int.Parse(licensePlateOrpoliciesNumber)).ToListAsync());

                if (result.Count<=0)
                    return new InsuranceResponse
                    {
                        Success = false,
                        Message = $"{param} no Encontrada.",
                        listInsurancePolicies = result.ToList()
                    };

                return new InsuranceResponse
                {
                    Success = true,
                    Message = "Done!",
                    listInsurancePolicies = result.ToList()
                };
            }
            catch (Exception ex)
            {
                return new InsuranceResponse
                {
                    Success = false,
                    Message = ex.Message,
                    listInsurancePolicies = Enumerable.Empty<InsurancePolicies>().ToList()
                };
            }
        }

        public async Task<RegisterResponse> Create(InsuranceRequest request)
        {
            try
            {
                var dto = _mapper.Map<InsurancePolicies>(request);

               
                if (request.PoliciesDateTake< DateTime.UtcNow)
                {
                    var validDatePolice = request.PoliciesDateTake.AddDays(PoliciesDays);
                    if (validDatePolice < DateTime.UtcNow)
                        return new RegisterResponse { Success = false, Message = "Poliza de seguro a crear no esta Vigente!" };
                }

                await Task.Run(() => _insurancePolicies.InsertOne(dto));
                return new RegisterResponse { Success = true, Message = "Poliza de Seguro Creada Exitosamente!" };
            }
            catch (Exception ex)
            {
                return new RegisterResponse { Success = false, Message = ex.Message };
            }
        }
    }
}
