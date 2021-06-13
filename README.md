# MyKlc - My Kernel Lightning Control

Kernel plugin for [DMXControl 3](https://www.dmxcontrol.de/en/).

DMXControl is a lightning control software provided by the registered non-profit association [DMXControl Projects e.V.](https://dmxcontrol-projects.org/en/)

This plugin provides a UDP socket transfer based communication between the DMXControl Kernel and a (.NET based) client application that can use UDP sockets.

## Available message actions

- OpenProject
- CloseProject
- LoadSceneLists
- RunSceneList
- StopSceneList
- LoadInputBanks
- ActivateInputBank
- DeactivateInputBank
