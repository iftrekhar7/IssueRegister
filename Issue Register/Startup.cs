using Issue_Register.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Issue_Register.Startup))]
namespace Issue_Register
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            Seed();
        }
        
        public void Seed()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            string roleName = "SuperAdmin";
            IdentityResult roleResult;
            var user = new ApplicationUser();
            user.UserName = "info@gmail.com";
            user.Email = "info@gmail.com";
            string password = "Password@2018";
            // Check to see if Role Exists, if not create it
            if (!RoleManager.RoleExists(roleName))
            {
                //create role superAdmin
                roleResult = RoleManager.Create(new IdentityRole(roleName));
                //create others role
                RoleManager.Create(new IdentityRole("Employee"));
                RoleManager.Create(new IdentityRole("Vendor"));
                //create default user
                var newUser = UserManager.Create(user, password);
                if (newUser.Succeeded)
                {
                    var result = UserManager.AddToRole(user.Id, "SuperAdmin");
                }

            }
        }
    }

}
