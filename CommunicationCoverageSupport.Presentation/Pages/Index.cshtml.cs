using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CommunicationCoverageSupport.Presentation.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public string? JwtToken { get; set; }

        public void OnGet()
        {
            JwtToken = HttpContext.Session.GetString("JwtToken");
        }
    }
}
