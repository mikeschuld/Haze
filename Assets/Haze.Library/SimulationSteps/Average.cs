namespace Haze.Library.SimulationSteps {
	using System.Linq;

	internal class Average : ISimulationStep {
		public void OnStep(Field field, int x, int y, int z) {
			double?[] values = field.GetKernelValues(x, y, z, 3);
			double? average = CalculateAverage(values);

			if (average.HasValue) {
				field.AccumulateCellValue(x, y, z, average.Value);
			}
		}

		private static double? CalculateAverage(params double?[] values) {
			double total = 0;
			int count = 0;
			double? average = null;

			foreach (double? value in values.Where(value => null != value)) {
				total += value.Value;
				count++;
			}

			if (count > 0) {
				average = total / count;
			}

			return average;
		}
	}
}