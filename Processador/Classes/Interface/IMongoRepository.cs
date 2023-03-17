using MongoDbGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processador.Classes.Interface
{
    public interface IMongoRepository : IBaseMongoRepository
    {
        void savePacket(PacketSave packet);

    }
}
