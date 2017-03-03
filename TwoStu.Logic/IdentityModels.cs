using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TwoStu.Settings.Statics;
using System.Security.Principal;
using System.Collections.Generic;
using System;
using System.Linq;

namespace TwoStu.Logic.Models
{

    public static class MyIdentityExtensions
    {
        #region Claims list
        public static IEnumerable<int> GetSubjectIds(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Subjects");
            // Test for null to avoid issues during local testing
            string value =  (claim != null) ? claim.Value : string.Empty;


            return value.Split(separator: new string[] { "," }, options: StringSplitOptions.None)
                .Select(x =>
                {
                    int temp;
                    if(int.TryParse(x, out temp))
                    {
                        return temp;
                    }

                    return -1;
                }).Where(x => x != -1).ToList();
        }

        
        #endregion
    }
    // Чтобы добавить данные профиля для пользователя, можно добавить дополнительные свойства в класс ApplicationUser. Дополнительные сведения см. по адресу: http://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public string Subjects { get; set; }

        public string Password { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            // Здесь добавьте настраиваемые утверждения пользователя
            userIdentity.AddClaim(new Claim("Subjects", this.Subjects));


            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base(ConnectionStringStatic.ConnectionString, throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}