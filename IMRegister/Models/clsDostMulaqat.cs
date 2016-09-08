using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMRegister.Models
{
    public class clsDostMulaqat
    {
        public String Id { get; set; }
        //public clsSarif Sarif { get; set; }
       // public clsDost Dost { get; set; }
        public String Tarekh { get; set; }
        public String Tafseel { get; set; }
        public String Halath { get; set; }

        /*public clsDostMulaqat() {
            Sarif = new clsSarif();
            Dost = new clsDost();
        }*/
    }

    public class clsMulaqatScren
    {
        String Dai { get; set; }
        clsDost oDost{ get; set; }
    }

}