using UnityEngine;

namespace CompleteProject
{
    public class Utility
    {
        public static T PickRandomly<T>(T[] items) {
            if (items.Length == 0)
            {
                return default(T);
            }
            int index = Random.Range(0, items.Length);

            return items[index];
        }
    }
}
