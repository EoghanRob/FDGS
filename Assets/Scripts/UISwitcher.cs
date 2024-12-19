using UnityEngine;

public class UISwitcher : MonoBehaviour
{
    // Show ComplexUI and hide SimpleUI
    public void ShowComplexUI()
    {
        ToggleUI("SimpleUI", false); // Turn off all SimpleUI objects
        ToggleUI("ComplexUI", true); // Turn on all ComplexUI objects
    }

    // Show SimpleUI and hide ComplexUI
    public void ShowSimpleUI()
    {
        ToggleUI("ComplexUI", false); // Turn off all ComplexUI objects
        ToggleUI("SimpleUI", true);   // Turn on all SimpleUI objects
    }

    // Dynamically find and toggle objects by tag
    private void ToggleUI(string tag, bool state)
    {
        GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in objects)
        {
            if (obj.CompareTag(tag) && obj.scene.isLoaded) // Ensure object is part of the active scene
            {
                obj.SetActive(state);
            }
        }
    }
}
