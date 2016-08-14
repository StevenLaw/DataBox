using DataBox.UserControls;
using DataBoxLibrary.DataModels;
using Microsoft.Win32;
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

namespace DataBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool changeMade = false;
        private DataBoxLibrary.DataBox databox = null;

        public MainWindow()
        {
            InitializeComponent();

            //LinkEntry entry1 = new LinkEntry("Test entry", "This is a test entry.");
            //entry1.Links.Add(new LinkItem("Test Link", "http://Google.com"));
            //entry1.Links.Add(new LinkItem("This is another link", "http://stackoverflow.com"));
            //sbMainView.Children.Add(new LinkItemControl(entry1));

            //LinkEntry entry2 = new LinkEntry("Another entry", "This is another entry.");
            //entry2.Links.Add(new LinkItem("Another Link", "http://youtube.com"));
            //sbMainView.Children.Add(new LinkItemControl(entry2));
        }

        private void SetChangeMade(bool value)
        {
            changeMade = value;
            if (changeMade)
                imgStatus.Source = BitmapFrame.Create(new Uri("pack://application:,,,/DataBox;component/Resources/redSave.png", UriKind.RelativeOrAbsolute));
            else
                imgStatus.Source = BitmapFrame.Create(new Uri("pack://application:,,,/DataBox;component/Resources/blueSave.png", UriKind.RelativeOrAbsolute));
        }

        private void DisplayFileName()
        {
            if (databox == null)
                lblStatus.Content = string.Empty;
            else
            {
                if (miShowFullPath.IsChecked == true)
                    lblStatus.Content = databox.Path + databox.Filename;
                else
                    lblStatus.Content = databox.Filename;
            }
        }

        private void CommandBinding_Open(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog(this) == true)
            {
                if (DataBoxLibrary.DataBox.IsEncrypted(ofd.FileName))
                {
                    PasswordDialogue pd = new PasswordDialogue();
                    if (pd.ShowDialog() == true)
                    {
                        try
                        {
                            databox = DataBoxLibrary.DataBox.Open(ofd.FileName, pd.Password);
                            DisplayFileName();
                            SetChangeMade(false);
                        }
                        catch (DataBoxLibrary.IncorrectPasswordException ex)
                        {
                            MessageBox.Show("Incorrect Password", "The password was incorrect", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }
                    else
                        return;
                }
                else
                {
                    databox = DataBoxLibrary.DataBox.Open(ofd.FileName);
                    DisplayFileName();
                    SetChangeMade(false);
                }
                if (databox == null)
                    MessageBox.Show("File is not a Data Box file.",
                        "The selected file was not the correct format.",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                else
                {
                    foreach (Entry entry in databox.Entries)
                    {
                        if (entry is LinkEntry)
                            sbMainView.Children.Add(new LinkItemControl((LinkEntry)entry));
                    }
                }
            }
        }

        private void CommandBinding_Close(object sender, ExecutedRoutedEventArgs e)
        {
            imgStatus.Source = null;
            DisplayFileName();
            databox = null;
        }

        private void CommandBinding_Save(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CommandBinding_SaveAs(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CommandBinding_Exit(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void CommandBinding_Add(object sender, ExecutedRoutedEventArgs e)
        { }

        private void CommandBinding_Edit(object sender, ExecutedRoutedEventArgs e)
        { }

        private void CommandBinding_Delete(object sender, ExecutedRoutedEventArgs e)
        { }

        private void CommandBinding_Cut(object sender, ExecutedRoutedEventArgs e)
        { }

        private void CommandBinding_Copy(object sender, ExecutedRoutedEventArgs e)
        {}

        private void CommandBinding_Paste(object sender, ExecutedRoutedEventArgs e)
        { }

        private void CommandBinding_CanExecute_Close(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = databox != null;
        }

        private void CommandBinding_CanExecute_Save(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = changeMade;
        }

        private void miShowFullPath_Click(object sender, RoutedEventArgs e)
        {
            DisplayFileName();
        }

        private void miShowStatusBar_Click(object sender, RoutedEventArgs e)
        {
            if (miShowStatusBar.IsChecked)
                sbStatus.Visibility = System.Windows.Visibility.Visible;
            else
                sbStatus.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
