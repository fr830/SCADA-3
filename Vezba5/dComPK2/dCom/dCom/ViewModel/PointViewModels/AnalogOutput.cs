using Common;
using ProcessingModule;
using System;

namespace dCom.ViewModel
{
	internal class AnalogOutput : AnalogBase
	{

		public AnalogOutput(IConfigItem c, IFunctionExecutor commandExecutor, IStateUpdater stateUpdater, IConfiguration configuration, int i)
			: base (c, commandExecutor, stateUpdater, configuration, i)
		{
			
		}

		protected override bool WriteCommand_CanExecute(object obj)
		{
            if (AlarmProcessor.GetAlarmForAnalogPoint(CommandedValue, configItem) == AlarmType.REASONABILITY_FAILURE)
                return false;
            else
                return true;
		}
	}
}