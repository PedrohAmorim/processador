using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processador.Classes
{

    class Event
    {

        public string[] OriginalMessage { get; set; }

        public PacketHeader Header { get; set; }

        public DateTime timestampStart { get; set; }

        public DateTime timestampEnd { get; set; }

        public double latitudeStart { get; set; }

        public double longitudeStart { get; set; }

        public double latitudeEnd { get; set; }

        public double longitudeEnd { get; set; }

        public long odometerStart { get; set; }

        public long odometerEnd { get; set; }

        public Int16 MaxSpeed { get; set; }

        public Int16 MaxRpm { get; set; }

        public Int16 MaxPedal { get; set; }

        public int Speed { get; set; }


        private string typeOriginalMessage { get; set; }

        public Event(string[] message, PacketHeader header)
        {
            Header = header;
            OriginalMessage = message;
            decript();
        }


        private void decript()
        {
            getTimestamp();
            latitudeStart = getGeoLocation(Misc.arrayToString(OriginalMessage, 11, 4, true), Misc.arrayToString(OriginalMessage, 17, 1, false), "Lat");
            //Longitude = getGeoLocation(Misc.arrayToString(OriginalMessage, 18, 4, true), Misc.arrayToString(OriginalMessage, 22, 1, false), "Lng");
            getSpeed(Misc.arrayToString(OriginalMessage, 25, 2, true));
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



        private void getTimestamp()
        {
            var decimalValue = Misc.decriptHexaDecimal(Misc.arrayToString(OriginalMessage, 27, 4, true));

            timestampStart = new DateTime(1970, 01, 01, 00, 00, 00).AddSeconds(decimalValue);

            decimalValue = Misc.decriptHexaDecimal(Misc.arrayToString(OriginalMessage, 31, 4, true));

            timestampEnd = new DateTime(1970, 01, 01, 00, 00, 00).AddSeconds(decimalValue);
        }

        private double getGeoLocation(string hexadecimal, string direction, string geoLocationType)
        {
            try
            {
                var decimalValue = Misc.decriptHexaDecimal(hexadecimal);

                var degrees = (int)(decimalValue / 1000);

                var minutes = (decimalValue % 1000);

                var geoLocation = (degrees + minutes / 60000.0);


                if ((geoLocationType == "Lat" && direction == "S") || (geoLocationType == "Lng" && direction == "W"))
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
            /*
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

            */

            return "";
        }
    }
}

