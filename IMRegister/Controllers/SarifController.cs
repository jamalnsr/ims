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
        [HttpGet]
        [Route("Login")]
        public clsResposce Login( String data)
        {
            clsResposce oRes = new clsResposce();
             
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
                String.Format("select id,naam, username,shankhtilafz,Status from sarif where username={0} and shankhtilafz={1})",
                                oRes.Data.ToString().Split('ß' )[0], oRes.Data.ToString().Split('ß')[1]));
            
            if(oRes.Data.ToString().Split('ß')[0] == dt.Rows[0]["username"].ToString() && 
               oRes.Data.ToString().Split('ß')[1] == dt.Rows[0]["shankhtilafz"].ToString())
            {
                if(dt.Rows[0]["Status"].ToString() == ((int)UserStatus.Active).ToString())
                {
                    oRes.Code = ResponseCode.LoginFailed;
                    oRes.Description = "آپ کا اکاونٹ عارضی طور پر بند ہے۔ ایڈمن سے رابطہ کریں۔";
                }
                else
                {
                    oRes.Token = (new DataEncryptor()).EncryptString(dt.Rows[0]["id"].ToString());
                    WebSession.Session.Add(oRes.Token, DateTime.Now);                    
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
