using System;
using System.Data;

namespace DAL
{
    public class clsResults
    {
        public String ErrorCode { get; set; }
        public String ErrorMessage { get; set; }
        public String AdditionalInfo { get; set; }
        public DataTable dtRecords { get; set; }
    }
}