using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.InteractionVector
{
    public class MotifCounter
    {
        public string Motif;

        public bool MotifTooGeneral;

        public int TotalFwd;
        public int TotalRev;
        public int TotalMix;

        //public int TotalFwdInHeterodimers;
        //public int TotalRevInHeterodimers;
        //public int TotalMixInHeterodimers;

        //public int TotalFwdInHomodimers;
        //public int TotalRevInHomodimers;
        //public int TotalMixInHomodimers;
    }
}
