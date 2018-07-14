using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models.AccountViewModels
{
    public class JWTAuthorizationResponse
    {
        public string Access_Token { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}
