using KonatsuWebApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloProgLib.Interfaces
{
    public interface IUserService : IDisposable
    {
        void AddUser(string telegramId, string? username);
        AppUser GetUser(string telegramId);
        void DeleteUser(AppUser user);
        void SetAbout(AppUser user, string body);
    }
}
