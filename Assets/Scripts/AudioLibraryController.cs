using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class AudioLibraryController : MonoBehaviour
    {
        private AudioSource[] sources;
        void Awake()
        {
            sources = GetComponentsInChildren<AudioSource>();
        }

        public void StopAll()
        {
            for (int i = 0; i < sources.Length; i++)
            {
                if (sources[i].isPlaying)
                {
                    sources[i].Stop();
                }
            }
        }

        public AudioSource[] GetByTag(string tag)
        {
            int count = 0;
            for (int i = 0; i < sources.Length; i++)
            {
                if (sources[i].CompareTag(tag))
                {
                    count++;
                }
            }
            if (count == 0)
            {
                bool tagExists = false;
                foreach (Transform t in transform)
                {
                    if (t.CompareTag(tag))
                    {
                        tagExists = true;
                    }
                }
                if (tagExists)
                {
                    Debug.LogWarning("Found tag " + tag + " but it had 0 audio sources for " + gameObject.name);
                    return new AudioSource[0];
                }
                else
                {
                    Debug.LogError("Could not find any audio clips or the tag " + tag
                    + " at all for " + gameObject.name);
                    return null;
                }
            }
            AudioSource[] result = new AudioSource[count];
            int c = 0;
            for (int i = 0; i < sources.Length; i++)
            {
                if (sources[i].CompareTag(tag))
                {
                    result[c++] = sources[i];
                }
            }
            return result;
        }

        public bool AreAnyPlaying(AudioSource[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].isPlaying)
                {
                    return true;
                }
            }
            return false;
        }

        public void PlayOnlyOne(AudioSource[] items)
        {
            if (items.Length == 0)
            {
                Debug.LogWarning("Received order to play one from a empty list for " + gameObject.name);
            }
            else
            {
                StopAll();
                Utility.PickRandomly(items).Play();
            }
        }
    }
}