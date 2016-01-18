using System;

namespace Haze.Library {
	public delegate void SimulationStep(Field field);

	public class Field {
		public SimulationStep Step;

		private readonly double[,,] fieldData;
		private readonly double[,,] afterStep;
		private readonly int initialExtents;

		public readonly int Offset;

		public Field(int extents) {
			initialExtents = extents;
			Offset = extents / 2;

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

		public void DoSimulationStep() {
			if (null != Step) {
				Step(this);
			}

			FinishStep();
		}

		public void FinishStep() {
			Array.Copy(afterStep, fieldData, afterStep.Length);
			Array.Clear(afterStep, 0, afterStep.Length);
		}
		
		private void OffsetDimensions(ref int dim1, ref int dim2, ref int dim3) {
			dim1 += Offset;
			dim2 += Offset;
			dim3 += Offset;
		}
	}
}