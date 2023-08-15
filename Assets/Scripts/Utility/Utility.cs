using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CherryTeaGames.Core.Utils
{
    public class Utility : MonoBehaviour
    {
        public static float reMap(float s, float low1, float high1, float low2, float high2)
        {
            return low2 + (s - low1) * (high2 - low2) / (high1 - low1);
        }
    }
}
