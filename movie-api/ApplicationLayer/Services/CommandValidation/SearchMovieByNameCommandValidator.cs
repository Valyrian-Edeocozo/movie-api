using FluentValidation;
using movie_api.ApplicationLayer.Services.Commands;

namespace movie_api.ApplicationLayer.Services.CommandValidation
{
    public class SearchMovieByNameCommandValidator : AbstractValidator<SearchMovieByNameCommand>
    {
        public SearchMovieByNameCommandValidator()
        {
            this.RuleFor(parameter => parameter.movieTitle)
            .NotEmpty().NotNull().WithMessage("Movie title must not be null or empty");

            this.RuleFor(parameter => parameter.movieTitle).Must(s => s!.Length >= 3).WithMessage("Input must be greater or equals to 3 characters");
        }
    }
}
