using UnityEngine;
using System.Collections;

public class VolumeController : MonoBehaviour {

    private const string VOLUME_KEY = "Global Volume";

	// Use this for initialization
	void Awake () {
        AudioListener.volume = PlayerPrefs.GetFloat(VOLUME_KEY, 1f);
	}

    // Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.F12))
        {
            float newValue = 0;
            if (AudioListener.volume == 0)
            {
                newValue = 1;
            }
            AudioListener.volume = newValue;
            PlayerPrefs.SetFloat(VOLUME_KEY, newValue);
        }
	}
}
