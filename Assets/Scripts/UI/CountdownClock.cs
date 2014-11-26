using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;

namespace CompleteProject
{
    public class CountdownClock : MonoBehaviour
    {
        public Animator levelCountdownAnimator;

        private Text text;

        float stopTime = float.NaN;

        void Start()
        {
            text = GetComponent<Text>();
            GameState.OnLevelOver += delegate()
            {
                this.enabled = false; // on level over signal, stop updating the clock
            };

            GameState.OnTimeIsUp += delegate()
            {
                ShowSecondsLeft(0); // force 00:00 on the clock when time is up
            };
        }

        void Update()
        {
            if (float.IsNaN(stopTime))
            {
                AnimationClip clip = GetCurrentClip();
                if (clip == null)
                {
                    return;
                }
                stopTime = AnimationUtility.GetAnimationEvents(clip)[0].time;
            }
            AnimatorStateInfo info = levelCountdownAnimator.GetCurrentAnimatorStateInfo(0);
            int secondsLeft = (int)(stopTime - info.length * info.normalizedTime);
            if (secondsLeft < 0)
            {
                secondsLeft = 0;
            }
            ShowSecondsLeft(secondsLeft);
        }

        private void ShowSecondsLeft(int secondsLeft)
        {
            System.TimeSpan span = System.TimeSpan.FromSeconds(secondsLeft);
            text.text = span.Minutes.ToString("00") + ":" + span.Seconds.ToString("00");
        }

        private AnimationClip GetCurrentClip()
        {
            AnimationInfo[] infos = levelCountdownAnimator.GetCurrentAnimationClipState(0);
            if (infos == null)
            {
                return null;
            }
            if (infos.Length == 0)
            {
                return null;
            }
            return infos[0].clip;
        }
    }
}