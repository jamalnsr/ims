using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMRegister
{
    public class Enumerations
    {
    }

    enum Halath {
        Not_In_Use=0,
        New       =1,
        Edited    =2,
        Saved     =3,
        Delete    =4
    };

    enum Hasiath {
        Droped = 0,
        Mujawizh =1,
        Baqaida = 2,
        House = 3                    
    }
    public enum enStringValues
    {
        STRING,
        TEXT,
        DATE,
        TIME,
        DATETIME,
        MEMO,
        BIT,
        VARCHAR,
        NVARCHAR
    }
}
 