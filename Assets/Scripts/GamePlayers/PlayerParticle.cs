using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticle : MonoBehaviour
{
    private float onTime;
    private static float Passedtime;
    [SerializeField] ParticleSystem particle;

    private void Start()
    {
        onTime = 1.5f;
        Passedtime = 10f;
    }

    private void Update()
    {
        Passedtime += Time.deltaTime;

        if(Passedtime < onTime)
        {
            return;
        }
        if(!particle.isPlaying)
        {
            return;
        }

        particle.Stop();

    }

    public void OnParticle()
    {
        Passedtime = 0;
        particle.Play();
    }
}
