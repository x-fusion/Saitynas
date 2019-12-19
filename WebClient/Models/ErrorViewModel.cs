using System;

namespace WebClient.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string ErrorTitle => "Įvyko klaida...";
        public string ErrorMessage { get; set; }
    }
}
