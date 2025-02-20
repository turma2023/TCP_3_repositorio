using Fusion;
using UnityEngine;

public class troca_De_armas : NetworkBehaviour
{
    public GameObject[] weapons;
    public Transform pivotGun; // Referência ao pivot da arma

    // Sincroniza o índice da arma para todos os jogadores
    [Networked] private int currentWeaponIndex { get; set; }

    private int lastWeaponIndex = -1; // Guarda o último índice para detectar mudanças

    void Start()
    {
        UpdateWeapon();
    }

    void Update()
    {
        if (Object.HasInputAuthority)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                RPC_SwitchWeapon(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Length > 1)
            {
                RPC_SwitchWeapon(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && weapons.Length > 2)
            {
                RPC_SwitchWeapon(2);
            }
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_SwitchWeapon(int weaponIndex)
    {
        if (weaponIndex != currentWeaponIndex && weaponIndex < weapons.Length)
        {
            currentWeaponIndex = weaponIndex;
            RPC_UpdateWeapon(weaponIndex); // Envia a atualização para todos os clientes
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_UpdateWeapon(int weaponIndex)
    {
        currentWeaponIndex = weaponIndex;
        UpdateWeapon();
    }

    void UpdateWeapon()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == currentWeaponIndex);
        }

        // Pequeno atraso para evitar sobrescrições indesejadas
        Invoke(nameof(ForceWeaponRotation), 0.05f);
    }

    void ForceWeaponRotation()
    {
        if (weapons[currentWeaponIndex] != null)
        {
            weapons[currentWeaponIndex].transform.rotation = pivotGun.rotation;
        }
    }
}
