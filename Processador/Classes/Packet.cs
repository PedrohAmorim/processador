using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processador.Classes
{
    class Packet
    {
        private string OriginalMessage { get; set; }

        public DateTime Timestamp { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public Packet(string message)
        {
            OriginalMessage = message;
        }

        private int decriptHexaDecimal(string hexadecimal)
        {
            try
            {
                var message = hexadecimal.Replace("0x", "").Split(' ').Reverse().ToArray();

                var concatString = String.Join("", message);

                return Convert.ToInt32(concatString, 16);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private void getTimestamp(string hexadecimal)
        {
            var decimalValue = decriptHexaDecimal(hexadecimal);

            if (decimalValue > 0)
            {
                Timestamp = new DateTime(1970, 01, 01, 00, 00, 00).AddSeconds(decimalValue);
            }
        }

        private float getGeoLocation(string hexadecimal, string direction, string typeLocation)
        {
            try
            {
                var decimalValue = decriptHexaDecimal(hexadecimal);

                var degrees = decimalValue / 100000;

                var minutes = decimalValue % 100000;

                var geoLocation = (degrees + minutes / 60000);


                if ((typeLocation == "Lat" && direction == "S") || (typeLocation == "Lng" && direction == "W"))
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
}
