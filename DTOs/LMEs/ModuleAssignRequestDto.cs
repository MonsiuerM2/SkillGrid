namespace DMed_Razor.DTOs.LMEs
{
    public class ModuleAssignRequestDto
    {
        public int LecturerId { get; set; }
        public string Subject { get; set; }
        public string GradeLevel { get; set; }
        public int Duration { get; set; }
        public string PreferredTeachingMethodology { get; set; }
        public List<string> SpecificRequirements { get; set; }
        public string AdditionalInformation { get; set; }
    }
}
