using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CherryTeaGames.Core.UI
{
    // used to dynamically create level panels to be used with scrollrectsnap
    public class LevelPanelGenerator : MonoBehaviour
    {
        public GameObject panelPrefab;
        public List<LevelPanel> panels;

        void Awake()
        {
            foreach (LevelPanel panel in panels)
            {
                GameObject go = Instantiate(panelPrefab, transform.position, Quaternion.identity, transform);
                go.GetComponent<LevelPanelComponent>().panelInfo = panel;
                go.transform.Find("Button/LevelName").GetComponent<TextMeshProUGUI>().text = panel.levelName;
                go.transform.Find("Button").GetComponent<Image>().sprite = panel.levelImage;
                if (!panel.isUnlocked)
                    go.transform.Find("Button").GetComponent<Button>().interactable = false;
            }
        }
    }

}
