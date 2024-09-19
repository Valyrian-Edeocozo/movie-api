using MediatR;
using Microsoft.OpenApi.Services;
using movie_api.ApplicationLayer.Common.ResponseModels;
using movie_api.ApplicationLayer.Common.ResponseModels.View;
using movie_api.ApplicationLayer.Exceptions;
using movie_api.ApplicationLayer.Services.Commands;
using movie_api.ApplicationLayer.UtilityService.Interface;
using movie_api.InfrastructureLayer.Interfaces;
using System.Text.Json;

namespace movie_api.ApplicationLayer.Services.Handlers
{
    public class SearchMovieByNameCommandHandler : IRequestHandler<SearchMovieByNameCommand, Result<MovieResponseDto>>
    {
        private readonly IConfiguration _configuration;
        private readonly IGenericService _genericService;
        private readonly ICacheRepository<MovieResponseDto> _cacheRepository;
        private readonly ILogger<SearchMovieByNameCommandHandler> _logger;

        public SearchMovieByNameCommandHandler(IConfiguration configuration, IGenericService genericService, ICacheRepository<MovieResponseDto> cacheRepository, ILogger<SearchMovieByNameCommandHandler> logger)
        {
            _configuration = configuration;
            _genericService = genericService;
            _cacheRepository = cacheRepository;
            _logger = logger;
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

            try
            {
                var cacheResult = _cacheRepository.GetFromCache(request.movieTitle);

                if (cacheResult == default)
                {
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

                    var isSavedSuccesfully = _cacheRepository.AddToCache(searchResult, request.movieTitle);

                    if (!isSavedSuccesfully)
                    {
                        _logger.LogInformation("Unable to save result from api to cache");
                        throw new OperationFailedException($"Unable to save result from api to cache. Cache key is {request.movieTitle}");
                    }
                    _logger.LogInformation($"{request.movieTitle} saved to cache");

                    return Result<MovieResponseDto>.Success(DateTime.Now, searchResult);
                }
                return Result<MovieResponseDto>.Success(DateTime.Now, cacheResult);

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Something went wrong. Details: {ex.Message}");
            }
        }
    }
}
