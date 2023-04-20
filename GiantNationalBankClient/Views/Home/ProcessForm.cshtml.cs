using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiantNationalBankClient.Pages
{
    public class ProcessForm : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public ProcessForm(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}