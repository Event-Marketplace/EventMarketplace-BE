using System.ComponentModel.DataAnnotations;

namespace EventMarketplace.Domain.Enums;

public enum LocationType
{
    [Display(Name = "Opis miejsca wydarzenia")]
    Description,
    [Display(Name = "Dokładny adres")]
    Address
}