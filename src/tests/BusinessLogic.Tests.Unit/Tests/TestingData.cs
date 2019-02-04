using BusinessLogic.DTO;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BusinessLogin.Unit.Tests
{
	internal class TestingData
	{
		public static IEnumerable<TestCaseData> EventsValid
		{
			get
			{
				yield return new TestCaseData(new EventDto
				{
					LayoutId = 1,
					Date = DateTime.Today.AddMonths(1).AddHours(18).AddMinutes(30),
                    CreatedBy = 1,
                    Description = "Solo-violin sonatas and partitas from Alina Ibragimova",
                    Title = "Late-night Bach: Proms",
                    ImageURL = "http://localhost:61962/Content/images/uploads/2.jpg"
                });
				yield return new TestCaseData(new EventDto
				{
					LayoutId = 5,
					Date = DateTime.Today.AddMonths(2).AddHours(8).AddMinutes(30),
                    CreatedBy = 1,
                    Description = "Conducted with perfect spaciousness by Edward Gardner, Richard Jones’s fabulous detailed and humane production of Wagner’s great comedy",
                    Title = "The Mastersingers of Nuremberg",
                    ImageURL = "http://localhost:61962/Content/images/uploads/4.jpg"
                });
				yield return new TestCaseData(new EventDto
				{
					LayoutId = 3,
					Date = DateTime.Today.AddMonths(1).AddHours(16),
                    CreatedBy = 1,
                    Description = "The highpoint of Andris Nelsons’ final season as the CBSO’s music director – a concert performance of Wagner’s final music drama of almost alarming maturity",
                    Title = "Parsifal",
                    ImageURL = "http://localhost:61962/Content/images/uploads/1.jpg"
                });
				yield return new TestCaseData(new EventDto
				{
					LayoutId = 1,
					Date = DateTime.Today.AddMonths(2).AddHours(8).AddMinutes(30),
                    CreatedBy = 1,
                    Description = "Conducted with perfect spaciousness by Edward Gardner, Richard Jones’s fabulous detailed and humane production of Wagner’s great comedy",
                    Title = "The Mastersingers of Nuremberg",
                    ImageURL = "http://localhost:61962/Content/images/uploads/4.jpg"
                });
				yield return new TestCaseData(new EventDto
				{
					LayoutId = 4,
					Date = DateTime.Today.AddMonths(2).AddHours(20).AddMinutes(30),
                    CreatedBy = 1,
                    Description = "The highpoint of Andris Nelsons’ final season as the CBSO’s music director – a concert performance of Wagner’s final music drama of almost alarming maturity",
                    Title = "Parsifal",
                    ImageURL = "http://localhost:61962/Content/images/uploads/1.jpg"
                });
			}
		}

		public static IEnumerable<TestCaseData> EventsDateIsNotValid
		{
			get
			{
				yield return new TestCaseData(new EventDto
				{
					LayoutId = 1,
					Date = DateTime.Today.AddMonths(1).AddHours(15).AddMinutes(30)
				});
				yield return new TestCaseData(new EventDto
				{
					LayoutId = 5,
					Date = DateTime.Today.AddMonths(1).AddHours(19).AddMinutes(30)
				});
				yield return new TestCaseData(new EventDto
				{
					LayoutId = 3,
					Date = DateTime.Today.AddMonths(2).AddHours(20)
				});
			}
		}

		public static IEnumerable<TestCaseData> EventInThePast
		{
			get
			{
				yield return new TestCaseData(new EventDto
				{
					LayoutId = 1,
					Date = DateTime.Today.AddMonths(-1).AddHours(15).AddMinutes(30)
				});
				yield return new TestCaseData(new EventDto
				{
					LayoutId = 5,
					Date = DateTime.Today.AddHours(-19).AddMinutes(30)
				});
				yield return new TestCaseData(new EventDto
				{
					LayoutId = 3,
					Date = DateTime.Today.AddMonths(-2).AddHours(20)
				});
				yield return new TestCaseData(new EventDto
				{
					LayoutId = 1,
					Date = DateTime.Today.AddMonths(-2).AddHours(-8).AddMinutes(30)
				});
			}
		}

		public static IEnumerable<TestCaseData> EventsByDateFilter
		{
			get
			{
				yield return new TestCaseData(new EventDto
				{
					LayoutId = 1,
					Date = DateTime.Today.AddMonths(1).AddHours(15).AddMinutes(30)
				});
				yield return new TestCaseData(new EventDto
				{
					LayoutId = 5,
					Date = DateTime.Today.AddMonths(2).AddHours(20)
				});
			}
		}
	}
}
