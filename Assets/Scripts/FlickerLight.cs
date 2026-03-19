using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    public Light targetLight;

    public float baseIntensity = 0.004f;
    public float flickerIntensity = 0.002f;

    public float minTimeBetweenFlickers = 0.4f;
    public float maxTimeBetweenFlickers = 1.5f;

    public float flickerDuration = 0.08f;
    public bool randomizeFlickerDuration = true;

    private float timer;
    private float flickerTimer;
    private bool isFlickering;

    void Start()
    {
        if (targetLight == null)
            targetLight = GetComponent<Light>();

        targetLight.intensity = baseIntensity;
        SetNextFlickerTime();
    }

    void Update()
    {
        if (targetLight == null) return;

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

            targetLight.intensity = Random.Range(flickerIntensity, baseIntensity);

            if (flickerTimer <= 0f)
            {
                isFlickering = false;
                targetLight.intensity = baseIntensity;
                SetNextFlickerTime();
            }
        }
    }

    void SetNextFlickerTime()
    {
        timer = Random.Range(minTimeBetweenFlickers, maxTimeBetweenFlickers);
    }
}