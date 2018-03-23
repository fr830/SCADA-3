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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dCom
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = new MainViewModel();
			this.Closed += Window_Closed;
		}

		private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			ControlWindow cw = new ControlWindow(dgPoints.SelectedItem as BasePointItem);
			cw.Owner = this;
			cw.ShowDialog();
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			(DataContext as IDisposable).Dispose();
		}
	}
}
