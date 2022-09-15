using System;
using System.Collections.Generic;
using System.Linq;
using NRules.Fluent.Dsl;
using nRulesExperiment.Earmarks.CareAndTreatment;

namespace nRulesExperiment.Earmarks
{
    [Priority(10)]  // ensure this rule runs first
    public class CareAndTreatmentProgramAreaRule : Rule
    {
        public override void Define()
        {
            COPMatrixRow row = default;
            CareAndTreatmentParameters parameters = default;

            When()
                .Match(() => parameters, x => true)
                .Match(() => row,
                    r => parameters.ApplicableFundingSources.Contains(r.FundingSourceName) && r.MajorProgramArea != "PM");

            Then()
                .Do(_ => SetContribution(row, parameters))
                .Do(_ => SetPMDenominator(row));
        }

        // ReSharper disable once InconsistentNaming
        private static void SetPMDenominator(COPMatrixRow row)
        {
            row.CareAndTreatmentEarmarkAmounts.PMDenominatorAmount = row.ProposedAmount ?? 0;
        }

        private void SetContribution(COPMatrixRow row, CareAndTreatmentParameters parameters)
        {
            var percentMultiplier = 0f;
            if (parameters.ApplicableMajorProgramAreaIDs.Contains(row.MajorProgramAreaID) || parameters.ApplicableProgramAreaIDs.Contains(row.ProgramAreaID))
            {
                percentMultiplier = 100;
            }
            else if (row.MajorBeneficiary == "Pregnant & Breastfeeding Women")
            {
                percentMultiplier = 70;
            }
            else if (row.MajorProgramArea == "HTS")
            {
                percentMultiplier = 50;
            }

            row.CareAndTreatmentEarmarkAmounts.ContributionAmount = Convert.ToInt64((row.ProposedAmount ?? 0) * (percentMultiplier / 100f));
        }
    }

    [Priority(0)]
    public class ProgramManagementRule : Rule
    {
        public override void Define()
        {
            COPMatrixRow row = default;
            IEnumerable<COPMatrixRow> nonProgramManagementRows = default;

            When().Match(() => row, r => r.MajorProgramArea == "PM")
                .Query(() => nonProgramManagementRows,
                    x => x.Match<COPMatrixRow>(r => r.MajorProgramArea != "PM" ).Collect());

            Then().Do(_ => SetContribution(row, nonProgramManagementRows.ToList()));

        }

        private static void SetContribution(COPMatrixRow row, List<COPMatrixRow> nonProgramManagementRows)
        {
            var numerator = nonProgramManagementRows.Sum(r => r.CareAndTreatmentEarmarkAmounts.ContributionAmount) ;
            var denominator = nonProgramManagementRows.Sum(r => r.CareAndTreatmentEarmarkAmounts.PMDenominatorAmount) ;
            
            var amount = (float)numerator / denominator ;

            row.CareAndTreatmentEarmarkAmounts.ContributionAmount = Convert.ToInt64(amount * (row.ProposedAmount ?? 0));
        }
    }
}