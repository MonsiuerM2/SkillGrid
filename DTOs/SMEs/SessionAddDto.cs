namespace DMed_Razor.DTOs.SMEs
{
    public class SessionAddDto
    {
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }
        public int NumStudents { get; set; }
        public int CourseId { get; set; }
        public int OrgId { get; set; }
        public Boolean MakingOwnCourse { get; set; } = false;
        public List<int> ModulesIdList { get; set; } = new List<int>();
        public string CourseName { get; set; } = "";
    }
}
