using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class HealthPickup : MonoBehaviour
    {

        public float rotationSpeed = 1;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter(Collider other)
        {

            GameObject go = other.gameObject;
            // If the entering collider is the player...
            if (go.CompareTag("Player"))
            {
                Debug.Log("HealthPickup");
                PlayerHealth health = go.GetComponentInChildren<PlayerHealth>();
                health.AddHealth(30);
                Destroy(gameObject);
            }
        }

        void FixedUpdate()
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 30);
        }
    }

}