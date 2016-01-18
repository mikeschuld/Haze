namespace Haze.Library.SimulationSteps {
	using System.Linq;
	using UnityEngine;

	internal static class Average {
		public static void OnStep(Field field) {
			int min = -field.Offset;
			int max = field.Offset;

			for (int x = min; x <= max; x++) {
				for (int y = min; y <= max; y++) {
					for (int z = min; z <= max; z++) {
						double?[] values = GetKernelValues(field, 3, x, y, z);
						double? average = CalculateAverage(values);

						if (average.HasValue) {
							field.AccumulateCellValue(x, y, z, average.Value);
						}
					}
				}
			}
		}

		private static double?[] GetKernelValues(Field field, int extents, int centerX, int centerY, int centerZ) {
			// Force odd kernel extents
			if (extents % 2 == 0) {
				extents--;
			}

			double?[] values = new double?[extents * extents * extents];

			int i = 0;
			int min = -extents / 2;
			int max = min + extents - 1;
			for (int x = min; x <= max; x++) {
				for (int y = min; y <= max; y++) {
					for (int z = min; z <= max; z++) {
						try {
							values[i++] = field.GetCellValue(x + centerX, y + centerY, z + centerZ);
						} catch {
							Debug.Log(string.Format("{0}: {1},{2},{3}", i, x, y, z));
						}
					}
				}
			}

			return values;
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