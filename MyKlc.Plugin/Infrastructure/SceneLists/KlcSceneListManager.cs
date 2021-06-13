using MyKlc.Plugin.Infrastructure.Messages;
using org.dmxc.lumos.Kernel.Project;
using org.dmxc.lumos.Kernel.SceneList;
using System.Collections.Generic;
using System.Linq;

namespace MyKlc.Plugin.Infrastructure.SceneLists
{
    public class KlcSceneListManager
    {
        private readonly ISceneListManager _sceneListManager;

        public KlcSceneListManager()
        {
            _sceneListManager = SceneListManager.getInstance();
        }
       
        public IList<KlcSceneList> GetSceneLists()
        {
            return _sceneListManager.SceneLists.Select(item => new KlcSceneList
            {
                Id = item.ID,
                Name = item.Name
            }).ToList();
        }

        public void RunSceneList(KlcMessage message)
        {
            var scenelist = GetSceneList(message);
            if (scenelist == null)
            {
                return;
            }

            scenelist.go();
        }

        public void StopSceneList(KlcMessage message)
        {
            var scenelist = GetSceneList(message);
            if (scenelist == null)
            {
                return;
            }

            scenelist.stop();
        }

        private ISceneList GetSceneList(KlcMessage message)
        {
            var sceneList = message.Payload as KlcSceneList;
            return _sceneListManager.GetSceneListByID(sceneList.Id);
        }
    }
}
