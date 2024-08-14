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

namespace TruckLoadingApp.Pages.Trucks
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
        ViewData["DriverId"] = new SelectList(_context.applicationUsers, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Truck Truck { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            // Get the current logged-in user's ID before the model state validation
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("User ID is null or empty.");
                return Page();
            }

            // Remove DriverId and Driver from ModelState
            ModelState.Remove("Truck.DriverId");
            ModelState.Remove("Truck.Driver");

            // Now check if the model state is valid
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

            // Assign the driver ID to the truck
            Truck.DriverId = userId;
            // No need to set Truck.Driver, it will be handled by EF Core

            _logger.LogInformation($"Creating truck for user {userId}.");

            try
            {
                _context.Trucks.Add(Truck);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Truck successfully created and saved to the database.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while saving the truck: {ex.Message}");
                throw; // Re-throw the exception after logging it
            }

            return RedirectToPage("./Index");
        }



    }
}
