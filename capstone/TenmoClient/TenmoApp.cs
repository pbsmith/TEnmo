using System;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoClient.Services;


namespace TenmoClient
{
    public class TenmoApp
    {
        private readonly TenmoConsoleService console;
        private readonly TenmoApiService tenmoApiService;


        public TenmoApp(string apiUrl)
        {
            tenmoApiService = new TenmoApiService(apiUrl);
            console = new TenmoConsoleService(apiUrl);
        }

        public void Run()
        {
            bool keepGoing = true;
            while (keepGoing)
            {
                // The menu changes depending on whether the user is logged in or not
                if (tenmoApiService.IsLoggedIn)
                {
                    keepGoing = RunAuthenticated();
                }
                else // User is not yet logged in
                {
                    keepGoing = RunUnauthenticated();
                }
            }
        }

        private bool RunUnauthenticated()
        {
            console.PrintLoginMenu();
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 2, 1);
            while (true)
            {
                if (menuSelection == 0)
                {
                    return false;   // Exit the main menu loop
                }

                if (menuSelection == 1)
                {
                    // Log in
                    Login();
                    return true;    // Keep the main menu loop going
                }

                if (menuSelection == 2)
                {
                    // Register a new user
                    Register();
                    return true;    // Keep the main menu loop going
                }
                console.PrintError("Invalid selection. Please choose an option.");
                console.Pause();
            }
        }

        private bool RunAuthenticated()
        {
            console.PrintMainMenu(tenmoApiService.Username);
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 6);
            if (menuSelection == 0)
            {
                // Exit the loop
                return false;
            }

            if (menuSelection == 1)
            {
                GetBalance();
            }

            if (menuSelection == 2)
            {
                ViewTransfers();
            }

            if (menuSelection == 3)
            {
                // View your pending requests
            }

            if (menuSelection == 4)
            {
                ViewUsers();
            }

            if (menuSelection == 5)
            {
                // Request TE bucks
            }

            if (menuSelection == 6)
            {
                // Log out
                tenmoApiService.Logout();
                console.PrintSuccess("You are now logged out");
            }

            return true;    // Keep the main menu loop going
        }

        private void Login()
        {
            LoginUser loginUser = console.PromptForLogin();
            if (loginUser == null)
            {
                return;
            }

            try
            {
                ApiUser user = tenmoApiService.Login(loginUser);
                if (user == null)
                {
                    console.PrintError("Login failed.");
                }
                else
                {
                    console.PrintSuccess("You are now logged in");
                }
            }
            catch (Exception)
            {
                console.PrintError("Login failed.");
            }
            console.Pause();
        }

        private void Register()
        {
            LoginUser registerUser = console.PromptForLogin();
            if (registerUser == null)
            {
                return;
            }
            try
            {
                bool isRegistered = tenmoApiService.Register(registerUser);
                if (isRegistered)
                {
                    console.PrintSuccess("Registration was successful. Please log in.");
                }
                else
                {
                    console.PrintError("Registration was unsuccessful.");
                }
            }
            catch (Exception)
            {
                console.PrintError("Registration was unsuccessful.");
            }
            console.Pause();
        }
        public void GetBalance()
        {
            decimal balance = tenmoApiService.GetAccountBalance();
            console.GetBalance(balance);
        }
        public void ViewTransfers()
        {
            List<Transfer> transfers = tenmoApiService.ListTransfers();
            Account userAccount = tenmoApiService.GetAccount();
            Transfer currentTransfer = new Transfer();

            string enteredId = console.ViewTransfers(transfers, userAccount);
            int transferId;
            bool success = int.TryParse(enteredId, out transferId);
            if (!success)
            {
                console.BadId();
            }
            else if (transferId == 0)
            {
                //Exit loop
            }
            else
            {
                currentTransfer = tenmoApiService.GetTransferDetails(transferId);
                if (currentTransfer.TransferId == 0)
                {
                    console.BadId();
                }
                else
                {
                    TransferDetails(currentTransfer);
                }
            }
        }
        public void TransferDetails(Transfer currentTransfer)
        {

            string transferId = currentTransfer.TransferId.ToString();
            string fromUser = tenmoApiService.GetUserById(currentTransfer.AccountFrom - 1000).Username;
            string toUser = tenmoApiService.GetUserById(currentTransfer.AccountTo - 1000).Username;
            string transferType = currentTransfer.TransferType;
            string transferStatus = currentTransfer.TransferStatus;
            decimal transferAmount = currentTransfer.Amount;

            console.DetailsDisplay(transferId, fromUser, toUser, transferType, transferStatus, transferAmount);
        }

        public void ViewUsers()
        {
            List<User> users = tenmoApiService.GetListUsers();
            Account updatedRecipientAccount = new Account();
            Account updatedUserAccount = tenmoApiService.GetAccount();

            Transfer transfer = new Transfer();

            string enteredId = console.ListUsers(users, updatedRecipientAccount);

            int userId;
            bool success = int.TryParse(enteredId, out userId);
            if (!success)
            {
                console.BadId();
            }
            else if (userId == 0)
            {
                //Exit loop
            }
            else
            {
                updatedRecipientAccount = tenmoApiService.GetAccountByUserId(userId);

                if (updatedRecipientAccount.AccountId == 0 || updatedRecipientAccount.AccountId == updatedUserAccount.AccountId)
                {
                    console.BadId();
                }
                else
                {
                    console.SendMoney(transfer, updatedUserAccount, updatedRecipientAccount);
                }    
            }
        }
    }
}
