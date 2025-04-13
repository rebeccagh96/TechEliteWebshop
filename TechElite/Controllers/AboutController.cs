using Microsoft.AspNetCore.Mvc;
using TechElite;
using TechElite.Models; 

public class AboutController : Controller
{
    private readonly ApplicationDbContext _context;

    public AboutController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View(); 
    }

    
    [HttpPost]
    public async Task<IActionResult> SubmitContactForm(string name, string phone, string email, string message)
    {
        var contactForm = new UserContact
        {
            Name = name,
            Phone = phone,
            Email = email,
            Message = message
        };

        _context.userContacts.Add(contactForm);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(ThankYou));
    }

    public IActionResult ThankYou()
    {
        return View(); 
    }
}