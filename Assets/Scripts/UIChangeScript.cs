using System.Collections;
using System.Collections.Generic;
using TinyGiantStudio.Text;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class UIChangeScript : MonoBehaviour
{
    private Transform targetObject;       // AR prefab reference

    // Start is called before the first frame update
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
            if (trackedImage.transform.childCount > 0) // Ensure there is at least one child
            {
                // Reference the first child of the tracked AR image
                targetObject = trackedImage.transform.GetChild(0);
            }
        }

    }

    public void ChangeScale(float scale)
    {
        Vector3 scaleVect = new Vector3(scale, scale, scale);
        targetObject.localScale = scaleVect;
    }

    public void ChangeHeight(float height)
    {
        Vector3 heightVect = new Vector3(0, targetObject.localPosition.y, height);
        targetObject.localPosition = heightVect;
    }

    public void ChangeTilt(float tilt)
    {
        targetObject.localRotation = Quaternion.Euler(tilt, 0, 0); // Tilt along the X-axis
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
