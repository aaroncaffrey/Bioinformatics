using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace BioinformaticsHelperLibrary.Misc
{
    public class XlSpreadsheetDocumentContainer
    {
        public SpreadsheetDocument SpreadsheetDocumentObject;
        public WorkbookPart WorkbookPartObject;
        public Sheets SheetsObject;
    }
}
