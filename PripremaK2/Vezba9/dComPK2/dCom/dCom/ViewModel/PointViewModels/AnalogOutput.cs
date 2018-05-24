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
            if (AlarmProcessor.CheckAlarmility(CommandedValue, configItem) == 0)
                return true;
            else
                return false;
        }
	}
}