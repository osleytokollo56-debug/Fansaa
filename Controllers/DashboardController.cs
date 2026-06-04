using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SomaShare.Models;
using SomaShare.Services;

namespace SomaShare.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly ICommunityTrustService _trustService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(IDashboardService dashboardService, ICommunityTrustService trustService, UserManager<ApplicationUser> userManager)
        {
            _dashboardService = dashboardService;
            _trustService = trustService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            // Check if user is a seller
            var isSellerRole = await _userManager.IsInRoleAsync(user, "Seller");
            var dashboard = isSellerRole 
                ? await _dashboardService.GetSellerDashboardAsync(user.Id)
                : await _dashboardService.GetBuyerDashboardAsync(user.Id);

            return View(dashboard);
        }

        public async Task<IActionResult> UserProfile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var trustInfo = await _trustService.GetUserTrustInfoAsync(id);
            ViewBag.TrustInfo = trustInfo;
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ExportReport()
        {
            var user = await _userManager.GetUserAsync(User);
            var isSellerRole = await _userManager.IsInRoleAsync(user, "Seller");
            var dashboard = isSellerRole
                ? await _dashboardService.GetSellerDashboardAsync(user.Id)
                : await _dashboardService.GetBuyerDashboardAsync(user.Id);

            // Generate CSV or PDF report
            // Implementation depends on reporting library
            return Ok();
        }
    }
}
