using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KzLibraries.KzWpfControl
{
    public class SelectAllTextBox : TextBox
    {
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            e.Handled = true;
            this.SelectAll();
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            if (!this.IsKeyboardFocusWithin)
            {
                e.Handled = true;
                this.Focus();
            }
        }
    }
}
