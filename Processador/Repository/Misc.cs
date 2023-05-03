using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processador.Classes
{
    public static class Misc
    {
        public static string ComputeTwosComplement(string hexValue, bool areAllBytesSignificant)
        {
            const int maxNumberOfBytes = 8;
            int lenght = hexValue.Length;

            //Não permite mais de 64bits
            if (lenght > maxNumberOfBytes * 2) throw new ArgumentOutOfRangeException(hexValue);

            //Converte hexadecimal para long
            ulong intValue = Convert.ToUInt64(hexValue, 16);

            //Calcula o complemento para 2
            ulong complement = (~intValue + 1);

            //Converte para hexadecimal
            string twosComplement = string.Format("{0:X16}", complement);

            if (areAllBytesSignificant)
            {
                //Ajusta para o mesmo número de bytes do valor passado
                twosComplement = twosComplement.Substring(maxNumberOfBytes * 2 - lenght);
            }
            else
            {
                //Representação hexadecimal do valor passado sem 00's à esquerda
                string significantBytes = string.Format("{0:X}", intValue);
                //Ajusta para o número de bytes significativos do valor passado
                twosComplement = twosComplement.Substring(maxNumberOfBytes * 2 - significantBytes.Length);
            }
            return twosComplement;
        }


        public static int decriptHexaDecimal(string hexadecimal)
        {
            try
            {
                return Convert.ToInt32(hexadecimal, 16);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public static string arrayToString(string[] message, int start, int iterations, bool invert)
        {
            string stringConcat = "";
            string[] generateArray = new string[iterations];

            for (int i = 0; i <= iterations - 1; i++)
            {
                generateArray[i] = message[start + i];
            }

            if (invert)
            {
                generateArray = generateArray.Reverse<string>().ToArray();
            }

            stringConcat = String.Join("", generateArray);

            return stringConcat;

        }

        public static int getSize(string message)
        {
            int size = 0;
            int.TryParse(message, out size);

            return size;
        }
    }
}
