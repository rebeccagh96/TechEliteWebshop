using Microsoft.AspNetCore.Mvc;
using TechElite.Models; 

public class UserController : Controller
{
  
    public IActionResult ContactForm()
    {
        return View(); 
    }

    
    [HttpPost]
    public IActionResult SubmitContactForm(ContactFormModel model)
    {
        if (ModelState.IsValid)
        {
           
            return RedirectToAction("ThankYou");
        }

       
        return View("ContactForm", model);
    }

    public IActionResult ThankYou()
    {
        return View(); 
    }
}