using Autofac;
using BusinessLogic.DTO;
using BusinessLogic.Exceptions;
using BusinessLogic.Services.Tests.DiContainer;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Tests
{
	internal class UserPurchaseProcess
	{
		private IContainer _container;
		private DeployDb dateBase;
		private UserDto _user;
		private EventSeatDto _seatToOrder;

		[OneTimeSetUp]
		public void Init()
		{
			dateBase = new DeployDb();
			dateBase.Deploy();

			_container = Container.GetContainer();
		}

		[Test, Order(1)]
		public void Registraion()
		{
			//Arrange
			var userService = _container.Resolve<IUserService>();
			_user = new UserDto
			{
				PasswordHash = "AL5sqo4EuA4QndxLBI76AP9qJhPKKsE4Z4BzPI9/dPfug71640OUJlF13BATwgq2ZA==",
				Culture = "en",
				Email = "superb@mail.ru",
				UserName = "vasia93",
				Timezone = "UTC",
				Salt = "xLBI76AP9qJhPKKsE4Z="
			};

			//Assert
			Assert.Multiple(async () =>
			{
				Assert.DoesNotThrowAsync(async () =>  await userService.Create(_user));
				Assert.IsNotNull(await  userService.Get(_user.Id));
			});
		}

		[Test, Order(2)]
		public async Task Update_user()
		{
			//Arrange
			var userService = _container.Resolve<IUserService>();
			_user.Surname = "Ivanov";
			_user.Firstname = "Dmitrii";

			//Act
			await userService.Update(_user);
			//Assert
			Assert.AreEqual(_user, await userService.Get(_user.Id));
		}

		[Test, Order(3)]
		public void Registraion_with_already_taken_username_exptected_exception()
		{
			//Arrange
			var userService = _container.Resolve<IUserService>();
			var user = new UserDto
			{
				PasswordHash = "AL5sqo4EuA4QndxLBI76AP9qJhPKKsE4Z4BzPI9/dPfug71640OUJlF13BATwgq2ZA==",
				Culture = "en",
				Email = "vanko@mail.ru",
				UserName = "vasia93"
			};

			//Act
			var exception = Assert.CatchAsync<UserException>(async () => await userService.Create(user));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("Username is already taken"));
		}

		[Test, Order(4)]
		public void Registraion_with_already_taken_email_exptected_exception()
		{
			//Arrange
			var userService = _container.Resolve<IUserService>();
			var user = new UserDto
			{
				PasswordHash = "AL5sqo4EuA4QndxLBI76AP9qJhPKKsE4Z4BzPI9/dPfug71640OUJlF13BATwgq2ZA==",
				Culture = "en",
				Email = "superb@mail.ru",
				UserName = "vasia95"
			};

			//Act
			var exception = Assert.CatchAsync<UserException>(async () => await userService.Create(user));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("Email is already taken"));
		}

		[Test, Order(5)]
		public async Task Add_seat_to_cart()
		{
			//Arrange
			var cartService = _container.Resolve<ICartService>();
			var seatService = _container.Resolve<IStoreService<EventSeatDto,int>>();
			var seatList = await seatService.GetList();
			_seatToOrder = seatList.ToList()[new Random().Next(seatList.Count())];

			//Assert
			Assert.DoesNotThrowAsync(async () => await cartService.AddSeat(_seatToOrder.Id, _user.Id));
		}

		[Test, Order(6)]
		public void Add_seat_to_cart_which_is_locked_expected_exception()
		{
			//Arrange
			var cartService = _container.Resolve<ICartService>();
			var seatService = _container.Resolve<IStoreService<EventSeatDto, int>>();

			//Act
			var exception = Assert.CatchAsync<CartException>(async () => await cartService.AddSeat(_seatToOrder.Id, _user.Id));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("Seat is locked"));
		}

		[Test, Order(7)]
		public async Task Get_ordered_seats()
		{
			//Arrange
			var cartService = _container.Resolve<ICartService>();

			//Act
			var purchaseList = await cartService.GetOrderedSeats(_user.Id);

			//Assert
			Assert.IsTrue(purchaseList.Any());
		}

		[Test, Order(8)]
		public void Complete_order_expected_exception()
		{
			//Arrange
			var orderService = _container.Resolve<IOrderService>();

			//Act
			var exception = Assert.CatchAsync<OrderException>(async () => await orderService.Create(_user.Id));

			//Assert
			Assert.That(exception.Message, Is.EqualTo("Balance of user is less than total amount of order"));
		}

		[Test, Order(9)]
		public async Task Complete_order()
		{
			//Arrange
			var userService = _container.Resolve<IUserService>();
			var orderService = _container.Resolve<IOrderService>();
			var amount = 100.50M;

			//Act
			_user.Amount += amount;
			await userService.Update(_user);

			//Assert
			Assert.DoesNotThrowAsync(async () => await orderService.Create(_user.Id));
		}

		[Test, Order(10)]
		public async Task Get_purchase_history()
		{
			//Arrange
			var orderService = _container.Resolve<IOrderService>();

			//Act
			var purchaseList = await orderService.GetPurchaseHistory(_user.Id);

			//Assert
			Assert.IsTrue(purchaseList.Any());
		}

		[Test, Order(11)]
		public async Task Refund_order()
		{
			//Arrange
			var orderService = _container.Resolve<IOrderService>();
			var purchaseHistory = await orderService.GetPurchaseHistory(_user.Id);
			var order = purchaseHistory.FirstOrDefault();

			//Act
			await orderService.CancelOrderAndRefund(order.Order.Id);
			purchaseHistory = await orderService.GetPurchaseHistory(_user.Id);

			//Assert
			Assert.IsFalse(purchaseHistory.Any(x => x.Order.Id == order.Order.Id));
		}

		[OneTimeTearDown]
		public void CleanUp()
		{
			dateBase.Drop();
		}
	}
}
