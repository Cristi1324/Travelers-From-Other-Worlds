using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineAudioManager : MonoBehaviour
{
    AudioSource audioSource;
    ParticleSystem particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Pause();
        particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        audioSource.volume = Mathf.Lerp(audioSource.volume, particleSystem.emissionRate / 200, Time.deltaTime*5f);
        if (audioSource.volume > 0.1f)
        {
            audioSource.UnPause();
        }
        else
        {
            audioSource.Pause();
        }
    }
}
