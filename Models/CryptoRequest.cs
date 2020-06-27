using System;

namespace Models
{
    public class CryptoRequest
    {
        public string Path { get; set; }
        
        public string Password { get; set; }
        
        public DateTime? UnlockDate { get; set; }
        
        public CryptoAction? Action { get; set; }
    }
}