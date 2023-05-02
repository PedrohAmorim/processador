using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processador.Classes
{

    class Track
    {

        public string[] OriginalMessage { get; set; }

        public PacketHeader Header { get; set; }

        public Int16 Orientation { get; set; }

        public DateTime Timestamp { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int Speed { get; set; }

        private string typeOriginalMessage { get; set; }

        public Track(string[] message, PacketHeader header)
        {
            Header = header;
            OriginalMessage = message;        
            decript();
        }


        private void decript()
        {
            getTimestamp(Misc.arrayToString(OriginalMessage, 9, 4, true));
            Latitude = getGeoLocation(Misc.arrayToString(OriginalMessage, 13, 4, true), Misc.arrayToString(OriginalMessage, 17, 1, false), "Lat");
            Longitude = getGeoLocation(Misc.arrayToString(OriginalMessage, 18, 4, true), Misc.arrayToString(OriginalMessage, 22, 1, false), "Lng");
            getSpeed(Misc.arrayToString(OriginalMessage, 25, 2, true));
            getOrientation();
        }


        private void getSpeed(string OriginalMessage)
        {
            try
            {
                Speed = Misc.decriptHexaDecimal(OriginalMessage) / 1000;
            }
            catch (Exception ex)
            {
                Speed = 0;
            }
        }


        private void getOrientation()
        {
            try
            {
                string reverse = Misc.arrayToString(OriginalMessage, 23, 2, true);

                Orientation = (Int16)(Misc.decriptHexaDecimal(reverse) / 100);
            }
            catch (Exception ex)
            {
                Orientation = 0;
            }
        }

        private void getTimestamp(string hexadecimal)
        {
            var decimalValue = Misc.decriptHexaDecimal(hexadecimal);

            if (decimalValue > 0)
            {
                Timestamp = new DateTime(1970, 01, 01, 00, 00, 00).AddSeconds(decimalValue);
            }
        }

        private double getGeoLocation(string hexadecimal, string direction, string geoLocationType)
        {
            try
            {
                // Converter para decimal  depois pegar o ASCii
                char ascValue = Convert.ToChar(Misc.decriptHexaDecimal(direction));

                var decimalValue = Misc.decriptHexaDecimal(hexadecimal);

                var degrees = (int)(decimalValue / 1000);

                var minutes = (decimalValue % 1000);

                var geoLocation = (degrees + minutes / 60000.0);


                if ((geoLocationType == "Lat" && ascValue == 'S') || (geoLocationType == "Lng" && ascValue == 'W'))
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
                                @"
                                Primeiro Byte: {0}
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
                                Header.FirstByte,
                                Header.MessageId,
                                Header.MessageType,
                                Header.MessageSize,
                                Header.MessageCRC,
                                Header.UnitId,
                                Timestamp.ToString("dd/MM/yyyy HH:mm:ss"),
                                Latitude,
                                Longitude,
                                Orientation,
                                Speed);
        }
    }
}

