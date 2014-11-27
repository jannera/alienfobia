using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CompleteProject
{
    public class FlamethrowerParticleDamager : PhotonBehaviour
    {

        public int damagePerUpdate = 10;

        private List<GameObject> itemsInFlame = new List<GameObject>(10);

        void Awake()
        {
            if (!photonView.isMine)
            {
                Destroy(this);
                return;
            }
        }

        void OnParticleCollision(GameObject go)
        {
            if (!go.CompareTag("Enemy"))
            {
                return;
            }
            if (itemsInFlame.Contains(go))
            {
                return;
            }
            itemsInFlame.Add(go);
        }

        void FixedUpdate()
        {
            foreach (GameObject go in itemsInFlame)
            {
                EnemyHealth enemyHealth = go.GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damagePerUpdate, go.transform.position);
                }   
            }
            itemsInFlame.Clear();
        }

        void OnEnable()
        {
            itemsInFlame.Clear();
        }
    }
}