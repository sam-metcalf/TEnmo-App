using System;
using System.Collections.Generic;
using System.Linq;
using TenmoClient.Data;

namespace TenmoClient
{
    public class ConsoleService
    {
        /// <summary>
        /// Prompts for transfer ID to view, approve, or reject
        /// </summary>
        /// <param name="action">String to print in prompt. Expected values are "Approve" or "Reject" or "View"</param>
        /// <returns>ID of transfers to view, approve, or reject</returns>
        public int PromptForTransferID(string action)
        {
            Console.WriteLine("");
            Console.Write("Please enter transfer ID to " + action + " (0 to cancel): ");
            if (!int.TryParse(Console.ReadLine(), out int transferID))
            {
                Console.WriteLine("Invalid input. Only input a number.");
                return 0;
            }
            else
            {
                return transferID;
            }
        }

        public LoginUser PromptForLogin()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            string password = GetPasswordFromConsole("Password: ");

            LoginUser loginUser = new LoginUser
            {
                Username = username,
                Password = password
            };
            return loginUser;
        }

        private string GetPasswordFromConsole(string displayMessage)
        {
            string pass = "";
            Console.Write(displayMessage);
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (!char.IsControl(key.KeyChar))
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Remove(pass.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine("");
            return pass;
        }

        public API_Transfer StartTransfer(List<API_User> users)
        {
            int selection = -1;
            while (selection != 0)
            {
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("Users");
                Console.WriteLine("ID\t \tName");
                Console.WriteLine("-------------------------------------");
                foreach (API_User user in users)
                {
                    Console.WriteLine($"{user.UserId}:\t \t{user.Username}");
                }
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("");
                API_Transfer transfer = new API_Transfer()
                {
                    UserFromID = UserService.GetUserId(),
                    UserToID = UserToReceiveTransfer(users),
                    TransferStatusID = 2, //Sending transfers default to approved and sending.
                    TransferTypeID = 2,
                    TransferAmount = AmountToTransfer()
                };
                return transfer;
            }
            return null;
        }
        public void WriteTransferList(List<API_Transfer> transfers)
        {
            int selection = -1;
            while (selection != 0)
            {
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("Transfers");
                Console.WriteLine("ID\t \tFrom/To\t \tAmount");
                Console.WriteLine("-------------------------------------");
                foreach (API_Transfer transfer in transfers)
                {
                    if (transfer.UserFromID == UserService.GetUserId())
                    {
                        Console.WriteLine($"{transfer.TransferID}\t \tTo: {transfer.UsernameTo}\t${transfer.TransferAmount}\r\n");
                    }
                    else if (transfer.UserFromID != UserService.GetUserId())
                    {
                        Console.WriteLine($"{transfer.TransferID}\t \tFrom: {transfer.UsernameFrom}\t${transfer.TransferAmount}\r\n");
                    }
                }
                selection = 0;
            }
        }
        public int TransferToDetail(List<API_Transfer> transfers)
        {
            int inputID = -1;
            bool doneChoosingID = false;
            while (!doneChoosingID)
            {
                Console.WriteLine("Please enter transfer ID to view details (0 to cancel): ");
                if (!int.TryParse(Console.ReadLine(), out inputID))
                {
                    Console.WriteLine("Invalid input. Only input a number.");
                    continue;
                }
                if (!transfers.Any((u) => { return u.TransferID == inputID; }))
                {
                    Console.WriteLine("");
                    Console.WriteLine("Could not find a user with that ID");
                    Console.WriteLine("");
                    continue;
                }
                doneChoosingID = true;
            }
            return inputID;
        }
        public void GetTransferDetails(API_Transfer transfer)
        {
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("Transfer Details");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("");
            Console.WriteLine($"ID: {transfer.TransferID}");
            Console.WriteLine($"From: {transfer.UsernameFrom}");
            Console.WriteLine($"To: {transfer.UsernameTo}");
            Console.WriteLine($"Type: {transfer.TransferTypeDescription}");
            Console.WriteLine($"Status: {transfer.TransferStatusDescription}");
            Console.WriteLine($"Amount: ${transfer.TransferAmount}");
        }
        public int UserToReceiveTransfer(List<API_User> users)
        {
            int inputID = -1;
            bool doneChoosingID = false;
            while (!doneChoosingID)
            {
                Console.WriteLine("Enter ID of user you are sending to (0 to cancel):");

                if (!int.TryParse(Console.ReadLine(), out inputID))
                {
                    Console.WriteLine("Invalid input. Only input a number.");
                    continue;

                }
                if (inputID == 0)
                {
                    doneChoosingID = true;
                    break;
                }

                if (!users.Any((u) => { return u.UserId == inputID; }))
                {
                    Console.WriteLine("");
                    Console.WriteLine("Could not find a user with that ID");
                    Console.WriteLine("");
                    continue;
                }
                doneChoosingID = true;
            }
            return inputID;
        }
        public decimal AmountToTransfer()
        {
            decimal inputAmount = -1;
            bool doneChoosingAmount = false;
            while (!doneChoosingAmount)
            {
                Console.WriteLine("Enter Amount:");
                if (!decimal.TryParse(Console.ReadLine(), out inputAmount))
                {
                    Console.WriteLine("Invalid input. Only input a valid amount.");
                    continue;
                }
                if (inputAmount <= 0)
                {
                    Console.WriteLine("Invalid input. Only input a positive amount.");
                    continue;
                }
                doneChoosingAmount = true;
            }
            return inputAmount;
        }
        public API_Transfer RequestTransfer(List<API_User> users)
        {
            int selection = -1;
            while (selection != 0)
            {
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("Users");
                Console.WriteLine("ID\t \tName");
                Console.WriteLine("-------------------------------------");
                foreach (API_User user in users)
                {
                    Console.WriteLine($"{user.UserId}:\t \t{user.Username}");
                }
                API_Transfer transfer = new API_Transfer()
                {
                    UserFromID = RequestedUserForTransfer(users),
                    UserToID = UserService.GetUserId(),
                    TransferAmount = AmountToTransfer(),
                    TransferStatusID = 1, //Request transfers default to pending untill approved
                    TransferTypeID = 1
                };
                return transfer;
            }
            return null;
        }
        public int RequestedUserForTransfer(List<API_User> users)
        {
            int inputID = -1;
            bool doneChoosingID = false;
            while (!doneChoosingID)
            {
                Console.WriteLine("Enter ID of user you are requesting from (0 to cancel):");

                if (!int.TryParse(Console.ReadLine(), out inputID))
                {
                    Console.WriteLine("Invalid input. Only input a number.");
                    continue;

                }
                if (inputID == 0)
                {
                    doneChoosingID = true;
                    break;
                }

                if (!users.Any((u) => { return u.UserId == inputID; }))
                {
                    Console.WriteLine("");
                    Console.WriteLine("Could not find a user with that ID");
                    Console.WriteLine("");
                    continue;
                }
                doneChoosingID = true;
            }
            return inputID;
        }
        public void WritePendingTransferList(List<API_Transfer> transfers)
        {
            int selection = -1;
            while (selection != 0)
            {
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("Transfers");
                Console.WriteLine("ID\t \tFrom/To\t \tAmount");
                Console.WriteLine("-------------------------------------");
                foreach (API_Transfer transfer in transfers)
                {
                    if (transfer.TransferStatusID == 1)
                    {
                        Console.WriteLine($"{transfer.TransferID}\t \tTo: {transfer.UsernameTo}\t \t${transfer.TransferAmount}\r\n");
                    }
                }
                selection = 0;
            }
        }
       /* public int UpdateTransferStatus(int transferID)
        {
            Console.WriteLine("Please enter transfer ID to approve/reject (0 to cancel): ");


        }*/
    }
}
