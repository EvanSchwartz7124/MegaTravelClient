using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiantNationalBankClient.Pages
{
    public class ViewTransactions : PageModel
    {
        private readonly ILogger<UserDashboardModel> _logger;

        public ViewTransactions(ILogger<UserDashboardModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}