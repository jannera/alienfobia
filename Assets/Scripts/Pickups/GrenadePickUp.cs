using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class GrenadePickUp : MonoBehaviour
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
                PlayerShooting shooting = go.GetComponentInChildren<PlayerShooting>();
                shooting.grenades++;
                Destroy(gameObject);
            }
        }

        void FixedUpdate()
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 30);
        }
    }

}