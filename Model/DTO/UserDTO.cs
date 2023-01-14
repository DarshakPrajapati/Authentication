using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class UserDTO
    {
        public String FullName { get; set; }
        public String Email { get; set; }
        public String UserName { get; set; }
        public DateTime DateCreated { get; set; }
        public String Token { get; set; }

        public UserDTO(String fullName, String email, String userName, DateTime createdDate)
        {
            FullName = fullName;
            Email = email;
            UserName = userName;
            DateCreated = createdDate;
        }
    }
}
