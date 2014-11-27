using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class VolumeController : MonoBehaviour
    {

        private const string VOLUME_KEY = "Global Volume";

        void Awake()
        {
            AudioListener.volume = PlayerPrefs.GetFloat(VOLUME_KEY, 1f);
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.F12))
            {
                float newValue = AudioListener.volume + 0.25f;
                if (newValue >= 1)
                {
                    newValue = 0;
                }
                AudioListener.volume = newValue;
                PlayerPrefs.SetFloat(VOLUME_KEY, newValue);
            }
        }
    }
}