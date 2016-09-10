using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace HermzApp.WODTimer.Windows81.Controls
{
    public sealed partial class TextBlockWithLabel : UserControl
    {
        public TextBlockWithLabel()
        {
            this.InitializeComponent();
        }

        public string LabelText
        {
            get { return this.HeaderLabel.Text; }
            set { this.HeaderLabel.Text = value; }
        }

        public string NumberText
        {
            get { return this.NumText.Text; }
            set { this.NumText.Text = value; }
        }

        public Brush NumberTextColor
        {
            get { return this.NumText.Foreground; }
            set { this.NumText.Foreground = value; }
        }
    }
}
