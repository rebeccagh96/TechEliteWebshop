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
        public IActionResult ProductPage()
        {
            return View();
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

            var model = await _context.Departments
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
