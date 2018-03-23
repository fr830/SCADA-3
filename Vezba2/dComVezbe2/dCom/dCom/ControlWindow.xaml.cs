using dCom.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace dCom
{
	/// <summary>
	/// Interaction logic for ControlWindow.xaml
	/// </summary>
	public partial class ControlWindow : Window
	{
		public ControlWindow()
		{
			InitializeComponent();
		}

		public ControlWindow(BasePointItem dataContext) : this()
		{
			this.DataContext = dataContext;
			Title = string.Format("Control Window - {0}", dataContext.Name);
        }
	}
}
