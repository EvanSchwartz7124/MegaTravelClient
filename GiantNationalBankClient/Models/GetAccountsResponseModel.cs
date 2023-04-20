namespace GiantNationalBankClient.Models
{
    public class GetAccountsResponseModel
    {

        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = null;
        public List<AccountModel> accountList { get; set; }
        public int UserID { get; set; }

    }
}
