// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using TechElite.Areas.Identity.Data;

namespace TechElite.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResendEmailConfirmationModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Justerad för att exkludera bekräftelse via mail. 

        [BindProperty]
        public InputModel Input { get; set; }

        public string Email { get; set; }
        public bool DisplayConfirmAccountLink { get; set; }
        public string EmailConfirmationUrl { get; set; }
        public string UserId { get; set; }
        public string Code { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Bekräftelse skickad.");
                return Page();
            }

            UserId = await _userManager.GetUserIdAsync(user);
            Code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            Code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(Code));
            EmailConfirmationUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = UserId, code = Code },
                protocol: Request.Scheme);

            Email = Input.Email;
            DisplayConfirmAccountLink = true;

            return Page();
        }
    }
}
