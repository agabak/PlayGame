using Play.Identity.Dtos;
using Play.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Identity
{
    public static class Extensions
    {
        public static UserDto AsDto(this ApplicationUser  user)
        {
            return new UserDto(user.Id,user.UserName, user.Email, user.Gil, user.CreatedOn);
        }
    }
}
