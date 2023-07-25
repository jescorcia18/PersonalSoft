using SeguroAPI.Interface;

namespace SeguroAPI.Models
{
    public class MongoSettings:IMongoSettings
    {
        public string Server { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
        public string Collection { get; set; } = string.Empty;
    }
}

