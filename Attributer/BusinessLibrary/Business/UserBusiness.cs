using BusinessLibrary.Attribute;
using BusinessLibrary.Interface;
using BusinessLibrary.Models;
using Microsoft.AspNetCore.Http;

namespace BusinessLibrary.Business
{
    [Log]
    public class UserBusiness : IUserBusiness
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserBusiness(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public BusinessResult<User> GetUser(int id)
        {
            return CustomPrivateMethod(id);
        }

        private BusinessResult<User> CustomPrivateMethod(int id)
        {
            return new BusinessResult<User>()
            {
                IsSuccess = true,
                ReturnObject = new User()
                {
                    Id = id,
                    Name = "Fatih",
                    Surname = "Kazancı",
                    UserAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"]
                }
            };
        }
    }
}
