using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class AudioListenerController : PhotonBehaviour
    {
        void Awake()
        {
            AudioListener camAudio = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioListener>();
            if (camAudio != null) {
                camAudio.enabled = false;
            }

            if (!photonView.isMine)
            {
                GetComponent<AudioListener>().enabled = false;
            }
        }
    }
}