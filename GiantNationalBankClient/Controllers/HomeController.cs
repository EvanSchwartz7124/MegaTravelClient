using Microsoft.AspNetCore.Mvc;
using GiantNationalBankClient.Models;
using Newtonsoft.Json;
using GiantNationalBankClient.Utility;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq.Expressions;

namespace GiantNationalBankClient.Controllers
{

    public class HomeController : Controller
    {
        #region Config
        public IConfiguration _configuration;

        public HomeController(IConfiguration config)
        {
            _configuration = config;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }
        #endregion

        #region Registration
        //Registers new user
        [HttpPost]
        public IActionResult Registration(RegistrationModel userData)
        {
            //Checks if the data entered is all valid by comparing to modelState
            if (ModelState.IsValid)
            {
                //If passes, processes form with new userData
                ProcessForm(userData);
                //Returns the view to say completed registration
                return View("Views/Home/ProcessForm.cshtml");
            }
            //Registration validation has failed, throw back to user
            else
            {
                return View();
            }
        }
        #endregion

        #region ProcessForm
        //Processes the regisstration form to validate info
        public async Task<IActionResult> ProcessForm(RegistrationModel userData)
        {
            //If optional data is empty, fills with empty
            if (userData.Street2 == null)
            {
                userData.Street2 = "string";
            }
            //Encrypts users password
            string hashedPassword = Helper.EncryptCredentials(userData.Password);

            userData.Password = hashedPassword;

            //Attempts API call to register user
            try
            {
                //Sets up resposne model
                RegistrationResponseModel responseModel = null;
                //Calls API RegisterUser and passes in the new userData
                var strSerializedData = JsonConvert.SerializeObject(userData);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, ConstantValues.RegisterUser, false, string.Empty).ConfigureAwait(true);
                //Deserializes json reponse data and parses into RegistrationResponseModel
                responseModel = JsonConvert.DeserializeObject<RegistrationResponseModel>(response);
                //If reponseModel parsed in data incorrectly
                if (responseModel == null)
                {
                    //There has been an error
                }
                //If register failed
                else if (responseModel.Status == false)
                {
                    //There has been an error
                }
                //Register success
                else
                {
                    //Return reigstration reponse model
                    return View(responseModel);
                }
            }
            catch(Exception ex)
            {
                //There has been an error
                Console.WriteLine("Registration API " + ex.Message);
            }
            return View();
        }
        #endregion

        #region ProcessLogin
        //Processes the login of user
        public async Task<IActionResult> ProcessLogin(LoginModel userData)
        {
            //Checks to see if data was entered
            if (userData != null)
            {
                Console.WriteLine("Process Login: " + userData);
                //Checks if userType is of User
                if (userData.UserType == "User")
                {
                    //Encrypts Password
                    string hashedPassword = Helper.EncryptCredentials(userData.Password);

                    userData.Password = hashedPassword;
                    //Attempts to call API
                    try
                    {
                        //Sets up response model
                        LoginResponseModel responseModel = null;
                        //Calls API LoginUser and passes in userData
                        var strSerializedData = JsonConvert.SerializeObject(userData);
                        ServiceHelper objService = new ServiceHelper();
                        string response = await objService.PostRequest(strSerializedData, ConstantValues.LoginUser, false, string.Empty).ConfigureAwait(true);
                        //Deserializes json response data into loginresponsemodel
                        responseModel = JsonConvert.DeserializeObject<LoginResponseModel>(response);

                        //Checks if response model parsed correctly
                        if (responseModel == null)
                        {
                            //There has been an error
                        }
                        //Api returned an error
                        else if (responseModel.Status == false)
                        {
                            //There was an error
                            ViewBag.ErrorMessage = "Email or Password is Incorrect";
                            return View("Views/Home/Login.cshtml");
                        }
                        //API login success
                        else
                        {
                            //Encrypts users login and validates login security
                            var claims = new[] {
                                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                                new Claim("UserId", responseModel.UserID.ToString())
                             };

                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                            var token = new JwtSecurityToken(
                                _configuration["Jwt:Issuer"],
                                _configuration["Jwt:Audience"],
                                claims,
                                expires: DateTime.UtcNow.AddMinutes(10),
                                signingCredentials: signIn);

                            //return users homepage view
                            return View("Views/User/Index.cshtml", responseModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        //There has been an error
                        Console.WriteLine("ProcessLogin API " + ex.Message);
                    }
                }
                //Checks if userType entered is Admin
                else if (userData.UserType == "Admin")
                {
                    //Encrypts user password
                    string hashedPassword = Helper.EncryptCredentials(userData.Password);

                    userData.Password = hashedPassword;
                    //Attempts API call
                    try
                    {
                        //Sets up response model
                        LoginResponseModel responseModel = null;
                        //Calls API LoginAdmin and passes login info
                        var strSerializedData = JsonConvert.SerializeObject(userData);
                        ServiceHelper objService = new ServiceHelper();
                        string response = await objService.PostRequest(strSerializedData, ConstantValues.LoginAdmin, false, string.Empty).ConfigureAwait(true);
                        System.Diagnostics.Debug.WriteLine(ConstantValues.LoginAdmin);
                        //Deserializes json repsonse data into loginresponsemodel
                        responseModel = JsonConvert.DeserializeObject<LoginResponseModel>(response);
                        //Checks if parsed correctly
                        if (responseModel == null)
                        {

                        }
                        //Checks if login attempt failed
                        else if (responseModel.Status == false)
                        {
                            //There has been a login error
                            ViewBag.ErrorMessage = "Email or Password Incorrect";
                            return View("Views/Home/Login.cshtml");


                        }
                        //Login Successful
                        else
                        {
                            //Encryts user login and validates security
                            var claims = new[] {
                                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                                new Claim("AdminID", responseModel.AdminData.AdminID.ToString())
                             };

                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                            var token = new JwtSecurityToken(
                                _configuration["Jwt:Issuer"],
                                _configuration["Jwt:Audience"],
                                claims,
                                expires: DateTime.UtcNow.AddMinutes(10),
                                signingCredentials: signIn);
                            //returns admin dashboard
                            return View("Views/Admin/Index.cshtml", responseModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        //There has been an error
                        Console.WriteLine("ProcessLogin API" + ex.Message);
                    }
                }
            }
            
            return View();
        }
        #endregion

    }
}
