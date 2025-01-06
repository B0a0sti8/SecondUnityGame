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
        FetchFields();
        LoadDataFromScriptableObject();
        UpdateCardUI();
    }

    public void FetchFields()
    {
        myCardImage = transform.Find("Content").Find("Picture").GetComponent<Image>();
        myCardTypeImage = transform.Find("Content").Find("Type").GetComponent<Image>();
        myTitleField = transform.Find("Content").Find("Name").GetComponent<TextMeshProUGUI>();
        myDescriptionField = transform.Find("Content").Find("Description").GetComponent<TextMeshProUGUI>();
        myLifeText = transform.Find("Content").Find("Life").GetComponent<TextMeshProUGUI>();
        myEnergyText = transform.Find("Content").Find("Energy").GetComponent<TextMeshProUGUI>();
    }

    public void UpdateCardUI()
    {
        LoadDataFromScriptableObject(); 
        myTitleField.text = cardName;
        myDescriptionField.text = cardDescription;
        myLifeText.text = maxCardLife.ToString();
        myEnergyText.text = maxCardEnergy.ToString();

        myCardTypeImage.sprite = typePicture;
        myCardImage.sprite = cardPicture;
    }

    public void UpdateCardUI(int currentLife, int currenEnergy)
    {
        LoadDataFromScriptableObject();
        myTitleField.text = cardName;
        myDescriptionField.text = cardDescription;
        myLifeText.text = currentLife.ToString() + " / " + maxCardLife.ToString();
        myEnergyText.text = currenEnergy.ToString() + " / " + maxCardEnergy.ToString();

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
        maxCardEnergy = myCardScriptable.maxEnergy;

        woodCost = myCardScriptable.woodCost;
        stoneCost = myCardScriptable.stoneCost;
        manaCost = myCardScriptable.manaCost;
    }
}