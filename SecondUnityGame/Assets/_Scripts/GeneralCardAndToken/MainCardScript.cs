using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainCardScript : MonoBehaviour
{
    public bool createsPlayerToken;
    public DefaultCardScriptable myCardToken;

    string cardName;
    Sprite cardPicture;
    public Sprite tokenPicture;
    Sprite typePicture;

    string myCardType = "";

    string cardDescription;

    float maxCardLife;
    float currentCardLife;

    float maxCardEnergy;
    float currentCardEnergy;

    public int woodCost;
    public int stoneCost;
    public int foodCost;
    public int reagentCost;


    float manaCost;

    Image myCardImage;
    Image myCardTypeImage;
    TextMeshProUGUI myTitleField;
    TextMeshProUGUI myDescriptionField;
    TextMeshProUGUI myLifeText;
    TextMeshProUGUI myEnergyText;

    [SerializeField] Material myMat;
    Material myBaseMat;

    float dissolveDuration = 1f;
    public bool isDissolving = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myBaseMat = transform.Find("Content").Find("Picture").GetComponent<Image>().material;
        myMat = Instantiate(myBaseMat);
        transform.Find("Content").Find("Picture").GetComponent<Image>().material = myMat;
        transform.Find("Content").GetComponent<Image>().material = myMat;
        transform.GetComponent<Image>().material = myMat;

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
        cardName = myCardToken.cardName;
        cardDescription = myCardToken.description;
        myCardType = myCardToken.cardTypeString;

        cardPicture = myCardToken.cardSprite;
        //tokenPicture = myPlayerTokenScriptable.tokenSprite;
        typePicture = myCardToken.cardTypeSprite;

        //maxCardLife = myPlayerTokenScriptable.maxLife;
        //maxCardEnergy = myPlayerTokenScriptable.maxEnergy;

        woodCost = myCardToken.woodCost;
        stoneCost = myCardToken.stoneCost;
        foodCost = myCardToken.foodCost;
        reagentCost = myCardToken.reagentCost;

        manaCost = myCardToken.manaCost;
    }

    public void DestroyCard()
    {
        isDissolving = true;
    }

    private void Update()
    {
        if (isDissolving)
        {
            dissolveDuration -= Time.deltaTime;
            if (dissolveDuration >= 0)
            {
                myMat.SetFloat("_Fade", dissolveDuration);
            }
        }

        if (dissolveDuration < 0)
        {
            Destroy(gameObject);
        }
    }
}