using QuartierLatin.Backend.Application.ApplicationCore.Models;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories
{
    public interface IPasswordAuthRepository<TUser> where TUser : IHaveId, IHavePasswordAuth
    {
        public TUser GetById(int id);
        public TUser FindByLogin(string login);
    }
}