using BusinessLibrary.Interface;
using BusinessLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;

        public UserController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        [HttpGet("get-user")]
        public BusinessResult<User> Get(int id)
        {
            return _userBusiness.GetUser(id);
        }
    }
}
