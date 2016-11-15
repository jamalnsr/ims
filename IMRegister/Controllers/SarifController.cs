using IMRegister.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IMRegister.Controllers
{
    public class SarifController : ApiController
    {
        #region Services
        [HttpPost]
        public clsResposce Login( String id)
        {
            clsResposce oRes = new clsResposce();
            oRes.Data = id;
            try
            {
                CheckeCredentials(oRes);
            }
            catch (Exception ex)
            {
                oRes.Code = ResponseCode.Error;
                oRes.Description = ex.Message;
                 
            }
            return oRes;
        }
        #endregion

        private bool CheckeCredentials(clsResposce oRes)
        {
            DataTable dt =  DAL.DBManager.GetDataTable(
                String.Format("select id,naam, username,ShanakhtiLafz,Status from sarif where username=N'{0}' and ShanakhtiLafz=N'{1}'",
                                oRes.Data.ToString().Split('ß' )[0], oRes.Data.ToString().Split('ß')[1]));
            
            if(dt.Rows.Count == 1 &&
               oRes.Data.ToString().Split('ß')[0] == dt.Rows[0]["username"].ToString() && 
               oRes.Data.ToString().Split('ß')[1] == dt.Rows[0]["ShanakhtiLafz"].ToString())
            {
                if(dt.Rows[0]["Status"].ToString() == ((int)UserStatus.Locked).ToString())
                {
                    oRes.Code = ResponseCode.LoginFailed;
                    oRes.Description = "آپ کا اکاونٹ عارضی طور پر بند ہے۔ ایڈمن سے رابطہ کریں۔";
                }
                else
                {
                    oRes.Code = ResponseCode.Success;
                    oRes.Token = dt.Rows[0]["id"].ToString();//  (new DataEncryptor()).EncryptString(dt.Rows[0]["id"].ToString());

                    if (WebSession.Session.ContainsKey(oRes.Token))
                    {
                        WebSession.Session[oRes.Token] = DateTime.Now;
                    }
                    else
                    {
                        WebSession.Session.Add(oRes.Token, DateTime.Now);
                    }
                    return true;
                }
            } 
            else
            {
                oRes.Code = ResponseCode.LoginFailed;
                oRes.Description = "مہیا کردہ اسناد ٹھیک نہیں۔";
            }
            return false;
        }
    }
}
