using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMRegister
{
    public class Enumerations
    {
    }

    enum UserStatus {
        RequestPending = 0 ,
        Active  = 1 ,
        Locked = 2,
        RequestRejected =3
    }

    enum UserType
    {
        Admin=1,
        User=2
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
 