using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BreathCamera : MonoBehaviour
{
    public float amplitude = 1f;
    public float frequency = 0.5f;

    [Space, Header("Axis")] [SerializeField]
    private bool x = true;
    [SerializeField] private bool y = true;
    [SerializeField] private bool z = true;

    private Vector3 m_finalRot;

    private PerlinNoiseScroller perlinNoiseScroller;

    private void Start()
    {
        perlinNoiseScroller = new PerlinNoiseScroller();
    }

    void LateUpdate()
    {
        perlinNoiseScroller.UpdateNoise(frequency, amplitude);

        Vector3 _rotOffset = Vector3.zero;

        if (x)
            _rotOffset.x += perlinNoiseScroller.noise.x;

        if (y)
            _rotOffset.y += perlinNoiseScroller.noise.y;

        if (z)
            _rotOffset.z += perlinNoiseScroller.noise.z;

        m_finalRot.x = x ? _rotOffset.x : transform.localEulerAngles.x;
        m_finalRot.y = y ? _rotOffset.y : transform.localEulerAngles.y;
        m_finalRot.z = z ? _rotOffset.z : transform.localEulerAngles.z;

        transform.localEulerAngles = m_finalRot;
    }
}

public class PerlinNoiseScroller
{
    Vector3 noiseOffset;
    public Vector3 noise;
    
    public PerlinNoiseScroller()
    {
        float _rand = 32f;

        noiseOffset.x = Random.Range(0f, _rand);
        noiseOffset.y = Random.Range(0f, _rand);
        noiseOffset.z = Random.Range(0f, _rand);
    }

    public void UpdateNoise(float frequency, float amplitude)
    {
        float _scrollOffset = Time.deltaTime * frequency;

        noiseOffset.x += _scrollOffset;
        noiseOffset.y += _scrollOffset;
        noiseOffset.z += _scrollOffset;

        noise.x = Mathf.PerlinNoise(noiseOffset.x, 0f);
        noise.y = Mathf.PerlinNoise(noiseOffset.x, 1f);
        noise.z = Mathf.PerlinNoise(noiseOffset.x, 2f);

        noise -= Vector3.one * 0.5f;
        noise *= amplitude;
    }
}