using Common;
using System;

namespace dCom.ViewModel
{
	internal class DigitalInput : DigitalBase
	{
		public DigitalInput(IConfigItem c, IFunctionExecutor commandExecutor, IStateUpdater stateUpdater, IConfiguration configuration, int i) 
			: base(c, commandExecutor, stateUpdater, configuration, i)
		{
		}
	}
}