using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CompleteProject
{
    public class GrenadeCounter : MonoBehaviour
    {
        private Text text;
        private GrenadeThrowing grenadeThrowing;
        
        void Start()
        {
            text = GetComponent<Text>();
        }

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
