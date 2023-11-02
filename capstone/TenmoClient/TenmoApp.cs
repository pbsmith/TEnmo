using System;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoClient.Services;

namespace TenmoClient
{
    public class TenmoApp
    {
        private readonly TenmoConsoleService console = new TenmoConsoleService();
        private readonly TenmoApiService tenmoApiService;


        public TenmoApp(string apiUrl)
        {
            tenmoApiService = new TenmoApiService(apiUrl);
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

                decimal balance = tenmoApiService.GetAccountBalance();
                Console.WriteLine($"Your current balance is: ${balance}");
                console.Pause();
            }

            if (menuSelection == 2)
            {
                // View your past transfers
            }

            if (menuSelection == 3)
            {
                // View your pending requests
            }

            if (menuSelection == 4)
            {
                List<User> users = tenmoApiService.GetListUsers();
                Account updatedRecipientAccount = new Account();
                Account updatedUserAccount = tenmoApiService.GetAccount();

                Console.WriteLine("| --------------Users-------------- |");
                Console.WriteLine("|    Id   |  Username               |");
                Console.WriteLine("| --------------------------------- |");
                char pad = ' ';
                foreach (User user in users)
                {
                    Console.WriteLine("| " + user.UserId.ToString().PadRight(6, pad) + "  | " + user.Username.PadRight(22, pad) + "  |");
                }

                Console.WriteLine("| --------------------------------- |");
                Console.WriteLine("Please enter ID of recipient:");
                string enteredId = Console.ReadLine();
                int userId;
                bool success = int.TryParse(enteredId, out userId);
                if (!success)
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
                else
                {
                    updatedRecipientAccount = tenmoApiService.GetAccountByUserId(userId);

                    if (updatedRecipientAccount.AccountId == 0 || updatedRecipientAccount.AccountId == updatedUserAccount.AccountId)
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
                    else
                    {

                        Console.WriteLine("Please enter amount you wish to send:");
                        decimal amountToSend = decimal.Parse(Console.ReadLine());


                        if (updatedUserAccount.Balance - amountToSend < 0)
                        {
                            Console.WriteLine("Insuffiecient funds");
                            Console.ReadLine();
                        }
                        else if (amountToSend == null || amountToSend == 0 || amountToSend < 0)
                        {
                            Console.WriteLine("Please enter a valid amount");
                            Console.ReadLine();
                        }
                        else
                        {
                            updatedUserAccount.Balance -= amountToSend;
                            updatedRecipientAccount.Balance += amountToSend;
                            tenmoApiService.UpdateAccount(updatedUserAccount);
                            tenmoApiService.UpdateAccount(updatedRecipientAccount);
                        }
                    }
                }
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
    }
}
