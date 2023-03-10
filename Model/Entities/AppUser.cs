using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class AppUser : IdentityUser
    {
        public String FullName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }


        public AppUser()
        {
            FullName = String.Empty;
        }
    }
}
