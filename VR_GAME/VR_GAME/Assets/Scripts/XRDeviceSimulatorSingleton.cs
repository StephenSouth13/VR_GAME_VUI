using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

public class XRDeviceSimulatorSingleton : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Debug.Log("✅ XR Device Simulator persistera entre les scènes");
    }
}