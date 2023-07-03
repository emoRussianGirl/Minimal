using MinimalJopeMethod;

namespace MinimalTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SimpleTest()
        {
            Assert.ThrowsException<NullReferenceException>(() => new MinimalMethod().FindBasePlan(null!, null!, null!));
        }
    }
}