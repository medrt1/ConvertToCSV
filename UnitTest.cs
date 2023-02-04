using NUnit.Framework;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ConsoleAutomapper
{
    [TestFixture]
    public class Tester
    {
        Team alpha, bravo, delta;

        [OneTimeSetUp]
        public void Init()
        {
            alpha = new Team
            {
                Id = 1,
                Description = "DevOPS APP1",
                Members = new List<Member>()
            };

            bravo = new Team
            {
                Id = 2,
                Description = "Dev frontend APP1",
                Members = new List<Member>()
            };

            delta = new Team
            {
                Id = 3,
                Description = "Dev backoffice APP1",
                Members = new List<Member>()
            };

            alpha.Members.AddRange(new List<Member>{
                new Member
                {
                    Id = 1,
                    FirstName = "Joe",
                    LastName = "Dark",
                    DaysOfPresence = new List<DayOfWeek> {
                        { DayOfWeek.Saturday },
                        { DayOfWeek.Sunday }
                    }
                },
                new Member
                {
                    Id = 3,
                    FirstName = "Matin",
                    LastName = "Yellow",
                    DaysOfPresence = new List<DayOfWeek> {
                        { DayOfWeek.Friday},
                    }
                }
            });

            bravo.Members.AddRange(new List<Member>{
                new Member
                {
                    Id = 1,
                    FirstName = "Joe",
                    LastName = "Dark",
                    DaysOfPresence = new List<DayOfWeek> {
                        { DayOfWeek.Wednesday },
                    }
                }
            });

            delta.Members.AddRange(new List<Member>{
                new Member
                {
                    Id = 1,
                    FirstName = "Joe",
                    LastName = "Dark",
                    DaysOfPresence = new List<DayOfWeek> {
                        { DayOfWeek.Friday }
                    }
                },
                new Member
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Light",
                    DaysOfPresence = new List<DayOfWeek> {
                        {DayOfWeek.Monday},
                        {DayOfWeek.Tuesday},
                        {DayOfWeek.Wednesday},
                    }
                }
            });

        }

        [OneTimeTearDown]
        public void Cleanup()
        {

        }

        [Test]
        public void Test_team_mapper()
        {
            List<PresenceCalendar> dtoTeamCSV = ConvertToCSV.mapper.Map<List<PresenceCalendar>>(new List<Team> { alpha, bravo, delta });
            Assert.IsNotNull(dtoTeamCSV);
            Assert.IsTrue(dtoTeamCSV.Count == 5);

            var joeAlpha = dtoTeamCSV.Where(w=> w.TeamID==1 && w.MemberID==1).FirstOrDefault();
            Assert.IsTrue(joeAlpha.FirstName == "Joe");
            Assert.IsTrue(joeAlpha.LastName == "Dark");
            Assert.IsTrue(joeAlpha.Saturday == "P");
            Assert.IsTrue(joeAlpha.Sunday == "P");

            var joeBravo = dtoTeamCSV.Where(w => w.TeamID == 2 && w.MemberID == 1).FirstOrDefault();
            Assert.IsTrue(joeBravo.FirstName == "Joe");
            Assert.IsTrue(joeBravo.LastName == "Dark");
            Assert.IsTrue(joeBravo.Wednesday == "P");
            Assert.IsTrue(joeBravo.Monday == "");

            var janeDelta = dtoTeamCSV.Where(w => w.TeamID == 3 && w.MemberID == 2).FirstOrDefault();
            Assert.IsTrue(janeDelta.FirstName == "Jane");
            Assert.IsTrue(janeDelta.LastName == "Light");
            Assert.IsTrue(janeDelta.Wednesday == "P");
            Assert.IsTrue(janeDelta.Tuesday == "P");


        }

        [Test]
        public void Test_team_csv()
        {
            var csvMachine = new ConvertToCSV();

            var csv = csvMachine.TransformToCSV(new List<Team> { alpha, bravo, delta });
            Assert.IsNotNull(csv);            
            Assert.True(csv.Substring(0, 10).Contains("TeamID"));
        }

    }
}
