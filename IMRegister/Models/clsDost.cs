using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMRegister.Models
{
    public class clsDost
    {
        public String Id { get; set; }
        public String Naam { get; set; }
        public String Tafseel { get; set; }
        public String Halath { get; set; }
        public List<clsDostMulaqat> lstMulaqat { get; set; }
        public string Hasiath { get; set; }
    }
}