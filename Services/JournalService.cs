using SecureJournalApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecureJournalApp.Services
{
    public class JournalService
    {
        private readonly List<JournalEntry> _entries = new();
        private int _nextId = 1;

        public List<JournalEntry> GetAll() => _entries;

        public JournalEntry Get(int id) => _entries.FirstOrDefault(e => e.Id == id);

        public void Add(JournalEntry entry)
        {
            entry.Id = _nextId++;
            _entries.Add(entry);
        }

        public void Update(JournalEntry entry)
        {
            var index = _entries.FindIndex(e => e.Id == entry.Id);
            if (index >= 0) _entries[index] = entry;
        }

        public void Delete(int id)
        {
            var entry = Get(id);
            if (entry != null) _entries.Remove(entry);
        }
    }
}
