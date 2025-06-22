using UnityEngine;
using TMPro;
using System.Collections;

public class DefaultTokenSlot : MonoBehaviour
{
    public GameObject myBackground;
    public GameObject myIsMarkedObject;
    public int markedCount;
    public bool isBuildingSlot;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myBackground = transform.Find("Background").gameObject;
        Color myColor = myBackground.GetComponent<SpriteRenderer>().color;
        myColor.a = 0.4f;
        myBackground.GetComponent<SpriteRenderer>().color = myColor;

        myIsMarkedObject = transform.Find("MarkedIndicator").gameObject;
    }

    public virtual void IncreaseMark()
    {
        markedCount += 1;
        myIsMarkedObject.SetActive(true);
        if (markedCount > 1)
        {
            myIsMarkedObject.transform.Find("Canvas").Find("Text").GetComponent<TextMeshProUGUI>().text = markedCount.ToString();
        }

    }

    public virtual void ClearMark()
    {
        markedCount = 0;
        myIsMarkedObject.transform.Find("Canvas").Find("Text").GetComponent<TextMeshProUGUI>().text = "";
        myIsMarkedObject.SetActive(false);
    }



}
