using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.AminoAcids
{
    public enum AminoAcidPropertyMatchType
    {
        NoneMatch,
        AnyMatch,
        AnyTrueMatch,
        AnyFalseMatch,
        AllTrueMatch,
        AllFalseMatch,
        AllMatch,
        PercentageTrueMatch,
        PercentageFalseMatch,
        PercentageMatch,
        MininumTrueMatch,
        MininumFalseMatch,
        MininumMatch
    };
}
