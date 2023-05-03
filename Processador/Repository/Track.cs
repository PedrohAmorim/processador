using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processador.Classes
{

    public class Track
    {

        public Module _module { get; set; }

        public PacketHeader _header { get; set; }

        public string[] originalMessage { get; set; }

        public Int16 orientation { get; set; }

        public DateTime timestamp { get; set; }

        public double latitude { get; set; }

        public double longitude { get; set; }

        public int speed { get; set; }

        public Track(string[] message, PacketHeader header, Module module)
        {
            _header = header;
            originalMessage = message;
            _module = module;
            decript();
        }


        private void decript()
        {
            getTimestamp(Misc.arrayToString(originalMessage, 9, 4, true));
            latitude = getGeoLocation(Misc.arrayToString(originalMessage, 13, 4, true), Misc.arrayToString(originalMessage, 17, 1, false), "Lat");
            longitude = getGeoLocation(Misc.arrayToString(originalMessage, 18, 4, true), Misc.arrayToString(originalMessage, 22, 1, false), "Lng");
            getSpeed(Misc.arrayToString(originalMessage, 25, 2, true));
            getOrientation();
        }


        private void getSpeed(string OriginalMessage)
        {
            try
            {
                speed = Misc.decriptHexaDecimal(OriginalMessage) / 1000;
            }
            catch (Exception ex)
            {
                speed = 0;
            }
        }


        private void getOrientation()
        {
            try
            {
                string reverse = Misc.arrayToString(originalMessage, 23, 2, true);

                orientation = (Int16)(Misc.decriptHexaDecimal(reverse) / 100);
            }
            catch (Exception ex)
            {
                orientation = 0;
            }
        }

        private void getTimestamp(string hexadecimal)
        {
            var decimalValue = Misc.decriptHexaDecimal(hexadecimal);

            if (decimalValue > 0)
            {
                timestamp = new DateTime(1970, 01, 01, 00, 00, 00).AddSeconds(decimalValue);
            }
        }

        private double getGeoLocation(string hexadecimal, string direction, string geoLocationType)
        {
            try
            {
                var decimalValue = Misc.decriptHexaDecimal(hexadecimal);

                var degrees = (int)(decimalValue / 1000);

                var minutes = (decimalValue % 1000);

                var geoLocation = (degrees + minutes / 60000.0);


                if (true)
                {
                    geoLocation *= -1;
                }

                return geoLocation;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }


        public override string ToString()
        {
            return string.Format(
@"Primeiro Byte: {0}
Id da mensagem: {1}
Tipo da mensagem: {2}
Tamanho da mensagem: {3}
Mensagem CRC: {4}
UnitID: {5}
Timestamp : {6}
Latitude : {7}
Longitude : {8}
Orientação : {9}
Velocidade : {10}
",
_header.FirstByte,
_header.MessageId,
_header.MessageType,
_header.MessageSize,
_header.MessageCRC,
_header.UnitId,
timestamp.ToString("dd/MM/yyyy HH:mm:ss"),
latitude,
longitude,
orientation,
speed);
        }
    }
}

