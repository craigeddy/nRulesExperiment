using NRules;
using NRules.Fluent;
using nRulesExperiment.Earmarks;
using NUnit.Framework;

namespace nRulesExperiment
{
    internal class Program
    {
        static void Main()
        {
            //Load rules
            var repository = new RuleRepository();

            repository.Load(x => x.From(typeof(CareAndTreatmentProgramAreaRule).Assembly));

            //Compile rules
            var factory = repository.Compile();

            //Create a working session
            var session = factory.CreateSession();

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

            //Start match/resolve/act cycle
            session.Fire();

            // check the results
        }
    }
}
