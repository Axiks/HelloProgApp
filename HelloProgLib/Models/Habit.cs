using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloProgLib.Models
{
    public class Habit
    {
        public Habit(int id, string title, string description)
        {
            this.id = id;
            this.title = title;
            this.description = description;
        }

        public int id { get; }
        public string title;
        public string description;
    }
}
