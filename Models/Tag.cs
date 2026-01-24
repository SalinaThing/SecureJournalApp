using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace SecureJournalApp.Models
{
    public class Tag
    {
        [PrimaryKey, AutoIncrement]
        public int TagId { get; set; }

    }
}
