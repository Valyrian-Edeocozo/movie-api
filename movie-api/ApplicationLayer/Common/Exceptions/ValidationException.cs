using FluentValidation.Results;

namespace movie_api.ApplicationLayer.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }
        public ValidationException() : base("One or more validation failure have occured") => this.Errors = new Dictionary<string, string[]>();

        public ValidationException(IEnumerable<ValidationFailure> failures) :
            this() => this.Errors = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage).ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }
}
