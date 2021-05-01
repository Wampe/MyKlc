using LumosLIB.Kernel.Log;
using MyKlc.Plugin.Infrastructure;
using MyKlc.Plugin.Infrastructure.Projects;
using MyKlc.Plugin.Infrastructure.Sockets;
using MyKlc.Plugin.Infrastructure.Messages;
using MyKlc.Plugin.Infrastructure.SceneLists;
using org.dmxc.lumos.Kernel.Plugin;

namespace MyKlc.Plugin
{
    /// <summary>
    /// Main class for the (My) Kernel Lightning Control plugin for DMXControl 3.
    /// It gets loaded and instantiated when the DMXControl Kernel gets started.
    /// </summary>
    public class MyKlcPlugin : KernelPluginBase
    {
        private readonly ILumosLog _logger;
        private readonly KlcUdpSocket _socket;
        private readonly KlcProjectManager _projectManager;
        private readonly KlcSceneListManager _sceneListManager;
       
        public MyKlcPlugin() : base(KlcConstants.PLUGIN_ID, KlcConstants.PLUGIN_NAME)
        {
            _logger = LumosLogger.getInstance(typeof(MyKlcPlugin));

            _socket = new KlcUdpSocket();
            _socket.MessageReceived += MessageReceived;

            _projectManager = new KlcProjectManager();
            _sceneListManager = new KlcSceneListManager();
        }

        /// <summary>
        /// Gets called when the plugin is getting initialized by the kernel.
        /// </summary>
        protected override void initializePlugin()
        {
            _logger.Info("Initialize");
            _socket.CreateServer();    
        }

        /// <summary>
        /// Gets called when the plugin gets shutdowned.
        /// 
        /// Note from author:
        /// As I do understand, this method just get called when the plugin is running in the DMXC GUI.
        /// At the moment I don´t know why this implementation (based on abstract declaration in <see cref="KernelPluginBase"/>) is mandatory.
        /// </summary>
        protected override void shutdownPlugin()
        {
            _logger.Info("Shutdown");
        }

        /// <summary>
        /// Gets called when the plugin starts.
        /// 
        /// Note from author:
        /// As I do understand, this method just get called when the plugin is running in the DMXC GUI.
        /// At the moment I don´t know why this implementation (based on abstract declaration in <see cref="KernelPluginBase"/>) is mandatory.
        /// </summary>
        protected override void startupPlugin()
        {
            _logger.Info("Startup");
        }

        private void MessageReceived(KlcMessage message)
        {
            switch (message.Action)
            {
                case KlcAction.OpenProject:
                    _projectManager.LoadProject(message);
                    break;
                case KlcAction.CloseProject:
                    _projectManager.CloseProject();
                    break;
                case KlcAction.LoadSceneLists:
                    _socket.SendToClient(
                        new KlcMessage
                        {
                            Action = KlcAction.LoadSceneLists,
                            Payload = _sceneListManager.GetSceneLists()
                        }
                    );
                    break;
                case KlcAction.RunSceneList:
                    _sceneListManager.RunSceneList(message);
                    break;
                case KlcAction.StopSceneList:
                    _sceneListManager.StopSceneList(message);
                    break;
            }
        }
    }
}
