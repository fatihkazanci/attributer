using BusinessLibrary.Models;

namespace BusinessLibrary.Interface
{
    public interface IUserBusiness
    {
        BusinessResult<User> GetUser(int id);
    }
}