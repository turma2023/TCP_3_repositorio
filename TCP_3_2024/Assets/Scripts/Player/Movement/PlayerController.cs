using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private Transform pivotGun;
    
    public PlayerMovement PlayerMovement {get; private set; }
    public PlayerInputController PlayerInputController { get; private set; }
    
    private void Awake()
    {
        PlayerInputController = new PlayerInputController(GetComponent<PlayerInput>());
        PlayerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        PlayerMovement.RotateGun(ref pivotGun);
        PlayerMovement.TurnToCameraDirection();
    }
    
}