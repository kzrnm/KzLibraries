using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KzLibraries.KzWpfControl
{
    public static class ComboBoxBehavior
    {
        public static bool GetIsFirstEmpty(DependencyObject obj) => (bool)obj.GetValue(IsFirstEmptyProperty);
        public static void SetIsFirstEmpty(DependencyObject obj, bool value) => obj.SetValue(IsFirstEmptyProperty, value);

        public static readonly DependencyProperty IsFirstEmptyProperty =
            DependencyProperty.RegisterAttached(
                "IsFirstEmpty",
                typeof(bool),
                typeof(ComboBoxBehavior),
                new PropertyMetadata(false, IsFirstEmptyChanged));

        private static void IsFirstEmptyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ComboBox comboBox)
            {
                if ((bool)e.NewValue)
                {
                    comboBox.PreviewKeyDown += IsFirstEmpty_PreviewKeyDown;
                }
                else
                {
                    comboBox.PreviewKeyDown -= IsFirstEmpty_PreviewKeyDown;
                }
            }
        }

        private static void IsFirstEmpty_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                if (e.Key == Key.Up && comboBox.SelectedIndex == 0)
                {
                    comboBox.Text = "";
                    e.Handled = true;
                }
            }
        }
    }
}
