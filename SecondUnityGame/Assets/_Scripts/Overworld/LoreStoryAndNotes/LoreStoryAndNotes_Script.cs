using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoreStoryAndNotes_Script : MonoBehaviour
{
    [SerializeField] GameObject myLoreElementPrefab;

    List<Transform> myLoreElementPlaceholders = new List<Transform>();
    List<LoreStoryNoteScriptable> listOfCollectedLoreElements = new List<LoreStoryNoteScriptable>();
    List<LoreStoryNoteScriptable> currentlyShownLoreElements = new List<LoreStoryNoteScriptable>();
    int currentPageCount = 1;
    int maxPageCount;

    private void Start()
    {
        InitReferences();
        //UpdateCollectedLoreElements();
        if (gameObject.activeSelf) OpenClose();
    }

    private void InitReferences()
    {
        GetElementSlots();

        Button myExitButton = transform.Find("ExitButton").GetComponent<Button>();
        myExitButton.onClick.AddListener(() => OpenClose());


        Button myNextPageButton = transform.Find("NextPageButton").GetComponent<Button>();
        myNextPageButton.onClick.AddListener(() => GoToNextPage());
        Button myPreviousPageButton = transform.Find("PreviousPageButton").GetComponent<Button>();
        myPreviousPageButton.onClick.AddListener(() => GoToPreviousPage());
    }

    private void GetElementSlots()
    {
        myLoreElementPlaceholders.Clear();

        for (int i = 0; i < transform.Find("Notebook").Find("LeftPage").Find("Content").childCount; i++)
        {
            myLoreElementPlaceholders.Add(transform.Find("Notebook").Find("LeftPage").Find("Content").GetChild(i));
        }

        for (int i = 0; i < transform.Find("Notebook").Find("RightPage").Find("Content").childCount; i++)
        {
            myLoreElementPlaceholders.Add(transform.Find("Notebook").Find("RightPage").Find("Content").GetChild(i));
        }
    }

    private void UpdateCollectedLoreElements()
    {
        GetElementSlots(); 
        listOfCollectedLoreElements = GameProgressManager.instance.GetListOfCollectedLore();
        maxPageCount = listOfCollectedLoreElements.Count / 8;
        maxPageCount = 10;
        GetCurrentlyShownElements(currentPageCount);

        for (int i = myLoreElementPlaceholders.Count - 1; i >= 0; i--)
        {
            if (myLoreElementPlaceholders[i] != null)
            {
                myLoreElementPlaceholders[i].Find("LoreStoryNote").gameObject.SetActive(false);
                myLoreElementPlaceholders[i].Find("EmptySlot").Find("PageMarker").GetComponent<TextMeshProUGUI>().text = ((currentPageCount - 1) * 8 + i + 1).ToString();
            }
        }

        //Debug.Log("Zeige so viele Elemente an: " + currentlyShownLoreElements.Count);

        for (int i = 0; i < currentlyShownLoreElements.Count; i++)
        {
            myLoreElementPlaceholders[i].Find("LoreStoryNote").gameObject.SetActive(true);
            myLoreElementPlaceholders[i].Find("LoreStoryNote").GetComponent<LoreStoryNote_Script>().myLoreStoryNoteScriptable = currentlyShownLoreElements[i];
            myLoreElementPlaceholders[i].Find("LoreStoryNote").GetComponent<LoreStoryNote_Script>().myPageCounter = (currentPageCount - 1) * 8 + i + 1;

            myLoreElementPlaceholders[i].Find("LoreStoryNote").GetComponent<LoreStoryNote_Script>().UpdateUI();
        }
    }

    private void GetCurrentlyShownElements(int pageNumber) 
    {
        currentlyShownLoreElements.Clear();
        if ((pageNumber - 1 <= listOfCollectedLoreElements.Count / 8) && (listOfCollectedLoreElements.Count != 0))
        {
            int firstElement = (pageNumber - 1) * 8;
            int lastElement = Mathf.Min(listOfCollectedLoreElements.Count, (pageNumber - 1) * 8 + 8);
            currentlyShownLoreElements = listOfCollectedLoreElements.GetRange(firstElement, lastElement - firstElement);
        }
    }

    public void OpenClose()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            UpdateCollectedLoreElements();
        }
    }

    public void GoToNextPage()
    {
        if (currentPageCount >= maxPageCount)
        {
            currentPageCount = maxPageCount;
            return;
        }
        else
        {
            currentPageCount++;
            UpdateCollectedLoreElements();
        }
    }

    public void GoToPreviousPage()
    {
        if (currentPageCount <= 1) 
        {
            currentPageCount = 1;
            return;
        }
        else
        {
            currentPageCount--;
            UpdateCollectedLoreElements();
        }
    }
}
