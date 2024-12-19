using TinyGiantStudio.Text; // Modular 3D Text namespace
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SpeedAnimator : MonoBehaviour
{
    [Header("Text Settings")]
    public float fluctuationRange = 2f;  // Max fluctuation above and below the speed limit
    public float adjustmentSpeed = 1f;   // How quickly the speed adjusts to the target
    public float fluctuationInterval = 0.5f; // How often the target speed changes (seconds)

    [Header("Speed Limit Cycle Settings")]
    public float speedLimitChangeInterval = 5f; // How often the speed limit changes (seconds)
    private float speedLimitTimer = 0f;         // Timer to trigger the speed limit change
    private int[] speedLimits = { 50, 80, 120 }; // Speed limit values to cycle through
    private int currentLimitIndex = 0;          // Current index in the speedLimits array

    private Modular3DText speedText;      // First child text (current speed)
    private Modular3DText speedLimitText; // Second child text (speed limit)
    private float targetSpeed = 0f;       // The fluctuating target speed
    private float currentSpeed = 0f;      // The current speed displayed
    private float fluctuationTimer = 0f;  // Timer to reset target speed

    private Transform targetObject;       // AR prefab reference
    private bool isRunning = false;       // Control whether to start updating

    void Start()
    {
        // Subscribe to ARTrackedImageManager events
        ARTrackedImageManager imageManager = FindObjectOfType<ARTrackedImageManager>();
        if (imageManager != null)
        {
            imageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            if (trackedImage.transform.childCount > 0)
            {
                // Access the intermediate child (assuming it's the first child)
                Transform intermediateChild = trackedImage.transform.GetChild(0);

                if (intermediateChild.childCount >= 2) // Ensure there are enough children
                {
                    // Now access the actual children from the intermediate child
                    targetObject = intermediateChild;
                    speedText = targetObject.GetChild(0).GetComponentInChildren<Modular3DText>();
                    speedLimitText = targetObject.GetChild(1).GetComponentInChildren<Modular3DText>();

                    if (speedText != null && speedLimitText != null)
                    {
                        isRunning = true;
                        ResetFluctuation(); // Initialize the first target speed
                        UpdateSpeedLimit(); // Initialize the speed limit
                    }
                }
            }
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            ResetValues();
        }
    }

    void Update()
    {
        if (isRunning && speedText != null && speedLimitText != null)
        {
            speedLimitTimer += Time.deltaTime;
            if (speedLimitTimer >= speedLimitChangeInterval)
            {
                UpdateSpeedLimit();
            }

            // Read the speed limit
            if (float.TryParse(speedLimitText.Text, out float speedLimit))
            {
                // Update fluctuation timer and reset target speed at intervals
                fluctuationTimer += Time.deltaTime;
                if (fluctuationTimer >= fluctuationInterval)
                {
                    ResetFluctuation(speedLimit);
                }

                // Smoothly adjust the current speed towards the fluctuating target speed
                currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, adjustmentSpeed * Time.deltaTime);

                // Update the displayed speed
                speedText.UpdateText(Mathf.RoundToInt(currentSpeed).ToString());
            }
        }
    }

    void UpdateSpeedLimit()
    {
        // Cycle through the speed limits and update the text instantly
        currentLimitIndex = (currentLimitIndex + 1) % speedLimits.Length;
        int newSpeedLimit = speedLimits[currentLimitIndex];
        speedLimitText.UpdateText(newSpeedLimit.ToString());

        // Reset the target speed fluctuation to match the new speed limit
        ResetFluctuation(newSpeedLimit);

        // Reset the speed limit change timer
        speedLimitTimer = 0f;
    }

    void ResetFluctuation(float speedLimit = 0f)
    {
        // Reset the target speed within the fluctuation range
        targetSpeed = speedLimit + Random.Range(-fluctuationRange, fluctuationRange);
        fluctuationTimer = 0f; // Reset the timer
    }

    void ResetValues()
    {
        targetObject = null;
        speedText = null;
        speedLimitText = null;
        isRunning = false;
        currentSpeed = 0f;
        targetSpeed = 0f;
        currentLimitIndex = 0;
        speedLimitTimer = 0f;
    }

    void OnDestroy()
    {
        // Clean up event listeners
        ARTrackedImageManager imageManager = FindObjectOfType<ARTrackedImageManager>();
        if (imageManager != null)
        {
            imageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }
}
