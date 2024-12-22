using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainCardScript : MonoBehaviour
{
    protected string cardName;
    protected Sprite cardPicture;

    protected enum CardType
    {
        MilitaryUnit,
        BuildingUnit,
        Building,
        Special
    }

    protected CardType myCardType = CardType.MilitaryUnit;

    protected string cardDescription;

    protected float maxCardLife;
    protected float currentCardLife;

    protected float maxCardEnergy;
    protected float currentCardEnergy;

    protected Image myCardImage;
    protected Image myCardTypeImage;
    protected TextMeshProUGUI myTitleField;
    protected TextMeshProUGUI myDescriptionField;
    protected TextMeshProUGUI myLifeText;
    protected TextMeshProUGUI myEnergyText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myCardImage = transform.Find("Content").Find("Picture").GetComponent<Image>();
        myCardTypeImage = transform.Find("Content").Find("Type").GetComponent<Image>();
        myTitleField = transform.Find("Content").Find("Name").GetComponent<TextMeshProUGUI>();
        myDescriptionField = transform.Find("Content").Find("Description").GetComponent<TextMeshProUGUI>();
        myLifeText = transform.Find("Content").Find("Life").GetComponent<TextMeshProUGUI>();
        myEnergyText = transform.Find("Content").Find("Energy").GetComponent<TextMeshProUGUI>();

        UpdateCardUI();
    }

    protected virtual void UpdateCardUI()
    {
        switch (myCardType)
        {
            case CardType.MilitaryUnit:
                myCardTypeImage.sprite = CardManager.instance.militaryUnitTypeSprite;
                break;
            case CardType.BuildingUnit:
                myCardTypeImage.sprite = CardManager.instance.buildingUnitTypeSprite;
                break;
            case CardType.Building:
                myCardTypeImage.sprite = CardManager.instance.buildingTypeSprite;
                break;
            case CardType.Special:
                myCardTypeImage.sprite = CardManager.instance.specialTypeSprite;
                break;
            default:
                break;
        }

        myTitleField.text = cardName;
        myDescriptionField.text = cardDescription;
        myLifeText.text = currentCardLife.ToString() + " / " + maxCardLife.ToString();
        myEnergyText.text = currentCardEnergy.ToString() + " / " + maxCardEnergy.ToString();
        myCardImage.sprite = cardPicture;
    }
}
