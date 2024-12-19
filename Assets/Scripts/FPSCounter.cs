using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;  // Reference to the UI Text element
    public float updateInterval = 0.5f; // How often the FPS text updates (seconds)

    private float accumulatedTime = 0f; // Total time accumulated
    private int frameCount = 0;         // Number of frames accumulated

    void Update()
    {
        // Accumulate time and frame count
        accumulatedTime += Time.deltaTime;
        frameCount++;

        // Update FPS display at intervals
        if (accumulatedTime >= updateInterval)
        {
            float fps = frameCount / accumulatedTime; // Calculate average FPS over interval

            if (fpsText != null)
            {
                fpsText.text = "FPS: " + Mathf.Ceil(fps); // Update text
            }

            // Reset counters
            accumulatedTime = 0f;
            frameCount = 0;
        }
    }
}
