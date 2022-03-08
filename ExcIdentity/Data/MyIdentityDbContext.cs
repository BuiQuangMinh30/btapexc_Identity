using ExcIdentity.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExcIdentity.Data
{
    public class MyIdentityDbContext : IdentityDbContext<User> //cau hinh ket noi db
    {
        public MyIdentityDbContext() : base("ConnectionString")
        {

        }
    }
}