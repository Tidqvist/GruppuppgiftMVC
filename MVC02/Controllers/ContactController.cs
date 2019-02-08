using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC02.Models;

namespace MVC02.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Success(Contact contact)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            return View("Success", contact);

        }

    }
}