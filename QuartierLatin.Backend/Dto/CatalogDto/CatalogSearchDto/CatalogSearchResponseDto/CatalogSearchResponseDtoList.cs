using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto.CatalogSearchResponseDto
{
    public class CatalogSearchResponseDtoList<T>
    {
        public int TotalPages { get; set; }

        public List<T> Items { get; set; }
    }

    public class CatalogUniversityDto
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }
        public List<string> InstructionLanguages { get; set; }
        public List<string> Degrees { get; set; }
    }
}
