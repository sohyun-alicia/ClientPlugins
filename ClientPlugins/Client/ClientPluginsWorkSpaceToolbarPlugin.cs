using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace ClientPlugins.Client
{
    internal class ClientPluginsWorkSpaceToolbarPluginInstance : WorkSpaceToolbarPluginInstance
    {
        private Item _window;

        public ClientPluginsWorkSpaceToolbarPluginInstance()
        {
        }

        public override void Init(Item window)
        {
            _window = window;

            Title = "ClientPlugins";
        }

        public override void Activate()
        {
            // Here you should put whatever action that should be executed when the toolbar button is pressed
        }

        public override void Close()
        {
        }

    }

    internal class ClientPluginsWorkSpaceToolbarPlugin : WorkSpaceToolbarPlugin
    {
        public ClientPluginsWorkSpaceToolbarPlugin()
        {
        }

        public override Guid Id
        {
            get { return ClientPluginsDefinition.ClientPluginsWorkSpaceToolbarPluginId; }
        }

        public override string Name
        {
            get { return "ClientPlugins"; }
        }

        public override void Init()
        {
            // TODO: remove below check when ClientPluginsDefinition.ClientPluginsWorkSpaceToolbarPluginId has been replaced with proper GUID
            if (Id == new Guid("22222222-2222-2222-2222-222222222222"))
            {
                System.Windows.MessageBox.Show("Default GUID has not been replaced for ClientPluginsWorkSpaceToolbarPluginId!");
            }

            WorkSpaceToolbarPlaceDefinition.WorkSpaceIds = new List<Guid>() { ClientControl.LiveBuildInWorkSpaceId, ClientControl.PlaybackBuildInWorkSpaceId, ClientPluginsDefinition.ClientPluginsWorkSpacePluginId };
            WorkSpaceToolbarPlaceDefinition.WorkSpaceStates = new List<WorkSpaceState>() { WorkSpaceState.Normal };
        }

        public override void Close()
        {
        }

        public override WorkSpaceToolbarPluginInstance GenerateWorkSpaceToolbarPluginInstance()
        {
            return new ClientPluginsWorkSpaceToolbarPluginInstance();
        }
    }
}
