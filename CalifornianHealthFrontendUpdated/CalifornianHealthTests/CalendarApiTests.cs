using CalifornianHealthLib.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CalifornianHealthFrontendUpdated.CalifornianHealthTests;

[TestFixture]
    public class CalendarApiTests
    {
        private const string CalendarApiUri = "https://localhost:44366/api/consultants";

        [TestCase(1)]
        [TestCase(50)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(3000)] 
        public void Should_return_expected_list_of_consultants_when_get_endpoints_is_called(int executionCounter)
        {
            var expectedConsultantList = new List<Consultant>()
            {
                new Consultant
                {
                    Id = 1,
                    FName = "Jessica",
                    LName = "Wally",
                    Speciality = "Cardiologist",
                },
                new Consultant()
                {
                    Id = 2,
                    FName = "Iai",
                    LName = "Donnas",
                    Speciality = "General Surgeon",
                },
                new Consultant()
                {
                    Id = 3,
                    FName = "Amanda",
                    LName = "Denyl",
                    Speciality = "Doctor",
                },
                new Consultant()
                {
                    Id = 4,
                    FName = "Jason",
                    LName = "Davis",
                    Speciality = "Cardiologist",
                }
            };

            for (int j = 0; j < executionCounter; j++)
            {
                var response = new HttpClient().GetAsync(CalendarApiUri);
                var apiResponse = response.Result.Content.ReadAsStringAsync().Result;
                var consultantListFromApi = JsonConvert.DeserializeObject<List<Consultant>>(apiResponse);

                Assert.That(response.Result.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
                Assert.That(response.Result.Content.ReadAsStringAsync().Result, Is.Not.Null);
                Assert.That(consultantListFromApi.Count, Is.EqualTo(expectedConsultantList.Count));

                for (var i = 0; i < expectedConsultantList.Count; i++)
                {
                    Assert.That(consultantListFromApi[i].Id, Is.EqualTo(expectedConsultantList[i].Id));
                    Assert.That(consultantListFromApi[i].FName, Is.EqualTo(expectedConsultantList[i].FName));
                    Assert.That(consultantListFromApi[i].LName, Is.EqualTo(expectedConsultantList[i].LName));
                    Assert.That(consultantListFromApi[i].Speciality, Is.EqualTo(expectedConsultantList[i].Speciality));
                }
            }
        }
        
        [TestCase(1)]
        [TestCase(50)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(3000)]
        public void Should_return_expected_consultant_when_get_by_id_called(int executionCounter)
        {
            var expectedConsultant = new Consultant()
            {
                Id = 1,
                FName = "Jessica",
                LName = "Wally",
                Speciality = "Cardiologist",
            };
            for (int i = 0; i < executionCounter; i++)
            {
                var response = new HttpClient().GetAsync(CalendarApiUri + "/" + expectedConsultant.Id);
                var apiResponse = response.Result.Content.ReadAsStringAsync().Result;
                var consultantFromApi = JsonConvert.DeserializeObject<Consultant>(apiResponse);

                Assert.That(response.Result.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
                Assert.That(response.Result.Content.ReadAsStringAsync().Result, Is.Not.Null);
                Assert.That(consultantFromApi.Id, Is.EqualTo(expectedConsultant.Id));
                Assert.That(consultantFromApi.FName, Is.EqualTo(expectedConsultant.FName));
                Assert.That(consultantFromApi.LName, Is.EqualTo(expectedConsultant.LName));
                Assert.That(consultantFromApi.Speciality, Is.EqualTo(expectedConsultant.Speciality));
            }
        }

        [TestCase(1, 1)]
        [TestCase(2, 100)]
        [TestCase(3, 1000)]
        [TestCase(4, 3000)]
        public void Should_return_expected_consultant_availabilities_when_called(int expectedConsultantId, int executionCounter)
        {
                var expectedConsultantCalendar = new List<ConsultantCalendar>()
                {
                    new ConsultantCalendar()
                    {
                        Id = 1,
                        ConsultantId = 1,
                        Date = new DateTime(2023, 10, 04),
                        Available = true

                    },
                    new ConsultantCalendar()
                    {
                        Id = 2,
                        ConsultantId = 1,
                        Date = new DateTime(2023, 10, 15),
                        Available = true

                    },
                    new ConsultantCalendar()
                    {
                        Id = 3,
                        ConsultantId = 1,
                        Date = new DateTime(2023, 10, 22),
                        Available = true
                    },
                    new ConsultantCalendar()
                    {
                        Id = 4,
                        ConsultantId = 1,
                        Date = new DateTime(2023, 10, 25),
                        Available = true
                    },
                    new ConsultantCalendar()
                    {
                        Id = 5,
                        ConsultantId = 1,
                        Date = new DateTime(2023, 10, 28),
                        Available = true
                    },
                    new ConsultantCalendar()
                    {
                        Id = 6,
                        ConsultantId = 2,
                        Date = new DateTime(2023, 10, 14),
                        Available = true
                    },
                    new ConsultantCalendar()
                    {
                        Id = 7,
                        ConsultantId = 2,
                        Date = new DateTime(2023, 10, 29),
                        Available = true
                    },
                    new ConsultantCalendar()
                    {
                        Id = 8,
                        ConsultantId = 2,
                        Date = new DateTime(2023, 10, 30),
                        Available = true
                    },
                    new ConsultantCalendar()
                    {
                        Id = 9,
                        ConsultantId = 3,
                        Date = new DateTime(2023, 10, 01),
                        Available = true
                    },
                    new ConsultantCalendar()
                    {
                        Id = 10,
                        ConsultantId = 3,
                        Date = new DateTime(2023, 10, 02),
                        Available = true
                    },
                    new ConsultantCalendar()
                    {
                        Id = 11,
                        ConsultantId = 3,
                        Date = new DateTime(2023, 10, 03),
                        Available = true
                    },
                    new ConsultantCalendar()
                    {
                        Id = 12,
                        ConsultantId = 3,
                        Date = new DateTime(2023, 10, 09),
                        Available = true
                    },
                    new ConsultantCalendar()
                    {
                        Id = 13,
                        ConsultantId = 3,
                        Date = new DateTime(2023, 10, 10),
                        Available = true
                    },
                    new ConsultantCalendar()
                    {
                        Id = 14,
                        ConsultantId = 4,
                        Date = new DateTime(2023, 10, 11),
                        Available = true
                    },
                    new ConsultantCalendar()
                    {
                        Id = 15,
                        ConsultantId = 4,
                        Date = new DateTime(2023, 10, 21),
                        Available = true
                    }
                };

                
                for (int i = 0; i < executionCounter; i++)
                {
                    var response =
                        new HttpClient().GetAsync(CalendarApiUri + "/" + expectedConsultantId +
                                                  "/ConsultantAvailabilities");
                    var apiResponse = response.Result.Content.ReadAsStringAsync().Result;
                    var consultantFromApi = JsonConvert.DeserializeObject<List<ConsultantCalendar>>(apiResponse);


                    Assert.That(response.Result.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
                    Assert.That(response.Result.Content.ReadAsStringAsync().Result, Is.Not.Null);

                    foreach (var c in consultantFromApi)
                    {
                        Assert.That(c.Id, Is.EqualTo(expectedConsultantCalendar[c.Id - 1].Id));
                        Assert.That(c.ConsultantId, Is.EqualTo(expectedConsultantCalendar[c.Id - 1].ConsultantId));
                        Assert.That(c.Date, Is.EqualTo(expectedConsultantCalendar[c.Id - 1].Date));
                    }
                }
        }
    }