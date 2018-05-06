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
    public class IndexModel : PageModel
    {
        private readonly IUserData userData;

        public IndexModel(IUserData _userData)
        {
            userData = _userData;
        }

        public List<User> Users { get; set; }
        public string Message { get; set; }
        public void OnGet()
        {
            Users = userData.Get();
        }

        
    }
}
