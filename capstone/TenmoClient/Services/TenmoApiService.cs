using RestSharp;
using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient.Services
{
    public class TenmoApiService : AuthenticatedApiService
    {
        public readonly string ApiUrl;
       

        public TenmoApiService(string apiUrl) : base(apiUrl) { }


        public decimal GetAccountBalance()
        {
            RestRequest request = new RestRequest("account");

            IRestResponse<Account> response = client.Get<Account>(request);

            CheckForError(response);

            return response.Data.Balance;

        }

        public Account GetAccount()
        {
            RestRequest request = new RestRequest("account");

            IRestResponse<Account> response = client.Get<Account>(request);

            CheckForError(response);

            return response.Data;

        }

        public List<User> GetListUsers()
        {
            //List<User> userList = new List<User>();

            RestRequest request = new RestRequest("user");
            IRestResponse<List<User>> response = client.Get<List<User>>(request);

            CheckForError(response);

            return response.Data;
        }
        // Add methods to call api here...

        public Account UpdateAccount(Account account)
        {
            RestRequest request = new RestRequest($"transfer/{account.UserId}");
            request.AddJsonBody(account);
            IRestResponse<Account> response = client.Put<Account>(request);

            CheckForError(response);

            return response.Data;
        }

        public Account GetAccountByUserId(int id)
        {
            RestRequest request = new RestRequest($"account/{id}");
            IRestResponse<Account> response = client.Get<Account>(request);

            CheckForError(response);
            return response.Data;
        }

    }
}
