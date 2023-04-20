using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiantNationalBankClient.Pages
{
    public class AccountView : PageModel
    {
        private readonly ILogger<UserDashboardModel> _logger;

        public AccountView(ILogger<UserDashboardModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}