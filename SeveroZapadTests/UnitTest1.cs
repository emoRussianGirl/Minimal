namespace SeveroZapadTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CheckNMTest()
        {
            var sz = new SeveroZapad();

            var result = sz.CheckNM(new int[] { 1, 2, 3 }, new int[] { 2, 3, 1 });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckNMFalseTest()
        {
            var sz = new SeveroZapad();

            var result = sz.CheckNM(new int[] { 1, 2, 3 }, new int[] { 1, 2, 4});

            Assert.IsFalse(result);
        }
    }
}