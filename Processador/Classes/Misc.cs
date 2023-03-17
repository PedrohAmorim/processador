using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processador.Classes
{
    public static class Misc
    {
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
