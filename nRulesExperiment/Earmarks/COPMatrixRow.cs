
using System.Collections.Generic;
using NUnit.Framework;

namespace nRulesExperiment.Earmarks
{
    // ReSharper disable once InconsistentNaming
    public class COPMatrixRow
    {
        public COPMatrixRow()
        {
            CareAndTreatmentEarmarkAmounts = new EarmarkAmounts();
            OvcEarmarkAmounts = new EarmarkAmounts();
            InitiativeName = string.Empty;
            FundingSourceName = string.Empty;
            MajorProgramArea = string.Empty;
            MajorBeneficiary = string.Empty;
            AbstinenceEarmarkAmounts = new AbstinenceEarmark();
        }

        public string InitiativeName { get; set; }
        public string FundingSourceName { get; set; }
        public string MajorProgramArea { get; set; }
        public int ProgramAreaID { get; set; }

        public int MajorProgramAreaID { get; set; }

        public int SubProgramID { get; set; }

        public string MajorBeneficiary { get; set; }
        
        public int BeneficiaryID { get; set; }

        public long? ProposedAmount { get; set; }


        public EarmarkAmounts CareAndTreatmentEarmarkAmounts { get; set; } 

        public EarmarkAmounts OvcEarmarkAmounts { get; set; }

        public AbstinenceEarmark AbstinenceEarmarkAmounts { get; set; }
    }

    public class EarmarkAmounts
    {
        public long ContributionAmount { get; set; }

        // ReSharper disable once InconsistentNaming
        public long PMDenominatorAmount { get; set; }

    }

    public class AbstinenceEarmark
    {
        public long NumeratorAmount { get; set; }

        public long DenominatorAmount { get; set; }
    }

}