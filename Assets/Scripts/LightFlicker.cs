using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private Light pointLight;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 0.1f;

    void Start()
    {
        pointLight = GetComponent<Light>();
        if (pointLight == null || pointLight.type != LightType.Point)
        {
            Debug.LogError("This script requires a Point Light component on the same GameObject.");
        }
    }

    void Update()
    {
        if (pointLight != null)
        {
            pointLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time * flickerSpeed, 1));
        }
    }
}
