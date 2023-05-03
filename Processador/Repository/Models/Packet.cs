using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using MongoDbGenericRepository.Models;

namespace Processador.Repository.Models
{
    [CollectionName("packets")]
    [BsonIgnoreExtraElements]
    public class PacketFromMongoDb : Document
    {

        public int AssetId { get; set; }

        public string UnitId { get; set; }

        public int OrgId { get; set; }

        public string Message { get; set; }

        public string OriginalMessage { get; set; }

        public string Database { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }

        public string ServerName { get; set; }

        public string TripId { get; set; }

        public bool Pending { get; set; }

        public string port { get; set; }

        public int TypePacket { get; set; }

        public override string ToString()
        {
            return Message;
        }

    }
}
