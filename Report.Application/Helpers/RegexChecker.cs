using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Report.Application.Helpers;

public static class RegexChecker
{
    public static bool DigitsAndLettersCheck(string overHeadNumber)
    {
        Regex regex = new("^[a-zA-Z0-9]*$");
        return regex.IsMatch(overHeadNumber);
    }
}
