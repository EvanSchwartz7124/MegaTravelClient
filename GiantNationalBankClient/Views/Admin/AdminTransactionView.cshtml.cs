using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiantNationalBankClient.Pages
{
    public class AdminTransactionView : PageModel
    {
        private readonly ILogger<UserDashboardModel> _logger;

        public AdminTransactionView(ILogger<UserDashboardModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}