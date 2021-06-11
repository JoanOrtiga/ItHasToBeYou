using UnityEngine;
using System.Collections.Generic;

public class LightFlicker : MonoBehaviour
{
    public bool changeColor = true;

    [SerializeField] private Color color1;
    [SerializeField] private Color color2;
    
    [Tooltip("How much to smooth out the randomness;")]
    [Range(0.0f, 5.0f)]
    public float smoothingColor = 5;

    public float timer;
    private float timing = .1f;

    [Tooltip("External light to flicker; you can leave this null if you attach script to a light")]
    public new Light light;

    public bool changeIntensity = true;
    
    [Tooltip("Minimum random light intensity")]
    public float minIntensity = 0f;
    [Tooltip("Maximum random light intensity")]
    public float maxIntensity = 1f;
    [Tooltip("How much to smooth out the randomness; lower values = sparks, higher = lantern")]
    [Range(1, 50)]
    public int smoothing = 5;

    // Continuous average calculation via FIFO queue
    // Saves us iterating every time we update, we just change by the delta
    Queue<float> smoothQueue;
    float lastSum = 0;


    private Color goToColor;
    
    /// <summary>
    /// Reset the randomness and start again. You usually don't need to call
    /// this, deactivating/reactivating is usually fine but if you want a strict
    /// restart you can do.
    /// </summary>
    public void Reset() {
        smoothQueue.Clear();
        lastSum = 0;
    }

    void Start() {
        smoothQueue = new Queue<float>(smoothing);

        // External or internal light?
        if (light == null) {
            light = GetComponent<Light>();
        }
    }

    void Update() {
        if (light == null)
            return;

        // pop off an item if too big
        while (smoothQueue.Count >= smoothing) {
            lastSum -= smoothQueue.Dequeue();
        }

        if (changeColor)
        {
            if (timing <= 0)
            {
                timing = timer;
                goToColor = new Color(Random.Range(color1.r, color2.r), Random.Range(color1.g, color2.g), Random.Range(color1.b, color2.b));
            }
            
            light.color = Color.Lerp(light.color, goToColor, smoothingColor);
            timing -= Time.deltaTime;
        }

        if (changeIntensity)
        {
            // Generate random new item, calculate new average
            float newVal = UnityEngine.Random.Range(minIntensity, maxIntensity);
            smoothQueue.Enqueue(newVal);
            lastSum += newVal;

            // Calculate new smoothed average
            light.intensity = lastSum / (float)smoothQueue.Count;
        }
    }
}