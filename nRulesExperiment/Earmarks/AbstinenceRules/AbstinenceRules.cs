using System;
using System.Collections.Generic;
using System.Linq;
using NRules.Fluent.Dsl;

namespace nRulesExperiment.Earmarks.AbstinenceRules
{
    public class AbstinenceNumeratorRule : Rule
    {
        public override void Define()
        {
            COPMatrixRow row = default;
            IEnumerable<AbstinenceNumeratorContribution> contributions = default;

            When()
                .Match(() => contributions, x => true) // take all the contributions
                .Match(() => row, // and match rows that have matching ProgramAreaID, BeneficiaryID, and Funding Source
                    matrixRow => contributions.ToList().Any(c =>
                        c.ApplicableProgramAreas.Contains(matrixRow.ProgramAreaID) &&
                        c.ApplicableBeneficiaries.Contains(matrixRow.BeneficiaryID) &&
                        c.ApplicableFundingSources.Contains(matrixRow.FundingSourceName)));

            Then()
                .Do(_ => SetNumerator(row, 
                    contributions.First(c => c.ApplicableProgramAreas.Contains(row.ProgramAreaID) && 
                                             c.ApplicableBeneficiaries.Contains(row.BeneficiaryID) && 
                                             c.ApplicableFundingSources.Contains(row.FundingSourceName))));

        }

        private void SetNumerator(COPMatrixRow row, AbstinenceNumeratorContribution contribution)
        {
            row.AbstinenceEarmarkAmounts.NumeratorAmount = Convert.ToInt64(Math.Round((row.ProposedAmount ?? 0) * contribution.Multiplier));
        }
    }

    public class AbstinenceDenominatorRule: Rule
    {
        public override void Define()
        {
            COPMatrixRow row = default;
            IEnumerable<DenominatorContributionParameters> contributions = default;

            When()
                .Match(() => contributions, x => true) // take all the contributions
                .Match(() => row, // and match rows that have matching ProgramAreaID and Funding Source
                    matrixRow => contributions.ToList().Any(c =>
                        c.ApplicableProgramAreas.Contains(matrixRow.ProgramAreaID) &&
                        c.ApplicableFundingSources.Contains(matrixRow.FundingSourceName)));

            Then()
                .Do(_ => SetNumerator(row, 
                    contributions.First(c => c.ApplicableProgramAreas.Contains(row.ProgramAreaID) && 
                                             c.ApplicableFundingSources.Contains(row.FundingSourceName))));

        }

        private void SetNumerator(COPMatrixRow row, DenominatorContributionParameters contribution)
        {
            row.AbstinenceEarmarkAmounts.DenominatorAmount = Convert.ToInt64(Math.Round((row.ProposedAmount ?? 0) * contribution.Multiplier));
        }

    }
}