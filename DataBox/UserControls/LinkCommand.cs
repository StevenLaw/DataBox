using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DataBox.UserControls
{
    public class LinkCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter == null)
            {
                MessageBox.Show("Parameter is null", "Null Parameter Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (parameter.GetType() == typeof(string))
            {
                System.Diagnostics.Process.Start((string)parameter);
            }
            else
            {
                MessageBox.Show("Parameter is not text", "Not Text Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
