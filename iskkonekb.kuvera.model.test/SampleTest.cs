using Xunit;

namespace iskkonekb.kuvera.model.test
{
    public class SampleTest
    {
        [Fact]
        public void SampleClassReturns2()
        {
            Assert.Equal(2, new SampleClass().getData());
        }
    }
}
