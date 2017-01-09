using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMRegister
{
    public static class WebSession
    {
        private static Dictionary<string, SessionDetails> _Session;

        public static Dictionary<string, SessionDetails> Session
        {
            get
            {
                if (_Session == null)
                {
                    _Session = new Dictionary<string, SessionDetails>();
                }
                RefreshSession();
                return _Session;
            }
        }

        private static void RefreshSession()
        {
            foreach(KeyValuePair<string, SessionDetails> kvp in _Session.Where(sn=> sn.Value.LastRequest.AddMinutes(30) < DateTime.Now)){
                _Session.Remove(kvp.Key);
            }
        }
    }

    public class SessionDetails
    {
        public SessionDetails(string token, string id, string name, DateTime now)
        {
            this.SessionId = token;
            this.SarifId = id;
            this.SarifName = name;
            this.LastRequest = now;
        }

        public  String SessionId { get; set; }
        public  String SarifId { get; set; }
        public  String SarifName { get; set; }
        public  DateTime LastRequest { get; set; }
    }
}