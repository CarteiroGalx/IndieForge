using IndieForge.Models;

namespace IndieForge.DTOs.Dashboard
{
    public class TopActivitesDto
    {
        public User TopCreator { get; set; }
        public User TopContribuitorInProjects { get; set; }
        public User TopContribuitorInValue { get; set; }
    }
}
