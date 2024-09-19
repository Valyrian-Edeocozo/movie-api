using MediatR;
using movie_api.ApplicationLayer.Common.ResponseModels;
using movie_api.ApplicationLayer.Common.ResponseModels.View;

namespace movie_api.ApplicationLayer.Services.Commands
{
    public class SearchMovieByNameCommand : IRequest<Result<MovieResponseDto>>
    {
        public string movieTitle { get; set; } = default!;
    }
}
