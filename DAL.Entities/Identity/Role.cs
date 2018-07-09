using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities.Contracts;
using Microsoft.AspNetCore.Identity;

namespace DAL.Entities.Identity
{
    public class Role: IdentityRole<int>, IEntity
    {
    }
}
