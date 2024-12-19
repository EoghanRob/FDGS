using UnityEngine;

public class BottomBarMove : MonoBehaviour
{
    public Vector3 upPosition;   // Target position when up
    public Vector3 downPosition; // Target position when down

    public bool isUp;            // Toggle state
    public float moveSpeed = 2f; // Speed of the movement

    private Vector3 targetPosition; // Current target position

    void Start()
    {
        targetPosition = isUp ? upPosition : downPosition;
    }

    public void SwitchBar()
    {
        // Set the target position based on the current state
        targetPosition = isUp ? downPosition : upPosition;

        isUp = !isUp;
    }

    void Update()
    {
        // Smoothly move the object towards the target position
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);
    }
}
