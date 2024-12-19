using UnityEngine;
using TMPro; // For TextMeshPro Text boxes
using TinyGiantStudio.Text; // Modular 3D Text namespace

using UnityEngine.XR.ARFoundation;

public class RPMCalculator : MonoBehaviour
{
    [Header("UI References")]
    private Modular3DText speedText;      // First child text (current speed)
    private Modular3DText rpmText; // Second child text (speed limit)
    private Modular3DText gearText; // Second child text (speed limit)

    [Header("Bike Parameters")]
    public float speed; // Current speed in km/h (simulate or set manually)

    private int currentGear = 1;   // Current gear
    private float rpm = 0;         // Current RPM

    private readonly int[] gearSpeeds = { 0, 20, 40, 60, 90, 100, 110 }; // Speed ranges for each gear
    private readonly float maxRPM = 9000f;  // Max RPM
    private readonly float idleRPM = 1200f; // Idle RPM

    [Header("RPM Indicator")]
    private Transform rpmIndicatorStick;   // The game object to rotate
    private float minRotation = -110f;    // Rotation at 0 RPM
    private float maxRotation = 110f;     // Rotation at max RPM


    private Transform targetObject;       // AR prefab reference
    private bool isRunning = false;       // Control whether to start updating

    public float offsetRotationIndicator;

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
                // Access the intermediate child (assuming it's the first child of trackedImage)
                Transform intermediateChild = trackedImage.transform.GetChild(0);

                if (intermediateChild.childCount >= 6) // Ensure there are enough children
                {
                    // Now access the actual children from the intermediate child
                    targetObject = intermediateChild;

                    speedText = targetObject.GetChild(0).GetComponentInChildren<Modular3DText>();
                    gearText = targetObject.GetChild(3).GetComponentInChildren<Modular3DText>();
                    rpmText = targetObject.GetChild(4).GetComponentInChildren<Modular3DText>();

                    rpmIndicatorStick = targetObject.GetChild(5);

                    if (speedText != null)
                    {
                        isRunning = true;
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
        if (isRunning && speedText != null)
        {
            // Read the speed
            if (float.TryParse(speedText.Text, out float speedValue))
            {
                // Smoothly adjust the current speed towards the fluctuating target speed
                //rpmText = Mathf.Lerp(rpmText, targetSpeed, adjustmentSpeed * Time.deltaTime);

                speed = speedValue;

                for (int i = 1; i < gearSpeeds.Length; i++)
                {
                    if (speed < gearSpeeds[i])
                    {
                        currentGear = i;
                        break;
                    }
                    else
                    {
                        currentGear = gearSpeeds.Length - 1; // Highest gear
                    }
                }

                // Calculate RPM: Scale based on speed within current gear range
                float minSpeed = gearSpeeds[currentGear - 1];
                float maxSpeed = gearSpeeds[currentGear];

                float gearSpeedRange = maxSpeed - minSpeed;
                float speedOffset = speed - minSpeed;

                rpm = idleRPM + (maxRPM - idleRPM) * (speedOffset / gearSpeedRange);

                // Clamp RPM to stay below max
                rpm = Mathf.Clamp(rpm, idleRPM, maxRPM);

                // Update the displayed speed
                //speedText.UpdateText(Mathf.RoundToInt(currentSpeed).ToString());

                gearText.UpdateText(Mathf.RoundToInt(currentGear).ToString());
                rpmText.UpdateText(Mathf.RoundToInt(rpm).ToString());

                // Update RPM Indicator Rotation
                UpdateRPMIndicator();
            }
        }
    }

    void UpdateRPMIndicator()
    {
        if (rpmIndicatorStick != null)
        {
            // Map RPM to rotation angle between -110 and 110
            float rotationAngle = offsetRotationIndicator + (-maxRotation*(rpm / maxRPM));

            // Apply rotation to the RPM indicator (only on the local X-axis)
            //rpmIndicatorStick.rotation = Quaternion.Euler(0, 0, rotationAngle);
        }
    }

    void ResetValues()
    {
        targetObject = null;
        speedText = null;
        isRunning = false;
    }
}
