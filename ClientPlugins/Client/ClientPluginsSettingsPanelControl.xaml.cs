using System.Windows;
using System.Windows.Controls;

namespace ClientPlugins.Client
{
    public partial class ClientPluginsSettingsPanelControl : UserControl
    {
        private readonly ClientPluginsSettingsPanelPlugin _plugin;
        private const string _propertyId = "aSettingId";
        public ClientPluginsSettingsPanelControl(ClientPluginsSettingsPanelPlugin plugin)
        {
            _plugin = plugin;

            InitializeComponent();

            _aSettingTextBox.Text = _plugin.GetProperty(_propertyId);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _plugin.SetProperty(_propertyId, _aSettingTextBox.Text);
            string errorMessage;
            if (!_plugin.TrySaveChanges(out errorMessage))
            {
                MessageBox.Show(errorMessage);
            }
        }
    }
}
