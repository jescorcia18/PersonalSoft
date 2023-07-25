using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Driver;
using MongoDbGenericRepository.Attributes;
using System.Runtime.Serialization;

namespace SeguroAPI.Models.Authentication
{
    [CollectionName("roles")]
    public class ApplicationRole : MongoIdentityRole<Guid>
    {
        //public string RoleName { get; set; } = string.Empty;
    }
}
