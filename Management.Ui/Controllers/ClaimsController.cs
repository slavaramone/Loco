using Contracts.Enums;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Exceptions;
using SharedLib.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Management.Ui.Controllers
{
    public abstract class ClaimsController : ControllerBase
    {
        private readonly IClaimsAccessor _claimsAccessor;

        public ClaimsController(IClaimsAccessor claimsAccessor)
        {
            _claimsAccessor = claimsAccessor;
        }

        protected Guid GetUserId()
        {
            if (_claimsAccessor.TryGetValue(ClientApiSecurity.Claims.UserId, out string userId))
                return new Guid(userId);
            else
                throw new NotFoundException(userId);
        }

        protected List<UserRole> GetUserRoles()
        {
            if (_claimsAccessor.TryGetValue(ClientApiSecurity.Claims.UserRoles, out string userRolesStr))
            {
                var userRoles = userRolesStr.Split(',')
                    .Select(x => (UserRole)Enum.Parse(typeof(UserRole), x))
                    .ToList();
                return userRoles;
            }
            else
                throw new NotFoundException(userRolesStr);
        }
    }
}
