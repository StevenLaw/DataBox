using DataBox.UserControls;
using DataBoxLibrary.DataModels;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DataBox.Core;

namespace DataBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool changeMade = false;
        /// <summary>
        /// Gets or sets the databox.
        /// </summary>
        /// <value>
        /// The databox.
        /// </value>
        //public DataBoxLibrary.DataBox Databox { get; set; } = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the change made.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        internal void SetChangeMade(bool value)
        {
            changeMade = value;
            if (changeMade)
                imgStatus.Source = BitmapFrame.Create(new Uri("pack://application:,,,/DataBox;component/Resources/redSave.png", UriKind.RelativeOrAbsolute));
            else
                imgStatus.Source = BitmapFrame.Create(new Uri("pack://application:,,,/DataBox;component/Resources/blueSave.png", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Displays the name of the file.
        /// </summary>
        private void DisplayFileName()
        {
            if (GlobalData.Databox == null)
                lblStatus.Content = string.Empty;
            else
            {
                if (string.IsNullOrWhiteSpace(GlobalData.Databox.Filename))
                    lblStatus.Content = "*Untitled*";
                else if (miShowFullPath.IsChecked == true)
                    lblStatus.Content = GlobalData.Databox.Path + GlobalData.Databox.Filename;
                else
                    lblStatus.Content = GlobalData.Databox.Filename;
            }
        }

        private void SaveAs()
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Databox file (*.dbx)|*.dbx|All files|*.*"
            };
            if (sfd.ShowDialog(this) == true)
            {
                GlobalData.Databox.Path = System.IO.Path.GetDirectoryName(sfd.FileName);
                GlobalData.Databox.Filename = System.IO.Path.GetFileName(sfd.FileName);
                GlobalData.Databox.Save();
                DisplayFileName();
                SetChangeMade(false);
            }
        }

        /// <summary>
        /// Handles the Open event of the CommandBinding control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void CommandBinding_Open(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog(this) == true)
            {
                //ClearEntries();
                if (DataBoxLibrary.DataBox.IsEncrypted(ofd.FileName))
                {
                    PasswordDialogue pd = new PasswordDialogue();
                    if (pd.ShowDialog() == true)
                    {
                        try
                        {
                            GlobalData.Databox = DataBoxLibrary.DataBox.Open(ofd.FileName, pd.Password);
                            DisplayFileName();
                            SetChangeMade(false);
                        }
                        catch (DataBoxLibrary.IncorrectPasswordException)
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
                    GlobalData.Databox = DataBoxLibrary.DataBox.Open(ofd.FileName);
                    DisplayFileName();
                    SetChangeMade(false);
                }
                ccMain.Content = new MainViewControl();
            }
        }

        /// <summary>
        /// Handles the Close event of the CommandBinding control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void CommandBinding_Close(object sender, ExecutedRoutedEventArgs e)
        {
            imgStatus.Source = null;
            ccMain.Content = null;
            DisplayFileName();
        }

        /// <summary>
        /// Handles the Close event of the CommandBinding_CanExecute control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CanExecuteRoutedEventArgs"/> instance containing the event data.</param>
        private void CommandBinding_CanExecute_Close(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = GlobalData.Databox != null;
        }

        /// <summary>
        /// Handles the Save event of the CommandBinding control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void CommandBinding_Save(object sender, ExecutedRoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GlobalData.Databox.Filename))
                SaveAs();
            else
            {
                GlobalData.Databox.Save();
                SetChangeMade(false);
            }
        }

        /// <summary>
        /// Handles the Save event of the CommandBinding_CanExecute control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CanExecuteRoutedEventArgs"/> instance containing the event data.</param>
        private void CommandBinding_CanExecute_Save(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = changeMade;
        }

        /// <summary>
        /// Handles the SaveAs event of the CommandBinding control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void CommandBinding_SaveAs(object sender, ExecutedRoutedEventArgs e)
        {
            SaveAs();
        }

        /// <summary>
        /// Handles the SaveAs event of the CommandBinding_CanExecute control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CanExecuteRoutedEventArgs"/> instance containing the event data.</param>
        private void CommandBinding_CanExecute_SaveAs(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = GlobalData.Databox != null;
        }

        /// <summary>
        /// Handles the Exit event of the CommandBinding control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void CommandBinding_Exit(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the Add event of the CommandBinding control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void CommandBinding_Add(object sender, ExecutedRoutedEventArgs e)
        {
            //var nlew = new NewLinkEntryWindow(this);
            //nlew.Show();
            ccMain.Content = new AddEditLinkEntryControl();
        }

        /// <summary>
        /// Handles the Edit event of the CommandBinding control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void CommandBinding_Edit(object sender, ExecutedRoutedEventArgs e)
        {
            if (ccMain.Content is MainViewControl mainViewControl)
            {
                if (mainViewControl.lvMain.SelectedItem is LinkEntry linkEntry)
                {
                    ccMain.Content = new AddEditLinkEntryControl(linkEntry);
                }
            }
        }

        private void CommandBinding_CanExecute_Edit(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (ccMain?.Content as MainViewControl)?.ItemSelected ?? false;
        }

        /// <summary>
        /// Handles the Delete event of the CommandBinding control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void CommandBinding_Delete(object sender, ExecutedRoutedEventArgs e)
        { }

        /// <summary>
        /// Handles the Cut event of the CommandBinding control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void CommandBinding_Cut(object sender, ExecutedRoutedEventArgs e)
        { }

        /// <summary>
        /// Handles the Copy event of the CommandBinding control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void CommandBinding_Copy(object sender, ExecutedRoutedEventArgs e)
        {}

        /// <summary>
        /// Handles the Paste event of the CommandBinding control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void CommandBinding_Paste(object sender, ExecutedRoutedEventArgs e)
        { }

        /// <summary>
        /// Handles the Click event of the miShowFullPath control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void miShowFullPath_Click(object sender, RoutedEventArgs e)
        {
            DisplayFileName();
        }

        /// <summary>
        /// Handles the Click event of the miShowStatusBar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void miShowStatusBar_Click(object sender, RoutedEventArgs e)
        {
            if (miShowStatusBar.IsChecked)
                sbStatus.Visibility = Visibility.Visible;
            else
                sbStatus.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Handles the New event of the CommandBinding control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void CommandBinding_New(object sender, ExecutedRoutedEventArgs e)
        {
            GlobalData.Databox = new DataBoxLibrary.DataBox();
            DisplayFileName();
            SetChangeMade(true);
            ccMain.Content = new MainViewControl();
        }

        /// <summary>
        /// Handles the Add event of the CommandBinding_CanExecute control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CanExecuteRoutedEventArgs"/> instance containing the event data.</param>
        private void CommandBinding_CanExecute_Add(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ccMain?.Content is MainViewControl;
        }

        /// <summary>
        /// Handles the Delete event of the CommandBinding_CanExecute control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CanExecuteRoutedEventArgs"/> instance containing the event data.</param>
        private void CommandBinding_CanExecute_Delete(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (ccMain?.Content as MainViewControl)?.ItemSelected ?? false;
        }
    }
}
