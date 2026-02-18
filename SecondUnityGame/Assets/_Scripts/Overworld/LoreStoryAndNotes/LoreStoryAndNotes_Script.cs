using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoreStoryAndNotes_Script : MonoBehaviour
{
    [SerializeField] GameObject myLoreElementPrefab;

    List<Transform> myLoreElementPlaceholders = new List<Transform>();
    List<LoreStoryNoteScriptable> listOfCollectedLoreElements = new List<LoreStoryNoteScriptable>();
    List<LoreStoryNoteScriptable> currentlyShownLoreElements = new List<LoreStoryNoteScriptable>();
    int currentPageCount;

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
        GetCurrentlyShownElements(currentPageCount);

        for (int i = myLoreElementPlaceholders.Count - 1; i >= 0; i--)
        {
            if (myLoreElementPlaceholders[i] != null && myLoreElementPlaceholders[i].GetComponentInChildren<LoreStoryNote_Script>() != null)
            {
                Destroy(myLoreElementPlaceholders[i].GetComponentInChildren<LoreStoryNote_Script>().gameObject);
            }

            GameObject newLoreElem = Instantiate(myLoreElementPrefab, myLoreElementPlaceholders[i]);
            newLoreElem.transform.position = myLoreElementPlaceholders[i].position;
            newLoreElem.GetComponent<LoreStoryNote_Script>().myPageCounter = (currentPageCount - 1) * 8 + i + 1;
            newLoreElem.GetComponent<LoreStoryNote_Script>().UpdateUI();
        }
    }

    private void GetCurrentlyShownElements(int pageNumber) 
    {
        if ((pageNumber - 1 < listOfCollectedLoreElements.Count / 8) && (listOfCollectedLoreElements.Count != 0))
        {
            int firstElement = (pageNumber - 1) * 8;
            int lastElement = Mathf.Min(listOfCollectedLoreElements.Count - 1, (pageNumber - 1) * 8 + 8);
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
}
