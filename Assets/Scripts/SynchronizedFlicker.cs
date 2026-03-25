using UnityEngine;

public class SynchronizedFlicker : MonoBehaviour
{
    [System.Serializable]
    public class FlickerLightEntry
    {
        public Light lightSource;
        public float baseIntensity = 1f;
    }

    public FlickerLightEntry[] lights;

    [Header("Flicker Multiplier")]
    public float minFlickerMultiplier = 0f;
    public float maxFlickerMultiplier = 0.2f;

    [Header("Timing")]
    public float minTimeBetweenBursts = 2.5f;
    public float maxTimeBetweenBursts = 4f;

    public float burstDuration = 0.6f;
    public bool randomizeBurstDuration = true;
    public float minBurstDuration = 0.45f;
    public float maxBurstDuration = 0.75f;

    [Header("Step Timing")]
    public float minStepTime = 0.14f;
    public float maxStepTime = 0.22f;

    [Header("Blackout")]
    public bool allowFullBlackoutDip = true;
    [Range(0f, 1f)]
    public float blackoutChance = 0.8f;

    [Header("Startup Randomness")]
    public bool randomizeInitialOffset = true;

    private float waitTimer;
    private float burstTimer;
    private float stepTimer;
    private bool isFlickering;

    void Start()
    {
        SetMultiplier(1f);

        if (randomizeInitialOffset)
            waitTimer = Random.Range(0f, maxTimeBetweenBursts);
        else
            SetNextBurstTime();
    }

    void Update()
    {
        if (lights == null || lights.Length == 0) return;

        if (!isFlickering)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                isFlickering = true;

                if (randomizeBurstDuration)
                    burstTimer = Random.Range(minBurstDuration, maxBurstDuration);
                else
                    burstTimer = burstDuration;

                stepTimer = 0f;
            }
        }
        else
        {
            burstTimer -= Time.deltaTime;
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                float multiplier;

                if (allowFullBlackoutDip && Random.value < blackoutChance)
                    multiplier = 0f;
                else
                    multiplier = Random.Range(minFlickerMultiplier, maxFlickerMultiplier);

                SetMultiplier(multiplier);
                stepTimer = Random.Range(minStepTime, maxStepTime);
            }

            if (burstTimer <= 0f)
            {
                isFlickering = false;
                SetMultiplier(1f);
                SetNextBurstTime();
            }
        }
    }

    void SetNextBurstTime()
    {
        waitTimer = Random.Range(minTimeBetweenBursts, maxTimeBetweenBursts);
    }

    void SetMultiplier(float multiplier)
    {
        foreach (var entry in lights)
        {
            if (entry.lightSource != null)
                entry.lightSource.intensity = entry.baseIntensity * multiplier;
        }
    }
}