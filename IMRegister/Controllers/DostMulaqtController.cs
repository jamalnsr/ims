using DAL;
using IMRegister.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Script.Serialization;

namespace IMRegister.Controllers
{
    public class DostMulaqtController : ApiController
    {
        #region Services
        [HttpGet]        
        public clsResposce getMulaqats()
        {
            clsResposce oRes = new clsResposce();
            oRes.Data = GetSarifData();

            return oRes;
        }
        
        [HttpPost]
        public clsResposce updateMulaqats([FromBody] clsSarif oSarif)
        {
            foreach (clsDost oDost in oSarif.lstDost)
            {
                SaveDost(oDost, oSarif.Id);
                foreach (clsDostMulaqat oMulaqat in oDost.lstMulaqat.Where(Row=> Row.Halath != "0" && Row.Halath !="3"))
                {
                    SaveMulaqat(oMulaqat, oSarif.Id, oDost.Id);
                }
            }

            clsResposce oRes = new clsResposce();
            oRes.Data = GetSarifData();

            return oRes;
        }
        
        private void SaveMulaqat(clsDostMulaqat oMulaqat, String SarifID, String DostId)
        {
            //if (oMulaqat.Halath == "0" || oMulaqat.Halath == "3")
            //    return;

            if (oMulaqat.Halath == "4")
            {
                DeleteMulaqat(oMulaqat);
            }
            else {
                if (oMulaqat.Halath == ((int)Halath.Edited).ToString())
                {
                    CFun.fnSaveData(oMulaqat.Id, "Mulaqatain",
                                   new string[] { "Tarekh", "Tafseel" },
                                   new string[] { oMulaqat.Tarekh, oMulaqat.Tafseel },
                                   "Id = " + oMulaqat.Id
                           );
                }
                else
                {
                    CFun.fnSaveData(oMulaqat.Id, "Mulaqatain",
                                       new string[] { "SarifId", "DostId", "Tarekh", "Tafseel" },
                                       new string[] { SarifID, DostId, oMulaqat.Tarekh, oMulaqat.Tafseel }
                               ); 
                }
            }
        }
        private void DeleteMulaqat(clsDostMulaqat oMulaqat)
        {
            DAL.DBManager.ExecuteQuery("delete from Mulaqatain where id = " + oMulaqat.Id);
        }
        private void SaveDost(clsDost oDost,String SarifID)
        {
            if (oDost.Halath == ((int)Halath.Edited).ToString())
            {
                CFun.fnSaveData(oDost.Id, "Dost",
                                new string[] { "Naam", "Tafseel", "Hasiath" },
                                new string[] { oDost.Naam, oDost.Tafseel, oDost.Hasiath },
                                "Id = " + oDost.Id
                        ); 
            }
            else if(oDost.Halath == ((int)Halath.New).ToString())
            {
                oDost.Id =  CFun.fnSaveData(oDost.Id, "Dost",
                                 new string[] { "Naam", "Tafseel", "Hasiath", "SarifId" },
                                 new string[] { oDost.Naam, oDost.Tafseel, oDost.Hasiath, SarifID }
                         ).AdditionalInfo;
                
            }
        }
        #endregion

        #region Operations
        private static clsSarif GetSarifData()
        {
            DataTable dtResult = DAL.DBManager.GetDataTable(String.Format(
                            @"  SELECT * 
                                    FROM SARIF LEFT OUTER JOIN 
		                                 DOST ON (Sarif.id = Dost.SarifId) LEFT OUTER JOIN  
		                                 Mulaqatain on (Dost.Id = Mulaqatain.DostId)
                                    WHERE Sarif.id ={0}
                                    ORDER BY Sarif.id Asc,Dost.id Asc,Mulaqatain.Tarekh desc",
                              "1"));

            clsSarif oSarif = new clsSarif();
            oSarif.Id = dtResult.Rows[0]["Id"].ToString();
            oSarif.Naam = dtResult.Rows[0]["Naam"].ToString();
            oSarif.Columns = 0;

            oSarif.lstDost = new List<clsDost>();
            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                if (dtResult.Rows[i]["Id1"].ToString() == "")
                {
                    break;
                }
                clsDost oDost = new clsDost();
                oDost.Id = dtResult.Rows[i]["Id1"].ToString();
                oDost.Naam = dtResult.Rows[i]["Naam1"].ToString();
                oDost.Tafseel = dtResult.Rows[i]["Tafseel1"].ToString();
                oDost.Halath = "3";
                oDost.Hasiath = dtResult.Rows[i]["Hasiath"].ToString();
                oDost.lstMulaqat = new List<clsDostMulaqat>();
                for (; i < dtResult.Rows.Count; i++)
                {
                    if (oDost.Naam == dtResult.Rows[i]["Naam1"].ToString())
                    {
                        if (dtResult.Rows[i]["Id2"].ToString() == "")
                        {
                            break;
                        }
                            clsDostMulaqat oMulaqat = new clsDostMulaqat();
                            oMulaqat.Id = dtResult.Rows[i]["Id2"].ToString();
                            oMulaqat.Tafseel = dtResult.Rows[i]["Tafseel2"].ToString();
                            oMulaqat.Tarekh = dtResult.Rows[i]["Tarekh"].ToString();                            
                            oMulaqat.Halath = "3";
                            oDost.lstMulaqat.Add(oMulaqat);
                    }
                    else {                        
                        i--;
                        break;
                    }
                    if (oSarif.Columns < oDost.lstMulaqat.Count)
                        oSarif.Columns = oDost.lstMulaqat.Count;
                }
                oSarif.lstDost.Add(oDost);
            }
            oSarif.Columns += 4;
            return oSarif;
        }
        #endregion
    }
}
