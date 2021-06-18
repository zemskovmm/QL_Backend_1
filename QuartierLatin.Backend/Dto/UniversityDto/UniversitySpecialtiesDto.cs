namespace QuartierLatin.Backend.Dto.UniversityDto
{
    public class UniversitySpecialtiesDto
    {
        public string Name { get; set; }
    }

    public class UniversityDegreeDto
    {
        public string Name { get; set; }
        public int CostFrom { get; set; }
        public int? CostTo { get; set; }
    }
}
