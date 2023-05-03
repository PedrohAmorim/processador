using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processador.Classes
{
    public class PacketHeader
    {
        public string FirstByte { get; set; }

        public string MessageId { get; set; }

        public string UnitId { get; set; }

        public int MessageType { get; set; }

        public int MessageSize { get; set; }

        public string MessageCRC { get; set; }

        public PacketHeader(string[] message)
        {
            /*
                FIELD INDEX SIZE CONVERSION
                MSG_START_BYTE 0 1 ASCII
                MSG_ID 1 4 ASCII
                MODULE_ID 5 2 -
                MSG_TYPE 7 1 -
                MSG_TYPE 8 1 -
                CORE MESSAGE
                MSG_CRC MSG_SIZE + 9 1 ASCII
            */

            FirstByte = message[0];
            MessageId = Misc.arrayToString(message, 1, 4, false);
            UnitId = Misc.arrayToString(message, 5, 2, false);
            GetMessageType(Misc.arrayToString(message, 7, 1, false));
            GetSize(Misc.arrayToString(message, 8, 1, false));
        }

        private void GetMessageType(string type)
        {
            int messageType = 0;

            int.TryParse(type, out messageType);

            MessageType = messageType;
        }

        private void GetSize(string message)
        {
            try
            {
                int value = Misc.decriptHexaDecimal(message);
                MessageSize = value;
            }
            catch (Exception ex)
            {
                MessageSize = 0;
                return;
            }
        }


    }
}
