using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CompleteProject
{
    public class FlamethrowerDamager : MonoBehaviour
    {
        public float damagePerUpdate = 10;

        private List<GameObject> itemsInFlame = new List<GameObject>(10);
        
        void Awake()
        {

        }

        
        void FixedUpdate()
        {
            CleanList();

            // todo: what happens to the collisions when the weapon gets disabled and enabled?
            for (int i = 0; i < itemsInFlame.Count; i++)
            {
                GameObject enemy = itemsInFlame[i];
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    // todo switch ints to floats in damage taking
                    enemyHealth.TakeDamage((int)damagePerUpdate, enemy.transform.position);
                    // Debug.Log("burning " + enemy + " " + enemy.GetInstanceID());
                }
            }
        }

        private static bool ShouldBeRemoved(GameObject go) {
            return go == null;
        }

        private void CleanList()
        {
            itemsInFlame.RemoveAll(ShouldBeRemoved);
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy"))
            {
                return;
            }

            itemsInFlame.Add(other.gameObject);
        }

        void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Enemy"))
            {
                return;
            }

            itemsInFlame.Remove(other.gameObject);
        }
    }
}