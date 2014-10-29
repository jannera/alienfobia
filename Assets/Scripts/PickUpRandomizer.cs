using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CompleteProject
{
    public class PickUpRandomizer : MonoBehaviour
    {
        private object[] p = { };

        public List<GameObject> possibleDrops = new List<GameObject>();

        public void DropRandomly(float chance, Vector3 pos)
        {
            if (!PhotonNetwork.isMasterClient)
            {
                return;
            }

            if (UnityEngine.Random.Range(0f, 1f) > chance)
            {
                return;
            }

            int size = possibleDrops.Count;

            if (size == 0)
            {
                return;
            }

            int choice = UnityEngine.Random.Range(0, size);

            GameObject go = possibleDrops[choice];

            PhotonNetwork.InstantiateSceneObject(go.name, pos, Quaternion.identity, 0, p);
        }
    }
}