using UnityEngine;

public class SynchronizedFlicker : MonoBehaviour
{
    public Light[] flickerLights;

    public float baseIntensity = 0.004f;
    public float flickerIntensity = 0.0007f;

    public float minTimeBetweenFlickers = 0.5f;
    public float maxTimeBetweenFlickers = 0.9f;

    public float flickerDuration = 0.08f;
    public bool randomizeFlickerDuration = true;

    private float timer;
    private float flickerTimer;
    private bool isFlickering;

    void Start()
    {
        SetAllLights(baseIntensity);
        SetNextFlickerTime();
    }

    void Update()
    {
        if (!isFlickering)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                isFlickering = true;
                flickerTimer = randomizeFlickerDuration
                    ? Random.Range(flickerDuration * 0.5f, flickerDuration * 1.5f)
                    : flickerDuration;
            }
        }
        else
        {
            flickerTimer -= Time.deltaTime;

            float currentIntensity = Random.Range(flickerIntensity, baseIntensity);
            SetAllLights(currentIntensity);

            if (flickerTimer <= 0f)
            {
                isFlickering = false;
                SetAllLights(baseIntensity);
                SetNextFlickerTime();
            }
        }
    }

    void SetNextFlickerTime()
    {
        timer = Random.Range(minTimeBetweenFlickers, maxTimeBetweenFlickers);
    }

    void SetAllLights(float intensity)
    {
        foreach (Light l in flickerLights)
        {
            if (l != null)
                l.intensity = intensity;
        }
    }
}