using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient
{
    public class API_Transfer
    {
        public int TransferID { get; set; }
        public int TransferTypeID { get; set; }
        public string TransferTypeDescription { get; set; }
        public int TransferStatusID { get; set; }
        public string TransferStatusDescription { get; set; }
        public int UserFromID { get; set; }
        public string UsernameFrom { get; set; }
        public int UserToID { get; set; }
        public string UsernameTo { get; set; }
        public decimal TransferAmount { get; set; }
    }
}
