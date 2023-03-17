using Dapper;
using MongoDB.Driver;
using MongoDbGenericRepository;
using Processador.Classes.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;


namespace Processador.Classes
{
    public class MongoRepository : BaseMongoRepository, IMongoRepository
    {

        static string connectionString = ConfigurationManager.ConnectionStrings["mobs2mongosinotico"].ConnectionString;

        private static readonly IMongoRepository _instance = new MongoRepository(connectionString, "sgf");


        static MongoRepository() { }


        public MongoRepository(string connectionString, string databaseName) : base(connectionString, databaseName)
        {

        }


        public static IMongoRepository Instance
        {
            get
            {
                return _instance;
            }
        }


        public void savePacket(PacketSave packet)
        {
            var mongoCollection = this.MongoDbContext.Database.GetCollection<PacketSave>("packets");

            mongoCollection.InsertOneAsync(packet);

        }

    }
}


