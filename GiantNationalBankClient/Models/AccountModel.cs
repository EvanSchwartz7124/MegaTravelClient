using System.ComponentModel.DataAnnotations;

namespace GiantNationalBankClient.Models
{
    public class AccountModel
    {
        public int AccountID { get; set; }
        
        public int UserID { get; set; }
        
        public decimal Balance { get; set; }
        public UserData User { get; set; } = null!;

        public List<Transaction> transactionList { get; set; }
    }
}
