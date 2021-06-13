using MyKlc.Plugin.Infrastructure.Messages;
using org.dmxc.lumos.Kernel.Input.v2;
using org.dmxc.lumos.Kernel.Input.v2.Bank;
using System.Collections.Generic;
using System.Linq;

namespace MyKlc.Plugin.Infrastructure.Inputs
{
    public class KlcInputManager
    {
        private readonly IInputManager _inputManager;

        public KlcInputManager()
        {
            _inputManager = InputManager.getInstance();
        }

        public IList<KlcBank> GetBanks()
        {
            return _inputManager.RegisteredBanks.Select(item => new KlcBank
            {
                Id = item.ID,
                Name = item.Name,
                Number = item.Number,
                Active = item.Active,
                //Graphs = item.Graphs // ToDo: Assignment of Graphs for handling the active state of single graph entry!
            }).ToList();
        }

        public void ActivateBank(KlcMessage message)
        {
            var bank = GetBank(message);
            if (bank == null)
            {
                return;
            }

            bank.Active = true;
        }

        public void DeactivateBank(KlcMessage message)
        {
            var bank = GetBank(message);
            if (bank == null)
            {
                return;
            }

            bank.Active = false;
        }

        private IBank GetBank(KlcMessage message)
        {
            var bankToLoad = message.Payload as KlcBank;
            return _inputManager.GetBank(bankToLoad.Id);
        }
    }

}
