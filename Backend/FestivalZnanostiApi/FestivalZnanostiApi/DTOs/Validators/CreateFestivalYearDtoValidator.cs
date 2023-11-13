using FluentValidation;

namespace FestivalZnanostiApi.DTOs.Validators
{
    public class CreateFestivalYearDtoValidator : AbstractValidator<CreateFestivalYearDto>
    {
        public CreateFestivalYearDtoValidator()
        {
            RuleFor(dto => dto.StartDate)
                .NotEmpty()
                .Must((dto, startDate) => BeInSameYearAs(dto.Year, startDate))
                .WithMessage("StartDate must be in the same year as the specified Year.");

            RuleFor(dto => dto.EndDate)
                .NotEmpty()
                .Must((dto, endDate) => BeInSameYearAs(dto.Year, endDate))
                .WithMessage("EndDate must be in the same year as the specified Year.")
                .GreaterThan(dto => dto.StartDate)
                .WithMessage("EndDate must be greater than StartDate.");

            When(dto => dto.EditUntil.HasValue, () =>
            {
                RuleFor(dto => dto.EditUntil)
                    .Must((dto, editUntil) => BeInSameYearAs(dto.Year, editUntil!.Value))
                    .WithMessage("EditUntil must be in the same year as the specified Year.")
                    .LessThan(dto => dto.StartDate)
                    .WithMessage("EditUntil must be less than StartDate.")
                    .LessThan(dto => dto.EndDate)
                    .WithMessage("EditUntil must be less than EndDate.");
            });
        }

        private bool BeInSameYearAs(int year, DateTime date)
        {
            return date.Year == year;
        }
    }
}
