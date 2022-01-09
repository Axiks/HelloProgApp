using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KonatsuWebApplication.Entities
{
    public class AppUser : BaseEntity
    {
        public string Username { get; }
        public int FirstName { get; }
        public int LastName { get; }
        public string About { get; set; }
        // public List<Habit> Habits { get; set; }
    }
}
