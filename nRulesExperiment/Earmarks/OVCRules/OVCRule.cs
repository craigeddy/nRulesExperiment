using System;
using System.Collections.Generic;
using System.Linq;
using NRules.Fluent.Dsl;

namespace nRulesExperiment.Earmarks.OVCRules
{
    [Priority(10)]  // ensure this rule runs first
    public class OvcRule: Rule
    {
        public override void Define()
        {
            COPMatrixRow row = default;
            OVCParameters parameters = default;

            When()
                .Match(() => parameters, r => true)
                .Match(() => row, r => parameters.ApplicableFundingSources.Contains(r.FundingSourceName) &&
                                       !parameters.ExcludedProgramAreas.Contains(r.MajorProgramArea) &&
                                       (r.MajorBeneficiary == parameters.MajorBeneficiary || 
                                        (r.InitiativeName.StartsWith("DREAMS") && r.MajorBeneficiary != parameters.MajorBeneficiary)));

            Then()
                .Do(_ => SetContribution(row, parameters))
                .Do(_ => SetPMDenominator(row));
        }

        // ReSharper disable once InconsistentNaming
        private static void SetPMDenominator(COPMatrixRow row)
        {
            row.OvcEarmarkAmounts.PMDenominatorAmount = row.ProposedAmount ?? 0;
        }

        private void SetContribution(COPMatrixRow row, OVCParameters parameters)
        {
            var percentMultiplier = 100f;
            if (row.InitiativeName == "DREAMS" && row.MajorBeneficiary != parameters.MajorBeneficiary)
            {
                percentMultiplier = 85;
            }

            row.OvcEarmarkAmounts.ContributionAmount = Convert.ToInt64((row.ProposedAmount ?? 0) * (percentMultiplier / 100f));
        }
    }

    [Priority(0)]
    public class OvcProgramManagementRule : Rule
    {
        public override void Define()
        {
            COPMatrixRow row = default;
            IEnumerable<COPMatrixRow> nonProgramManagementRows = default;

            When().Match(() => row, r => r.MajorProgramArea == "PM")
                .Query(() => nonProgramManagementRows,
                    x => x.Match<COPMatrixRow>(r => r.MajorProgramArea != "PM").Collect());

            Then().Do(_ => SetContribution(row, nonProgramManagementRows.ToList()));

        }

        private static void SetContribution(COPMatrixRow row, List<COPMatrixRow> nonProgramManagementRows)
        {
            var numerator = nonProgramManagementRows.Sum(r => r.OvcEarmarkAmounts.ContributionAmount);
            var denominator = nonProgramManagementRows.Sum(r => r.OvcEarmarkAmounts.PMDenominatorAmount);

            var amount = (float)numerator / denominator;

            row.OvcEarmarkAmounts.ContributionAmount = Convert.ToInt64(amount * (row.ProposedAmount ?? 0));
        }
    }
}