using UnityEngine;
using System.Collections;

namespace CompleteProject
{

    public class HighlightSphereController : MonoBehaviour
    {
        private const float SHOWING_PERIOD = 0.3f;
        private float showedFor = 0;
        public void Hide()
        {
            this.renderer.enabled = false;
        }

        public void Show()
        {
            this.renderer.enabled = true;
            showedFor = 0;
        }

        // Use this for initialization
        void Start()
        {
            this.renderer.material.color = Color.red;
            Hide();
        }

        // Update is called once per frame
        void Update()
        {
            showedFor += Time.deltaTime;
            if (showedFor > SHOWING_PERIOD)
            {
                Hide();
            }
        }
    }

}