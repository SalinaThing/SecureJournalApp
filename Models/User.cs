using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureJournalApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Pin { get; set; } = string.Empty; // store hashed PIN ideally
    }
}
