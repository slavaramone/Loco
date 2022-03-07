using Contracts.Enums;
using System;

namespace Management.Db.Entities
{
    public class UserToRole
    {
        public Guid UserId { get; set; }

        public User User { get; set; }

        public UserRole UserRole { get; set; }

        public DateTimeOffset CreationDateTimeUtc { get; set; }
    }
}
