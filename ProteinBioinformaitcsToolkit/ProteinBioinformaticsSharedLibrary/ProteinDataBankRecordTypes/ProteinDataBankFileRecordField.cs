using System;
using System.ComponentModel;

namespace ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes
{
    /// <summary>
    ///     abstract class ProteinDataBankFileRecordField.
    /// </summary>
    [Serializable]
    public abstract class ProteinDataBankFileRecordField : IEquatable<ProteinDataBankFileRecordField>
    {
        [Description("The field's DataType as stated in the specification"), Category("")]
        public string DataType;

        [Description("The field's definition as stated in the specification"), Category("")]
        public string Definition;

        [Description("The field's name as stated in the specification"), Category("")]
        public string FieldName;

        [Description("The field's value as found in the line between FirstColumn and LastColumn"), Category("")]
        public string FieldValue;

        [Description("The field's first column on the line"), Category("")]
        public int FirstColumn;

        [Description("The field's last column on the line"), Category("")]
        public int LastColumn;

        protected ProteinDataBankFileRecordField(string dataType, string definition, string fieldName, string fieldValue, int firstColumn, int lastColumn)
        {
            DataType = dataType;
            Definition = definition;
            FieldName = fieldName;
            FieldValue = fieldValue;
            FirstColumn = firstColumn;
            LastColumn = lastColumn;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProteinDataBankFileRecordField" /> class.
        /// </summary>
        public bool Equals(ProteinDataBankFileRecordField other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(DataType, other.DataType) && string.Equals(Definition, other.Definition) && string.Equals(FieldName, other.FieldName) && string.Equals(FieldValue, other.FieldValue) && FirstColumn == other.FirstColumn && LastColumn == other.LastColumn;
        }

        /// <summary>
        ///     ToString method.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return FieldValue;
        }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ProteinDataBankFileRecordField)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (DataType != null ? DataType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Definition != null ? Definition.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FieldName != null ? FieldName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FieldValue != null ? FieldValue.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ FirstColumn;
                hashCode = (hashCode * 397) ^ LastColumn;
                return hashCode;
            }
        }

        public static bool operator ==(ProteinDataBankFileRecordField left, ProteinDataBankFileRecordField right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ProteinDataBankFileRecordField left, ProteinDataBankFileRecordField right)
        {
            return !Equals(left, right);
        }
    }
}
