using Microsoft.Win32;
using MyKlc.Plugin.Infrastructure.Messages;
using MyKlc.Plugin.Infrastructure.SceneLists;
using MyKlc.Plugin.Infrastructure.Sockets;
using System;
using System.Collections.Generic;
using System.Windows;

namespace MyKlc.TestClient
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KlcUdpSocket _socket;

        public MainWindow()
        {
            InitializeComponent();
            _socket = new KlcUdpSocket();
            _socket.MessageReceived += MessageReceived;
            _socket.CreateClient(12301);
        }

        private void MessageReceived(KlcMessage message)
        {
            if (message.Action == KlcAction.LoadSceneLists)
            {
                Dispatcher.BeginInvoke(new Action(delegate ()
                {
                    SceneListsBox.ItemsSource = message.Payload as IList<KlcSceneList>;
                }));
            }
        }

        private void LoadProject_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _socket.SendToServer(new KlcMessage
                {
                    Action = KlcAction.OpenProject,
                    Payload = openFileDialog.FileName
                });
            }
        }

        private void CloseProject_Click(object sender, RoutedEventArgs e)
        {
            _socket.SendToServer(new KlcMessage
            {
                Action = KlcAction.CloseProject
            });
        }

        private void GetSceneLits_Click(object sender, RoutedEventArgs e)
        {
            _socket.SendToServer(new KlcMessage
            {
                Action = KlcAction.LoadSceneLists
            });
        }

        private void StartSceneList_Click(object sender, RoutedEventArgs e)
        {
            _socket.SendToServer(new KlcMessage
            {
                Action = KlcAction.RunSceneList,
                Payload = SceneListsBox.SelectedItem
            });
        }

        private void StopSceneList_Click(object sender, RoutedEventArgs e)
        {
            _socket.SendToServer(new KlcMessage
            {
                Action = KlcAction.StopSceneList,
                Payload = SceneListsBox.SelectedItem
            });
        }
    }
}
