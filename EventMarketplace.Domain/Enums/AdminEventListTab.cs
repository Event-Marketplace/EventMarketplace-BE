using System.ComponentModel.DataAnnotations;

namespace EventMarketplace.Domain.Enums;

public enum AdminEventListTab
{
    [Display(Name = "Oczekujące")]
    Pending = 0,
    [Display(Name = "Zatwierdzone")]
    Approved = 1,
    [Display(Name = "Odrzucone")]
    Rejected = 2,
    [Display(Name = "Wszystkie")]
    All = 3
}