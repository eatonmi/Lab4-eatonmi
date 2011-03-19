
using System;
using NUnit.Framework;
using Expedia;
using System.Reflection;

namespace ExpediaTest
{
	[TestFixture()]
	public class UserTest
	{
		private User target;
		private readonly DateTime StartDate = new DateTime(2009, 11, 1);
		private readonly DateTime EndDate = new DateTime(2009, 11, 30);
		
		[SetUp()]
		public void Setup()
		{
			target = new User("Bob Dole");
		}
		
		[Test()]
		public void TestThatUserInitializes()
		{
			Assert.AreEqual("Bob Dole", target.Name);
		}
		
		[Test()]
		public void TestThatUserHasZeroFrequentFlierMilesOnInit()
		{
			Assert.AreEqual(0, target.FrequentFlierMiles);
		}
		
		[Test()]
		public void TestThatUserCanBookEverything()
		{
			target.book(new Flight(StartDate, EndDate, 0), new Hotel(5), new Car(3));
			Assert.AreEqual(3, target.Bookings.Count);
		}
		
		[Test()]
		public void TestThatUserHasFrequentFlierMilesAfterBooking()
		{
			target.book(new Flight(StartDate, EndDate, 1), new Hotel(5), new Car(3));
			Assert.Less(0, target.FrequentFlierMiles);
			Assert.AreEqual(3, target.Bookings.Count);
		}
		
		[Test()]
		public void TestThatUserCanBookAOnlyFlight()
		{
			target.book(new Flight(StartDate, EndDate, 0));
			Assert.AreEqual(1, target.Bookings.Count);
		}
		
		[Test()]
		public void TestThatUserCanBookAHotalAndACar()
		{
			target.book(new Car(5), new Hotel(5));
			Assert.AreEqual(2, target.Bookings.Count);
		}
		
		[Test()]
		public void TestThatUserHasCorrectNumberOfFrequentFlyerMilesAfterOneFlight()
		{
			target.book(new Flight(StartDate, EndDate, 500));
			Assert.AreEqual(500, target.FrequentFlierMiles);
		}
		
		[Test()]
		public void TestThatUserTotalCostIsCorrect()
		{
			var flight = new Flight(StartDate, EndDate, 500);
			target.book(flight);	
			Assert.AreEqual(flight.getBasePrice(), target.Price);
		}
		
		[Test()]
		public void TestThatUserTotalCostIsCorrectWhenMoreThanFlights()
		{
			var car = new Car(5);
			var flight = new Flight(StartDate, EndDate, 500);
			target.book(flight);	
			target.book(car);
			Assert.AreEqual(flight.getBasePrice() + car.getBasePrice(), target.Price);
		}

        [Test()]
        public void TestThatUserDoesRemoveCarFromServiceLocatorWhenBooked()
        {
            ServiceLocator serviceloc = new ServiceLocator();
            var carToBook = new Car(5);
            var remainingCar = new Car(7);
            serviceloc.AddCar(carToBook);
            serviceloc.AddCar(remainingCar);
            typeof(ServiceLocator).GetField("_instance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).SetValue(serviceloc, serviceloc);

            var target = new User("Bob");
            target.book(carToBook);

            Assert.AreEqual(1, ServiceLocator.Instance.AvailableCars.Count);
            Assert.AreSame(remainingCar, ServiceLocator.Instance.AvailableCars[0]);
        }

        [Test()]
        public void TestThatUserDoesRemoveFlightFromSerivceLocatorWhenBooked()
        {
            ServiceLocator serviceloc = new ServiceLocator();
            var flightToBook = new Flight(new DateTime(2009, 11, 1), new DateTime(2009, 11, 30), 100);
            var remainingFlight = new Flight(new DateTime(2009, 12, 1), new DateTime(2009, 12, 30), 1000);
            serviceloc.AddFlight(flightToBook);
            serviceloc.AddFlight(remainingFlight);

            typeof(ServiceLocator).GetField("_instance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).SetValue(serviceloc, serviceloc);

            var target = new User("Bobby");
            target.book(flightToBook);

            Assert.AreEqual(1, ServiceLocator.Instance.AvailableFlights.Count);
            Assert.AreSame(remainingFlight, ServiceLocator.Instance.AvailableFlights[0]);
        }

		
		[TearDown()]
		public void TearDown()
		{
			target = null; // this is entirely unnecessary.. but I'm just showing a usage of the TearDown method here
		}
	}
}
