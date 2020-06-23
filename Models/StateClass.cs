using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bkfc.Models
{
    public class StateClass
    {
        static string _Mode = "On";
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
