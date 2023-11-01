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

        public List<User> GetListUsers()
        {
            List<User> userList = new List<User>();

            RestRequest request = new RestRequest("transfer");
            IRestResponse<List<User>> response = client.Get<List<User>>(request);

            CheckForError(response);

            return userList;
        }
        // Add methods to call api here...


    }
}
