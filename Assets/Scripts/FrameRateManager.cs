using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    public int targetFPS;
    void Start()
    {
        // Unlock frame rate and disable VSync
        Application.targetFrameRate = targetFPS; // Set this to 60 FPS or higher if your device allows
        QualitySettings.vSyncCount = 0;   // Disable VSync to avoid FPS limits
        Debug.Log("Target Frame Rate set to: " + Application.targetFrameRate);
    }
}
