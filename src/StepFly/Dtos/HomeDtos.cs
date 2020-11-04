using System;

namespace StepFly.Dtos
{
    public class HomeConfigDto
    {
        public string SiteName { get; set; }

        public string FooterInfo { get; set; }

        public string ShowSentence { get; set; }

        public string CandidateHomePics { get; set; }
    }

    public class VersionDto
    {
        public string Version { get; set; }

        public string CompatibleVersion { get; set; }
    }
}
