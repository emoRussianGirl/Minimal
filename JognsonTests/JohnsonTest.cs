using MathModelingForExam;

namespace JognsonTests
{
    [TestClass]
    public class JohnsonTest
    {
        [TestMethod]
        public void StartPathTest()
        {
            Assert.ThrowsException<FileNotFoundException>(() => TaskJonhson.StartTask("jkdl"));
        }
    }
}