using ExcIdentity.Data;
using ExcIdentity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ExcIdentity.Controllers
{
    public class UserController: Controller
    {
        private MyIdentityDbContext myIdentityDbContext;
        private UserManager<User> userManager;
        private RoleManager<Role> roleManager;

        public UserController()
        {
            myIdentityDbContext = new MyIdentityDbContext();
            UserStore<User> userStore = new UserStore<User>(myIdentityDbContext);
            userManager = new UserManager<User>(userStore); //nhu userservice
            RoleStore<Role> roleStore = new RoleStore<Role>(myIdentityDbContext);
            roleManager = new RoleManager<Role>(roleStore);
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> AddRole(string RoleName)
        {
            Role role = new Role()
            {
                Name = RoleName
            };
            var result = await roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return View("ViewSuccess");
            }
            else
            {
                //result.Errors.First().ToString();
                return View("ViewError");
            }


        }
        public async Task<ActionResult> AddUserToRole(string UserId, string RoleId)
        {
            //find db -> id => ten Role
            var user = myIdentityDbContext.Users.Find(UserId);
            var role = myIdentityDbContext.Roles.Find(RoleId);
            if (user == null || role == null)
            {
                return View("ViewError");
            }
            else
            {
                var result = await userManager.AddToRoleAsync(user.Id, role.Name);
                //string roleName1 = "Admin";
                //string roleName2 = "User";
                ////var result = await userManager.AddToRoleAsync(userId, roleName);
                //var result = await userManager.AddToRolesAsync(userId, roleName1, roleName2);
                if (result.Succeeded)
                {
                    return View("ViewSuccess");
                }
                else
                {
                    return View("ViewError");
                }
            }

        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(string UserName,string cmnd,
           string status, string PasswordHash)
        {
            User user = new User()
            {
                UserName = UserName,
                cmnd = cmnd,
                status = status,
            };
            var result = await userManager.CreateAsync(user, PasswordHash);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("ViewError");
            }

        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string UserName, string PasswordHash)
        {
            var user = await userManager.FindAsync(UserName, PasswordHash);
           
            if (user == null)
            {
                return View("ViewError");
            }
            else
            {
                SignInManager<User, string> signInManager =
               new SignInManager<User, string>(userManager, Request.GetOwinContext().Authentication);
                await signInManager.SignInAsync(user, false, false);
                return RedirectToAction("Index", "Home");
            }
           

           
           

        }
        public ActionResult LogOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}