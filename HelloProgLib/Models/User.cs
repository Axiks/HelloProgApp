using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloProgLib.Models
{
    public class User
    {
        public User(string TelegramId, string Username, string About, DateTime AccessionTime)
        {
            this.TelegramId = TelegramId;
            this.Username = Username;
            this.About = About;
            this.AccessionTime = AccessionTime;
        }

        public string TelegramId { get; }
        public string Username { get; }

        public string About { get; set; }
        public DateTime AccessionTime { get; }

        public List<Habit> Habits { get; set; }
    }
}
