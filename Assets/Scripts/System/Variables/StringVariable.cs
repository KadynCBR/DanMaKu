using UnityEngine;

namespace CherryTeaGames.Core.Variables
{
    [CreateAssetMenu(menuName = "Variables/StringVariable")]
    public class StringVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        [SerializeField]
        private string value = "";

        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}