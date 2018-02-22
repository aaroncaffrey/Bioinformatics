using System;
using System.Globalization;

namespace BioinformaticsHelperLibrary.Spreadsheets
{
    [Serializable]
    public class SpreadsheetCell : IEquatable<SpreadsheetCell>, IComparable<SpreadsheetCell>
    {

        public string CellData;
        public SpreadsheetDataTypes SpreadsheetDataType;
        //public SpreadsheetCellColourScheme CellColourScheme = SpreadsheetCellColourScheme.Default;
        
        public SpreadsheetCell()
        {
            CellData = "";
            SpreadsheetDataType = SpreadsheetDataTypes.String;
        }

        public SpreadsheetCell(string cellData, SpreadsheetDataTypes spreadsheetDataType = SpreadsheetDataTypes.String)
        {
            CellData = cellData;
            SpreadsheetDataType = spreadsheetDataType;
        }
        public SpreadsheetCell(int cellData, SpreadsheetDataTypes spreadsheetDataType = SpreadsheetDataTypes.Integer)
        {
            CellData = cellData.ToString(CultureInfo.InvariantCulture);
            SpreadsheetDataType = spreadsheetDataType;
        }

        public SpreadsheetCell(double cellData, SpreadsheetDataTypes spreadsheetDataType = SpreadsheetDataTypes.Double)
        {
            CellData = cellData.ToString(CultureInfo.InvariantCulture);
            SpreadsheetDataType = spreadsheetDataType;
        }

        public SpreadsheetCell(decimal cellData, SpreadsheetDataTypes spreadsheetDataType = SpreadsheetDataTypes.Decimal)
        {
            CellData = cellData.ToString(CultureInfo.InvariantCulture);
            SpreadsheetDataType = spreadsheetDataType;
        }

        public override string ToString()
        {
            return CellData;
        }

        public bool Equals(SpreadsheetCell other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(CellData, other.CellData);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SpreadsheetCell) obj);
        }

        public override int GetHashCode()
        {
            return (CellData != null ? CellData.GetHashCode() : 0);
        }

        public static bool operator ==(SpreadsheetCell left, SpreadsheetCell right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SpreadsheetCell left, SpreadsheetCell right)
        {
            return !Equals(left, right);
        }

        public int CompareTo(SpreadsheetCell other)
        {
            return string.Compare(CellData, other.CellData, System.StringComparison.Ordinal);
        }
    }
}
