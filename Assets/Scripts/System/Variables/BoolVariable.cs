using UnityEngine;

namespace CherryTeaGames.Core.Variables
{
    [CreateAssetMenu(menuName = "Variables/BoolVariable")]
    public class BoolVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        [SerializeField]
        private bool value = false;

        public bool Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}