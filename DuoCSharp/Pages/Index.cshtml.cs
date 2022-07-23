using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DuoSecurity;
using DuoUniversal;

namespace DuoCSharp.Pages
{
    public class IndexModel : PageModel
    {
        internal const string STATE_SESSION_KEY = "_State";
        internal const string USERNAME_SESSION_KEY = "_Username";
        private readonly IDuoClientProvider _duoClientProvider;
        public IndexModel(IDuoClientProvider duoClientProvider)
        {
            _duoClientProvider = duoClientProvider;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost(string username)
        {
            Client duoClient = _duoClientProvider.GetDuoClient();
            var isDuoHealthy = await duoClient.DoHealthCheck();
            string state = Client.GenerateState();
            HttpContext.Session.SetString(STATE_SESSION_KEY, state);
            HttpContext.Session.SetString(USERNAME_SESSION_KEY, username);
            string promptUri = duoClient.GenerateAuthUri(username, state);
            return new RedirectResult(promptUri);
        }
    }
}
