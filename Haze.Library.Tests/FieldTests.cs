using Xunit;

namespace Haze.Library.Tests {
	public class FieldTests {
		[Theory]
		[InlineData(0, 0, 0, 0.01)]
		public void TestSetCellValue(int dim1, int dim2, int dim3, int value) {
			var field = new Field(2);

			field.SetCellValue(dim1, dim2, dim3, value);
		}

		[Theory]
		[InlineData(0, 0, 0, 0.01)]
		public void TestGetCellValue(int dim1, int dim2, int dim3, int value) {
			var field = new Field(2);

			field.SetCellValue(dim1, dim2, dim3, value);
			var getValue = field.GetCellValue(dim1, dim2, dim3);

			Assert.Equal(value, getValue);
		}
	}
}