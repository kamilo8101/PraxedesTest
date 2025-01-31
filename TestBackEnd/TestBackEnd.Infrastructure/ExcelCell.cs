using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBackEnd.Infrastructure
{

    public enum FormatStyle { None, Date, DateTime, Time }
    public class ExcelCell
    {
        public string Header { get; set; }

        public ExcelStyle Style { get; set; }

        public string ColName { get; set; }

        public ExcelCell(string header)
        {
            this.Header = header;
            this.ColName = header;
        }

        public ExcelCell(string header, string colname)
        {
            this.Header = header;
            this.ColName = colname;
        }

        public ExcelCell(string header, string colname, ExcelStyle style)
        {
            this.Header = header;
            this.ColName = colname;
            this.Style = style;
        }
    }

    public class ExcelStyle
    {
        public FormatStyle FormatStyle { get; set; } = FormatStyle.None;

        public string Format
        {
            get
            {
                switch (FormatStyle)
                {
                    case FormatStyle.None: return "";
                    case FormatStyle.Date: return "dd/mm/yyyy";
                    case FormatStyle.DateTime: return "dd/mm/yyyy hh:mm";
                    case FormatStyle.Time: return "hh:mm";
                    default: return "";
                }
            }
        }
    }
}