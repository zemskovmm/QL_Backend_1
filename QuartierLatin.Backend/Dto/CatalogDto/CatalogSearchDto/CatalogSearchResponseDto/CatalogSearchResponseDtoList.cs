using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto.CatalogSearchResponseDto
{
    public class CatalogSearchResponseDtoList
    {
        public int TotalPages { get; set; }

        public List<CatalogSearchResponseDtoItem> Items { get; set; }
    }
}
