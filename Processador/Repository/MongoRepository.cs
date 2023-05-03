using Dapper;
using MongoDB.Driver;
using MongoDbGenericRepository;
using Processador.Classes.Interface;
using Processador.Repository.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;


namespace Processador.Classes
{
    public class MongoRepository : BaseMongoRepository, IMongoRepository
    {

        static string connectionString = ConfigurationManager.AppSettings["MONGODB03"];

        private static readonly IMongoRepository _instance = new MongoRepository(connectionString, "mobs2");


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


        public void savePacket(Event Event)
        {

            // Salvar primeiro no Sql
            SqlDataBase.InsertEventFromOrganization(Event);

            // Gerar pacote para o MongoDB
            var savePacket = new PacketFromMongoDb()
            {
                UnitId = Event._header.UnitId,
                AssetId = Event._module.AssetId,
                Database = Event._module.Database,
                OrgId = Event._module.OrgId,
                Message = String.Join(" ", Event.originalMessage),
                port = "PnP",
                TypePacket = 1,
                Pending = false
            };

            var mongoCollection = this.MongoDbContext.Database.GetCollection<PacketFromMongoDb>("pnp_packets");


            mongoCollection.InsertOneAsync(savePacket);

        }

        public void savePacket(Track track)
        {
            

            // Salvar primeiro no Sql
            SqlDataBase.InsertTrackFromOrganization(track);

            var savePacket = new PacketFromMongoDb()
            {
                UnitId = track._header.UnitId,
                AssetId = track._module.AssetId,
                Database = track._module.Database,
                OrgId = track._module.OrgId,
                Message = String.Join(" ", track.originalMessage),
                port = "PnP",
                TypePacket = 0,
                Pending = false
            };

            var mongoCollection = this.MongoDbContext.Database.GetCollection<PacketFromMongoDb>("pnp_packets");

            mongoCollection.InsertOneAsync(savePacket);

        }

    }
}


