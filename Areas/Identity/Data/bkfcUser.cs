using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using bkfc.Models;
using Microsoft.AspNetCore.Identity;

namespace bkfc.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the bkfcUser class
    // user model
    public class bkfcUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Image { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string History { get; set; }
        [PersonalData]
        [Column(TypeName = "int")]
        public int Balance { get; set; }

        public IList<UserVendor> UserVendor { get; set; }
    }
}
