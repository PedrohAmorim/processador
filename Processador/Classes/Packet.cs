using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processador.Classes
{
    public enum valores
    {
        TIMESTAMP = 9,

    }
    class Packet
    {
        public bool Valid { get { return int.Parse(OriginalMessage[8]) <= 67; } }

        public string[] OriginalMessage { get; set; }

        public PacketHeader Header { get; set; }

        public Int16 Orientation { get; set; }

        public DateTime Timestamp { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int Speed { get; set; }

        private string typeOriginalMessage { get; set; }

        public Packet(string[] message)
        {
            OriginalMessage = message;
            if (Valid)
            {
                Header = new PacketHeader(OriginalMessage);
                processTypePacket();
            }
        }

        private void processTypePacket()
        {
            /*
            FIELD INDEX SIZE CONVERSION
            TRK_TIMESTAMP 9 4 Epoch conversion
            TRK_LATITUDE 13 4 See latitude equation
            TRK_LATITUDE_DIR 17 1 ASCII
            TRK_LONGITUDE 18 4 See longitude equation
            TRK_LONGITUDE_DIR 22 1 ASCII
            TRK_COURSE 23 2 -
            TRK_SPEED 25 2 -
            TRK_LOC_STATUS 27 1 See table 1
            */
            if (Header.MessageSize <= 15) // Pacote de track
            {
                trackerParser();
            }
            else if (Header.MessageSize <= 40) //  Pacote de evento
            {
                eventParser();
            }
            else
            {

            }
        }


        private void trackerParser()
        {
            getTimestamp(Misc.arrayToString(OriginalMessage, 9, 4, true));
            getGeoLocation(Misc.arrayToString(OriginalMessage, 13, 4, true), Misc.arrayToString(OriginalMessage, 17, 1, false), "Lat");
            getGeoLocation(Misc.arrayToString(OriginalMessage, 18, 4, true), Misc.arrayToString(OriginalMessage, 22, 1, false), "Lng");
            getSpeed(Misc.arrayToString(OriginalMessage, 25, 2, false));
        }

        private void eventParser()
        {
            getTimestamp(Misc.arrayToString(OriginalMessage, 9, 4, true));
            getGeoLocation(Misc.arrayToString(OriginalMessage, 13, 4, true), Misc.arrayToString(OriginalMessage, 17, 1, false), "Lat");
            getGeoLocation(Misc.arrayToString(OriginalMessage, 18, 4, true), Misc.arrayToString(OriginalMessage, 22, 1, false), "Lng");
            getSpeed(Misc.arrayToString(OriginalMessage, 25, 2, false));
        }

        private void getSpeed(string OriginalMessage)
        {
            var speed = 0;

            int.TryParse(OriginalMessage, out speed);

            Speed = speed;
        }

        private int decriptHexaDecimal(string hexadecimal)
        {
            try
            {
                var OriginalMessage = hexadecimal.Replace("0x", "").Split(' ').Reverse().ToArray();

                var concatString = String.Join("", OriginalMessage);

                return Convert.ToInt32(concatString, 16);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        private void getOrientation()
        {
            Int16 orientation = 0;

            Int16.TryParse(Misc.arrayToString(OriginalMessage, 23, 2, false), out orientation);

            Orientation = orientation;
        }
        private void getTimestamp(string hexadecimal)
        {
            var decimalValue = decriptHexaDecimal(hexadecimal);

            if (decimalValue > 0)
            {
                Timestamp = new DateTime(1970, 01, 01, 00, 00, 00).AddSeconds(decimalValue);
            }
        }

        private double getGeoLocation(string hexadecimal, string direction, string geoLocationType)
        {
            try
            {
                var decimalValue = decriptHexaDecimal(hexadecimal);

                var degrees = (int)(decimalValue / 100000);

                var minutes = decimalValue % 100000;

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
    }
}

