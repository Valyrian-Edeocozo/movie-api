using System.Text.Json.Serialization;

namespace movie_api.ApplicationLayer.Common.ResponseModels
{
    public class MovieResponseProperties
    {
        [JsonPropertyName("Title")]
        public string? Title { get; set; }

        [JsonPropertyName("Year")]
        public string? Year { get; set; }

        [JsonPropertyName("imdbID")]
        public string? ImdbID { get; set; }

        [JsonPropertyName("Type")]
        public string? Type { get; set; }

        [JsonPropertyName("Poster")]
        public string? Poster { get; set; }
    }

    public class MovieResponseDto
    {
        [JsonPropertyName("Search")]
        public List<MovieResponseProperties>? Search { get; set; }

        [JsonPropertyName("totalResults")]
        public string? TotalResults { get; set; }

        [JsonPropertyName("Response")]
        public string? Response { get; set; }
    }
}
