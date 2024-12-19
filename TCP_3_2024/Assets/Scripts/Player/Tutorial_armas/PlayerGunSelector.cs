using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerGunSelector : NetworkBehaviour
{
    // public Camera Camera;
    [field: SerializeField] public GunType Gun { get; private set; }

    [SerializeField] private Transform GunParent;
    [field: SerializeField] public List<GunScriptableObject> Guns { get; private set; }
    // [SerializeField] private PlayerIK InverseKinematics;

    [Space] [Header("Runtime Filled")] public GunScriptableObject ActiveGun;
    [field: SerializeField] public GunScriptableObject ActiveBaseGun { get; private set; }

    // [SerializeField] private bool InitializeOnStart = false;

    private void Start()
    {
        // if (InitializeOnStart)
        // {
            GunScriptableObject gun = Guns.Find(gun => gun.Type == Gun);

            if (gun == null)
            {
                Debug.LogError($"No GunScriptableObject found for GunType: {gun}");
                return;
            }
            ActiveGun = gun;
            gun.Spawn(GunParent, this);
            // SetupGun(gun);
        // }
    }
}
