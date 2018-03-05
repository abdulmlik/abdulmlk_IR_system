using Microsoft.Win32;
using System;
using System.IO;
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
using System.Text.RegularExpressions;

namespace IR_System
{
    /// <summary>
    /// Interaction logic for PageHome.xaml
    /// </summary>
    public partial class PageHome : Page
    {
        public PageHome()
        {
            InitializeComponent();
        }
        ~PageHome()
        {
        }

        public Dictionary<string, string> stopwords = new Dictionary<string, string>();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = true;                      // choose multi File
                openFileDialog.Filter = "Text Files|*.txt";

                if (openFileDialog.ShowDialog() == true)
                {
                    if (File.Exists(@"inverted.dat"))
                    {
                        File.Delete(@"inverted.dat");
                    }
                    if (File.Exists(@"index.dat"))
                    {
                        File.Delete(@"index.dat");
                    }

                    int noPage = 0;
                    int noDocument = 0;
                    int noWordPosition = 0;
                    string endPage = "*****";



                    Dictionary<string, List<nodePosting>> invertedList = new Dictionary<string, List<nodePosting>>();


                    if (!File.Exists(@"stopwords.txt"))
                    {
                        throw new Exception("1");
                    }


                    foreach (string filename in openFileDialog.FileNames)
                    {
                        if(System.IO.Path.GetExtension(filename) != ".txt")
                        {
                            throw new Exception();
                        }
                        noDocument++;//number file
                        noPage = 0;
                        noWordPosition = 0;
                        using (StreamReader sr = new StreamReader(filename))
                        {
                            noPage++;
                            string line;
                            string[] words;
                            while ((line = sr.ReadLine()) != null)
                            {
                                if (String.IsNullOrEmpty(line) || String.IsNullOrWhiteSpace(line))
                                {
                                    continue;
                                }
                                line = Regex.Replace(line, @"(?:\s+|[^A-Za-z*])", " ");
                                words = line.Split();
                                foreach(string word1 in words)
                                {
                                    noWordPosition++;
                                    string word = word1.Trim().ToLower();
                                    if (word == endPage)
                                    {
                                        noPage++;
                                        continue;
                                    }
                                    else if ( String.IsNullOrEmpty(word))
                                    {
                                        continue;
                                    }
                                    else if (stopwords.ContainsKey(word))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        nodePosting temp = new nodePosting();
                                        temp.noDoc = noDocument;
                                        temp.posting = noWordPosition;
                                        temp.noPage = noPage;
                                        temp.nameDoc = filename;
                                        
                                        if (invertedList.ContainsKey(word))
                                        {
                                            invertedList[word].Add(temp);
                                        }
                                        else
                                        {
                                            List<nodePosting> parts = new List<nodePosting>();
                                            parts.Add(temp);
                                            invertedList.Add(word, parts);
                                        }

                                    }
                                }// end words
                            }//end read lines
                        }// end using
                    }//end for files
                    //Create index file

                    using (BinaryWriter writer = new BinaryWriter(File.Open("inverted.dat", FileMode.Create)))// save posting file
                    using (BinaryWriter writer2 = new BinaryWriter(File.Open("index.dat", FileMode.Create)))  // save index for read in posting file
                    {
                        foreach (KeyValuePair<string, List<nodePosting>> kvp in invertedList)
                        {
                            foreach (nodePosting nop in kvp.Value)
                            {
                                writer.Write(nop.noDoc);
                                writer.Write(nop.posting);
                                writer.Write(nop.noPage);
                                writer.Write(nop.nameDoc);
                            }
                            writer2.Write(kvp.Key);
                            writer2.Write(kvp.Value.Count);
                        }
                    }
                    //
                    butSerch.IsEnabled = true;// don
                }
               
            }//end try
            catch(Exception e1)
            {
                butSerch.IsEnabled = false;// don
                if (e1.Message == "1")
                    MessageBox.Show("stopwords.txt does not exist");
                else
                    MessageBox.Show("Please choose a valid file");
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // start save stop words in stopwords
                if (!File.Exists(@"stopwords.txt"))
                {
                    throw new Exception();
                }
                using (StreamReader sr = new StreamReader("stopwords.txt"))
                {
                    string line;
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim().ToLower();
                        if (!String.IsNullOrEmpty(line) && !stopwords.ContainsKey(line))
                            stopwords.Add(line, null);
                    }
                }
                // end save stop words
            }
            catch
            {
                MessageBox.Show("stopwords.txt does not exist");
            }
            if ( File.Exists(@"inverted.dat") && File.Exists(@"index.dat"))
            {
                butSerch.IsEnabled = true;
            }
        }

        private void butSerch_Click(object sender, RoutedEventArgs e)
        {
            //this.NavigationService.Navigate(new Uri("PageShow.xaml", UriKind.Relative), text_search.Text);
            string query = text_search.Text.Trim().ToLower();
            if (stopwords.ContainsKey(query))
            {
                MessageBox.Show(query + " is stop-Word ,Please enter again");
            }
            else
            {
                PageShow page2 = new PageShow();
                page2.Guess = query;
                NavigationService.Navigate(page2);
            }
        }
    }
}
