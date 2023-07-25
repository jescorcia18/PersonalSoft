using Microsoft.AspNetCore.Mvc;
using Moq;
using SeguroAPI.Controllers;
using SeguroAPI.Dtos.InsurancePolicy;
using SeguroAPI.Dtos.Login;
using SeguroAPI.Interface;
using SeguroAPI.Models;
using SeguroAPI.Services;
using System.Net;


namespace SeguroAPI.Unit.Test.Controller
{
    [TestFixture]
    public class InsurancePoliciesControllerTests
    {
        private InsurancePoliciesController testInsurancePoliciesController;
        private Mock<IInsurancePoliciesService> mockInsurancePolicies;

        [SetUp]
        public void SetUp()
        {
            mockInsurancePolicies = new Mock<IInsurancePoliciesService>();
            testInsurancePoliciesController = new InsurancePoliciesController(mockInsurancePolicies.Object);

        }
        [Test]
        public async Task GetInfoPolicies_Success()
        {
            //Arrange
            string licensePlateOrpoliciesNumber = "1";
            bool isPoliciesNumber = true;
            List<InsurancePolicies> insuranceList = new List<InsurancePolicies>()
            {
                new  InsurancePolicies{
                    Id=Guid.NewGuid(), PoliciesNumber=1, CustomerName="jack",CustomerId=123456,CustomerDateBirth= new DateTime().ToUniversalTime(),PoliciesDateTake= new DateTime().ToUniversalTime(),
                    PoliciesCoverage="Text", PoliciesMaxCoveredValue=700000, PoliciesNamePlan="NamePlanText", CustomerCityResidence="CityText", CustomerAddressResidence="AddressText",
                    VehicleLicensePlate="ABC123", VehicleModel="XTS",VehicleHasInspection=true
                }
            };
            var insuranceResponse = new InsuranceResponse
            {
                Success = true,
                Message = "Done!",
                listInsurancePolicies = insuranceList.ToList()
            };

            mockInsurancePolicies.Setup(m => m.Get(licensePlateOrpoliciesNumber, isPoliciesNumber)).ReturnsAsync(insuranceResponse);

            //Act
            var result = await testInsurancePoliciesController.GetInfoPolicies(licensePlateOrpoliciesNumber, isPoliciesNumber) as ObjectResult;

            //Asserts
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            var response = result.Value as InsuranceResponse;
            Assert.That(response?.Message, Is.EqualTo("Done!"));
            var listResponse = response?.listInsurancePolicies as List<InsurancePolicies>;
            Assert.IsNotNull(listResponse);
            Assert.IsNotEmpty(listResponse);
            Assert.That(listResponse.Count, Is.EqualTo(1));

            mockInsurancePolicies.Verify(m => m.Get(licensePlateOrpoliciesNumber, isPoliciesNumber), Times.Once());
        }

        [Test]
        public async Task GetInfoPolicies_Failed()
        {
            //Arrange
            string licensePlateOrpoliciesNumber = "";
            bool isPoliciesNumber = true;

            var insuranceResponse = new InsuranceResponse
            {
                Success = false,
                Message = "Parametros de Busqueda Vacíos.",
                listInsurancePolicies = Enumerable.Empty<InsurancePolicies>().ToList()
            };

            mockInsurancePolicies.Setup(m => m.Get(licensePlateOrpoliciesNumber, isPoliciesNumber)).ReturnsAsync(insuranceResponse);

            //Act
            var result = await testInsurancePoliciesController.GetInfoPolicies(licensePlateOrpoliciesNumber, isPoliciesNumber) as ObjectResult;

            //Asserts
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            var response = result.Value as InsuranceResponse;
            var listResponse = response?.listInsurancePolicies as List<InsurancePolicies>;
            Assert.IsNull(listResponse);

            mockInsurancePolicies.Verify(m => m.Get(licensePlateOrpoliciesNumber, isPoliciesNumber), Times.Once());
        }

        [Test]
        public async Task Create_Success()
        {
            //Arrange

            var insuranceRequest = new InsuranceRequest
            {
                PoliciesNumber = 1,
                CustomerName = "Juan Escorcia",
                CustomerId = 1043875678,
                CustomerDateBirth = new DateTime().ToUniversalTime(),
                PoliciesDateTake = new DateTime().ToUniversalTime(),
                PoliciesCoverage = "Vehiculo de Remplazo, 100% devolucion de dinero",
                PoliciesMaxCoveredValue = 65000000,
                PoliciesNamePlan = "Plan Familiar",
                CustomerCityResidence = "Barranquilla",
                CustomerAddressResidence = "Cl 34 89-34",
                VehicleLicensePlate = "ABC345",
                VehicleModel = "KIA",
                VehicleHasInspection = true
            };

            var registerResponse = new RegisterResponse
            {
                Success=true,
                Message= "Poliza de Seguro Creada Exitosamente!"
            };

            mockInsurancePolicies.Setup(m => m.Create(insuranceRequest)).ReturnsAsync(registerResponse);

            //Act
            var result = await testInsurancePoliciesController.CreateInfoPolicies(insuranceRequest) as ObjectResult;

            //Asserts
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            var response = result.Value as RegisterResponse;
            Assert.IsNotNull(response);
            Assert.IsNotEmpty(response.Message);
            Assert.That(response.Success, Is.EqualTo(true));

            mockInsurancePolicies.Verify(m => m.Create(insuranceRequest), Times.Once());
        }


    }
}