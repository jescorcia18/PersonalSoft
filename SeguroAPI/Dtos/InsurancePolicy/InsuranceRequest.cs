namespace SeguroAPI.Dtos.InsurancePolicy
{
    public class InsuranceRequest
    {
        //public string vehicleLicensePlate { get; set; } = string.Empty;
        //public int policiesnumber { get; set; } = 0;
        //public string Id { get; set; }
        public int PoliciesNumber { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public DateTime CustomerDateBirth { get; set; }
        public DateTime PoliciesDateTake { get; set; }
        public string PoliciesCoverage { get; set; } = string.Empty;
        public decimal PoliciesMaxCoveredValue { get; set; }
        public string PoliciesNamePlan { get; set; } = string.Empty;
        public string CustomerCityResidence { get; set; } = string.Empty;
        public string CustomerAddressResidence { get; set; } = string.Empty;
        public string VehicleLicensePlate { get; set; } = string.Empty;
        public string VehicleModel { get; set; } = string.Empty;
        public bool VehicleHasInspection { get; set; }
    }
}
