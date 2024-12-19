using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class MaterialChanger : MonoBehaviour
{
    [Header("UI Button")]
    public Button changeMaterialButton;

    [Header("Materials")]
    public Material[] materials; // Materials to cycle through

    private GameObject trackedInstance; // Reference to the spawned prefab instance
    private int currentMaterialIndex = 0;
    private bool isSpinning = false; // Flag to check if spinning is active
    private float spinSpeed = 100f;  // Speed of spinning

    void Start()
    {
        // Set up the button listener
        if (changeMaterialButton != null)
        {
            changeMaterialButton.onClick.AddListener(OnButtonPressed);
        }

        // Subscribe to ARTrackedImageManager events
        ARTrackedImageManager trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
    }

    void Update()
    {
        // Make the tracked instance spin if enabled
        if (isSpinning && trackedInstance != null)
        {
            trackedInstance.transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.Self);
        }
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Check for added or updated tracked images
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            trackedInstance = trackedImage.transform.GetChild(0).gameObject; // Get the prefab instance
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            trackedInstance = trackedImage.transform.GetChild(0).gameObject;
        }
    }

    void OnButtonPressed()
    {
        // Change material
        ChangeMaterial();

        // Toggle spinning
        isSpinning = !isSpinning; // Switch spinning on/off
    }

    void ChangeMaterial()
    {
        if (trackedInstance != null && materials.Length > 0)
        {
            Renderer renderer = trackedInstance.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                // Cycle through the materials
                currentMaterialIndex = (currentMaterialIndex + 1) % materials.Length;
                renderer.material = materials[currentMaterialIndex];
            }
        }
    }

    void OnDestroy()
    {
        // Clean up the event listener
        ARTrackedImageManager trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }
}
