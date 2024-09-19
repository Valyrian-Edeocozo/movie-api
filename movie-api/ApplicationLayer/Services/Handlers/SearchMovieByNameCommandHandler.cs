using MediatR;
using movie_api.ApplicationLayer.Common.ResponseModels;
using movie_api.ApplicationLayer.Common.ResponseModels.View;
using movie_api.ApplicationLayer.Services.Commands;
using movie_api.ApplicationLayer.UtilityService.Interface;
using System.Text.Json;

namespace movie_api.ApplicationLayer.Services.Handlers
{
    public class SearchMovieByNameCommandHandler : IRequestHandler<SearchMovieByNameCommand, Result<MovieResponseDto>>
    {
        private readonly IConfiguration _configuration;
        private readonly IGenericService _genericService;

        public SearchMovieByNameCommandHandler(IConfiguration configuration, IGenericService genericService)
        {
            _configuration = configuration;
            _genericService = genericService;
        }

        public async Task<Result<MovieResponseDto>> Handle(SearchMovieByNameCommand request, CancellationToken cancellationToken)
        {
            var serviceBaseUrl = _configuration["WebServices:OMDB_BASE_URL"];
            var serviceApiKey = _configuration["Secrets:OMDB_API_KEY"];
            var queryVariable = "&s=";

            if (string.IsNullOrEmpty(serviceBaseUrl) || string.IsNullOrEmpty(serviceApiKey))
            {
                return Result<MovieResponseDto>.Failure("Service URL or API Key is not configured properly.");
            }

            var callUrl = $"{serviceBaseUrl}{serviceApiKey}{queryVariable}{request.movieTitle}";
            var response = await _genericService.ConsumeGetApi(callUrl, new Dictionary<string, string>());

            if (string.IsNullOrEmpty(response))
            {
                return Result<MovieResponseDto>.Failure("No record found for movie.");
            }

            var searchResult = JsonSerializer.Deserialize<MovieResponseDto>(response);
            if (searchResult == null)
            {
                return Result<MovieResponseDto>.Failure("Failed to deserialize movie data.");
            }

            return Result<MovieResponseDto>.Success(DateTime.Now, searchResult);
        }
    }
}
