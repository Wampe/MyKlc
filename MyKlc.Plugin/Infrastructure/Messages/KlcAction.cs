using System;

namespace MyKlc.Plugin.Infrastructure.Messages
{
    [Serializable]
    public enum KlcAction
    {
        OpenProject,
        CloseProject,
        LoadSceneLists,
        RunSceneList,
        StopSceneList,
        LoadInputBanks,
        ActivateInputBank,
        DeactivateInputBank
    }
}
