using System.ComponentModel.DataAnnotations;

namespace EventMarketplace.Domain.Enums;

public enum RoleType
{
    [Display(Name = "Uczestnik")]
    Participant,
    [Display(Name = "Organizator")]
    Organizer,
    [Display(Name = "Administrator")]
    Admin
}