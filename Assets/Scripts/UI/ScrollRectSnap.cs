using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ScrollRectSnap : MonoBehaviour
{
    public RectTransform panel;
    public List<RectTransform> carouselItem;
    public bool carouselActive = false;
    public int currentIndex = 0;
    public float distanceBetween = 450;
    public float inputTime = .5f;
    public float inputTimer = 0;
    public float direction;

    private DefaultInputActions uicontrols;

    void Awake()
    {
        uicontrols = new DefaultInputActions();
        uicontrols.UI.Navigate.performed += ctx => { direction = ctx.ReadValue<Vector2>().x; };
        uicontrols.UI.Submit.performed += ctx => { OnSubmit(); };
    }

    void OnEnable()
    {
        uicontrols.Enable();
        panel = GetComponent<RectTransform>();
        carouselItem.Clear();
        foreach (Transform child in transform)
        {
            carouselItem.Add(child.GetComponent<RectTransform>());
        }
        SpreadOutButtons();
        carouselActive = true;
    }

    void OnDisable()
    {
        uicontrols.Disable();
        carouselActive = false;
        currentIndex = 0;
        GoToIndex(0);
    }

    void Update()
    {
        if (!carouselActive)
            return;
        if (inputTimer >= inputTime)
        {
            if (direction == 1)
            {
                currentIndex += 1;
                inputTimer = 0;
            }
            else if (direction == -1)
            {
                currentIndex -= 1;
                inputTimer = 0;
            }
            currentIndex = Mathf.Clamp(currentIndex, 0, carouselItem.Count - 1);
        }
        else
        {
            inputTimer += Time.deltaTime;
        }

        LerpToIndex(currentIndex);
        SelectButton();
    }


    public void OnSubmit()
    {
        int sceneIndex = carouselItem[currentIndex].GetComponent<LevelPanelComponent>().panelInfo.sceneIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    void SpreadOutButtons()
    {
        for (int j = 0; j < carouselItem.Count; j++)
        {
            RectTransform bPanel = carouselItem[j];
            float newx = distanceBetween * j;
            Vector2 pos = new Vector2(newx, bPanel.anchoredPosition.y);
            bPanel.anchoredPosition = pos;
        }
    }

    void LerpToIndex(int index)
    {
        float newX = Mathf.Lerp(panel.anchoredPosition.x, index * distanceBetween * -1, Time.deltaTime * 5f);
        Vector2 newPosition = new Vector2(newX, panel.anchoredPosition.y);

        panel.anchoredPosition = newPosition;
    }

    void SelectButton()
    {
        // This required there be button components on carousel items. not as dynamic as I'd
        Button btn = carouselItem[currentIndex].GetComponentInChildren<Button>();
        if (btn) btn.Select();
    }

    void GoToIndex(int index)
    {
        float newX = Mathf.Lerp(panel.anchoredPosition.x, index * distanceBetween * -1, Time.deltaTime * 5f);
        panel.anchoredPosition = new Vector2(newX, panel.anchoredPosition.y);
    }


}
