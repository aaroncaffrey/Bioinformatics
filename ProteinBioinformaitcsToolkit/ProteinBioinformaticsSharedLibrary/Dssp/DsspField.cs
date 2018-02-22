using System;

namespace ProteinBioinformaticsSharedLibrary.Dssp
{
    [Serializable]
    public abstract class DsspField
    {
        public string FieldName;
        public int FirstColumn;
        public int LastColumn;
        public string FieldValue;


        protected DsspField(string fieldName, int firstColumn, int lastColumn, string fieldValue)
        {
            FieldName = fieldName;
            FirstColumn = firstColumn;
            LastColumn = lastColumn;
            FieldValue = fieldValue;
        }
    }
}
