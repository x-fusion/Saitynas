using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Models
{
    public class AuthResponse
    {
        public string Message { get; set; }
        public object Payload { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
