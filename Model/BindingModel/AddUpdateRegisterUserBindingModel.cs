using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.BindingModel
{
    public class AddUpdateRegisterUserBindingModel
    {
        public String FullName { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }

        public AddUpdateRegisterUserBindingModel()
        {
            FullName = String.Empty;
            Email = String.Empty;
            Password = String.Empty;
        }
    }
}
