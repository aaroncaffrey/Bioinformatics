using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioinformaticsHelperLibrary.TypeConversions;

namespace BioinformaticsHelperLibraryUnitTests
{
    public static class CommonMethods
    {
        public static void PrintMatrix(List<List<decimal>> matrix)
        {
            PrintMatrix(ConvertTypes.ListDecimalListToDecimal2DArray(matrix));
        }

        public static void PrintMatrix(decimal[,] matrix)
        {
            var maxlen = matrix.Cast<decimal>().Select(a => a.ToString().Length).Max();

            var sb = new StringBuilder("".PadRight(maxlen + 1));
            for (var x = 0; x < matrix.GetLength(0); x++)
            {
                sb.Append(x.ToString().PadRight(maxlen+1));
            }

            sb.AppendLine();
            sb.AppendLine();

            for (var y = 0; y < matrix.GetLength(1); y++)
            {
                sb.Append(y.ToString().PadRight(maxlen + 1));

                for (var x = 0; x < matrix.GetLength(0); x++)
                {
                    string s = matrix[x, y].ToString().PadRight(maxlen + 1);

                    sb.Append(s);
                }

                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());
        }
    }
}
