using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KonatsuWebApplication.Entities
{
    public class AppUser : BaseEntity
    {
        public string Username { get; set; }
        public int FirstName { get; set; }
        public int LastName { get; set; }
        public string About { get; set; }
        public List<Habit> Habits { get; set; }
    }
}
