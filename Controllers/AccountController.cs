using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NG_Core_Auth.Models;

namespace NG_Core_Auth.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly SignInManager<IdentityUser> _signManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signManager)
        {
            _userManager = userManager;
            _signManager = signManager;
        }

        [HttpPost("action")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel formdata)
        {
            // Will hold all the errors related to the registration
            List<string> errorList = new List<String>();

            var user = new IdentityUser
            {
                Email = formdata.Email,
                UserName = formdata.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, formdata.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");

                // sending confirmation email

                return Ok(new { username = user.UserName, email = user.Email, status = 1, message = "Registration successful" });
            }


            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
                errorList.Add(error.Description);
            }

            return BadRequest(new JsonResult(errorList);
        }
    }
}
