namespace GiantNationalBankClient.Models
{
    public class CreateAccountResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = null;
        public AccountModel account { get; set; } = null;
    }
}
