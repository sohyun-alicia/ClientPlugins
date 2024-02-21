using System;
using System.Drawing;
using VideoOS.Platform.Client;

namespace ClientPlugins.Client
{
    public class ClientPluginsViewLayout : ViewLayout
    {
        public override Image Icon
        {
            get { return ClientPluginsDefinition.TreeNodeImage; }
            set { }
        }

        public override Rectangle[] Rectangles
        {
            get { return new Rectangle[] { new Rectangle(000, 000, 999, 499), new Rectangle(000, 499, 499, 499), new Rectangle(499, 499, 499, 499) }; }
            set { }
        }

        public override Guid Id
        {
            get { return ClientPluginsDefinition.ClientPluginsViewLayoutId; }
            set { }
        }

        public override string DisplayName
        {
            get { return "ClientPlugins"; }
            set { }
        }
    }
}
