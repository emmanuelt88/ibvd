using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBVD.Digital.Logic.Entities
{
    public class UsuarioMinimo
    {
        public string UserName { get; private set; }
        public string Email { get; private set; }
        public bool IsEnabled { get; private set; }
        public string Password { get; private set; }

        public UsuarioMinimo()
        {

        }

        public UsuarioMinimo(string userName, string email, bool isEnabled, string password)
        {
            this.UserName = userName;
            this.Email = email;
            this.IsEnabled = isEnabled;
            this.Password = password;
        }


    }
}
