using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;

namespace TenmoServer
{
    [Route("/transfer")]
    [ApiController]
    [Authorize]
    public class TransferController : ControllerBase
    {
        private static IUserDAO userDAO;
        private static IAccountDAO accountDAO;
        private static ITransferDAO transferDAO;

        public TransferController(IUserDAO _userDAO, IAccountDAO _accountDAO, ITransferDAO _transferDAO)
        {
            userDAO = _userDAO;
            accountDAO = _accountDAO;
            transferDAO = _transferDAO;
        }

        [HttpPost]
        public ActionResult<Transfer> InsertTransfer(Transfer transferToInsert)
        {
            Transfer transfer = transferDAO.InsertTransfer(transferToInsert);
            if (transfer != null)
            {
                return Ok(transfer);
            }
            else return NotFound();

        }

        [HttpGet]
        public List<Transfer> ListTransfers()
        {
            return transferDAO.ListTransfers();
        }

        [HttpGet("{id}")]
        public ActionResult<Transfer> GetTransfer(int id)
        {
            Transfer transfer = transferDAO.GetTransfer(id);
            if (transfer != null)
            {
                return Ok(transfer);
            }
            else return NotFound();
        }

        [HttpPut]
        public ActionResult<Transfer> UpdateBalance(Transfer transfer)
        {
            if (transfer == null)
            {
                return NotFound();
            }
            Transfer verifiedTransfer = transferDAO.GetTransfer(transfer.TransferID);
            if (verifiedTransfer.TransferStatusID == 2) //approved status ID
            {
                transferDAO.UpdateBalance(verifiedTransfer);
                return Ok(transfer);
            }
            else return BadRequest();

        }


    }
}
