using Microsoft.AspNetCore.Mvc;
using GiantNationalBankClient.Models;
using Microsoft.AspNetCore.Authorization;
using GiantNationalBankClient.Utility;
using Newtonsoft.Json;

namespace GiantNationalBankClient.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index(LoginResponseModel userData)
        {
            return View();
        }

        #region Edit
        public async Task<ActionResult> Edit(int id)
        {
            //Attempts API call
            try
            {
                //Sets up userData and Response Model
                UserData currentUser = null;

                GetUserResponseModel responseModel = null;

                //Calls API GetUserByID and passes in UserID
                var strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.GetRequest(strSerializedData, ConstantValues.GetUserByID, false, string.Empty).ConfigureAwait(true);
                responseModel = JsonConvert.DeserializeObject<GetUserResponseModel>(response);

                currentUser = responseModel.user;
                return View(currentUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get User Data " + ex.Message);
            }
            return View();
        }
        #endregion

        #region UpdateUser
        //Updates user profile info
        [HttpPost]
        public async Task<ActionResult> UpdateUser(UserData updatedUser)
        {
            //Sets up responseModel
            LoginResponseModel responseModel = null;
            try
            {
                //Calls API SaveUserRecord
                var strSerializedData = JsonConvert.SerializeObject(updatedUser);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, ConstantValues.SaveUserRecord, false, string.Empty).ConfigureAwait(true);
                //Deserializes json response data into LoginResponseModel
                responseModel = JsonConvert.DeserializeObject<LoginResponseModel>(response);

                //If response model parsed correctly
                if (responseModel == null)
                {
                    //There has been an error
                }
                //If Save user record failed
                else if (responseModel.Status == false)
                {
                    //There has been an error
                }
                //Successful
                else
                {
                    //return updated user
                    return View("Index", responseModel);
                }

            }
            catch (Exception ex)
            {
                //There has been an error
                Console.WriteLine("Save User API " + ex.Message);
            }
            //returns user dashboard
            return View("Index", responseModel);
        }
        #endregion

        #region AllUserAccountsView
        //Views all accounts by the current user
        public async Task<IActionResult> AllUserAccountsView([FromQuery(Name = "userID")] int userID)
        {
            //Sets up response model
            GetAccountsResponseModel ResponseModel = null;
            //Attempts API call
            try
            {
                //Calls API GetAccountByUser and passes in userID
                var strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.GetRequest(strSerializedData, ConstantValues.GetAccountByUser + "?userID=" + userID, false, string.Empty);
                //Deserializes json resposne data into GetAccountsResponseModel
                ResponseModel = JsonConvert.DeserializeObject<GetAccountsResponseModel>(response);
                //Also assigns userID to resposneModel
                ResponseModel.UserID = userID;
            }
            catch (Exception ex)
            {
                //There has been an error
                Console.WriteLine("GetUserAccounts API " + ex.Message);
            }
            //Return User Accounts view
            return View(ResponseModel);
        }
        #endregion

        #region ViewTransactions
        //Goes to the account transaction view for the users account
        public async Task<IActionResult> ViewTransactions([FromQuery(Name = "accountID")] int accountID)
        {
            //Sets up Response model
            GetAccountResponseModel ResponseModel = null;
            try
            {
                //Api call to GetAccountById and passes in account id
                var strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                System.Diagnostics.Debug.WriteLine(ConstantValues.GetAccountByID + "?accountID=" + accountID);
                string response = await objService.GetRequest(strSerializedData, ConstantValues.GetAccountByID + "?accountID=" + accountID, false, string.Empty);
                
                System.Diagnostics.Debug.WriteLine(response);
                //Deserializes json response data into GetAccountResponseModel
                ResponseModel = JsonConvert.DeserializeObject<GetAccountResponseModel>(response);
            }
            catch (Exception ex)
            {
                //There has been an error
                Console.WriteLine("Get Account By ID API " + ex.Message);
            }

            System.Diagnostics.Debug.WriteLine("For this account: " + accountID);
            System.Diagnostics.Debug.WriteLine(ResponseModel.account.AccountID);
            //Returns view for account
            return View(ResponseModel);
        }
        #endregion

        #region CreateAccount
        //Creates an account for current user
        public async Task<ActionResult> CreateAccount([FromQuery(Name = "userID")]int userID)
        {
            //Sets up response model
            CreateAccountResponseModel responseModel = null;
            try
            {
                //Calls API createnewaccount and passes in the userID
                var strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, ConstantValues.CreateNewAccount + "?userID=" + userID, false, string.Empty);
                responseModel = JsonConvert.DeserializeObject<CreateAccountResponseModel>(response);
                System.Diagnostics.Debug.WriteLine(response);
                
            }
            catch (Exception ex)
            {
                //There has been an error
                Console.WriteLine("Create New Account API " + ex.Message);
            }

            //Sets up response model
            GetAccountsResponseModel ResponseModel = null;
            try
            {
                //Calls API GetAccounts by user and passes in user ID to pull in the all accounts by user view
                var strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.GetRequest(strSerializedData, ConstantValues.GetAccountByUser + "?userID=" + userID, false, string.Empty);
                //Deserializes json resposne data into GetAccountsresponsemodel
                ResponseModel = JsonConvert.DeserializeObject<GetAccountsResponseModel>(response);
                ResponseModel.UserID = userID;
            }
            catch (Exception ex)
            {
                //There has been an error
                Console.WriteLine("GetUserAccounts API " + ex.Message);
            }
            //Returns the users All accounts view
            return View("AllUserAccountsView", ResponseModel);
        }
        #endregion
    }
}
