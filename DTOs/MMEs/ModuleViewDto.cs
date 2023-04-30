namespace DMed_Razor.DTOs.CMEs
{
    public class ModuleViewDto
    {
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public int MaxDurationHours { get; set; }
        public List<ModulePreReqsDto> ModulePreReqs { get; set; }
    }
}
