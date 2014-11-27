using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class CameraFollow : MonoBehaviour
    {

        Transform target;
        public float smoothing = 5f;

        public Vector3 offset;

        void FixedUpdate()
        {
            if (target == null)
            {
                GameObject player = PlayerManager.GetMyPlayer();
                if (player != null)
                {
                    target = player.transform;
                }

            }
            if (target == null)
            {
                return;
            }
            Vector3 targetCamPos = target.position + offset;

            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }
}