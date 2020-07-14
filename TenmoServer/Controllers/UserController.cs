using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer
{
    [Route("/")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private static IUserDAO userDAO;

        public UserController (IUserDAO _userDAO)
        {
            userDAO = _userDAO;
        }

        [HttpGet("users")]
        public List<User> GetUsersList()
        {
            return userDAO.GetUsers();         
        }
    }
}
