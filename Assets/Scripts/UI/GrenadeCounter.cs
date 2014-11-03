using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CompleteProject
{
    public class GrenadeCounter : MonoBehaviour
    {
        private Text text;
        private GrenadeThrowing grenadeThrowing;
        // Use this for initialization
        void Start()
        {
            text = GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            if (grenadeThrowing == null)
            {
                grenadeThrowing = PlayerManager.GetComponentFromMyPlayer<GrenadeThrowing>();
                if (grenadeThrowing == null)
                {
                    text.text = "";
                    return;
                }
            }

            text.text = "X " + grenadeThrowing.grenades;
        }
    }

}
