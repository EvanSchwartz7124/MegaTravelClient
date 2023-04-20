namespace GiantNationalBankClient.Models
{
    public class RegistrationResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = null;
        public UserData Data { get; set; } = null;
    }
}
