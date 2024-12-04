// using Fusion;
// using UnityEngine;

// public class FirstPersonCamera : NetworkBehaviour
// {
//     public Transform Target;
//     public float MouseSensitivity = 10f;

//     private float verticalRotation;
//     private float horizontalRotation;

//     void LateUpdate()
//     {
//         if (Target == null)
//         {
//             return;
//         }

//         transform.position = Target.position;

//         float mouseX = Input.GetAxis("Mouse X");
//         float mouseY = Input.GetAxis("Mouse Y");

//         verticalRotation -= mouseY * MouseSensitivity;
//         verticalRotation = Mathf.Clamp(verticalRotation, -70f, 70f);

//         horizontalRotation += mouseX * MouseSensitivity;

//         transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
//     }
// }

using Fusion;
using UnityEngine;

public class FirstPersonCamera : NetworkBehaviour
{
    public Transform Target;
    public float MouseSensitivity = 10f;

    private float verticalRotation;
    private float horizontalRotation;

    void Start()
    {
        // Prender o cursor e torná-lo invisível
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }

        // Liberar o cursor ao pressionar "Esc"
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Prender o cursor novamente ao clicar com o botão esquerdo do mouse
        if (Input.GetMouseButtonDown(0))
        {
            //Cursor.lockState = CursorLockMode.Locked;
           // Cursor.visible = false;
        }
        if (!Cursor.visible){
            transform.position = Target.position;

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            verticalRotation -= mouseY * MouseSensitivity;
            verticalRotation = Mathf.Clamp(verticalRotation, -70f, 70f);

            horizontalRotation += mouseX * MouseSensitivity;

            transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
        }
    }
}
