using VideoOS.Platform.Admin;

namespace ClientPlugins.Admin
{
    public partial class ClientPluginsToolsOptionDialogUserControl : ToolsOptionsDialogUserControl
    {
        public ClientPluginsToolsOptionDialogUserControl()
        {
            InitializeComponent();
        }

        public override void Init()
        {
        }

        public override void Close()
        {
        }

        public string MyPropValue
        {
            set { textBoxPropValue.Text = value ?? ""; }
            get { return textBoxPropValue.Text; }
        }
    }
}
