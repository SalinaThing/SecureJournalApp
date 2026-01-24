using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureJournalApp.Models
{
    public class EntryTag
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int EntryId { get; set; }

        public string Tag { get; set; } = string.Empty;

    }
}
