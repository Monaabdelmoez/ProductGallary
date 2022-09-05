using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductGallary.Constants;
using ProductGallary.Models;
using ProductGallary.TDO;

namespace ProductGallary.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManger;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> _userManger,SignInManager<ApplicationUser> _signInManager,RoleManager<IdentityRole> _roleManager)
        {
            userManger = _userManger;
            signInManager = _signInManager;
            roleManager = _roleManager;
        }
       
        // open register view
        [HttpGet]
        public IActionResult Register()
        {
            return View("ShowRegisterOptions");
        }

        // register as Buyer

        [HttpGet]
        public IActionResult BuyerRegister()
        {
            ViewBag.Role = Roles.BUYER_ROLE;
            return View("Register");
        }

        //Register As Customer

        [HttpGet]
        public IActionResult CustomerRegister()
        {
            ViewBag.Role = Roles.CUSTOMER_ROLE;
            return View("Register");
        }


        // Admin Register

        [HttpGet]
        public IActionResult AdminRegister()
        {

            ViewBag.Role = Roles.ADMIN_ROLE;
            return View("Register");
        }

        [HttpPost("{role}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(ApplicationUserCreateDTO newUser, [FromRoute] string? role)
        {
            // Check if Data Is Valid
            if(!ModelState.IsValid)
            {
                return View("Register", newUser);
               
            }
            else{
                // create Id
                Guid userId = Guid.NewGuid();

                // create new Application User
                ApplicationUser user = new ApplicationUser
                {
                    Id = userId.ToString(),
                    Name = newUser.Name,
                    UserName = newUser.UserName,
                    Address = newUser.Address,
                    PhoneNumber = newUser.PhoneNumber,
                    Email = newUser.Email,
                    PasswordHash = newUser.Password

                };

               // getting the result of creating new user
                IdentityResult result = await userManger.CreateAsync(user, newUser.Password);

                if (result.Succeeded)
                {
                    // check if role is exists
                    if(!await this.roleManager.RoleExistsAsync(role))
                    {
                        // create new Role
                      IdentityResult result2= await this.roleManager.CreateAsync(new IdentityRole(role));
                    }
                    // add Role To The User
                    await userManger.AddToRoleAsync(user, role);

                    //redirect to login page
                    if(this.User==null)
                    return RedirectToAction("Login");
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    // print errors
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);

                    }
                    return View("Register", newUser);
                }
            }

            //return RedirectToAction("Index","Home");
        }

        // Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // confirm login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(ApplicationUserLoginDTO loginUser)
        {
            if(!ModelState.IsValid)
            {
                return View(loginUser);
            }

            // get user with usernam
            ApplicationUser user = await this.userManger.FindByNameAsync(loginUser.UserName);
            
            
            // check if User is found
            if(user==null)
            {
                // check if user is the admin
                if(loginUser.UserName.Trim().ToLower().Equals("admin")&& loginUser.Password.Trim().ToLower().Equals("adminadmin")&& !await this.roleManager.RoleExistsAsync(Roles.ADMIN_ROLE))
                {
                    // create account for admin show register view
                    
                    return RedirectToAction("AdminRegister");

                }


                ModelState.AddModelError("", "اسم المستخدم غير موجود");
            }
            else
            {
                // check for user password

                if(await this.userManger.CheckPasswordAsync(user,loginUser.Password))
                {
                    // login user 
                    // create cookie
                    await this.signInManager.SignInAsync(user, loginUser.RememberMe);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("","كلمه المرور غير صحيحه *");
                }

            }
            return View(loginUser);
        }


        // SignOut
        [HttpGet]
        public  async Task<IActionResult> SignOut()
        {
            
            //sign out the user
            await this.signInManager.SignOutAsync();
            // clear cart During LogOut
            if(TempData.ContainsKey(Constant.PRODUCTS))
                TempData.Remove(Constant.PRODUCTS);
            // remove order id when log out
            if (TempData.ContainsKey(Constant.ORDERID))
                TempData.Remove(Constant.ORDERID);

            return RedirectToAction("Login", "Account");
        }




    }
}
