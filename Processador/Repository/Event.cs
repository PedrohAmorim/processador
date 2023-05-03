using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Processador.Classes
{

    public class Event
    {
        public Module _module { get; set; }

        public PacketHeader _header { get; set; }

        public int EventId { get; set; }

        public string[] originalMessage { get; set; }

        public DateTime timestampStart { get; set; }

        public DateTime timestampEnd { get; set; }

        public double latitudeStart { get; set; }

        public double longitudeStart { get; set; }

        public double latitudeEnd { get; set; }

        public double longitudeEnd { get; set; }

        public long odometerStart { get; set; }

        public long odometerEnd { get; set; }

        public Int16 maxSpeed { get; set; }

        public Int16 maxRpm { get; set; }

        public Int16 maxPedal { get; set; }

        public int speed { get; set; }

        public Event(string[] message, PacketHeader header, Module module)
        {
            _header = header;
            originalMessage = message;
            _module = module;
            decript();
        }


        private void decript()
        {
            // Processar horários
            getTimestamp();

            // Processar hodômetro
            getOdometers();

            // Processar coordenadas
            latitudeStart = getGeoLocation(Misc.arrayToString(originalMessage, 11, 4, true), "Lat");
            longitudeStart = getGeoLocation(Misc.arrayToString(originalMessage, 15, 4, true), "Lng");
            latitudeEnd = getGeoLocation(Misc.arrayToString(originalMessage, 19, 4, true), "Lat");
            longitudeEnd = getGeoLocation(Misc.arrayToString(originalMessage, 23, 4, true), "Lng");

            // Obter velocidade
            getMaxSpeed(Misc.arrayToString(originalMessage, 45, 1, false));

            // Obter Rpm
            getMaxRpm(Misc.arrayToString(originalMessage, 46, 2, true));

            // Obter Pedal
            getMaxPedal(Misc.arrayToString(originalMessage, 48, 1, true));

        }


        private void getEventId()
        {
            int eventId = 0;

            int.TryParse(Misc.arrayToString(originalMessage, 10, 1, true), out eventId);

            EventId = eventId;
        }

        private void getOdometers()
        {
            try
            {
                int odometer = 0;

                int.TryParse(Misc.arrayToString(originalMessage, 37, 4, true), out odometer);
                odometerStart = odometer;

                int.TryParse(Misc.arrayToString(originalMessage, 41, 4, true), out odometer);
                odometerEnd = odometer;
            }
            catch (Exception ex)
            {
                odometerStart = 0;
                odometerEnd = 0;
            }
        }

        private void getMaxPedal(string OriginalMessage)
        {
            try
            {
                maxPedal = (Int16)Misc.decriptHexaDecimal(OriginalMessage);
            }
            catch (Exception ex)
            {
                maxPedal = 0;
            }
        }

        private void getMaxRpm(string OriginalMessage)
        {
            try
            {
                maxRpm = (Int16)Misc.decriptHexaDecimal(OriginalMessage);
            }
            catch (Exception ex)
            {
                maxRpm = 0;
            }
        }

        private void getMaxSpeed(string strSpeed)
        {
            try
            {
                var speed = 0;

                int.TryParse(strSpeed, out speed);

                this.speed = speed;
            }
            catch (Exception ex)
            {
                speed = 0;
            }
        }




        private void getTimestamp()
        {
            var decimalValue = Misc.decriptHexaDecimal(Misc.arrayToString(originalMessage, 27, 4, true));

            timestampStart = new DateTime(1970, 01, 01, 00, 00, 00).AddSeconds(decimalValue);

            decimalValue = Misc.decriptHexaDecimal(Misc.arrayToString(originalMessage, 31, 4, true));

            timestampEnd = new DateTime(1970, 01, 01, 00, 00, 00).AddSeconds(decimalValue);
        }

        private double getGeoLocation(string hexadecimal, string geoLocationType)
        {
            try
            {
                var decimalValue = Misc.decriptHexaDecimal(hexadecimal);

                if (decimalValue > 162000000)
                    decimalValue = Misc.decriptHexaDecimal(Misc.ComputeTwosComplement(hexadecimal, true));


                var degrees = (int)(decimalValue / 1000);

                var minutes = (decimalValue % 1000);

                var geoLocation = (degrees + minutes / 60000.0);


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

