using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NSE.Core.Utils
{
    public static class StringUtils
    {
        public static string ApenasNumeros(this string str, string input)
        {
            return new string(input.Where(char.IsDigit).ToArray());
        }
    }
}
