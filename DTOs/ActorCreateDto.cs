using System;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs;

public class ActorCreateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [CustomValidation(typeof(ActorCreateDto), nameof(ValidateBirthYear))]
    public int BirthYear { get; set; }

    public static ValidationResult? ValidateBirthYear(int birthYear, ValidationContext context)
    {
        int currentYear = DateTime.Now.Year;
        if (birthYear < 1800 || birthYear > currentYear)
        {
            return new ValidationResult($"Birth year must be between 1800 and {currentYear}.");
        }
        return ValidationResult.Success;
    }
}