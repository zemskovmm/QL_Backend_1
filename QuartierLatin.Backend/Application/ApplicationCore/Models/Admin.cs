using System;
using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models
{
    public interface IHaveId
    {
        public int Id { get; set; }
        public Guid AzureIdentityId { get; set; }
    }

    public interface IHavePasswordAuth
    {
        public string Login { get; }
        public string PasswordHash { get; }
    }

    [Table("Admins")]
    public class Admin : IHaveId, IHavePasswordAuth
    {
        [PrimaryKey, Identity] public int Id { get; set; }
        [Column] public Guid AzureIdentityId { get; set; }

        [Column] public string Name { get; set; }

        [Column, Nullable] public string Email { get; set; }
        string IHavePasswordAuth.Login => Email;

        [Column, Nullable] public bool Confirmed { get; set; }
        [Column, Nullable] public string ConfirmationCode { get; set; }

        [Column, Nullable] public string PasswordHash { get; set; }
        [Column, Nullable] public int AvatarImage { get; set; }
    }
}