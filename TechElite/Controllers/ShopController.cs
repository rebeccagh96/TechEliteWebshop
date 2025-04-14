using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechElite.Areas.Identity.Data;
using TechElite.Models;

namespace TechElite.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {

            var departments = await _context.Departments.ToListAsync();
            var products = await _context.Products.ToListAsync();
            var reviews = await _context.Reviews.ToListAsync();

            ShopViewModel model = new(
                Departments: departments,
                Products: products,
                Reviews: reviews
                );

            return View(model);
        }

        public async Task<IActionResult> ProductPage(int? id)
        {
            if (id is null)
            {
                return BadRequest("You must pass in a ProductId.");
            }

            var model = await _context.Products
                .Include(r => r.Reviews)
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (model is null)
            {
                return NotFound("Product not found.");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(int? id, string name, string title, int rating, string comment)

        {
            if (id is null)
            {
                return BadRequest("You must pass in a DepartmentId.");
            }
            var model = await _context.Products
                .Include(p => p.ProductName)
                .ThenInclude(d => d.Description)
                .FirstOrDefaultAsync(t => t.ProductId == id);
            if (model == null)
            {
                return NotFound($"Department with ID {id} was not found.");
            }
            return View(model);
        }

        public IActionResult Datorer()
        {
            return View();
        }
        public IActionResult Gaming()
        {
            return View();
        }
        public IActionResult Hörlurar()
        {
            return View();
        }
        public IActionResult Telefoner()
        {
            return View();
        }
        public IActionResult Tillbehör()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> FilterDepartments(int? id)
        {
            if (id is null)
            {
                return BadRequest("You must pass in a DepartmentId.");
            }

            var model = await _context.Products
                .Include(r => r.Reviews)
                .Where(d => d.DepartmentId == id)
                .ToListAsync();

            if (model == null)
            {
                return NotFound($"Department with ID {id} was not found.");
            }

            return View(model);
        }
    }
}
