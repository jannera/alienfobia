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

            GameState.OnMyPlayerJoined += delegate()
            {
                grenadeThrowing = PlayerManager.GetComponentFromMyPlayer<GrenadeThrowing>();
            };
        }

        void Update()
        {
            if (grenadeThrowing == null)
            {
                text.text = "";
                return;
            }   

            text.text = "X " + grenadeThrowing.grenades;
        }
    }

}
