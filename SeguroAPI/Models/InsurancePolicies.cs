using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace SeguroAPI.Models
{
    //[CollectionName("InsurancePolicies")]
    public class InsurancePolicies
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("policiesnumber")]
        [BsonRequired]
        public int PoliciesNumber { get; set; }

        [BsonElement("customername")]
        public string CustomerName { get; set; } = string.Empty;

        [BsonElement("customerid")]
        [BsonRequired]
        public int CustomerId { get; set; }

        [BsonElement("customerdatebirth")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CustomerDateBirth { get; set; }

        [BsonElement("policiesdatetake")]
        [BsonRequired]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime PoliciesDateTake{ get; set; }

        [BsonElement("policiescoverage")]
        [BsonRequired]
        public string PoliciesCoverage { get; set; } = string.Empty;

        [BsonElement("policiesmaxcoveredvalue")]
        [BsonRequired]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal PoliciesMaxCoveredValue { get; set; }

        [BsonElement("policiesnameplan")]
        [BsonRequired]
        public string PoliciesNamePlan { get; set; } = string.Empty;

        [BsonElement("customercityresidence")]
        public string CustomerCityResidence { get; set; } = string.Empty;

        [BsonElement("customeraddressresidence")]
        public string CustomerAddressResidence { get; set; } = string.Empty;

        [BsonElement("vehiclelicenseplate")]
        [BsonRequired]
        public string VehicleLicensePlate { get; set; } = string.Empty;

        [BsonElement("vehiclemodel")]
        public string VehicleModel { get; set; } = string.Empty;

        [BsonElement("vehiclehasinspection")]
        public bool VehicleHasInspection { get; set; }

    }
}
