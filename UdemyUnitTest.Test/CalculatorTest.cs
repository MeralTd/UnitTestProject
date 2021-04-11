using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyUnitTest.APP;
using Xunit;

namespace UdemyUnitTest.Test
{
    public class CalculatorTest
    {
        public Calculator calculator { get; set; }
        public Mock<ICalculatorService> myMock { get; set; }

        public CalculatorTest()
        {
            myMock = new Mock<ICalculatorService>();
            this.calculator = new Calculator(myMock.Object);
        }

        [Theory]
        [InlineData(2,5,7)]
        [InlineData(10,2,12)]
        public void Add_simpleValues_ReturnTotalValue(int a,int b, int expectedTotal)
        {
            myMock.Setup(x => x.add(a, b)).Returns(expectedTotal);
            var actualTotal = calculator.add(a, b);
            Assert.Equal(expectedTotal, actualTotal);

            myMock.Verify(x => x.add(a, b), Times.Once);
        }

        [Theory]
        [InlineData(0, 5, 0)]
        [InlineData(10, 0, 0)]
        public void Add_zeroValues_ReturnZeroValue(int a, int b, int expectedTotal)
        {
            var actualTotal = calculator.add(a, b);
            Assert.Equal(expectedTotal, actualTotal);
        }

        [Theory]
        [InlineData(3, 5, 15)]
        public void Multip_simpleValues_ReturnMultipValue(int a, int b, int expectedValue)
        {
            int actualMultip =0;
            myMock.Setup(x => x.multip(It.IsAny<int>(), It.IsAny<int>())).
                Callback<int, int>((x, y) => actualMultip = x * y);

            calculator.multip(a, b);
            Assert.Equal(expectedValue, actualMultip);

            calculator.multip(5, 20);
            Assert.Equal(100, actualMultip);
        }

        [Theory]
        [InlineData(0, 5)]
        public void Multip_simpleValues_ReturnsException(int a, int b)
        {
            myMock.Setup(x => x.multip(a, b)).Throws(new Exception("a=0 olamaz"));
            
            Exception exception= Assert.Throws<Exception>(() => calculator.multip(a, b));
            Assert.Equal("a=0 olamaz", exception.Message);
        }
    }
}
