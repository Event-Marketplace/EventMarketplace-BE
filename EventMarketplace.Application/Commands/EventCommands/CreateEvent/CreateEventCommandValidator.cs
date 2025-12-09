using EventMarketplace.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace EventMarketplace.Application.Commands.EventCommands.CreateEvent;

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];
    private const long _maxFileSize = 3 * 1024 * 1024;
    
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Dto.Title)
            .NotEmpty().WithMessage("Tytuł jest wymagany.")
            .MaximumLength(200).WithMessage("Tytuł może mieć maksymalnie 200 znaków.");

        RuleFor(x => x.Dto.Description)
            .NotEmpty().WithMessage("Opis jest wymagany.")
            .MaximumLength(4000).WithMessage("Opis może mieć maksymalnie 4000 znaków.");

        RuleFor(x => x.Dto.Price)
            .GreaterThanOrEqualTo(1).WithMessage("Cena musi być większa lub równa 1.");

        RuleFor(x => x.Dto.AvailableTicketsCount)
            .GreaterThanOrEqualTo(10).WithMessage("Dostępna liczba biletów musi być większa lub równa 10.");

        RuleFor(x => x.Dto.StartDateTime)
            .Must(BeAValidDate).WithMessage("Data rozpoczęcia wydarzenia musi być datą prawidłową.")
            .GreaterThanOrEqualTo(_ => DateTime.UtcNow.AddMinutes(-1))
            .WithMessage("Data rozpoczęcia wydarzenia nie może być datą przeszłą.");
        
        RuleFor(x => x.Dto.EndDateTime)
            .Must(BeAValidDate).WithMessage("Data zakończenia wydarzenia musi być datą prawidłową.")
            .GreaterThan(x => x.Dto.StartDateTime)
            .WithMessage("Data zakończenia wydarzenia nie może być wcześniejsza od daty rozpoczęcia.");

        RuleFor(x => x.Dto.Image)
            .NotNull().WithMessage("Zdjęcie wydarzenia jest wymagane.")
            .Must(file => file.Length > 0).WithMessage("Plik ze zdjęciem nie może być pusty.")
            .Must(IsAllowedSize).WithMessage("Plik jest zbyt duży. Maksymalnie 3MB.")
            .Must(IsAllowedExtension).WithMessage("Dozwolone formaty zdjęć to: .jpg, .jpeg, .png");

        RuleFor(x => x.Dto.EventPlaceDescribtion)
            .NotEmpty()
            .When(x => x.Dto.LocationType == LocationType.DescriptionPlace)
            .WithMessage("Musisz podać opis miejsca wydarzenia");
        
        RuleFor(x => x.Dto.PostalCode)
            .NotEmpty()
            .When(x => x.Dto.LocationType == LocationType.Address)
            .WithMessage("Kod pocztowy jest wymagany.");
        
        RuleFor(x => x.Dto.City)
            .NotEmpty()
            .When(x => x.Dto.LocationType == LocationType.Address)
            .WithMessage("Miasto jest wymagane.");
        
        RuleFor(x => x.Dto.Street)
            .NotEmpty()
            .When(x => x.Dto.LocationType == LocationType.Address)
            .WithMessage("Ulica jest wymagana.");
        
        RuleFor(x => x.Dto.Number)
            .NotEmpty()
            .When(x => x.Dto.LocationType == LocationType.Address)
            .WithMessage("Numer budynku/lokalu jest wymagany.");
    }
    
    private bool IsAllowedSize(IFormFile file)
        => file.Length <= _maxFileSize;

    private bool IsAllowedExtension(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLower();
        return _allowedExtensions.Contains(extension);
    }
    
    private bool BeAValidDate(DateTime date)
        => date != default;
}