using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("Speed of rotation in degrees per second")]
    public float rotationSpeed = 1f;
    
    private float currentRotation = 0f;

    void Update()
    {
        // Increment rotation
        currentRotation += rotationSpeed * Time.deltaTime;
        
        // Keep rotation between 0-360
        if (currentRotation >= 360f)
            currentRotation -= 360f;
        
        // Apply to skybox
        RenderSettings.skybox.SetFloat("_Rotation", currentRotation);
    }
}