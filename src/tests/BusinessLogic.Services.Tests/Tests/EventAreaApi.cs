using Autofac;
using BusinessLogic.DTO;
using BusinessLogic.Exceptions.EventExceptions;
using BusinessLogic.Services.Tests.DiContainer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Tests
{
    internal class EventAreaApi
    {
        private IStoreService<EventAreaDto, int> _eventAreaService;
        private DeployDb dateBase;
        private EventAreaDto _area;

        [OneTimeSetUp]
        public void Init()
        {
            dateBase = new DeployDb();
            dateBase.Deploy();

            _eventAreaService = Container.GetContainer().Resolve<IStoreService<EventAreaDto, int>>();
        }

        [Test, Order(1)]
        public void Create_area()
        {
            //Arrange
            _area = new EventAreaDto
            {
                CoordX = 1,
                CoordY = 2,
                Description = "Area #5",
                EventId = 1,
                Seats = new List<EventSeatDto>
                {
                    new EventSeatDto { Row = 1, Number = 1, State = 0},
                    new EventSeatDto { Row = 1, Number = 2, State = 0},
                    new EventSeatDto { Row = 1, Number = 3, State = 0},
                    new EventSeatDto { Row = 2, Number = 1, State = 0},
                    new EventSeatDto { Row = 2, Number = 2, State = 0}
                }
            };

            //Assert
            Assert.DoesNotThrowAsync(async () => await _eventAreaService.Create(_area));
        }

        [Test, Order(2)]
        public void Create_area_with_event_id_equals_zero_expected_exception()
        {
            //Arrange
            var area = new EventAreaDto
            {
                CoordX = 1,
                CoordY = 2,
                Description = "Area #5",
                EventId = 0,
                Seats = new List<EventSeatDto>
                {
                    new EventSeatDto { Row = 1, Number = 1, State = 0},
                    new EventSeatDto { Row = 1, Number = 2, State = 0},
                    new EventSeatDto { Row = 1, Number = 3, State = 0},
                    new EventSeatDto { Row = 2, Number = 1, State = 0},
                    new EventSeatDto { Row = 2, Number = 2, State = 0}
                }
            };

            //Act
            var exception = Assert.CatchAsync<EventAreaException>(async () => await _eventAreaService.Create(area));

            //Assert
            Assert.That(exception.Message, Is.EqualTo("EventId is invalid"));
        }

        [Test, Order(3)]
        public void Create_area_with_description_that_already_exists_expected_exception()
        {
            //Arrange
            var area = new EventAreaDto
            {
                CoordX = 1,
                CoordY = 2,
                Description = "The area #2",
                EventId = 1,
                Seats = new List<EventSeatDto>
                {
                    new EventSeatDto { Row = 1, Number = 1, State = 0},
                    new EventSeatDto { Row = 1, Number = 2, State = 0},
                    new EventSeatDto { Row = 1, Number = 3, State = 0},
                    new EventSeatDto { Row = 2, Number = 1, State = 0},
                    new EventSeatDto { Row = 2, Number = 2, State = 0}
                }
            };

            //Act
            var exception = Assert.CatchAsync<EventAreaException>(async () => await _eventAreaService.Create(area));

            //Assert
            Assert.That(exception.Message, Is.EqualTo("Area description isn't unique"));
        }

        [Test, Order(4)]
        public void Create_area_without_seats_expected_exception()
        {
            //Arrange
            var area = new EventAreaDto
            {
                CoordX = 1,
                CoordY = 2,
                Description = "Area #4",
                EventId = 1,
                Seats = new List<EventSeatDto>()
            };

            //Act
            var exception = Assert.CatchAsync<EventAreaException>(async () => await _eventAreaService.Create(area));

            //Assert
            Assert.That(exception.Message, Is.EqualTo("Invalid state of event area. Seat list is empty"));
        }

        [Test, Order(5)]
        public async Task Update_area_without_seats_expected_exception()
        {
            //Arrange
            var area = await _eventAreaService.Get(_area.Id);
            area.Seats = new List<EventSeatDto>();

            //Act
            var exception = Assert.CatchAsync<EventAreaException>(async () => await _eventAreaService.Update(area));

            //Assert
            Assert.That(exception.Message, Is.EqualTo("Invalid state of event area. Seat list is empty"));
        }

        [Test, Order(6)]
        public async Task Update_area_with_changed_description_expected_exception()
        {

            //Arrange
            var area = await _eventAreaService.Get(_area.Id);
            area.Description = "The area #1";

            //Act
            var exception = Assert.CatchAsync<EventAreaException>(async () => await _eventAreaService.Update(area));

            //Assert
            Assert.That(exception.Message, Is.EqualTo("Area description isn't unique"));
        }

        [Test, Order(7)]
        public async Task Update_area_with_invalid_event_id_expected_exception()
        {

            //Arrange
            var area = await _eventAreaService.Get(_area.Id);
            area.EventId = 0;

            //Act
            var exception = Assert.CatchAsync<EventAreaException>(async () => await _eventAreaService.Update(area));

            //Assert
            Assert.That(exception.Message, Is.EqualTo("EventId is invalid"));
        }

        [Test, Order(8)]
        public void Update_area_which_is_null()
        {
            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _eventAreaService.Update(null));
        }

        [Test, Order(9)]
        public async Task Event_area_get_list()
        {
            //Act
            var areas = await _eventAreaService.GetList();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsTrue(areas.Any());
                Assert.IsTrue(areas.Contains(_area));
            });
        }

        [Test, Order(10)]
        public async Task Delete_event_area()
        {
            //Act
            await _eventAreaService.Delete(_area.Id);

            //Assert
            Assert.IsNull(await _eventAreaService.Get(_area.Id));
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            dateBase.Drop();
        }
    }
}
