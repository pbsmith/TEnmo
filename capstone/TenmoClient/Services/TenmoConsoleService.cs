using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient.Services
{
    public class TenmoConsoleService : ConsoleService
    {
        private readonly TenmoApiService tenmoApiService;

        public TenmoConsoleService(string apiUrl)
        {
            tenmoApiService = new TenmoApiService(apiUrl);
        }

        /************************************************************
            Print methods
        ************************************************************/
        public void PrintLoginMenu()
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("Welcome to TEnmo!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.WriteLine("0: Exit");
            Console.WriteLine("---------");
        }

        public void PrintMainMenu(string username)
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine($"Hello, {username}!");
            Console.WriteLine("1: View your current balance");
            Console.WriteLine("2: View your past transfers");
            Console.WriteLine("3: View your pending requests");
            Console.WriteLine("4: Send TE bucks");
            Console.WriteLine("5: Request TE bucks");
            Console.WriteLine("6: Log out");
            Console.WriteLine("0: Exit");
            Console.WriteLine("---------");
        }
        public LoginUser PromptForLogin()
        {
            string username = PromptForString("User name");
            if (String.IsNullOrWhiteSpace(username))
            {
                return null;
            }
            string password = PromptForHiddenString("Password");

            LoginUser loginUser = new LoginUser
            {
                Username = username,
                Password = password
            };
            return loginUser;
        }
        public void GetBalance(decimal balance)
        {
            Console.WriteLine($"Your current balance is: ${balance}");
            Pause();
        }
        public string ViewTransfers(List<Transfer> transfers, Account userAccount)
        {

            Console.WriteLine("------------------------------------");
            Console.WriteLine("Transfers");
            Console.WriteLine("ID          From/To          Amount");
            Console.WriteLine("------------------------------------");
            char pad = ' ';
            foreach (Transfer transfer in transfers)
            {
                if (transfer.AccountFrom == userAccount.AccountId)
                {
                    Console.WriteLine(transfer.TransferId.ToString().PadRight(12, pad) + "TO: " + tenmoApiService.GetUserById(transfer.AccountTo - 1000).Username.PadRight(12, pad) + "$" + transfer.Amount.ToString());
                }
                else if (transfer.AccountFrom != userAccount.AccountId && transfer.AccountTo != userAccount.AccountId)
                {
                    //Exits
                }
                else
                {
                    Console.WriteLine(transfer.TransferId.ToString().PadRight(12, pad) + "FROM: " + tenmoApiService.GetUserById(transfer.AccountFrom - 1000).Username.PadRight(10, pad) + "$" + transfer.Amount.ToString());

                }
            }
            Console.WriteLine("------------------------------------");
            Console.WriteLine();
            Console.WriteLine("Please enter transfer ID to view details or enter 0 to cancel: ");

            string enteredId = Console.ReadLine();
            return enteredId;
        }
        public string ListUsers(List<User> users, Account updatedRecipientAccount)
        {

            Console.WriteLine("| --------------Users-------------- |");
            Console.WriteLine("|    Id   |  Username               |");
            Console.WriteLine("| --------------------------------- |");
            char pad = ' ';
            foreach (User user in users)
            {
                Console.WriteLine("| " + user.UserId.ToString().PadRight(6, pad) + "  | " + user.Username.PadRight(22, pad) + "  |");
            }

            Console.WriteLine("| --------------------------------- |");
            Console.WriteLine("Please enter ID of recipient or enter 0 to cancel:");
            string enteredId = Console.ReadLine();
            return enteredId;

        }
        public void DetailsDisplay(string transferId, string fromUser, string toUser, string transferType, string transferStatus, decimal transferAmount)
        {
            Console.Clear();
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("Transfer Details");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine();
            Console.WriteLine("ID: " + transferId);
            Console.WriteLine("FROM: " + fromUser);
            Console.WriteLine("TO: " + toUser);
            Console.WriteLine("TYPE: " + transferType);
            Console.WriteLine("STATUS: " + transferStatus);
            Console.WriteLine("AMOUNT: $" + transferAmount);
            Console.WriteLine();
            Pause();
        }
        public void BadId()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Please enter a valid ID");
            Console.WriteLine("*****************************");
            Console.WriteLine("Press enter to return to menu");
            Console.ReadLine();
        }
        public void BadValue()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Please enter a valid amount");
            Console.WriteLine("*****************************");
            Console.WriteLine("Press enter to return to menu");
            Console.ReadLine();
        }
        public void BeginTransfer(Transfer transfer, Account recipientAccount, Account userAccount, decimal amountToSend)
        {
            transfer.Amount = amountToSend;
            transfer.AccountTo = recipientAccount.AccountId;
            transfer.AccountFrom = userAccount.AccountId;

            transfer.TransferTypeId = 2;
            transfer.TransferStatusId = 2;
            tenmoApiService.CreateTransfer(transfer);
        }

        public bool CheckValidAmount(string input)
        {
            
            decimal amount;
            bool isValid = decimal.TryParse(input,out amount);
            if(amount <= 0)
            {
                isValid = false;
            }
            return isValid;

        }
        // Add application-specific UI methods here...
        public void SendMoney(Transfer transfer,Account updatedUserAccount, Account updatedRecipientAccount)
        {
            Console.WriteLine("Please enter amount you wish to send:");
            string input = Console.ReadLine();

            bool isValid = CheckValidAmount(input);

            if (!isValid)
            {
                BadValue();
            }
            else
            {
                decimal amountToSend = decimal.Parse(input);


                if (updatedUserAccount.Balance - amountToSend < 0)
                {
                    Console.WriteLine("Insufficient funds");
                    Console.ReadLine();
                }
                else if (amountToSend == 0 || amountToSend < 0)
                {
                    Console.WriteLine("Please enter a valid amount");
                    Console.ReadLine();
                }
                else
                {
                    BeginTransfer(transfer, updatedRecipientAccount, updatedUserAccount, amountToSend);
                }
            }
        }

    }
}
