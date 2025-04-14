using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechElite.Models;

namespace TechElite.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Search(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return BadRequest("Hittade inget sökord.");
            }

            var departments = await _context.Departments.ToListAsync();
            var products = await _context.Products.ToListAsync();
            var reviews = await _context.Reviews.ToListAsync();

            var filteredDepartments =departments
                .Where(d => d.DepartmentName.Contains(search, StringComparison.OrdinalIgnoreCase)
                || d.DepartmentDescription.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();
            var filteredProducts = products
                .Where(p => p.ProductName.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();
            var filteredReviews = reviews
                .Where(r => r.ReviewTitle.Contains(search, StringComparison.OrdinalIgnoreCase)
                || r.ReviewText.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var searchViewModel = new SearchViewModel(
                Departments: filteredDepartments,
                Products: filteredProducts,
                Reviews: filteredReviews
                );

            return View(searchViewModel);
        }
    }
}
