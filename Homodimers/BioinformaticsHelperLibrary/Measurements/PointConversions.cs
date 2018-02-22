using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioinformaticsHelperLibrary.Misc;
using BioinformaticsHelperLibrary.ProteinDataBankFormat;
using BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes;

namespace BioinformaticsHelperLibrary.Measurements
{
    public static class PointConversions
    {
        public static Point3D AtomPoint3D(ATOM_Record atom)
        {
            if (ParameterValidation.IsAtomNullOrEmpty(atom))
            {
                throw new ArgumentOutOfRangeException(nameof(atom));
            }

            return new Point3D(atom.x.FieldValue, atom.y.FieldValue, atom.z.FieldValue);
        }

        public static Point1D AtomResidueSequencePoint1D(ATOM_Record atom)
        {
            if (ParameterValidation.IsAtomNullOrEmpty(atom))
            {
                throw new ArgumentOutOfRangeException(nameof(atom));
            }

            return new Point1D(atom.resSeq.FieldValue);    
        }
        
    }
}
