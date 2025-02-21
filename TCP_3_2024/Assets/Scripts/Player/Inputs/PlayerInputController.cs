using UnityEngine.InputSystem;

public class PlayerInputController
{
    
    public InputAction MoveAction { get; private set; } 
    public InputAction FireAction { get; private set; }
    public InputAction JumpAction { get; private set; }
    public InputAction Interact { get; private set; }
    // private PlayerInput playerInput;
   
    public PlayerInputController(PlayerInput playerInput)
    {
        MoveAction = playerInput.actions["Move"];
        FireAction = playerInput.actions["Fire"];
        JumpAction = playerInput.actions["Jump"];
        Interact = playerInput.actions["Interact"];
        
        
    }
    
    
    
}
