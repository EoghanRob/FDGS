using TinyGiantStudio.Text;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageScaleManager : MonoBehaviour
{
    [Header("Target Scale")]
    public Vector3 targetScale = new Vector3(1, 1, 1); // Final scale of the object
    public float scaleSpeed = 2f;                      // Speed of the scaling animation

    [Header("Text Settings")]
    public int startValue = 1;   // Starting value of the text
    public int endValue = 50;    // Target value of the text

    private bool isScaling = false;                   // Flag to control scaling animation
    private Transform targetObject;                   // Reference to the instantiated object
    private Vector3 initialScale = Vector3.zero;      // Start scale (0)
    private float currentProgress = 0f;               // Progress of scaling (0 to 1)


    void Start()
    {
        // Subscribe to the ARTrackedImageManager events
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
                targetObject = trackedImage.transform.GetChild(0); // Get the spawned prefab

                StartScaling();
            }
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                if (targetObject == null || targetObject != trackedImage.transform.GetChild(0))
                {
                    targetObject = trackedImage.transform.GetChild(0);

                    StartScaling();
                }
            }
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            targetObject = null;
        }
    }

    void StartScaling()
    {
        if (targetObject != null)
        {
            targetObject.localScale = initialScale; // Start at 0 scale
            currentProgress = 0f;                  // Reset progress
            isScaling = true;                      // Enable scaling
        }
    }

    void Update()
    {
        if (isScaling && targetObject != null)
        {
            // Progress scaling
            currentProgress += scaleSpeed * Time.deltaTime;

            // Smoothly scale the object
            targetObject.localScale = Vector3.Lerp(initialScale, targetScale, currentProgress);


            // Stop scaling once the target scale is reached
            if (currentProgress >= 1.0f)
            {
                targetObject.localScale = targetScale;
                isScaling = false;
            }
        }
    }

    void OnDestroy()
    {
        // Clean up the event listener
        ARTrackedImageManager imageManager = FindObjectOfType<ARTrackedImageManager>();
        if (imageManager != null)
        {
            imageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }
}
