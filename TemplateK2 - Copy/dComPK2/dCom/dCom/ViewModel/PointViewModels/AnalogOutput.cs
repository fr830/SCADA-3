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
            return (AlarmProcessor.CheckAlarmility(CommandedValue,configItem)== 0);
                
        }
	}
}