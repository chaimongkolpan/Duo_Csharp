using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DuoSecurity;
using DuoUniversal;

namespace DuoCSharp.Pages
{
    public class CallbackModel : PageModel
    {
        private readonly IDuoClientProvider _duoClientProvider;

        public string AuthResponse { get; set; }

        public CallbackModel(IDuoClientProvider duoClientProvider)
        {
            _duoClientProvider = duoClientProvider;
        }

        public async Task<IActionResult> OnGet(string state, string code)
        {
            if (string.IsNullOrWhiteSpace(state))
            {
                throw new DuoException("Required state value was empty");
            }
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new DuoException("Required code value was empty");
            }
            Client duoClient = _duoClientProvider.GetDuoClient();
            var sessionState = HttpContext.Session.GetString(IndexModel.STATE_SESSION_KEY);
            var sessionUsername = HttpContext.Session.GetString(IndexModel.USERNAME_SESSION_KEY);
            if (string.IsNullOrEmpty(sessionState) || string.IsNullOrEmpty(sessionUsername))
            {
                throw new DuoException("State or username were missing from your session");
            }
            if (!sessionState.Equals(state))
            {
                throw new DuoException("Session state did not match the expected state");
            }
            HttpContext.Session.Clear();
            IdToken token = await duoClient.ExchangeAuthorizationCodeFor2faResult(code, sessionUsername);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            AuthResponse = JsonSerializer.Serialize(token, options);
            return Page();
        }
    }
}