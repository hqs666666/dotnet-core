using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.Data;
using DotNetCore.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotNetCore.Pages
{
    public class AboutModel : PageModel
    {
        private readonly IUserData userData;

        public AboutModel(IUserData _userData)
        {
            userData = _userData;
        }
        public User user { get; set; }
        public void OnGet()
        {
            user = new User();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult OnPost(User user)
        {
            if (ModelState.IsValid)
            {
                var lResult = userData.Save(user);
                if (lResult.Result)
                    return Redirect("Index");

                ModelState.AddModelError("", lResult.Message);
            }
            return Page();
        }
    }
}
