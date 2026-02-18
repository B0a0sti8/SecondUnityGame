using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoreStoryNote_Script : MonoBehaviour
{
    public LoreStoryNoteScriptable myLoreStoryNoteScriptable;
    TextMeshProUGUI myTitle, myContent, myPageMarker;
    public int myPageCounter = 0;
    
    private void FetchFields()
    {
        myTitle = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        myContent = transform.Find("Content").GetComponent<TextMeshProUGUI>();
        myPageMarker = transform.Find("PageMarker").GetComponent<TextMeshProUGUI>();
    }

    public void UpdateUI()
    {
        FetchFields();

        if (myLoreStoryNoteScriptable == null) return;

        myTitle.text = myLoreStoryNoteScriptable.loreNoteTitle;
        myContent.text = myLoreStoryNoteScriptable.loreNoteContent;
        myPageMarker.text = myPageCounter.ToString();
    }
}
