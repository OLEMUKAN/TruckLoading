using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TruckLoadingApp.Data;
using TruckLoadingApp.Models;

namespace TruckLoadingApp.Pages.Troutes
{
    [Authorize(Roles = "Driver")]
    public class CreateModel : PageModel
    {
        private readonly TruckLoadingApp.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(TruckLoadingApp.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<CreateModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public TRoute TRoute { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            // Remove DriverId and Driver from ModelState
            ModelState.Remove("TRoute.DriverId");
            ModelState.Remove("TRoute.Driver");

            if (!ModelState.IsValid)
            {
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        _logger.LogWarning($"Validation error in '{modelStateKey}': {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("User ID is null or empty.");
                return Page();
            }

            TRoute.DriverId = userId;

            // Ensure AvailableDate is set to a valid date
            if (TRoute.AvailableDate == default)
            {
                TRoute.AvailableDate = DateTime.UtcNow.Date;
            }

            try
            {
                _context.TRoutes.Add(TRoute);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"TRoute created successfully for user {userId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while saving the TRoute: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred while saving the route. Please try again.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
