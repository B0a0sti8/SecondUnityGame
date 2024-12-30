using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainCardScript : MonoBehaviour
{
    public CardPrefabScriptable myCardScriptable;

    string cardName;
    Sprite cardPicture;
    public Sprite tokenPicture;
    Sprite typePicture;


    string myCardType = "Military Unit";

    string cardDescription;

    float maxCardLife;
    float currentCardLife;

    float maxCardEnergy;
    float currentCardEnergy;

    float woodCost;
    float stoneCost;
    float manaCost;

    Image myCardImage;
    Image myCardTypeImage;
    TextMeshProUGUI myTitleField;
    TextMeshProUGUI myDescriptionField;
    TextMeshProUGUI myLifeText;
    TextMeshProUGUI myEnergyText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myCardImage = transform.Find("Content").Find("Picture").GetComponent<Image>();
        myCardTypeImage = transform.Find("Content").Find("Type").GetComponent<Image>();
        myTitleField = transform.Find("Content").Find("Name").GetComponent<TextMeshProUGUI>();
        myDescriptionField = transform.Find("Content").Find("Description").GetComponent<TextMeshProUGUI>();
        myLifeText = transform.Find("Content").Find("Life").GetComponent<TextMeshProUGUI>();
        myEnergyText = transform.Find("Content").Find("Energy").GetComponent<TextMeshProUGUI>();

        LoadDataFromScriptableObject();
        UpdateCardUI();
    }

    void UpdateCardUI()
    {
        myTitleField.text = cardName;
        myDescriptionField.text = cardDescription;
        myLifeText.text = currentCardLife.ToString() + " / " + maxCardLife.ToString();
        myEnergyText.text = currentCardEnergy.ToString() + " / " + maxCardEnergy.ToString();

        myCardTypeImage.sprite = typePicture;
        myCardImage.sprite = cardPicture;
    }

    void LoadDataFromScriptableObject()
    {
        cardName = myCardScriptable.cardName;
        cardDescription = myCardScriptable.description;
        myCardType = myCardScriptable.unitTypeString;

        cardPicture = myCardScriptable.cardSprite;
        tokenPicture = myCardScriptable.tokenSprite;
        typePicture = myCardScriptable.unitTypeSprite;

        maxCardLife = myCardScriptable.maxLife;
        currentCardLife = maxCardLife;
        maxCardEnergy = myCardScriptable.maxEnergy;
        currentCardEnergy = maxCardEnergy;

        woodCost = myCardScriptable.woodCost;
        stoneCost = myCardScriptable.stoneCost;
        manaCost = myCardScriptable.manaCost;
    }
}