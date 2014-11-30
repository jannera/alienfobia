using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CompleteProject
{
    public enum Option { Health, Speed, Damage, Amount };
    public class DifficultyOptionPersister : MonoBehaviour
    {
        private static int[] values;

        private ToggleGroup[] groups;
        private string[] optionNames;

        private const string KEY_BASE = "Difficulty_";

        void Start()
        {
            groups = GetComponentsInChildren<ToggleGroup>();
            System.Array allOptions = System.Enum.GetValues(typeof(Option));
            values = new int[allOptions.Length];
            optionNames = new string[allOptions.Length];
            if (groups.Length != allOptions.Length)
            {
                Debug.LogError("Did not find correct amount of options");
                return;
            }

            int i = 0;
            foreach (Option o in allOptions)
            {
                optionNames[i] = o.ToString();
                string key = KEY_BASE + optionNames[i];
                int value = PlayerPrefs.GetInt(key, 0);
                SetValue(groups[i], value);
                values[i] = value;
                i++;
            }
        }

        void Update()
        {
            for (int i = 0; i < groups.Length; i++)
            {
                int newValue = GetValue(groups[i]);
                if (newValue != values[i])
                {
                    values[i] = newValue;
                    PlayerPrefs.SetInt(KEY_BASE + optionNames[i], values[i]);
                }
            }
        }

        public static int GetValue(Option o)
        {
            return values[(int)o];
        }

        private int GetValue(ToggleGroup group)
        {
            int v = 0;
            foreach (Toggle t in group.GetComponentsInChildren<Toggle>())
            {
                if (t.isOn)
                {
                    return v;
                }
                v++;
            }
            return -1;
        }

        private void SetValue(ToggleGroup group, int newValue)
        {
            int v = 0;
            foreach (Toggle t in group.GetComponentsInChildren<Toggle>())
            {
                t.isOn = v == newValue;
                v++;
            }
        }
    }
}