namespace QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto.CatalogSearchResponseDto
{
    public class CatalogSearchResponseDtoItem : BaseDto
    {
        public string Name { get; set; }

        public int Price { get; set; }
        public string TermDuration { get; set; }
    }
}
