using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace KzLibraries.KzWpfControl
{
    public static class TextBoxBehavior
    {
        public static bool GetSelectAllOnFocus(DependencyObject obj) => (bool)obj.GetValue(SelectAllOnFocusProperty);
        public static void SetSelectAllOnFocus(DependencyObject obj, bool value) => obj.SetValue(SelectAllOnFocusProperty, value);

        public static readonly DependencyProperty SelectAllOnFocusProperty =
            DependencyProperty.RegisterAttached(
                "SelectAllOnFocus",
                typeof(bool),
                typeof(TextBoxBehavior),
                new PropertyMetadata(false, SelectAllOnFocusChanged));

        private static void SelectAllOnFocusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is TextBoxBase textBox)
            {
                if ((bool)e.NewValue)
                {
                    textBox.GotFocus += SelectAllOnFocus_GotFocus;
                    textBox.PreviewMouseLeftButtonDown += SelectAllOnFocus_PreviewMouseLeftButtonDown;
                }
                else
                {
                    textBox.GotFocus -= SelectAllOnFocus_GotFocus;
                    textBox.PreviewMouseLeftButtonDown -= SelectAllOnFocus_PreviewMouseLeftButtonDown;
                }
            }
        }

        private static void SelectAllOnFocus_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBoxBase textBox)
            {
                e.Handled = true;
                textBox.SelectAll();
            }
        }
        private static void SelectAllOnFocus_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBoxBase textBox)
            {
                if (!textBox.IsKeyboardFocusWithin)
                {
                    e.Handled = true;
                    textBox.Focus();
                }
            }
        }
    }
}
