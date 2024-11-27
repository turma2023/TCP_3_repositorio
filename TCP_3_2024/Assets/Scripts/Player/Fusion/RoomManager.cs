// using UnityEngine;
// using Fusion;
// using Fusion.Sockets;
// using System;

// public class RoomManager : MonoBehaviour
// {
//     private NetworkRunner _networkRunner;

//     private void Start()
//     {
//         _networkRunner = gameObject.AddComponent<NetworkRunner>();
//         _networkRunner.ProvideInput = true;
//     }

//     public void CreateRoom(string roomName)
//     {
//         _networkRunner.StartGame(new StartGameArgs()
//         {
//             GameMode = GameMode.Host,
//             SessionName = roomName,
//             Scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex,
//             SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
//         });
//     }

//     public void JoinRoom(string roomName)
//     {
//         _networkRunner.StartGame(new StartGameArgs()
//         {
//             GameMode = GameMode.Client,
//             SessionName = roomName,
//             SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
//         });
//     }

//     public void LeaveRoom()
//     {
//         _networkRunner.Shutdown();
//     }
// }
