using System.Text.Json.Serialization;

namespace WpfLibrary.Models
{
    public class Book
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("author")]
        public string Author { get; set; }
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
    }
}
