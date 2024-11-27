// using Fusion;
// using Photon.Realtime;
// using UnityEditor.Experimental.GraphView;
// using UnityEditor.ShaderGraph;
// using UnityEngine;

// public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
// {
//     public GameObject PlayerPrefab;
//     public PlayerRef player = new PlayerRef();
//     public void PlayerJoined(PlayerRef player)
//     {
//         Debug.Log("metodo chamado aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
//         if (player == Runner.LocalPlayer)
//         {
//             Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
//         }
//     }
//     public void PSeila(){
//         PlayerJoined(player);
//     }
// }
