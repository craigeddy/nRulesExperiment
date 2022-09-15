using System.Collections.Generic;
using NRules;
using NRules.Fluent;
using nRulesExperiment.Earmarks;
using nRulesExperiment.Earmarks.AbstinenceRules;
using nRulesExperiment.Earmarks.CareAndTreatment;
using nRulesExperiment.Earmarks.OVCRules;
using NUnit.Framework;

namespace nRulesExperiment
{
    [TestFixture]
    public class TestFixture
    {
        private ISessionFactory _factory;

        [SetUp]
        public void Setup()
        {
            var repository = new RuleRepository();

            repository.Load(s => s
                .From(typeof(CareAndTreatmentProgramAreaRule), 
                    typeof(OvcRule),
                    typeof(ProgramManagementRule),
                    typeof(OvcProgramManagementRule),
                    typeof(AbstinenceNumeratorRule),
                    typeof(AbstinenceDenominatorRule)
                    ));

            //Compile rules
            _factory = repository.Compile();
        }

        [Test]
        public void AbstinenceNumerator()
        {
            var session = _factory.CreateSession();

            var row1 = new COPMatrixRow
            {
                ProposedAmount = 100,
                ProgramAreaID = 12,
                BeneficiaryID = 1,
                FundingSourceName = "GHP-USAID"
            };

            session.Insert(row1);

            var row2 = new COPMatrixRow
            {
                ProposedAmount = 100,
                ProgramAreaID = 12,
                BeneficiaryID = 2,
                FundingSourceName = "GHP-USAID",
                MajorProgramArea = "PREV",
                MajorBeneficiary = "Pregnant & Breastfeeding Women",
                MajorProgramAreaID = 3,

            };

            session.Insert(row2);

            var row3 = new COPMatrixRow
            {
                ProposedAmount = 100,
                ProgramAreaID = 14,
                BeneficiaryID = 3,
                FundingSourceName = "GHP-State"
            };

            session.Insert(row3);

            var row4 = new COPMatrixRow
            {
                ProposedAmount = 100,
                ProgramAreaID = 14,
                BeneficiaryID = 3,
                FundingSourceName = "ESF"
            };

            session.Insert(row4);

            var list = new List<AbstinenceNumeratorContribution>
            {
                new AbstinenceNumeratorContribution
                {
                    ApplicableBeneficiaries = new List<int>(new[] { 2, 4 }),
                    ApplicableProgramAreas = new List<int>(new[] { 12, 13 }),
                    ApplicableFundingSources = new List<string>(new []{ "GHP-State", "GHP-USAID" })
                },
                new AbstinenceNumeratorContribution
                {
                    ApplicableBeneficiaries = new List<int>(new[] { 3, 5 }),
                    ApplicableProgramAreas = new List<int>(new[] { 14, 15 }),
                    ApplicableFundingSources = new List<string>(new []{ "GHP-State", "GHP-USAID" })
                }
            };

            session.Insert(list);

            var denominatorParameters = new List<DenominatorContributionParameters>
            {
                new DenominatorContributionParameters
                {
                    ApplicableProgramAreas = new List<int>(new[] { 12, 13 }),
                    ApplicableFundingSources = new List<string>(new []{ "GHP-State", "GHP-USAID" })
                },
                new DenominatorContributionParameters
                {
                    ApplicableProgramAreas = new List<int>(new[] { 14, 15 }),
                    ApplicableFundingSources = new List<string>(new []{ "GHP-State", "GHP-USAID" }),
                    Multiplier = 0.5
                }
            };

            session.Insert(denominatorParameters);

            session.Fire();

            Assert.That(row1.AbstinenceEarmarkAmounts.NumeratorAmount, Is.EqualTo(0));
            Assert.That(row2.AbstinenceEarmarkAmounts.NumeratorAmount, Is.EqualTo(100));
            Assert.That(row3.AbstinenceEarmarkAmounts.NumeratorAmount, Is.EqualTo(100));
            Assert.That(row4.AbstinenceEarmarkAmounts.NumeratorAmount, Is.EqualTo(0));

            Assert.That(row2.AbstinenceEarmarkAmounts.DenominatorAmount, Is.EqualTo(100));
            Assert.That(row3.AbstinenceEarmarkAmounts.DenominatorAmount, Is.EqualTo(50));
            Assert.That(row4.AbstinenceEarmarkAmounts.DenominatorAmount, Is.EqualTo(0));
        }


        [Test]
        public void TheTest()
        {
            //Create a working session
            var session = _factory.CreateSession();

            var row1 = new COPMatrixRow
            {
                MajorProgramArea = "PREV",
                MajorBeneficiary = "Pregnant & Breastfeeding Women",
                FundingSourceName = "GHP-State",
                ProposedAmount = 300,
                MajorProgramAreaID = 3,
                ProgramAreaID = 27
            };

            var row2 = new COPMatrixRow
            {
                FundingSourceName = "GHP-State",
                MajorProgramArea = "C&T",
                ProposedAmount = 300,
                MajorProgramAreaID = 1,
                ProgramAreaID = 12
            };

            var row3 = new COPMatrixRow
            {
                FundingSourceName = "GHP-State",
                MajorProgramArea = "PM",
                ProposedAmount = 100,
                MajorProgramAreaID = 6,
                ProgramAreaID = 26
            };

            var row4 = new COPMatrixRow
            {
                FundingSourceName = "GHP-USAID",
                MajorProgramArea = "PREV",
                ProposedAmount = 300,
                MajorBeneficiary = "OVC",
                MajorProgramAreaID = 3,
                ProgramAreaID = 27
            };

            var row5 = new COPMatrixRow
            {
                FundingSourceName = "GHP-USAID",
                MajorProgramArea = "PM",
                ProposedAmount = 200,
                MajorProgramAreaID = 6,
                ProgramAreaID = 26
            };

            var row6 = new COPMatrixRow
            {
                FundingSourceName = "GHP-USAID",
                MajorProgramArea = "HTS",
                MajorBeneficiary = "OVC",
                ProposedAmount = 200,
                MajorProgramAreaID = 2,
                ProgramAreaID = 20
            };

            var row7 = new COPMatrixRow
            {
                FundingSourceName = "GHP-USAID",
                MajorProgramArea = "PREV",
                MajorBeneficiary = "OVC",
                ProposedAmount = 200,
                InitiativeName = "DREAMS",
                MajorProgramAreaID = 3,
                ProgramAreaID = 27
            };

            var row8 = new COPMatrixRow
            {
                FundingSourceName = "ESF",
                MajorProgramArea = "C&T",
                MajorBeneficiary = "OVC",
                ProposedAmount = 200,
                MajorProgramAreaID = 1,
                ProgramAreaID = 12
            };

            session.Insert(row1);
            session.Insert(row2);
            session.Insert(row3);
            session.Insert(row4);
            session.Insert(row5);
            session.Insert(row6);
            session.Insert(row7);
            session.Insert(row8);

            var parameters = new CareAndTreatmentParameters
            {
                ApplicableMajorProgramAreaIDs = new List<int>(new[] { 1 }),
                ApplicableProgramAreaIDs = new List<int>(new []{ 5 }),
                ApplicableFundingSources = new List<string>(new [] { "GHP-State", "GHP-USAID" })
            };
            
            session.Insert(parameters);

            var ovcParameters = new OVCParameters()
            {
                ApplicableFundingSources = new List<string>(new[] { "GHP-State", "GHP-USAID" })
            };
            
            session.Insert(ovcParameters);

            //Start match/resolve/act cycle
            session.Fire();

            Assert.That(row1.CareAndTreatmentEarmarkAmounts.ContributionAmount, Is.EqualTo(row1.ProposedAmount * 0.7),
                "C&T should be 70% of the budget if the beneficiary is Pregnant & Breastfeeding Women");

            Assert.That(row2.CareAndTreatmentEarmarkAmounts.ContributionAmount, Is.EqualTo(row2.ProposedAmount),
                "C&T should be 100% of the initiative budget if the major program is C&T or ASP");

            Assert.That(row2.CareAndTreatmentEarmarkAmounts.PMDenominatorAmount, Is.EqualTo(row2.ProposedAmount),
                "Row should contribute 100% of the initiative budget to the PM denominator if the major program is C&T or ASP");

            Assert.That(row3.CareAndTreatmentEarmarkAmounts.ContributionAmount, Is.EqualTo(47), "PM contribution not calculated correctly for C&T");
            Assert.That(row3.OvcEarmarkAmounts.ContributionAmount, Is.EqualTo(row3.ProposedAmount), "PM contribution not calculated correctly for OVC");

            Assert.That(row8.CareAndTreatmentEarmarkAmounts.ContributionAmount, Is.EqualTo(0), "Funding source most be GHP-State or GHP-USAID");
            Assert.That(row8.CareAndTreatmentEarmarkAmounts.PMDenominatorAmount, Is.EqualTo(0), "Funding source most be GHP-State or GHP-USAID");
            Assert.That(row8.OvcEarmarkAmounts.ContributionAmount, Is.EqualTo(0), "Funding source most be GHP-State or GHP-USAID");
            Assert.That(row8.OvcEarmarkAmounts.PMDenominatorAmount, Is.EqualTo(0), "Funding source most be GHP-State or GHP-USAID");
        }
    }
}