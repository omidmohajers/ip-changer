using PA.OnClickIpChanger.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PA.OnClickIpChanger
{
    /// <summary>
    /// Interaction logic for IPSettingWindow.xaml
    /// </summary>
    public partial class IPSettingWindow : Window
    {
        public IPSettingWindow()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //((IPSettingViewModel)DataContext).Close();
        }
    }
}
