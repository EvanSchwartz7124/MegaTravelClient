using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiantNationalBankClient.Pages
{
    public class AdminDashboardModel : PageModel
    {
        private readonly ILogger<UserDashboardModel> _logger;

        public AdminDashboardModel(ILogger<UserDashboardModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}