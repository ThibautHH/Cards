﻿using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Cards.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class RegisterModel : PageModel
	{
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly ILogger<RegisterModel> _logger;
		private readonly IEmailSender _emailSender;

		public RegisterModel(
			UserManager<IdentityUser> userManager,
			SignInManager<IdentityUser> signInManager,
			ILogger<RegisterModel> logger,
			IEmailSender emailSender)
		{
			this._userManager = userManager;
			this._signInManager = signInManager;
			this._logger = logger;
			this._emailSender = emailSender;
		}

		[BindProperty]
		public InputModel? Input { get; set; }

		public string? ReturnUrl { get; set; }

		public IList<AuthenticationScheme>? ExternalLogins { get; set; }

		public class InputModel
		{
			[Required]
			[EmailAddress]
			[Display(Name = "Email")]
			public string? Email { get; set; }

			[Required]
			[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
			[DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string? Password { get; set; }

			[DataType(DataType.Password)]
			[Display(Name = "Confirm password")]
			[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
			public string? ConfirmPassword { get; set; }
		}

		public async Task OnGetAsync(string? returnUrl = null)
		{
			this.ReturnUrl = returnUrl;
			this.ExternalLogins = (await this._signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
		}

		public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
		{
			returnUrl ??= this.Url.Content("~/");
			this.ExternalLogins = (await this._signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
			if (this.ModelState.IsValid)
			{
				IdentityUser user = new() { UserName = this.Input!.Email, Email = this.Input.Email };
				IdentityResult result = await this._userManager.CreateAsync(user, this.Input.Password!);
				if (result.Succeeded)
				{
					this._logger.LogInformation("User created a new account with password.");

					string code = await this._userManager.GenerateEmailConfirmationTokenAsync(user);
					code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
					string? callbackUrl = this.Url.Page(
						"/Account/ConfirmEmail",
						pageHandler: null,
						values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
						protocol: this.Request.Scheme);

					await this._emailSender.SendEmailAsync(this.Input.Email!, "Confirm your email",
						$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>clicking here</a>.");

					if (this._userManager.Options.SignIn.RequireConfirmedAccount)
					{
						return this.RedirectToPage("RegisterConfirmation", new { email = this.Input.Email, returnUrl = returnUrl });
					} else
					{
						await this._signInManager.SignInAsync(user, isPersistent: false);
						return this.LocalRedirect(returnUrl);
					}
				}
				foreach (IdentityError error in result.Errors)
				{
					this.ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			// If we got this far, something failed, redisplay form
			return this.Page();
		}
	}
}
