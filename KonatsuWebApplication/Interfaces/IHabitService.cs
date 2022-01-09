using KonatsuWebApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloProgLib
{
    public interface IHabitService : IDisposable
    {
        void AddHabbit(String title, String description);
        Habit GetHabbit(int id);
        Habit FindHabbit(string title);
        void DeleteHabbit(int id);
        List<Habit> AllHabits();
    }
}
