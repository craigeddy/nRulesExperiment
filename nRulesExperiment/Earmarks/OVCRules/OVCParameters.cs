using System.Collections.Generic;

namespace nRulesExperiment.Earmarks.OVCRules
{
    public class OVCParameters
    {
        public OVCParameters()
        {
            ApplicableFundingSources = new List<string>();
            ExcludedProgramAreas = new List<string>(new[] {"HTS"});
            MajorBeneficiary = "OVC";
        }

        public List<string> ApplicableFundingSources { get; set; }

        public string MajorBeneficiary { get; set; }

        public List<string> ExcludedProgramAreas { get; set; }
    }
}