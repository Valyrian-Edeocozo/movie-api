using MediatR;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using movie_api.ApplicationLayer.Services.Commands;
using System.Text.RegularExpressions;

namespace movie_api.ApiLayer.Endponts
{
    public static class MovieFeedEndpoints
    {
        public static RouteGroupBuilder MovieFeedsGroup(this RouteGroupBuilder group)
        {
            group.MapGet("/getMovieByName", async (ISender sender, [AsParameters] SearchMovieByNameCommand command) => await sender.Send(command)).WithName("SearchMovieByMovieNameV1")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Search and gets Movies by movie name",
                Description = "Retrieves movies that match the input"
            });

            return group;

        }
    }
}
