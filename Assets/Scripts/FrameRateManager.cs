using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    public int targetFPS;
    void Start()
    {
        // Unlock frame rate and disable VSync
        Application.targetFrameRate = targetFPS; // Aims for 60 fps
        QualitySettings.vSyncCount = 0;   // Disable VSync
        Debug.Log("Target Frame Rate set to: " + Application.targetFrameRate);
    }
}
