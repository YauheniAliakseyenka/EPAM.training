using BusinessLogic.DTO;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
					Date = DateTime.Today.AddMonths(1).AddHours(18).AddMinutes(30)
				});
				yield return new TestCaseData(new EventDto
				{
					LayoutId = 5,
					Date = DateTime.Today.AddMonths(2).AddHours(8).AddMinutes(30)
				});
				yield return new TestCaseData(new EventDto
				{
					LayoutId = 3,
					Date = DateTime.Today.AddMonths(1).AddHours(16)
				});
				yield return new TestCaseData(new EventDto
				{
					LayoutId = 1,
					Date = DateTime.Today.AddMonths(2).AddHours(8).AddMinutes(30)
				});
				yield return new TestCaseData(new EventDto
				{
					LayoutId = 4,
					Date = DateTime.Today.AddMonths(2).AddHours(20).AddMinutes(30)
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
	}
}
