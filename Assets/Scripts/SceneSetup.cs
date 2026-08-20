using UnityEngine;
using UnityEngine.UI;

public class SceneSetup : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void SetupScene()
    {
        // This script will auto-setup everything when the scene loads
    }
}

// This is a helper script - you can delete this after setup is complete
