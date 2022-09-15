using System.Collections.Generic;

namespace nRulesExperiment.Earmarks.AbstinenceRules
{
    public class DenominatorContributionParameters
    {
        public DenominatorContributionParameters()
        {
            Multiplier = 1;
            ApplicableProgramAreas = new List<int>();
            ApplicableFundingSources = new List<string>();
        }
        
        public List<string> ApplicableFundingSources { get; set; }

        public double Multiplier { get; set; }

        public List<int> ApplicableProgramAreas { get; set; }
    }

    public class AbstinenceNumeratorContribution 
    {
        public AbstinenceNumeratorContribution()
        {
            Multiplier = 1;
            ApplicableBeneficiaries = new List<int>();
            ApplicableProgramAreas = new List<int>();
            ApplicableFundingSources = new List<string>();
        }

        public List<int> ApplicableBeneficiaries { get; set; }

        public List<string> ApplicableFundingSources { get; set; }

        public List<int> ApplicableProgramAreas { get; set; }
        
        public double Multiplier { get; set; }
    }
}