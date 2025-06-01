using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.Models.Entities
{
    public class UserRegister
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "user";
    }
}
