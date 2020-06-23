using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bkfc
{
    
    public class GlobalClass
    {
        static string _Mode= "On";
        public static string Mode
        {
            get
            {
                return _Mode;
            }
            [Authorize(Roles = "Admin")]
            set
            {
                _Mode = value;
            }
        }

    }
}
