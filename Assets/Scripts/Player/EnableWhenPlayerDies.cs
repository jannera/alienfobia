using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnableWhenPlayerDies : MonoBehaviour
    {
        public Behaviour target;
        
        void Start()
        {
            GameState.OnMyPlayerDied += delegate()
            {
                target.enabled = true;
            };
        }

    }
}