using UnityEngine;
using System.Collections;

public class FlamethrowerInput : MonoBehaviour {

    private ParticleSystem particles;
    private AudioSource audio;
    private const float fadeOutTime = 0.5f;
    private float fader;

	// Use this for initialization
	void Awake() {
        particles = GetComponent<ParticleSystem>();
        audio = GetComponent<AudioSource>();

        particles.Stop();
        audio.Stop();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire1"))
        {
            particles.Play();
            audio.volume = 1f;
            fader = fadeOutTime;

            if (!audio.isPlaying)
            {
                audio.Play();    
                audio.time = 0.5f;
            }
        }
        else
        {
            fader -= Time.deltaTime;
            if (fader < 0)
            {
                fader = 0;
                audio.Stop();
            }
            particles.Stop();
            audio.volume = fader / fadeOutTime;
        }
	}
}
