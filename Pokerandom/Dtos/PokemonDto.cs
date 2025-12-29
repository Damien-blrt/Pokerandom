using Shared;

namespace Dtos
{
    public class PokemonDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public TypePkm Type1 { get; set; }
        public TypePkm Type2 { get; set; }
    }
}
