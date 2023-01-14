using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.BindingModel
{
    public class LoginBindingModel
    {
        public String Email { get; set; }
        public String Password { get; set; }

        public LoginBindingModel()
        {
            Email = String.Empty;
            Password = String.Empty;
        }
    }
}
