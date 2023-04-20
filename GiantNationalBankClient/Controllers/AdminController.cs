using Microsoft.AspNetCore.Mvc;
using GiantNationalBankClient.Models;
using GiantNationalBankClient.Utility;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace GiantNationalBankClient.Controllers
{
    public class AdminController : Controller
    {

        public IActionResult Index(LoginResponseModel userData)
        {
            return View();
        }

        #region AllAccountsView
        //Pulls all accounts into view
        public async Task<IActionResult> AllAccountsView()
        {
            //Set up a reponse model
            GetAccountsResponseModel ResponseModel = null;
            //Attempts to get response
            try
            {
                //Sends API request to GetAllAccountsView 
                var strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.GetRequest(strSerializedData, ConstantValues.GetAllAccountData, false, string.Empty);
                //Deserializes response into the GetAccountsResponseModel for view
                ResponseModel = JsonConvert.DeserializeObject<GetAccountsResponseModel>(response);

            }
            catch (Exception ex)
            {
                //Error with getting data back from API GetAllAccountData
                Console.WriteLine("Get All Account Data API " + ex.Message);
            }
            //Sorts the response model account list by User ID
            ResponseModel.accountList.Sort((a, b) => a.UserID.CompareTo(b.UserID));
            System.Diagnostics.Debug.WriteLine("Sorting List: " + ResponseModel.accountList);
            //Returns sorted GetAccountsReponseModel View
            return View(ResponseModel);
        }
        #endregion

        #region AdminTransactionView
        //Pulls up the transactions view of a spcecific account
        public async Task<IActionResult> AdminTransactionView([FromQuery(Name = "accountID")] int accountID)
        {
            //Sets up a reponse model
            GetAccountResponseModel ResponseModel = null;
            //Attempts to get response
            try
            {
                //Sends API call to GetAccountByID and passses in AccountID
                var strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                System.Diagnostics.Debug.WriteLine(ConstantValues.GetAccountByID + "?accountID=" + accountID);
                string response = await objService.GetRequest(strSerializedData, ConstantValues.GetAccountByID + "?accountID=" + accountID, false, string.Empty);
                System.Diagnostics.Debug.WriteLine(response);
               //Deserializes Json response data into the GetAccountResponseModel
                ResponseModel = JsonConvert.DeserializeObject<GetAccountResponseModel>(response);
            }
            catch (Exception ex)
            {
                //Error in recieving API data
                Console.WriteLine("Get Account By ID API " + ex.Message);
            }

            System.Diagnostics.Debug.WriteLine("For this account: " + accountID);
            System.Diagnostics.Debug.WriteLine(ResponseModel.account.AccountID);
            //return GetAccountResponseModel to view
            return View(ResponseModel);
        }
        #endregion

        #region ChargeAccount
        //Creates a charge to the account being viewed
        public async Task<IActionResult> ChargeAccount(string chargetype, int accountID, decimal amount, string name)
        {
            //Checks if transaction being passed in is of type Debit
            if (chargetype == "Debit")
            {
                //Attempts to call API with transaction data
                try
                {
                    //Sets up response model
                    TransactionResponseModel responseModel = null;
                    //Calls API DebitTransaction and passes in the account ID, charge amount, and name of transaction
                    var strSerializedData = string.Empty;
                    ServiceHelper objService = new ServiceHelper();
                    string response = await objService.PostRequest(strSerializedData, ConstantValues.DebitTransaction + "?accountID=" + accountID + "&amount=" + amount + "&name=" + name, false, string.Empty).ConfigureAwait(true);
                    //Deserializes Json response data into the TransactionResponseModel
                    responseModel = JsonConvert.DeserializeObject<TransactionResponseModel>(response);

                    //Checks if the response model was recieved and data was parsed correctly
                    if (responseModel == null)
                    {
                        //There has been an error
                    }
                    else if (responseModel.Status == false)
                    {
                        //There has been an error
                    }
                    else
                    {
                       //Transaction recieved correctly
                    }

                }
                catch (Exception ex)
                {
                    //Failed API call
                    Console.WriteLine("Debit API " + ex.Message);
                }
            }
            //Checks if transaction being passed is of type credit
            else if (chargetype == "Credit")
            {
                //Attempts to call API to recieve data
                try
                {
                    //Sets up response model
                    TransactionResponseModel responseModel = null;
                    //Calls API CreditTransaction and passes account ID, credit amount, and name of transaction
                    var strSerializedData = string.Empty;
                    ServiceHelper objService = new ServiceHelper();
                    string response = await objService.PostRequest(strSerializedData, ConstantValues.CreditTransaction + "?accountID=" + accountID + "&amount=" + amount + "&name=" + name, false, string.Empty).ConfigureAwait(true);
                    //Deserializes Json response data into the TransactionResponseModel
                    responseModel = JsonConvert.DeserializeObject<TransactionResponseModel>(response);
                    //Checks if data was parsed correctly
                    if (responseModel == null)
                    {
                        //There has been an error
                    }
                    //Checks if api call failed
                    else if (responseModel.Status == false)
                    {
                        //There has been an error
                    }
                    else
                    {
                        //Call succeeded
                    }

                }
                catch (Exception ex)
                {
                    //There has been an error
                    Console.WriteLine("Credit API " + ex.Message);
                }
            }
            //Sets up response model
            GetAccountResponseModel ResponseModel = null;
            //Gets account view back to update transaction list
            try
            {
                //Calls API for GetAccountByID passing in accountID
                var strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                System.Diagnostics.Debug.WriteLine(ConstantValues.GetAccountByID + "?accountID=" + accountID);
                string response = await objService.GetRequest(strSerializedData, ConstantValues.GetAccountByID + "?accountID=" + accountID, false, string.Empty);
                System.Diagnostics.Debug.WriteLine(response);
                //Deserializes json reponse data into GetAccountResponseModel
                ResponseModel = JsonConvert.DeserializeObject<GetAccountResponseModel>(response);
            }
            catch (Exception ex)
            {
                //There has been an error
                Console.WriteLine("Get Account By ID API " + ex.Message);
            }
            //Returns updated Account transactions view 
            return View("AdminTransactionView", ResponseModel);

        }
        #endregion
    }
}
