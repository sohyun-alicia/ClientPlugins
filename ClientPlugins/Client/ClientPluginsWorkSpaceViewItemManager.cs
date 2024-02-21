using VideoOS.Platform.Client;

namespace ClientPlugins.Client
{
    public class ClientPluginsWorkSpaceViewItemManager : ViewItemManager
    {
        public ClientPluginsWorkSpaceViewItemManager() : base("ClientPluginsWorkSpaceViewItemManager")
        {
        }

        public override ViewItemWpfUserControl GenerateViewItemWpfUserControl()
        {
            return new ClientPluginsWorkSpaceViewItemWpfUserControl();
        }
    }
}
