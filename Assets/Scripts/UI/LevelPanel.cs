using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CherryTeaGames.Core.UI
{
    [CreateAssetMenu(menuName = "LevelPanel/PanelInfo")]
    public class LevelPanel : ScriptableObject
    {
        public string levelName;
        public int sceneIndex;
        public Sprite levelImage;
        public bool isUnlocked;
    }
}
