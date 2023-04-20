namespace GiantNationalBankClient.Models
{
    public class GetUserResponseModel
    {

        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = null;
        public UserData user { get; set; } = null;

    }
}
