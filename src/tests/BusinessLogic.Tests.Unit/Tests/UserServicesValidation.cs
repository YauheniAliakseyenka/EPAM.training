using Autofac;
using BusinessLogic.DTO;
using BusinessLogic.Exceptions;
using BusinessLogic.Helpers;
using BusinessLogic.Services;
using BusinessLogic.Tests.Unit.DiContainer;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogin.Unit.Tests
{
	internal class UserServicesValidation 
	{
		private IContainer _container;

		[SetUp]
		public void Init()
		{
            _container = Container.GetContainer();
		}

        [Test]
        public void Add_seat_to_cart_user_id_is_null_throws_exception()
        {
			//Arrange
			var cartService = _container.Resolve<ICartService>();

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await cartService.AddSeat(3, 0));
        }

        [Test]
        public void Attempt_of_adding_seat_which_is_already_locked_expected_exception()
        {
            //Arrange
            var cartService = _container.Resolve<ICartService>();

			//Act
			var exception = Assert.CatchAsync<CartException>(async () => await cartService.AddSeat(13, 3));

            //Assert
            StringAssert.AreEqualIgnoringCase(exception.Message, "Seat is locked");
        }

        [Test]
        public void Adding_of_seat_to_cart()
        {
            //Arrange
            var cartService = _container.Resolve<ICartService>();

			//Assert
			Assert.DoesNotThrowAsync(async () => await cartService.AddSeat(4, 3));
        }

        [Test]
        public void Unlock_seat_invalid_id_expected_exception()
        {
            //Arrange
            var cartService = _container.Resolve<ICartService>();
			
			//Assert
			Assert.ThrowsAsync<ArgumentException>(async () => await cartService.DeleteSeat(-1));
        }

		[Test]
		public void Unlock_seat()
		{
            //Arrange
            var cartService = _container.Resolve<ICartService>();

            //Assert
            Assert.DoesNotThrowAsync(async () => await cartService.DeleteSeat(13));
		}

		[Test]
		public void Create_order_uer_id_equals_zero()
		{
            //Arrange
            var orderService = _container.Resolve<IOrderService>();

			//Assert
			Assert.ThrowsAsync<ArgumentException>(async () => await orderService.Create(-1));
		}

        [Test]
        public void Create_order__balance_of_account_of_user_is_less_than_total_amount_expected_exception()
        {
            //Arrange
            var orderService = _container.Resolve<IOrderService>();

            //Act
            var exception = Assert.CatchAsync<OrderException>(async () => await orderService.Create(1));

            //Assert
            StringAssert.AreEqualIgnoringCase(exception.Message, "Balance of user is less than total amount of order");
        }

        [Test]
        public void Complete_order()
        {
            //Arrange
            var orderService = _container.Resolve<IOrderService>();

            //Assert
            Assert.DoesNotThrowAsync(async () => await orderService.Create(2));
        }

		[Test]
		public void Create_user()
		{
			//Arrange
			var userService = _container.Resolve<IUserService>();
			var user = new UserDto
			{
				PasswordHash = "AL5sqo4EuA4QndxLBI76AP9qJhPKKsE4Z4BzPI9/dPfug71640OUJlF13BATwgq2ZA==",
				Culture = "en",
				Email = "superb@mail.ru",
				UserName = "vasia93",
				Timezone = "UTC"
			};

			//Assert
			Assert.DoesNotThrowAsync(async () => await userService.Create(user));
		}

		[Test]
		public void Create_user_email_is_taken_expected_exception()
		{
			//Arrange
			var userService = _container.Resolve<IUserService>();
			var user = new UserDto
			{
				PasswordHash = "AL5sqo4EuA4QndxLBI76AP9qJhPKKsE4Z4BzPI9/dPfug71640OUJlF13BATwgq2ZA==",
				Culture = "en",
				Email = "admin_vasa@outlook.com",
				UserName = "vasia93",
				Timezone = "UTC"
			};

			//Act
			var exception = Assert.CatchAsync<UserException>(async () => await userService.Create(user));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Email is already taken");
		}

		[Test]
		public void Create_user_username_is_taken_expected_exception()
		{
			//Arrange
			var userService = _container.Resolve<IUserService>();
			var user = new UserDto
			{
				PasswordHash = "AL5sqo4EuA4QndxLBI76AP9qJhPKKsE4Z4BzPI9/dPfug71640OUJlF13BATwgq2ZA==",
				Culture = "en",
				Email = "superb@mail.ru",
				UserName = "lena",
				Timezone = "UTC"
			};

			//Act
			var exception = Assert.CatchAsync<UserException>(async () => await userService.Create(user));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Username is already taken");
		}

		[Test]
		public void Refund_order_expected_exception()
		{
			//Arrange
			var orderService = _container.Resolve<IOrderService>();
			var orderId = 4; //id of any order which doesn't exist

			//Act
			var exception = Assert.CatchAsync<OrderException>(async () => await orderService.CancelOrderAndRefund(orderId));

			//Assert
			StringAssert.AreEqualIgnoringCase(exception.Message, "Order does not exist");
		}

		[Test]
		public async Task Refund_order()
		{
			//Arrange
			var orderService = _container.Resolve<IOrderService>();
			var userId = 1; //id of user from fake repositorty
			var purchaseHistory = await orderService.GetPurchaseHistory(userId);
			var order = purchaseHistory.FirstOrDefault();

			//Assert
			Assert.DoesNotThrowAsync(async () => await orderService.CancelOrderAndRefund(order.Order.Id));
		}

		[Test]
		public async Task Add_Role()
		{
			//Arrange
			var userService = _container.Resolve<IUserService>();
			var user = await userService.Get(2);

			//Assert
			Assert.Multiple(async () =>
			{
				Assert.DoesNotThrowAsync(async () => await userService.AddRole(user, Role.VenueManager));
				var roles = await userService.GetRoles(user.UserName);

				Assert.IsTrue(roles.Any(x => x.Equals(GetEnumItemDescription.GetEnumDescription(Role.VenueManager), StringComparison.OrdinalIgnoreCase)));
			});
		}

		[Test]
		public async Task Delete_Role()
		{
			//Arrange
			var userService = _container.Resolve<IUserService>();
			var user = await userService.Get(1);

			//Assert
			Assert.Multiple(async () =>
			{
				Assert.DoesNotThrowAsync(async () => await userService.DeleteRole(user, Role.VenueManager));
				var roles = await userService.GetRoles(user.UserName);

				Assert.IsFalse(roles.Any(x => x.Equals(GetEnumItemDescription.GetEnumDescription(Role.VenueManager), StringComparison.OrdinalIgnoreCase)));
			});
		}
	}
}
