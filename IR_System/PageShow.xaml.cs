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

namespace IR_System
{
    /// <summary>
    /// Interaction logic for PageShow.xaml
    /// </summary>
    public sealed partial class PageShow : Page
    {
        public PageShow()
        {
            InitializeComponent();
        }
        ~PageShow()
        {
        }

        string guess;
        public string Guess
        {
            get { return guess; }
            set { guess = value; }
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            tex.Text = Guess;

            Dictionary<string, List<nodePosting>> invertedList = new Dictionary<string, List<nodePosting>>();
            nodePosting p = new nodePosting();
            

            using (BinaryReader reader = new BinaryReader(File.Open("inverted.dat", FileMode.Open)))
            using (BinaryReader reader2 = new BinaryReader(File.Open("index.dat", FileMode.Open)))
            {
                while (reader2.BaseStream.Position < reader2.BaseStream.Length)
                {
                    string s = reader2.ReadString();
                    int a = reader2.ReadInt32();
                    for (int i = 0; i < a; i++)
                    {
                        p.noDoc = reader.ReadInt32();
                        p.posting = reader.ReadInt32();
                        p.noPage = reader.ReadInt32();
                        p.nameDoc = reader.ReadString();
                        if (!invertedList.ContainsKey(s))
                        {
                            invertedList.Add(s, new List<nodePosting> { p });
                           
                        }
                        else
                        {
                            invertedList[s].Add(p);
                        }
                        p = null;
                        p = new nodePosting();
                            
                    }
                }
                if (invertedList.ContainsKey(Guess))
                {
                    list_searching.ItemsSource = invertedList[Guess];
                }
                else
                {
                    tex.Text += " not found";
                }
            }

        }

        private void but_back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}
