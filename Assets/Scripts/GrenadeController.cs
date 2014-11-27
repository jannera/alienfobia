using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class GrenadeController : CompleteProject.PhotonBehaviour
    {
        public float startSpeed = 10;
        public float explosionRadius = 5;
        public float explosionForce = 100;
        public int damagePerShot = 100;

        private Vector3 push;

        public GameObject explosionPreFab;
        public AudioSource explosionSound;

        public float explosionTime = 2f;
        public float startTime;

        private bool playingExplosionSound = false;

        void Start()
        {
            float angle = (float)photonView.instantiationData[0];
            angle *= Mathf.Deg2Rad;
            startTime = Time.time;

            rigidbody.velocity = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * startSpeed;
        }

        void FixedUpdate()
        {
            if (playingExplosionSound)
            {
                if (!explosionSound.isPlaying)
                {
                    if (photonView.isMine)
                    {
                        PhotonNetwork.Destroy(gameObject);
                    }
                }
            }
            else if (Time.time - startTime > explosionTime)
            {
                GetComponentInChildren<MeshRenderer>().enabled = false; // stop rendering the grenade, it's gone!
                GetComponent<Light>().enabled = false;
                CreateExplosion();
                explosionSound.Play();
                playingExplosionSound = true;
            }
        }

        void CreateExplosion()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

            for (int i = 0; i < hitColliders.Length; i++)
            {
                // todo push only enemies?
                GameObject go = hitColliders[i].gameObject;

                EnemyHealth enemyHealth = go.GetComponent<EnemyHealth>();

                // If the EnemyHealth component exist...
                if (enemyHealth != null)
                {
                    // ... the enemy should take damage.
                    enemyHealth.TakeDamage(damagePerShot, go.transform.position);
                }

                if (go.GetComponent<Rigidbody>() == null)
                {
                    continue;
                }

                if (go.CompareTag("Grenade"))
                {
                    continue; // don't throw yourself or other grenades around
                }

                Vector3 fromExplosion = go.transform.position - transform.position;
                float forceMultiplier = explosionRadius / fromExplosion.magnitude;
                // bigger force at the center, lesser on the sides

                fromExplosion.Normalize();
                fromExplosion *= explosionForce * forceMultiplier;

                // Debug.Log("causing force " + fromExplosion);
                go.rigidbody.AddForce(fromExplosion);
            }
            Instantiate(explosionPreFab, transform.position, Quaternion.identity);
        }
    }
}