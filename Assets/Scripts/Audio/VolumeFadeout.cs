using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class VolumeFadeout : MonoBehaviour
    {
        public float fadeOutTime = 1f;
        public AudioSource audioSource;
        
        private float startVolume;
        float timeLasted = 0;
        void Start()
        {
            startVolume = audioSource.volume;
        }

        void Update()
        {
            timeLasted += Time.deltaTime;

            float volume = (1f - timeLasted / fadeOutTime) * startVolume;
            audioSource.volume = Mathf.Clamp(volume, 0, startVolume);
            
        }
    }
}