using DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Tests.Unit.FakeRepositories.Data
{
	internal class FakeUserData
	{
		public static List<User> Users()
		{
			return new List<User>
			{
				new User { Id = 1,
					PasswordHash ="AJIgHBUxxdjDCQnGdWpOduEHY3H9njwFR7xQiv+W/S71mnfHtnL+FakAla8OLctSPw==",
				Amount = 0M, UserName = "admin", Email ="admin_vasa@outlook.com", Culture = "ru",
					Timezone = TimeZoneInfo.Utc.Id},
				new User { Id = 2,
					PasswordHash ="AL5sqo4EuA4QndxLBI76AP9qJhPKKsE4Z4BzPI9/dPfug71640OUJlF13BATwgq2ZA==",
				Amount = 1150.23M, UserName = "lena", Email ="lena@gmail.com", Culture = "ru",
					Timezone = TimeZoneInfo.Utc.Id},
				new User { Id = 3,
					PasswordHash ="AL5sqo6EuA4QndxLsI76AP94JhPKKsE4Z4BzPI3/dPfug71640OUJlF13scTwgq2ZA==",
				Amount = 123.23M, UserName = "event_manager", Email ="ivanov@gmail.com", Culture = "be",
					Timezone = TimeZoneInfo.Utc.Id}
			};
		}

		public static List<Role> Roles()
		{
			return new List<Role>
			{
				new Role{ Id =1, Name = "User"},
				new Role{ Id =2, Name = "Venue manager"},
				new Role{ Id =3, Name = "Event manager"}
			};
		}

		public static List<UserRole> UserRoles()
		{
			return new List<UserRole>
			{
				new UserRole{ RoleId = 2, UserId = 1},
				new UserRole{ RoleId = 1, UserId = 1},
				new UserRole{ RoleId = 1, UserId = 2},
				new UserRole{ RoleId = 1, UserId = 3}
			};
		}

		public static List<PurchasedSeat> PurchasedSeats()
		{
			return new List<PurchasedSeat>
			{
				new PurchasedSeat {SeatId = 15, OrderId = 1, Price = 45.7M },
				new PurchasedSeat {SeatId = 16, OrderId = 2, Price = 145.25M }
			};
		}

		public static List<Order> Orders()
		{
			return new List<Order>
			{
				new Order { Id = 1, Date = DateTimeOffset.UtcNow.AddHours(2).AddMinutes(45),
				UserId = 1 },
				new Order { Id = 2, Date = DateTimeOffset.UtcNow.AddDays(2).AddHours(6).AddMinutes(45),
				UserId = 2 },
			};
		}

		public static List<OrderedSeat> OrderedSeats()
		{
			return new List<OrderedSeat>
			{
				new OrderedSeat {SeatId = 5,CartId = 1 },
				new OrderedSeat {SeatId = 13,CartId = 2 }
			};
		}

		public static List<Cart> Carts()
		{
			return new List<Cart>
			{
				new Cart { Id = 1, UserId = 2},
				new Cart { Id = 2, UserId = 1},
			};
		}
	}
}
