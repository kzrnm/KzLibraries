using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace KzLibraries.KzWpfControl
{
    public class DoubleTextBox : TextBox
    {
        public string DoubleText
        {
            get => (string)GetValue(DoubleTextProperty);
            set => SetValue(DoubleTextProperty, value);
        }
        public static readonly DependencyProperty DoubleTextProperty =
              DependencyProperty.Register(
                  nameof(DoubleText),
                  typeof(string),
                  typeof(DoubleTextBox),
                  new FrameworkPropertyMetadata(
                      string.Empty,
                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal,
                      new PropertyChangedCallback(OnDoubleTextChanged),
                      null,
                      true,
                      UpdateSourceTrigger.LostFocus));

        private static void OnDoubleTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                var currentText = textBox.Text;
                var newText = (string)e.NewValue;
                if (currentText == newText)
                    return;
                if (
                    double.TryParse(currentText, out var currentDouble) &&
                    double.TryParse(newText, out var newDouble) &&
                    currentDouble == newDouble
                    )
                    return;

                textBox.Text = newText;
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            this.DoubleText = this.Text;
        }
    }
}
