using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralParticle : MonoBehaviour {

    public int frequency = 1;
    public float resolution = 20.0f;
    public float amplitude = 1.0f;
    public float zValue = 0.0f;

    ParticleSystem ps;

    // Use this for initialization
    void Start () {
        ps = GetComponent<ParticleSystem>();
        CreateSpiral();
	}
	
    public void CreateSpiral()
    {
        //ParticleSystem ps = GetComponent<ParticleSystem>();
        var vel = ps.velocityOverLifetime;
        vel.enabled = true;
        vel.space = ParticleSystemSimulationSpace.Local;

        ps.startSpeed = 0.0f;

        vel.z = new ParticleSystem.MinMaxCurve(10.0f, zValue);

        AnimationCurve curveX = new AnimationCurve();
        for (int i = 0; i < resolution; i++)
        {
            float newTime = (i / (resolution - 1));
            float myValue = amplitude * Mathf.Sin(newTime * (frequency * 2) * Mathf.PI);

            curveX.AddKey(newTime, myValue);

        }

        vel.x = new ParticleSystem.MinMaxCurve(10.0f, curveX);


        AnimationCurve curveY = new AnimationCurve();
        for (int i = 0; i < resolution; i++)
        {
            float newTime = (i / (resolution - 1));
            float myValue = amplitude * Mathf.Cos(newTime * (frequency * 2) * Mathf.PI);

            curveY.AddKey(newTime, myValue);

        }

        vel.y = new ParticleSystem.MinMaxCurve(10.0f, curveY);

        //ps.Play();
    }

    public void Play()
    {
        //Debug.Log("play spiral particle");
        ps.Play();
    }

    public void Stop()
    {
        //Debug.Log("stop spiral particle");
        ps.Stop();
    }

    public void Emit(int amount)
    {
        //Debug.Log("emit spiral particle");
        ps.Emit(amount);
    }

    public bool IsPlaying()
    {
        return ps.isPlaying;
    }

    public int GetParticleCount()
    {
        return ps.particleCount;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
