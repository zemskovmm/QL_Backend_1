using System;

namespace QuartierLatin.Backend.Dto.PortalDto
{
    public class PortalUserListAdminDto : BaseDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
