using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlashCards.Data;
using FlashCards.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace FlashCards.Controllers
{
    public class UsersValidationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersValidationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckNickName(string nickName, InputModel input )
        {
           
            var result =  _context.Users.Any(a => string.Equals(a.nickName, input.nickName) || string.Equals(a.nickName, nickName));
            return Json(!result);
        }

    }
}
