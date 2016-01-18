namespace Haze.Library.SimulationSteps {
	using System.Collections.Generic;
	using System.Linq;

	internal class Automata : ISimulationStep {
		private readonly IEnumerable<int> birth;
		private readonly IEnumerable<int> survival;

		public Automata(IEnumerable<int> birth, IEnumerable<int> survival) {
			this.birth = birth;
			this.survival = survival;
		}

		public void OnStep(Field field, int x, int y, int z) {
			double? value = field.GetKernelValues(x, y, z, 1).FirstOrDefault();
			double?[] neighbors = field.GetKernelValues(x, y, z, 3);
			int total = (int)CalculateSum(neighbors);

			if (value == 0 && birth.Contains(total)) {
				field.SetCellValue(x, y, z, 1);
			}

			if (value > 0 && survival.Contains(total)) {
				field.SetCellValue(x, y, z, 1);
			}
		}

		private static double CalculateSum(params double?[] values) {
			return values.Where(value => null != value).Sum(value => value.Value);
		}
	}
}