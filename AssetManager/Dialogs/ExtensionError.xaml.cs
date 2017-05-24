using FirstFloor.ModernUI.Windows.Controls;
using System.Windows.Controls;

namespace AssetManager.Dialogs
{
    /// <summary>
    /// Interaction logic for ExtensionError.xaml
    /// </summary>
    public partial class ExtensionError : ModernDialog
    {
        public ExtensionError()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { this.OkButton };
        }
    }
}
