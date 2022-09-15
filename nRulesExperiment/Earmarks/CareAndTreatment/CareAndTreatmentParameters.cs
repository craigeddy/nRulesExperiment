using System.Collections.Generic;

namespace nRulesExperiment.Earmarks.CareAndTreatment
{
    public class CareAndTreatmentParameters
    {
        public CareAndTreatmentParameters()
        {
            ApplicableMajorProgramAreaIDs = new List<int>();
            ApplicableProgramAreaIDs = new List<int>();
            ApplicableFundingSources = new List<string>();
        }

        public List<int> ApplicableMajorProgramAreaIDs { get; set; }

        public List<int> ApplicableProgramAreaIDs { get; set; }

        public List<string> ApplicableFundingSources { get; set; }
    }
}