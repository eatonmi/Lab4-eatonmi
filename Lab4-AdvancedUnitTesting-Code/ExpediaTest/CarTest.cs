using System;
using NUnit.Framework;
using Expedia;
using Rhino.Mocks;

namespace ExpediaTest
{
    [TestFixture()]
    public class CarTest
    {
        private Car targetCar;
        private MockRepository mocks;

        [SetUp()]
        public void SetUp()
        {
            targetCar = new Car(5);
            mocks = new MockRepository();
        }

        [Test()]
        public void TestThatCarInitializes()
        {
            Assert.IsNotNull(targetCar);
        }

        [Test()]
        public void TestThatCarHasCorrectBasePriceForFiveDays()
        {
            Assert.AreEqual(50, targetCar.getBasePrice());
        }

        [Test()]
        public void TestThatCarHasCorrectBasePriceForTenDays()
        {
            var target = new Car(10);
            Assert.AreEqual(80, target.getBasePrice());
        }

        [Test()]
        public void TestThatCarHasCorrectBasePriceForSevenDays()
        {
            var target = new Car(7);
            Assert.AreEqual(10 * 7 * .8, target.getBasePrice());
        }

        [Test()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestThatCarThrowsOnBadLength()
        {
            new Car(-5);
        }

        [Test()]
        public void TestThatCarGetsLocation()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();
            using (mocks.Record())
            {

                mockDatabase.getCarLocation(10);
                LastCall.Return("Sheboygan");
            }

            var target = new Car(10);
            target.Database = mockDatabase;

            String result = target.getCarLocation(10);

            Assert.AreEqual("Sheboygan", result);
        }

        [Test()]
        public void TestThatCarGetsMlieage()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();
            mockDatabase.Miles = 9001;

            var target = new Car(10);
            target.Database = mockDatabase;

            int result = target.Mileage;

            Assert.AreEqual(9001, result);
        }

        [Test()]
        public void TestThatObjectMotherSpawnedCarWorks()
        {
            Car beamer = ObjectMother.BMW();
            String nameResult = beamer.Name;
            Assert.AreEqual("Beamer", nameResult);
        }
    }
}
