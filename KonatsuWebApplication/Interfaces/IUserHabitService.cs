using KonatsuWebApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloProgLib.Interfaces
{
    public interface IUserHabitService
    {
        List<Habit> GetUserHabits(string telegramId);
        void AddUserHabit(string telegramId, Habit habit);
        void DeleteUserHabit(string telegramId, Habit habit);
        List<AppUser> GetHabitUsers(int habitId);
        bool HasUserHabit(string telegramId, Habit habit);
        List<AppUser> AllUsersHasHabit();
    }
}
