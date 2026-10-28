using System.ComponentModel.DataAnnotations;

namespace EventMarketplace.Domain.Enums;

public enum UserActivity
{
    [Display(Name = "Aktywni")]
    Active,
    [Display(Name = "Nieaktywni")]
    UnActive,
    [Display(Name = "Zablokowani")]
    Blocked
}