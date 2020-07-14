using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;

namespace TenmoServer
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {        
        private static IAccountDAO accountDAO;

        public AccountController(IAccountDAO _accountDAO)
        {            
            accountDAO = _accountDAO;
        }

        [HttpGet("balance")]
        public ActionResult<decimal> GetAccountBalance()
        {
            int currentID = int.Parse(User.FindFirst("sub").Value);
            decimal balance = accountDAO.GetBalance(currentID);
            if (balance >= 0)
            {
                return Ok(balance);
            }
            else return NotFound();
        }

        [HttpGet("{id}")]
        public ActionResult<Account> GetAccount(int id)
        {
            Account account = accountDAO.GetAccount(id);
            if (account != null)
            {
                return Ok(account);
            }
            else return NotFound();
        }
    }
}
