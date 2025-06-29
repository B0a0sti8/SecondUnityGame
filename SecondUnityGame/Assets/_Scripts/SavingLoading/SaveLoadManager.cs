using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void Save()
    {
        Debug.Log("Ich versuche einfach mal zu speichern... :)");

        string filePrefix = "ABC1_";
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + filePrefix + "SaveFile_X" + ".dat", FileMode.Create);

            SaveData data = new SaveData();

            SaveCardData(data);

            bf.Serialize(file, data);

            file.Close();
        }
        catch (System.Exception)
        {
            throw;
            // Handling Errors
        }

        Debug.Log("Anzahl der Karten im Deck: " + ListOfAllCards.instance.myDeckList.Count);
    }

    private void SaveCardData(SaveData data)
    {
        data.MyCardData = new CardData();

        Dictionary<DefaultCardScriptable, int> ownedCards = ListOfAllCards.instance.ownedCards;
        Dictionary<DefaultCardScriptable, int> deckCards = ListOfAllCards.instance.myDeckList;

        if (ownedCards.Count > 0)
        {
            foreach (KeyValuePair<DefaultCardScriptable, int> keyVal in ownedCards) data.MyCardData.ownedCards.Add(keyVal.Key.cardName, keyVal.Value);
        }

        if (deckCards.Count > 0)
        {
            foreach (KeyValuePair<DefaultCardScriptable, int> keyVal in deckCards) data.MyCardData.deckCards.Add(keyVal.Key.cardName, keyVal.Value);
        }
    }

    public void Load()
    {
        Debug.Log("Loading DeckList");
        string filePrefix = "ABC1_";
        string myFileName = filePrefix + "SaveFile_X" + ".dat";

        if (myFileName == "") { Debug.Log("No File Found! "); return; }

        SaveData data = new SaveData();

        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + myFileName, FileMode.Open);
            data = (SaveData)bf.Deserialize(file);
            file.Close();
        }
        catch (System.Exception)
        {
            // Handling Errors
            throw;
        }

        LoadCardData(data);

        Debug.Log("Anzahl der Karten im Deck: " + ListOfAllCards.instance.myDeckList.Count);
    }

    private void LoadCardData(SaveData data)
    {
        Dictionary<DefaultCardScriptable, int> ownedCards = new Dictionary<DefaultCardScriptable, int>();
        Dictionary<DefaultCardScriptable, int> deckCards = new Dictionary<DefaultCardScriptable, int>();

        foreach (KeyValuePair<string, int> keyVal in data.MyCardData.ownedCards)
        {
            DefaultCardScriptable myCard = ListOfAllCards.instance.FindCardByName(keyVal.Key);
            if(myCard != null) ownedCards.Add(myCard, keyVal.Value);
        }

        foreach (KeyValuePair<string, int> keyVal in data.MyCardData.deckCards)
        {
            DefaultCardScriptable myCard = ListOfAllCards.instance.FindCardByName(keyVal.Key);
            if (myCard != null) deckCards.Add(myCard, keyVal.Value);
        }

        ListOfAllCards.instance.myDeckList = deckCards;
        ListOfAllCards.instance.ownedCards = ownedCards;
    }
}