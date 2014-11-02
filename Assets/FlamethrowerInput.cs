using UnityEngine;
using System.Collections;

public class FlamethrowerInput : MonoBehaviour {

    private BoxCollider collider;
    private ParticleSystem particles;

	// Use this for initialization
	void Awake() {
        collider = GetComponent<BoxCollider>();
        particles = GetComponentInChildren<ParticleSystem>();

        SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire1"))
        {
            SetActive(true);
        }
        else
        {
            SetActive(false);
        }
	}

    private void SetActive(bool b)
    {
        collider.enabled = b;
        if (b)
        {
            particles.Play();
        }
        else
        {
            particles.Stop();
        }
        
    }
}
