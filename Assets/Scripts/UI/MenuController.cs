using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/**
    This system as it is REQUIRES an initial page, it seems to be more suited for main menu interfacing.
    In the scenario where there is no inital page (in game) you can create a dummy page for the inital page
    Otherwise consider refactoring this to allow for pushing and popping with no inital page to work as intended 
    for like say... an ingame menu.
**/

[RequireComponent(typeof(Canvas))]
[DisallowMultipleComponent]
public class MenuController : MonoBehaviour
{
    [SerializeField]
    private Page initialPage;
    // [SerializeField]
    // public GameObject firstFocusItem;

    private Canvas rootCanvas;

    private Stack<Page> pageStack = new Stack<Page>();

    private DefaultInputActions uicontrols;

    void OnEnable() { uicontrols.Enable(); }
    void OnDisable() { uicontrols.Disable(); }

    void Awake()
    {
        uicontrols = new DefaultInputActions();
        uicontrols.UI.Cancel.performed += ctx => { OnCancel(); };
    }

    private void Start()
    {
        rootCanvas = GetComponent<Canvas>();
        // if (firstFocusItem != null)
        // {
        //     EventSystem.current.SetSelectedGameObject(firstFocusItem);
        // }
        if (initialPage != null)
        {
            PushPage(initialPage);
        }
    }


    public void OnCancel()
    {
        if (rootCanvas.enabled && rootCanvas.gameObject.activeInHierarchy)
        {
            if (pageStack.Count != 0)
            {
                PopPage();
            }
        }
    }

    public bool IsPageInStack(Page page)
    {
        return pageStack.Contains(page);
    }

    public bool IsPageOnTopOfStack(Page page)
    {
        return pageStack.Count > 0 && page == pageStack.Peek();
    }


    public void PushPage(Page page)
    {
        Debug.Log($"Pushing page: {page.gameObject.name}");
        page.Enter(true);
        if (pageStack.Count > 0)
        {
            Page currentPage = pageStack.Peek();
            if (currentPage.ExitOnNewPagePush)
            {
                currentPage.Exit(false);
            }
        }
        pageStack.Push(page);
    }

    public void PopPage()
    {
        if (pageStack.Count > 1)
        {
            Page page = pageStack.Pop();
            Debug.Log($"Popping page: {page.gameObject.name}");
            page.Exit(true);
            Page newCurentPage = pageStack.Peek();
            if (newCurentPage.ExitOnNewPagePush)
            {
                newCurentPage.Enter(false);
            }
            else
            {
                newCurentPage.Reenter();
            }
        }
        else
        {
            Debug.LogWarning("Trying to pop menu page but only 1 page on stack.");
        }
    }

    public void PopAllPages()
    {
        for (int i = 1; i < pageStack.Count; i++)
        {
            PopPage();
        }
    }
}
