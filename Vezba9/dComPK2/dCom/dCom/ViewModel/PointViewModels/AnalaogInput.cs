using Common;

namespace dCom.ViewModel
{
	internal class AnalaogInput : AnalogBase
	{
		public AnalaogInput(IConfigItem c, IFunctionExecutor commandExecutor, IStateUpdater stateUpdater, IConfiguration configuration, int i)
			: base(c, commandExecutor, stateUpdater, configuration, i)
		{
		}
	}
}