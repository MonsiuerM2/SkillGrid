namespace DMed_Razor.DTOs.CMEs
{
    public class CourseEnrollDto
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public List<KeyValuePair<int, int>>? ModuleMAs { get; set; }
    }
}
