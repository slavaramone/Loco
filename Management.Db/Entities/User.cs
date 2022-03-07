using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Management.Db.Entities
{
    [Table("Users")]
    public class User
    {
        public Guid Id { get; set; }

        public DateTimeOffset CreationDateTimeUtc { get; set; }

        [Required]
        [MaxLength(256)]
        public string FirstName { get; set; }

        [MaxLength(256)]
        public string LastName { get; set; }
        
        [MaxLength(512)]
        [Required]
        public string Login { get; set; }

        [Required]
        [MaxLength(256)]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(256)]
        public string PasswordSalt { get; set; }

        public bool IsActive { get; set; }

        public List<UserToRole> UserToRoles { get; set; }
    }
}
