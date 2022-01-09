using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloProgLib.Models
{
    public class UserHabit
    {
        public string TelegramId { get; }
        public int HabitId { get; }

        public UserHabit(string TelegramId, int HabitId)
        {
            this.TelegramId = TelegramId;
            this.HabitId = HabitId;
        }
    }
}
