using LumosLIB.Kernel.Log;
using MyKlc.Plugin.Infrastructure.Messages;
using org.dmxc.lumos.Kernel.GUISession;
using org.dmxc.lumos.Kernel.Project;
using System.IO;

namespace MyKlc.Plugin.Infrastructure.Projects
{
    public class KlcProjectManager
    {
        private readonly ILumosLog _logger;
        private readonly IProjectManager _projectManager;

        public KlcProjectManager()
        {
            _logger = LumosLogger.getInstance(typeof(MyKlcPlugin));
            _projectManager = ProjectManager.getInstance();
        }

        public void LoadProject(KlcMessage message)
        {
            var projectFilePath = message.Payload.ToString();
            var projectFile = new FileInfo(projectFilePath);
            if (projectFile.Exists)
            {
                _logger.Info($"Loading project file {projectFile.FullName}");
                if (_projectManager.loadProject(projectFilePath, Session.KERNEL_SESSION_NAME))
                {
                    _logger.Info($"Project file {projectFilePath} opened in the session \"{Session.KERNEL_SESSION_NAME}\"");
                }
            }
            else
            {
                _logger.Info($"Could not found the given project file {projectFile.FullName}!");
            }
        }
        
        public void CloseProject()
        {
            _logger.Info($"Closing project in session \"{Session.KERNEL_SESSION_NAME}\"");
            _projectManager.closeProject(Session.KERNEL_SESSION_NAME);
        }
    }
}
