namespace QuartierLatin.Backend.Dto.CurseCatalogDto.Curse.ModuleDto
{
    public class CurseModuleDto
    {
        public string Title { get; set; }

        public string DescriptionHtml { get; set; }

        public int SchoolId { get; set; }

        public CurseModuleTraitsDto Traits { get; set; }
    }
}
