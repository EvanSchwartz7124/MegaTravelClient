using System.ComponentModel.DataAnnotations;

namespace GiantNationalBankClient.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public int AccountID { get; set; }
        public string TransactionName { get; set; } = null!;
        public string TransactionType { get; set; } = null!;
        public decimal Amount { get; set; }
    }
}
