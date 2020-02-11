using System.Windows.Controls;
using System.Windows.Input;

namespace KzLibraries.KzWpfControl
{
    public class FirstEmptyComboBox : ComboBox
    {
        public FirstEmptyComboBox() : base()
        {
            this.IsEditable = true;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Up && this.SelectedIndex == 0)
            {
                this.Text = string.Empty;
                e.Handled = true;
            }
            else base.OnPreviewKeyDown(e);
        }
    }
}
