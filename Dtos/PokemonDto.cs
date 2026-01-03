using Shared;
using System.Text.Json.Serialization;

namespace Dtos
{
    public class PokemonDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TypePkm Type1 { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TypePkm Type2 { get; set; }
    }
}
