using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cards.Areas.Identity.Pages.Account.Manage
{
	public partial class IndexModel : PageModel
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;

		public IndexModel(
			UserManager<IdentityUser> userManager,
			SignInManager<IdentityUser> signInManager)
		{
			this._userManager = userManager;
			this._signInManager = signInManager;
		}

		[TempData]
		public string? StatusMessage { get; set; }

		[BindProperty]
		public InputModel? Input { get; set; }

		public class InputModel
		{
			[Display(Name = "Username")]
			public string? Username { get; set; }

			[Phone]
			[Display(Name = "Phone number")]
			public string? PhoneNumber { get; set; }
		}

		private async Task LoadAsync(IdentityUser user)
		{
			string userName = await this._userManager.GetUserNameAsync(user);
			string phoneNumber = await this._userManager.GetPhoneNumberAsync(user);

			this.Input = new InputModel
			{
				Username = userName,
				PhoneNumber = phoneNumber
			};
		}

		public async Task<IActionResult> OnGetAsync()
		{
			IdentityUser user = await this._userManager.GetUserAsync(this.User);
			if (user == null)
			{
				return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
			}

			await this.LoadAsync(user);
			return this.Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			IdentityUser user = await this._userManager.GetUserAsync(this.User);
			if (user == null)
			{
				return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
			}

			if (!this.ModelState.IsValid)
			{
				await this.LoadAsync(user);
				return this.Page();
			}

			string userName = await this._userManager.GetUserNameAsync(user);
			if (this.Input!.Username != userName)
			{
				IdentityResult setUserNameResult = await this._userManager.SetUserNameAsync(user, this.Input.Username);
				if (!setUserNameResult.Succeeded)
				{
					this.StatusMessage = "Unexpected error when trying to set username.";
					return this.RedirectToPage();
				}
			}

			string phoneNumber = await this._userManager.GetPhoneNumberAsync(user);
			if (this.Input!.PhoneNumber != phoneNumber)
			{
				IdentityResult setPhoneResult = await this._userManager.SetPhoneNumberAsync(user, this.Input.PhoneNumber);
				if (!setPhoneResult.Succeeded)
				{
					this.StatusMessage = "Unexpected error when trying to set phone number.";
					return this.RedirectToPage();
				}
			}

			await this._signInManager.RefreshSignInAsync(user);
			this.StatusMessage = "Your profile has been updated";
			return this.RedirectToPage();
		}
	}
}
