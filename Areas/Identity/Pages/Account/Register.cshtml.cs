// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace LUXLocks_projekt.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "E-postadress är obligatorisk.")]
            [EmailAddress(ErrorMessage = "Ange en giltig e-postadress.")]
            [Display(Name = "E-postadress")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Lösenord är obligatoriskt.")]
            [StringLength(100, ErrorMessage = "Lösenordet måste vara minst {2} och max {1} tecken långt.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Lösenord")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Bekräfta lösenord")]
            [Compare("Password", ErrorMessage = "Lösenordet och bekräftelselösenordet matchar inte.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/Identity/Account/Login");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = CreateUser();
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Användaren skapade ett nytt konto med lösenord.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Bekräfta din e-post",
                        $"Bekräfta ditt konto genom att <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>klicka här</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        return RedirectToPage("/Account/Login");
                    }
                }

                // Anpassad felhantering
                foreach (var error in result.Errors)
                {
                    if (error.Code == "PasswordRequiresNonAlphanumeric")
                    {
                        ModelState.AddModelError(string.Empty, "Lösenordet måste innehålla minst ett specialtecken (t.ex. @, #, !).");
                    }
                    else if (error.Code == "PasswordRequiresUpper")
                    {
                        ModelState.AddModelError(string.Empty, "Lösenordet måste innehålla minst en stor bokstav (A-Z).");
                    }
                    else if (error.Code == "PasswordRequiresDigit")
                    {
                        ModelState.AddModelError(string.Empty, "Lösenordet måste innehålla minst en siffra (0-9).");
                    }
                    else if (error.Code == "PasswordTooShort")
                    {
                        ModelState.AddModelError(string.Empty, "Lösenordet måste vara minst 6 tecken långt.");
                    }
                    else if (error.Code == "DuplicateUserName")
                    {
                        ModelState.AddModelError(string.Empty, "Denna e-postadress är redan registrerad.");
                    }
                    else if (error.Code == "InvalidEmail")
                    {
                        ModelState.AddModelError(string.Empty, "Ange en giltig e-postadress.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, error.Description); // Andra fel
                    }
                }
            }

            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Kan inte skapa en instans av '{nameof(IdentityUser)}'. " +
                    $"Se till att '{nameof(IdentityUser)}' inte är en abstrakt klass och har en parameterlös konstruktor, " +
                    $"eller alternativt, åsidosätt registreringssidan i /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("Den inbyggda UI:n kräver att användarbutiken har stöd för e-post.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}