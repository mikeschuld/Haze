using Haze.Library;
using Haze.Library.SimulationSteps;
using UnityEngine;

public class FieldGameObject : MonoBehaviour {
	public Color Color = Color.white;

	public bool DrawGrid;

	[Range(1, 100)]
	public int Extents = 5;

	[Range(10, 100)]
	public float SideLength = 10;
	public bool Simulate;
	public bool Step;

	private Field field;

	private void Start() {
		field = new Field(Extents);
		field.Step += Average.OnStep;

		field.SetCellValue(0, 0, 0, 100);
		field.FinishStep();
	}

	private void Update() {
		if (!Simulate && !Step) {
			return;
		}

		Step = false;
		field.DoSimulationStep();
	}

	private void OnDrawGizmos() {
		float offset = SideLength / 2f;
		float interval = SideLength / Extents;
		float halfInterval = interval / 2;

		int min = -Extents / 2;
		int max = -min;

		if (DrawGrid) {
			Gizmos.color = Color;

			for (float a = -offset; a <= offset; a += interval) {
				for (float b = -offset; b <= offset; b += interval) {
					Gizmos.DrawLine(new Vector3(-offset, a, b), new Vector3(offset, a, b));
					Gizmos.DrawLine(new Vector3(a, -offset, b), new Vector3(a, offset, b));
					Gizmos.DrawLine(new Vector3(a, b, -offset), new Vector3(a, b, offset));
				}
			}
		}

		for (float x = min, xPos = -offset + halfInterval; x <= max; x++, xPos += interval) {
			for (float y = min, yPos = -offset + halfInterval; y <= max; y++, yPos += interval) {
				for (float z = min, zPos = -offset + halfInterval; z <= max; z++, zPos += interval) {
					int cellX = (int)x;
					int cellY = (int)y;
					int cellZ = (int)z;
					float alpha = 0.5f;

					if (null != field) {
						double? cellValue = field.GetCellValue(cellX, cellY, cellZ);
						if (cellValue != null) {
							alpha = (float)cellValue;
						} else {
							alpha = 0;
						}
					}

					Gizmos.color = new Color(Color.r, Color.g, Color.b, alpha);
					Gizmos.DrawCube(
						new Vector3(xPos, yPos, zPos),
						new Vector3(interval, interval, interval));
				}
			}
		}
	}
}