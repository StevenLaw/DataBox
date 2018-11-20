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

namespace DataBox.UserControls
{
    /// <summary>
    /// Interaction logic for TagUserControl.xaml
    /// </summary>
    public partial class TagUserControl : UserControl
    {
        /// <summary>
        /// The function used to get the autocomplete string array.
        /// </summary>
        /// <param name="String">The string.</param>
        /// <returns>the string array returned by the autocomplete function.</returns>
        public delegate string[] AutocompleteFunction(string text);

        #region Properties
        public static readonly DependencyProperty TokenTemplateProperty =
            DependencyProperty.Register("TokenTemplate", typeof(DataTemplate), typeof(TagUserControl));
        protected bool menuOpen = false;

        /// <summary>
        /// Gets or sets the token template.
        /// </summary>
        /// <value>
        /// The token template.
        /// </value>
        public DataTemplate TokenTemplate
        {
            get { return (DataTemplate)GetValue(TokenTemplateProperty); }
            set { SetValue(TokenTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the current autocomplete function.
        /// </summary>
        /// <value>
        /// The current autocomplete function.
        /// </value>
        public AutocompleteFunction CurrentAutocomplete { get; set; }

        //private string[] _initialTags = null;

        public string[] Tags => rtbMain.CaretPosition.Paragraph.Inlines.Select(x =>
                                                           {
                                                               if (x is InlineUIContainer container)
                                                               {
                                                                   if (container.Child is ContentPresenter content)
                                                                   {
                                                                       return content.Content as string;
                                                                   }
                                                               }
                                                               return "";
                                                           }).ToArray();
        #endregion

        public TagUserControl()
        {
            InitializeComponent();

            // Set default Token Template
            if (TokenTemplate == null)
                SetResourceReference(TokenTemplateProperty, "DefaultTokenTemplate");

            Loaded += TagUserControl_Loaded;
        }

        private void TagUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Add additional events to the parent window
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.LocationChanged += ParentWindow_LocationChanged;
                parentWindow.StateChanged += ParentWindow_StateChanged;
            }

            // Set control events
            GotKeyboardFocus += TagUserControl_GotKeyboardFocus;
            LostKeyboardFocus += TagUserControl_LostKeyboardFocus;
            PreviewKeyDown += TagUserControl_PreviewKeyDown;

            // Prevent Autocomplete function from being null
            if (CurrentAutocomplete == null)
                CurrentAutocomplete = (string text) => { return new string[0]; };

            //if (_initialTags != null)
            //    AddTags(_initialTags);
        }

        /// <summary>
        /// Handles the PreviewKeyDown event of the TagUserControl control.  Allows the user to scroll up
        /// and down in the autocomplete list with the up and down keys as well as capturing the enter event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void TagUserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (popAutoComplete.IsOpen)
            {
                int index = lstAutoComplete.SelectedIndex;
                switch (e.Key)
                {
                    case Key.Down:
                        if (index < lstAutoComplete.Items.Count - 1)
                            lstAutoComplete.SelectedIndex = index + 1;
                        else
                            lstAutoComplete.SelectedIndex = -1;
                        break;
                    case Key.Up:
                        if (index >= 0)
                            lstAutoComplete.SelectedIndex = index - 1;
                        else
                            lstAutoComplete.SelectedIndex = lstAutoComplete.Items.Count - 1;
                        break;
                    case Key.Enter:
                        string content = lstAutoComplete.SelectedItem as string;
                        if (content != null)
                        {
                            ReplaceTextWithToken(rtbMain.CaretPosition.GetTextInRun(LogicalDirection.Backward), content);
                            e.Handled = true;
                        }
                        popAutoComplete.IsOpen = false;
                        menuOpen = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Handles the LostKeyboardFocus event of the TagUserControl control.  Hides the autocomplete list if the control 
        /// looses focus.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyboardFocusChangedEventArgs"/> instance containing the event data.</param>
        private void TagUserControl_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (menuOpen)
                popAutoComplete.IsOpen = false;
        }

        /// <summary>
        /// Handles the GotKeyboardFocus event of the TagUserControl control.  Shows the autocomplete list if the control
        /// regains focus.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyboardFocusChangedEventArgs"/> instance containing the event data.</param>
        private void TagUserControl_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (menuOpen)
                popAutoComplete.IsOpen = true;
        }

        /// <summary>
        /// Handles the StateChanged event of the ParentWindow control.  Hides the autocomplete list
        /// if the parent window is minimized and brings it back when it is restored.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ParentWindow_StateChanged(object sender, EventArgs e)
        {
            if (sender is Window main && menuOpen)
            {
                if (main.WindowState == WindowState.Minimized)
                    popAutoComplete.IsOpen = false;
                else
                    popAutoComplete.IsOpen = true;
            }
        }

        /// <summary>
        /// Handles the LocationChanged event of the ParentWindow control.  Resets position of 
        /// autocomplete list when the parent window is moved.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ParentWindow_LocationChanged(object sender, EventArgs e)
        {
            var offsett = popAutoComplete.HorizontalOffset;
            popAutoComplete.HorizontalOffset = offsett + 1;
            popAutoComplete.HorizontalOffset = offsett;
        }

        /// <summary>
        /// Handles the TextChanged event of the rtbMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void rtbMain_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Prevents the code from attempting to access controls before they are loaded.
            if (IsInitialized) 
            {
                var text = rtbMain.CaretPosition.GetTextInRun(LogicalDirection.Backward);
                var token = TokenMatcher(text);
                string[] sublist;
                if (string.IsNullOrWhiteSpace(text))
                    sublist = new string[0];
                else
                    sublist = CurrentAutocomplete(text);
                if (token != null)
                    ReplaceTextWithToken(text, token);
                else if (sublist.Count() > 0)
                {
                    lstAutoComplete.Items.Clear();
                    foreach (string item in sublist)
                        lstAutoComplete.Items.Add(item);
                    popAutoComplete.IsOpen = true;
                    menuOpen = true;
                }
                else
                {
                    popAutoComplete.IsOpen = false;
                    menuOpen = false;
                }
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the lstAutoComplete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void lstAutoComplete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (lstAutoComplete.SelectedItem is string content)
            {
                ReplaceTextWithToken(rtbMain.CaretPosition.GetTextInRun(LogicalDirection.Backward), content);
                popAutoComplete.IsOpen = false;
                menuOpen = false;
            }
        }

        /// <summary>
        /// Finds a match with the token end character.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>the matched string minus the token end character.</returns>
        protected string TokenMatcher(string text)
        {
            if (text.EndsWith(";"))
            {
                // Remove the ';'
                return text.Substring(0, text.Length - 1).Trim();
            }
            return null;
        }

        /// <summary>
        /// Replaces the text with token.
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="token">The token.</param>
        private void ReplaceTextWithToken(string inputText, object token)
        {
            // Remove the handler temporarily as we will be modifying tokens below, causing more TextChanged events
            rtbMain.TextChanged -= rtbMain_TextChanged;

            var para = rtbMain.CaretPosition.Paragraph;


            if (para.Inlines.FirstOrDefault(inline =>
            {
                return (inline is Run run && run.Text.EndsWith(inputText));
            }) is Run matchedRun) //Found a run that matched the inputText
            {
                var tokenContainer = CreateTokenWithContainer(inputText, token);
                para.Inlines.InsertBefore(matchedRun, tokenContainer);

                // Remove only if the Text in the Run is the same as inputText, else split up
                if (matchedRun.Text == inputText)
                    para.Inlines.Remove(matchedRun);
                else // Split up
                {
                    var index = matchedRun.Text.IndexOf(inputText) + inputText.Length;
                    var tailEnd = new Run(matchedRun.Text.Substring(index));
                    para.Inlines.InsertAfter(matchedRun, tailEnd);
                    para.Inlines.Remove(matchedRun);
                }
            }

            rtbMain.TextChanged += rtbMain_TextChanged;
        }

        public void AddTags(IEnumerable<string> tagTexts)
        {
            // Remove the handler temporarily as we will be modifying tokens below, causing more TextChanged events
            rtbMain.TextChanged -= rtbMain_TextChanged;

            foreach (string tagText in tagTexts)
            {
                if (rtbMain.Document.Blocks.Count() == 0)
                    rtbMain.Document.Blocks.Add(new Paragraph());
                var tokenContainer = CreateTokenWithContainer(null, tagText);
                if (rtbMain.Document.Blocks.FirstOrDefault(x => x is Paragraph) is Paragraph para)
                    para.Inlines.Add(tokenContainer);
            }

            rtbMain.TextChanged += rtbMain_TextChanged;
        }

        public void ClearTags()
        {
            rtbMain.Document.Blocks.Clear();
        }

        /// <summary>
        /// Creates the token with container.
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="token">The token.</param>
        /// <returns>the token container.</returns>
        private InlineUIContainer CreateTokenWithContainer(string inputText, object token)
        {
            // Note: we are not using the inputText here, but could be used in future

            var presenter = new ContentPresenter()
            {
                Content = token,
                ContentTemplate = TokenTemplate
            };

            // BaselineAlignment is needed to align with Run
            return new InlineUIContainer(presenter) { BaselineAlignment = BaselineAlignment.TextBottom };
        }

    }
}
