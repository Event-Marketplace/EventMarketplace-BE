using System.ComponentModel.DataAnnotations;

namespace EventMarketplace.Domain.Enums;

public enum EventStatus
{
    [Display(Name = "Nieaktywne")]
    Draft,
    [Display(Name = "Wysłane do akceptacji")]
    Submitted,
    [Display(Name = "Aktywne")]
    Approved,
    [Display(Name = "Odrzucone")]
    Rejected,
    [Display(Name = "Zarchiwizowane")]
    Archived,
    [Display(Name = "Usunięte przez organizatora")]
    DeletedByOrganizer,
    [Display(Name = "Usunięte przez administratora")]
    DeletedByAdmin
}
