using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMRegister.Models
{
    public class clsResposce
    {
        public ResponseCode Code { get; set; }
        public String Description { get; set; }
        public object Data { get; set; }

        public clsResposce()
        {
            Description = ""; Code = ResponseCode.Success;
        }
    }
    public enum ResponseCode {
        Success = 0,
        Error = 1
    };
}