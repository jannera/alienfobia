using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class CameraFollow : MonoBehaviour
    {

        Transform target;
        public float smoothing = 5f;

        public Vector3 offset;

        void Awake()
        {
            GameState.OnMyPlayerJoined += delegate()
            {
                target = PlayerManager.GetMyPlayer().transform;
            };
        }

        void FixedUpdate()
        {
            if (target == null)
            {
                return;
            }
            Vector3 targetCamPos = target.position + offset;

            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }
}