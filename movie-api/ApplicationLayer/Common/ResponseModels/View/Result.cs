namespace movie_api.ApplicationLayer.Common.ResponseModels.View
{
    public class Result<T>
    {
        internal Result(DateTime requestTime, T? data, string? description = null)
        {
            this.RequestTime = requestTime;
            this.Value = data;
            this.ResponseDescription = description;
        }

        internal Result(DateTime requestTime, T? data, string responseCode, string? description = null, string? requestId = null)
        {
            this.RequestTime = requestTime;
            this.Value = data;
            this.ResponseDescription = description;
            this.ResponseCode = responseCode;
            this.RequestId = requestId;
        }


        internal Result(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        internal Result(DateTime requestTime, string errorMessage, IDictionary<string, string[]> Errors)
        {
            this.ErrorMessage = errorMessage;
            this.Errors = Errors;
            this.RequestTime = requestTime;
        }


        internal Result(DateTime requestTime, string errorMessage)
        {
            this.RequestTime = requestTime;
            this.ErrorMessage = errorMessage;
        }

        public T? Value { get; init; }
        public string? RequestId { get; init; }
        public string? ErrorMessage { get; init; }

        public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
        public string? ResponseCode { get; init; }
        public string? ResponseDescription { get; init; }
        public DateTime RequestTime { get; init; }
        public DateTime ResponseTime { get; init; } = DateTime.Now;
        public double ActivityTime => ResponseTime.Subtract(RequestTime).TotalMilliseconds;
        public bool? IsSuccess => !HasError;


        public static Result<T> Success(DateTime requestTime, T value, string responseCode = "000", string? ResponseDescription = null, string? requestId = null) => new(requestTime, value, responseCode, ResponseDescription, requestId);

        public static Result<T> Success(DateTime requestTime, T data) => new(requestTime, data);

        public static Result<T> Failure(DateTime requestTime, string errorMessage) =>
            new(requestTime, errorMessage);

        public static Result<T> Failure(string errorMessage) =>
            new(errorMessage);
        public static Result<T> Failure(DateTime requestTime, string errorMessage, IDictionary<string, string[]> Errors) =>
            new(requestTime, errorMessage, Errors);
    }
}
