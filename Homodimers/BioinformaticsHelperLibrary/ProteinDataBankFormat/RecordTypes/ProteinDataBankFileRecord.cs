using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes
{
    [Serializable]
    public abstract class ProteinDataBankFileRecord : IEquatable<ProteinDataBankFileRecord>
    {
        public readonly string ColumnFormatLine;

        protected ProteinDataBankFileRecord(string columnFormatLine)
        {
            ColumnFormatLine = columnFormatLine;
        }

        public bool Equals(ProteinDataBankFileRecord other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(ColumnFormatLine, other.ColumnFormatLine);
        }

        public abstract override string ToString();

        public abstract string[] ToArray();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ProteinDataBankFileRecord)obj);
        }

        public override int GetHashCode()
        {
            return (ColumnFormatLine != null ? ColumnFormatLine.GetHashCode() : 0);
        }

        public static bool operator ==(ProteinDataBankFileRecord left, ProteinDataBankFileRecord right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ProteinDataBankFileRecord left, ProteinDataBankFileRecord right)
        {
            return !Equals(left, right);
        }
    }
}
