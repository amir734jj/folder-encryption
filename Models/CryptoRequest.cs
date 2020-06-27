using System;

namespace Models
{
    public class CryptoRequest
    {
        public string Path { get; set; }
        
        public string Password { get; set; }
        
        public DateTime? UnlockDate { get; set; } = DateTime.Today.AddDays(1);
        
        public CryptoAction? Action { get; set; }
    }
}