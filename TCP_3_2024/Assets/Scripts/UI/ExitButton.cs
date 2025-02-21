using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class ExitButton : MonoBehaviour
{

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; 
#else
            Application.Quit();
#endif
    }
}
