namespace Haze.Library {
	using System;
	using SimulationSteps;
	using UnityEngine;

	public class Field {
		private readonly double[,,] afterStep;
		private readonly double[,,] fieldData;

		private readonly int initialExtents;
		private readonly int offset;

		private readonly ISimulationStep step;

		public Field(int extents, ISimulationStep step) {
			this.step = step;

			initialExtents = extents;
			offset = extents / 2;

			fieldData = new double[extents, extents, extents];
			afterStep = new double[extents, extents, extents];
		}

		public void AccumulateCellValue(int dim1, int dim2, int dim3, double value) {
			OffsetDimensions(ref dim1, ref dim2, ref dim3);

			if (dim1 < 0 || dim2 < 0 || dim3 < 0) {
				return;
			}

			if (dim1 >= initialExtents || dim2 >= initialExtents || dim3 >= initialExtents) {
				return;
			}

			afterStep[dim1, dim2, dim3] += value;
		}

		public void SetCellValue(int dim1, int dim2, int dim3, double value) {
			OffsetDimensions(ref dim1, ref dim2, ref dim3);

			if (dim1 < 0 || dim2 < 0 || dim3 < 0) {
				return;
			}

			if (dim1 >= initialExtents || dim2 >= initialExtents || dim3 >= initialExtents) {
				return;
			}

			afterStep[dim1, dim2, dim3] = value;
		}

		public double? GetCellValue(int dim1, int dim2, int dim3) {
			OffsetDimensions(ref dim1, ref dim2, ref dim3);

			if (dim1 < 0 || dim2 < 0 || dim3 < 0) {
				return null;
			}

			if (dim1 >= initialExtents || dim2 >= initialExtents || dim3 >= initialExtents) {
				return null;
			}

			return fieldData[dim1, dim2, dim3];
		}

		public double?[] GetKernelValues(int centerX, int centerY, int centerZ, int extents) {
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
							values[i++] = GetCellValue(x + centerX, y + centerY, z + centerZ);
						} catch {
							Debug.Log(string.Format("{0}: {1},{2},{3}", i, x, y, z));
						}
					}
				}
			}

			return values;
		}

		public void DoSimulationStep() {
			if (null != step) {
				int min = -offset;
				int max = offset;

				for (int x = min; x <= max; x++) {
					for (int y = min; y <= max; y++) {
						for (int z = min; z <= max; z++) {
							step.OnStep(this, x, y, z);
						}
					}
				}
			}

			FinishStep();
		}

		public void FinishStep() {
			Array.Copy(afterStep, fieldData, afterStep.Length);
			Array.Clear(afterStep, 0, afterStep.Length);
		}

		private void OffsetDimensions(ref int dim1, ref int dim2, ref int dim3) {
			dim1 += offset;
			dim2 += offset;
			dim3 += offset;
		}
	}
}