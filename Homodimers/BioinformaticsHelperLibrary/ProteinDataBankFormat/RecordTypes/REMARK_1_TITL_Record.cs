using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes
{

    [Serializable]
    public class REMARK_1_TITL_Record : ProteinDataBankFileRecord
    {
        [Description(""), Category("Data")]
        public REMARK_Field REMARK;
        [Description(""), Category("Data")]
        public REMARK2_Field REMARK2;
        [Description(""), Category("Data")]
        public REMARK_1_TITL_Field REMARK_1_TITL;

        [Description(""), Category("Data")]
        public REMARK_1_TITL2_Field REMARK_1_TITL2;
        [Description("Appears on all continuation records."), Category("Data")]
        public TITL_Field TITL;

        [Description("Appears on all continuation records."), Category("Data")]
        public TITL2_Field TITL2;

        [Description("Appears on all continuation records."), Category("Data")]
        public TITL3_Field TITL3;

        [Description("Appears on all continuation records."), Category("Data")]
        public TITL4_Field TITL4;
        [Description(""), Category("Data")]
        public _1_Field _1;
        [Description(""), Category("Data")]
        public _12_Field _12;
        [Description(""), Category("Data")]
        public _13_Field _13;
        [Description(""), Category("Data")]
        public _14_Field _14;
        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        public additionalData_Field additionalData;
        [Description("Permits long titles."), Category("Data")]
        public continuation_Field continuation;
        [Description("Permits long titles."), Category("Data")]
        public continuation2_Field continuation2;
        [Description("Permits long titles."), Category("Data")]
        public continuation3_Field continuation3;

        [Description("Permits long list of editors."), Category("Data")]
        public continuation4_Field continuation4;

        [Description("List of the editors."), Category("Data")]
        public editorList_Field editorList;
        [Description("Title of the article."), Category("Data")]
        public title_Field title;
        [Description("Title of the article."), Category("Data")]
        public title2_Field title2;
        [Description("Title of the article."), Category("Data")]
        public title3_Field title3;

        public REMARK_1_TITL_Record(string columnFormatLine)
            : base(columnFormatLine)
        {
            REMARK_1_TITL = new REMARK_1_TITL_Field(columnFormatLine);
            _1 = new _1_Field(columnFormatLine);
            TITL = new TITL_Field(columnFormatLine);
            continuation = new continuation_Field(columnFormatLine);
            title = new title_Field(columnFormatLine);
            REMARK_1_TITL2 = new REMARK_1_TITL2_Field(columnFormatLine);
            _12 = new _12_Field(columnFormatLine);
            TITL2 = new TITL2_Field(columnFormatLine);
            continuation2 = new continuation2_Field(columnFormatLine);
            title2 = new title2_Field(columnFormatLine);
            REMARK = new REMARK_Field(columnFormatLine);
            _13 = new _13_Field(columnFormatLine);
            TITL3 = new TITL3_Field(columnFormatLine);
            continuation3 = new continuation3_Field(columnFormatLine);
            title3 = new title3_Field(columnFormatLine);
            REMARK2 = new REMARK2_Field(columnFormatLine);
            _14 = new _14_Field(columnFormatLine);
            TITL4 = new TITL4_Field(columnFormatLine);
            continuation4 = new continuation4_Field(columnFormatLine);
            editorList = new editorList_Field(columnFormatLine);
            additionalData = new additionalData_Field(columnFormatLine);
        }

        public override string ToString()
        {
            return string.Join("\t", ToArray());
        }

        public override string[] ToArray()
        {
            var result = new[]
                {
                    REMARK_1_TITL.FieldValue,
                    _1.FieldValue,
                    TITL.FieldValue,
                    continuation.FieldValue,
                    title.FieldValue,
                    REMARK_1_TITL2.FieldValue,
                    _12.FieldValue,
                    TITL2.FieldValue,
                    continuation2.FieldValue,
                    title2.FieldValue,
                    REMARK.FieldValue,
                    _13.FieldValue,
                    TITL3.FieldValue,
                    continuation3.FieldValue,
                    title3.FieldValue,
                    REMARK2.FieldValue,
                    _14.FieldValue,
                    TITL4.FieldValue,
                    continuation4.FieldValue,
                    editorList.FieldValue,
                    additionalData.FieldValue
                };
            return result;
        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class REMARK2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "REMARK2";


            public REMARK2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class REMARK_1_TITL2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "REMARK_1_TITL2";


            public REMARK_1_TITL2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class REMARK_1_TITL_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "REMARK_1_TITL";


            public REMARK_1_TITL_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class REMARK_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 1;
            public new const int LastColumn = 6;
            public new const string DataType = "Record name";
            public new const string Definition = "";
            public new const string FieldName = "REMARK";


            public REMARK_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Appears on all continuation records."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class TITL2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 13;
            public new const int LastColumn = 16;
            public new const string DataType = "LString(4)";
            public new const string Definition = "Appears on all continuation records.";
            public new const string FieldName = "TITL2";


            public TITL2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Appears on all continuation records."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class TITL3_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 13;
            public new const int LastColumn = 16;
            public new const string DataType = "LString(4)";
            public new const string Definition = "Appears on all continuation records.";
            public new const string FieldName = "TITL3";


            public TITL3_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Appears on all continuation records."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class TITL4_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 13;
            public new const int LastColumn = 16;
            public new const string DataType = "LString(4)";
            public new const string Definition = "Appears on all continuation records.";
            public new const string FieldName = "TITL4";


            public TITL4_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Appears on all continuation records."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class TITL_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 13;
            public new const int LastColumn = 16;
            public new const string DataType = "LString(4)";
            public new const string Definition = "Appears on all continuation records.";
            public new const string FieldName = "TITL";


            public TITL_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class _12_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 10;
            public new const int LastColumn = 10;
            public new const string DataType = "LString(1)";
            public new const string Definition = "";
            public new const string FieldName = "_12";


            public _12_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class _13_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 10;
            public new const int LastColumn = 10;
            public new const string DataType = "LString(1)";
            public new const string Definition = "";
            public new const string FieldName = "_13";


            public _13_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description(""), Category("Data")]
        [Serializable]
        public class _14_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 10;
            public new const int LastColumn = 10;
            public new const string DataType = "LString(1)";
            public new const string Definition = "";
            public new const string FieldName = "_14";


            public _14_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description(""), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class _1_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 10;
            public new const int LastColumn = 10;
            public new const string DataType = "LString(1)";
            public new const string Definition = "";
            public new const string FieldName = "1";


            public _1_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Any data found which is additional to the fields listed in the specification"), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class additionalData_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 80;
            public new const int LastColumn = -1;
            public new const string DataType = "";
            public new const string Definition = "Any data found which is additional to the fields listed in the specification";
            public new const string FieldName = "additionalData";


            public additionalData_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1) : "")
            {

            }


        }

        [Description("Permits long titles."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class continuation2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 17;
            public new const int LastColumn = 18;
            public new const string DataType = "Continuation";
            public new const string Definition = "Permits long titles.";
            public new const string FieldName = "continuation2";


            public continuation2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Permits long titles."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class continuation3_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 17;
            public new const int LastColumn = 18;
            public new const string DataType = "Continuation";
            public new const string Definition = "Permits long titles.";
            public new const string FieldName = "continuation3";


            public continuation3_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Permits long list of editors."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class continuation4_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 17;
            public new const int LastColumn = 18;
            public new const string DataType = "Continuation";
            public new const string Definition = "Permits long list of editors.";
            public new const string FieldName = "continuation4";


            public continuation4_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Permits long titles."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class continuation_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 17;
            public new const int LastColumn = 18;
            public new const string DataType = "Continuation";
            public new const string Definition = "Permits long titles.";
            public new const string FieldName = "continuation";


            public continuation_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("List of the editors."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class editorList_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 20;
            public new const int LastColumn = 79;
            public new const string DataType = "LString";
            public new const string Definition = "List of the editors.";
            public new const string FieldName = "editorList";


            public editorList_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Title of the article."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class title2_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 20;
            public new const int LastColumn = 79;
            public new const string DataType = "LString";
            public new const string Definition = "Title of the article.";
            public new const string FieldName = "title2";


            public title2_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Title of the article."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class title3_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 20;
            public new const int LastColumn = 79;
            public new const string DataType = "LString";
            public new const string Definition = "Title of the article.";
            public new const string FieldName = "title3";


            public title3_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }

        [Description("Title of the article."), Category("Data")]
        [DefaultProperty("FieldValue")]
        [Serializable]
        public class title_Field : ProteinDataBankFileRecordField
        {
            public new const int FirstColumn = 20;
            public new const int LastColumn = 79;
            public new const string DataType = "LString";
            public new const string Definition = "Title of the article.";
            public new const string FieldName = "title";


            public title_Field(string columnFormatLine)
                : base(DataType, Definition, FieldName, firstColumn: FirstColumn, lastColumn: LastColumn, fieldValue: (FirstColumn > 0 && LastColumn > 0 && columnFormatLine.Length >= FirstColumn) ? columnFormatLine.Substring(FirstColumn - 1, (columnFormatLine.Length >= LastColumn ? ((LastColumn - FirstColumn) + 1) : columnFormatLine.Length - (FirstColumn - 1))).Trim() : "")
            {
            }



        }
    }

}
