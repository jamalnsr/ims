using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMRegister
{
    public static class WebSession
    {
        private static Dictionary<string, DateTime> _Session;

        public static Dictionary<string, DateTime> Session
        {
            get
            {
                if (_Session == null)
                {
                    _Session = new Dictionary<string, DateTime>();
                }
                return _Session;
            }
        }
    }
}