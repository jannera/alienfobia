using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    // TODO rename to grenade controller
    public class RocketController : MonoBehaviour {
        public PhotonView photonView;
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


        // todo: needs still some kind of position syncing

	    // Use this for initialization
	    void Start () {
            float angle = (float) photonView.instantiationData[0];
            Debug.Log("angle " + angle);
            angle *= Mathf.Deg2Rad;
            startTime = Time.time;
        
            // TODO all rigidbody stuff should only be done in the server side
            rigidbody.velocity = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * startSpeed;
	    }
	
	    // Update is called once per frame
	    void FixedUpdate () {
            if (playingExplosionSound)
            {
                if (!explosionSound.isPlaying)
                {
                    Destroy(gameObject);
                }
            }
            else if (Time.time - startTime > explosionTime)
            {
                // pretty much any collision should explode it right?
                object[] p = { };
                photonView.RPC("CreateExplosion", PhotonTargets.MasterClient, p);
                explosionSound.Play();
                playingExplosionSound = true;
            }
	    }

        [RPC]
        void CreateExplosion()
        {
            if (PhotonNetwork.isMasterClient)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
            
                for (int i=0; i < hitColliders.Length; i++)
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
 
                    if (go.CompareTag("Player"))
                    {
                        continue; // don't throw players around
                    }
                
                    Vector3 fromExplosion = go.transform.position - transform.position;
                    float forceMultiplier = explosionRadius / fromExplosion.magnitude;
                    // bigger force at the center, lesser on the sides

                    fromExplosion.Normalize();
                    fromExplosion *= explosionForce * forceMultiplier;

                    // Debug.Log("causing force " + fromExplosion);
                    go.rigidbody.AddForce(fromExplosion);
                }
                object[] p = { };
                PhotonNetwork.InstantiateSceneObject(explosionPreFab.name, transform.position, Quaternion.identity, 0, p);
            }
        }
    }
}