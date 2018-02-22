using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.Dssp
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
        protected DsspField(string columnFormatLine)
        {
            FieldValue = (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "";
        }
    }
}
