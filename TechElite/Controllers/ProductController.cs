using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechElite.Models;

namespace TechElite.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Department)  
                .ToListAsync();
            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductViewModel model, IFormFile? Image)
        {
            // Loggar för debugging
            Console.WriteLine($"Received ProductId: {model.ProductId}");
            Console.WriteLine($"Received ProductName: {model.ProductName}");
            Console.WriteLine($"Received DepartmentId: {model.DepartmentId}");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                // Specifika felmeddelanden för varje fält
                foreach (var key in ModelState.Keys)
                {
                    if (ModelState[key].Errors.Count > 0)
                    {
                        errors.Add($"Field '{key}': {string.Join(", ", ModelState[key].Errors.Select(e => e.ErrorMessage))}");
                    }
                }

                return BadRequest(new { message = "Ogiltig data", errors });
            }

            if (Image != null && Image.Length > 0)
            {
                if (Image.ContentType == "image/svg+xml")
                {
                    using (var reader = new StreamReader(Image.OpenReadStream()))
                    {
                        string svgContent = await reader.ReadToEndAsync();

                        model.Image = System.Text.Encoding.UTF8.GetBytes(svgContent);
                    }
                }
                else
                {
                    return BadRequest(new { success = false, message = "Endast SVG-filer stöds." });
                }
            }

            if (model.ProductId.HasValue && model.ProductId > 0)
            {
                var product = await _context.Products.FindAsync(model.ProductId);
                if (product == null)
                {
                    return NotFound("Produkten kunde inte hittas.");
                }

                product.ProductName = model.ProductName;
                product.Quantity = model.Quantity;
                product.Price = model.Price;
                product.Description = model.Description;
                product.DepartmentId = model.DepartmentId; 

                if (model.Image != null)
                {
                    product.Image = model.Image;
                }

                _context.Update(product);
            }
            else
            {
                var newProduct = new Product
                {
                    ProductName = model.ProductName,
                    Quantity = model.Quantity,
                    Price = model.Price,
                    Description = model.Description,
                    DepartmentId = model.DepartmentId, 
                    Image = model.Image
                };

                await _context.Products.AddAsync(newProduct);
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Produktuppdatering lyckades." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Ett fel inträffade: " + ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] dynamic request)
        {
            int productId = (int)request.ProductId;
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound("Produkten hittades inte.");
            }
            _context.Products.Remove(product);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Produkten raderades framgångsrikt." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Ett fel inträffade: " + ex.Message });
            }
        }

        public static string IsSelected(int departmentId, int selectedDepartmentId)
        {
            return departmentId == selectedDepartmentId ? "selected" : string.Empty;
        }

        [HttpGet]
        public async Task<IActionResult> GetImage(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null || product.Image == null || product.Image.Length == 0)
            {
                return Json(new { success = false, message = "Ingen bild hittades." });
            }

            try
            {
                string svgContent = System.Text.Encoding.UTF8.GetString(product.Image);

                if (svgContent.Contains("<svg") && svgContent.Contains("</svg>"))
                {
                    return Json(new
                    {
                        success = true,
                        svgContent = svgContent
                    });
                }
                else
                {
                    return Json(new { success = false, message = "Bilden är inte i SVG-format." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Fel vid läsning av bilden: " + ex.Message });
            }
        }
    }
}

