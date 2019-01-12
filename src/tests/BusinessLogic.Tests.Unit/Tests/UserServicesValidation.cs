using Autofac;
using BusinessLogic.DTO;
using BusinessLogic.Exceptions;
using BusinessLogic.Services;
using BusinessLogic.Tests.Unit.DiContainer;
using NUnit.Framework;
using System;

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
            Assert.ThrowsAsync<ArgumentNullException>(async () => await cartService.AddSeat(3, null));
        }

        [Test]
        public void Add_seat_to_cart_seat_id_equals_zero_throws_exception()
        {
            //Arrange
            var cartService = _container.Resolve<ICartService>();

            //Act
            var exception = Assert.CatchAsync<CartException>(async () => await cartService.AddSeat(0, "AL5sqo4EuA4QndxLBI76AP9qJhPKKsE4Z4BzPI9/dPfug71640OUJlF13BATwgq2ZA=="));

            //Assert
            StringAssert.AreEqualIgnoringCase(exception.Message, "SeadId is invalid");

        }

        [Test]
        public void Attempt_of_adding_seat_which_is_already_locked_expected_exception()
        {
            //Arrange
            var cartService = _container.Resolve<ICartService>();

            //Act
            var exception = Assert.CatchAsync<CartException>(async () => await cartService.AddSeat(13, "c4195241-4241-se52-95a2-71dsa105cbaq"));

            //Assert
            StringAssert.AreEqualIgnoringCase(exception.Message, "Seat is locked");
        }

        [Test]
        public void Adding_of_seat_to_cart()
        {
            //Arrange
            var cartService = _container.Resolve<ICartService>();

            //Assert
            Assert.DoesNotThrowAsync(async () => await cartService.AddSeat(4, "AL5sqo4EuA4QndxLBI76AP9qJhPKKsE4Z4BzPI9/dPfug71640OUJlF13BATwgq2ZA=="));
        }

        [Test]
        public void Delete_cart_of_user_user_id_is_null_throws_exception()
        {
            //Arrange
            var cartService = _container.Resolve<ICartService>();

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await cartService.DeleteUserCart(null));
        }

        [Test]
        public void Delete_cart_of_user_cart_does_not_exists_expected_exception()
        {
            //Arrange
            var cartService = _container.Resolve<ICartService>();

            //Act
            var exception = Assert.CatchAsync<CartException>(async () => await cartService.DeleteUserCart("c4195241-4241-se52-95a2-71dsa105cbaq"));

            //Assert
            StringAssert.AreEqualIgnoringCase(exception.Message, "Cart for this user does not exists");
        }

        [Test]
        public void Delete_cart_of_user()
        {
            //Arrange
            var cartService = _container.Resolve<ICartService>();

            //Assert
            Assert.DoesNotThrowAsync(async () => await cartService.DeleteUserCart("c4195241-4621-4e52-95a1-714d2f005cbb"));
        }

        [Test]
        public void Unlock_seat_invalid_id_expected_exception()
        {
            //Arrange
            var cartService = _container.Resolve<ICartService>();

            //Act
            var exception = Assert.CatchAsync<CartException>(async () => await cartService.DeleteSeat(-1));

            //Assert
            StringAssert.AreEqualIgnoringCase(exception.Message, "SeadId is invalid");
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
            Assert.ThrowsAsync<ArgumentNullException>(async () => await orderService.Create(null));
		}


        [Test]
        public void Create_order__balance_of_account_of_user_is_less_than_total_amount_expected_exception()
        {
            //Arrange
            var orderService = _container.Resolve<IOrderService>();

            //Act
            var exception = Assert.CatchAsync<OrderException>(async () => await orderService.Create("57b9433e-78ac-4619-91eb-dd7a5c130f08"));

            //Assert
            StringAssert.AreEqualIgnoringCase(exception.Message, "Balance of user is less than total amount of order");
        }

        [Test]
        public void Complete_order()
        {
            //Arrange
            var orderService = _container.Resolve<IOrderService>();

            //Assert
            Assert.DoesNotThrowAsync(async () => await orderService.Create("c4195241-4621-4e52-95a1-714d2f005cbb"));
        }

		[Test]
		public void Create_user()
		{
			//Arrange
			var userService = _container.Resolve<IUserService>();
			var user = new UserDto
			{
				PasswordHash = "AL5sqo4EuA4QndxLBI76AP9qJhPKKsE4Z4BzPI9/dPfug71640OUJlF13BATwgq2ZA==",
				Id = "57b9433e-78ac-4619-91eb-dd7a5c130f08",
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
				Id = "57b9433e-78ac-4619-91eb-dd7a5c130f08",
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
				Id = "57b9433e-78ac-4619-91eb-dd7a5c130f08",
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
	}
}
