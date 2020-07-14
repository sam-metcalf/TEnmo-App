using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using RestSharp.Authenticators;
using TenmoClient.Data;

namespace TenmoClient
{
    class APIService
    {
        private readonly string API_URL = "";
        private readonly RestClient client = new RestClient();
        //private API_User user = new API_User();

        public APIService(string api_url)
        {
            API_URL = api_url;
        }
        public API_Account GetAccount(int id)
        {
            RestRequest restRequest = new RestRequest($"{API_URL}/account/{id}");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<API_Account> response = client.Get<API_Account>(restRequest);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }
            return null;
        }
        public decimal GetBalance()
        {            
            decimal balance = 0;
            RestRequest restRequest = new RestRequest(API_URL + "/account/balance");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());            
            IRestResponse<decimal> response = client.Get<decimal>(restRequest);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                balance = response.Data;
                return balance;
            }
            return balance;

        }
        public API_Transfer DoTransfer(API_Transfer transfer)
        {
            RestRequest restRequest = new RestRequest(API_URL + "/transfer");
            restRequest.AddJsonBody(transfer);
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<API_Transfer> response = client.Post<API_Transfer>(restRequest);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }
            return null;
        }
        public API_Transfer UpdateBalance(API_Transfer transfer)
        {            
            RestRequest restRequest = new RestRequest($"{API_URL}/transfer");
            restRequest.AddJsonBody(transfer);
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<API_Transfer> response = client.Put<API_Transfer>(restRequest);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }
            return null;
        }
        public List<API_User> ListUsers()
        {
            RestRequest request = new RestRequest(API_URL + "/users");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<List<API_User>> response = client.Get<List<API_User>>(request);
            

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }
            return null;
        }
        public List<API_Transfer> ListTransfers()
        {
            RestRequest request = new RestRequest(API_URL + "/transfer");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<List<API_Transfer>> response = client.Get<List<API_Transfer>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }
            return null;
        }
        public API_Transfer GetTransfer(int id)
        {
            RestRequest restRequest = new RestRequest($"{API_URL}/transfer/{id}");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<API_Transfer> response = client.Get<API_Transfer>(restRequest);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }
            return null;
        }
        private void ProcessErrorResponse(IRestResponse response)
        {
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("Error occurred - unable to reach server.");
            }
            else if (!response.IsSuccessful)
            {
                Console.WriteLine("Error occurred - received non-success response: " + (int)response.StatusCode);
            }
        }

    }
}
