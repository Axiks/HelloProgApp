using HelloProgLib.Models;
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
        User GetUser(string telegramId);
        void DeleteUser(User user);
        void SetAbout(User user, string body);
    }
}
