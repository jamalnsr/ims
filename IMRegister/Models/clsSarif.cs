using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IMRegister.Models
{
    public class clsSarif
    {
        public String Id { get; set; }
        public String Naam { get; set; }
        public String ShanakhtiLafz { get; set; }
        public String Tafseel { get; set; }
        public String Halath { get; set; }
        public List<clsDost> lstDost { get; set; }

        public int Columns { get; set; }
        public clsSarif( )
        {/*
            this.Id = dr["id"].ToString();
            this.Naam = dr["Naam"].ToString();

            lstDost = new List<clsDost>();
            for

            oDM.Id = dr["id2"].ToString();
            oDM.Tafseel = dr["tafseel2"].ToString();
            oDM.tarekh = dr["tarekh"].ToString();
            oDM.Halat = "1";

            

            oDM.Dost.Id = dr["id"].ToString();
            oDM.Dost.Naam = dr["Naam1"].ToString();
            oDM.Dost.Tafseel = dr["tafseel1"].ToString();
            oDM.Dost.Halath = dr["Halath1"].ToString();*/
        }
    }
}