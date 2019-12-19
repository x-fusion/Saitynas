using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ApiRequests
{
    public class AuthResponse : Response
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
