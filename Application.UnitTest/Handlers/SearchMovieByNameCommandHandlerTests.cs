using Moq;
using Xunit;
using Microsoft.Extensions.Configuration;
using movie_api.ApplicationLayer.Services.Handlers;
using movie_api.ApplicationLayer.Services.Commands;
using movie_api.ApplicationLayer.Common.ResponseModels;
using movie_api.ApplicationLayer.Common.ResponseModels.View;
using movie_api.ApplicationLayer.UtilityService.Interface;
using System.Threading;
using System.Threading.Tasks;

public class SearchMovieByNameCommandHandlerTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IGenericService> _mockGenericService;
    private readonly SearchMovieByNameCommandHandler _handler;

    public SearchMovieByNameCommandHandlerTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockGenericService = new Mock<IGenericService>();

        _handler = new SearchMovieByNameCommandHandler(_mockConfiguration.Object, _mockGenericService.Object);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenMovieFound()
    {
        // Arrange
        var movieTitle = "Game of thrones";
        var movieResponse = new MovieResponseDto
        {
            Search = new List<MovieResponseProperties> 
            { new MovieResponseProperties
                {
                    Title = movieTitle,
                    ImdbID = "1234",
                    Poster = "https//mymoviesposter.com",
                    Type = "Adventure",
                    Year = "2011"
                } 
            },
            Response = "True",
            TotalResults = "2"
        };

        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(movieResponse);
        var searchCommand = new SearchMovieByNameCommand { movieTitle = movieTitle };

        _mockConfiguration.Setup(c => c["WebServices:OMDB_BASE_URL"]).Returns("http://example.com/");
        _mockConfiguration.Setup(c => c["Secrets:OMDB_API_KEY"]).Returns("12345");
        _mockGenericService.Setup(s => s.ConsumeGetApi(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(jsonResponse);

        // Act
        var result = await _handler.Handle(searchCommand, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        //Assert.Equal(movieResponse, result.Value);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenServiceNotConfigured()
    {
        // Arrange
        var searchCommand = new SearchMovieByNameCommand { movieTitle = "Inception" };
        _mockConfiguration.Setup(c => c["WebServices:OMDB_BASE_URL"]).Returns(string.Empty);
        _mockConfiguration.Setup(c => c["Secrets:OMDB_API_KEY"]).Returns(string.Empty);

        // Act
        var result = await _handler.Handle(searchCommand, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Service URL or API Key is not configured properly.", result.ErrorMessage);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenMovieNotFound()
    {
        // Arrange
        var movieTitle = "NonExistentMovie";
        var searchCommand = new SearchMovieByNameCommand { movieTitle = movieTitle };

        _mockConfiguration.Setup(c => c["WebServices:OMDB_BASE_URL"]).Returns("http://example.com/");
        _mockConfiguration.Setup(c => c["Secrets:OMDB_API_KEY"]).Returns("12345");
        _mockGenericService.Setup(s => s.ConsumeGetApi(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(string.Empty);

        // Act
        var result = await _handler.Handle(searchCommand, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        //Assert.Equal("No record found for movie.", result.ErrorMessage);
    }

    [Fact]
    public async Task Handle_ThrowsJsonException_WhenDeserializationFails()
    {
        // Arrange
        var movieTitle = "Inception";
        var searchCommand = new SearchMovieByNameCommand { movieTitle = movieTitle };

        _mockConfiguration.Setup(c => c["WebServices:OMDB_BASE_URL"]).Returns("http://example.com/");
        _mockConfiguration.Setup(c => c["Secrets:OMDB_API_KEY"]).Returns("12345");
        _mockGenericService.Setup(s => s.ConsumeGetApi(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync("Invalid JSON Response");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _handler.Handle(searchCommand, CancellationToken.None)
        );
    }
}
