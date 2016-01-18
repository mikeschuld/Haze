namespace Haze.Library.SimulationSteps {
	public interface ISimulationStep {
		void OnStep(Field field, int x, int y, int z);
	}
}