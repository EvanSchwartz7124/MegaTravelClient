using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiantNationalBankClient.Pages
{
    public class UserDashboardModel : PageModel
    {
        private readonly ILogger<UserDashboardModel> _logger;

        public UserDashboardModel(ILogger<UserDashboardModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}