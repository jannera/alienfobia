using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    // organizes gameobjects under given gameobject automatically
    public class Organizer : MonoBehaviour
    {
        public GameObject group;

        void Start()
        {
            GameObject parent = GameObject.Find(group.name);
            if (parent == null)
            {
                parent = (GameObject) Instantiate(group, Vector3.zero, Quaternion.identity);
                parent.name = group.name;
            }
            transform.parent = parent.transform;
        }
    }
}