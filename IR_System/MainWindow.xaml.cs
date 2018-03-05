using System;
using System.Collections.Generic;
using System.IO;
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

namespace IR_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(@"inverted.dat"))
            {
                File.Delete(@"inverted.dat");
            }
            if (File.Exists(@"index.dat"))
            {
                File.Delete(@"index.dat");
            }
        }
    }
    public class nodePosting
    {
        public int noDoc { get; set; }
        public int posting { get; set; }
        public int noPage { get; set; }
        public string nameDoc { get; set; }
}
    
}
