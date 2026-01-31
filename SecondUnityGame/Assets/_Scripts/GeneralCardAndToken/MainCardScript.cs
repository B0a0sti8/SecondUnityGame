using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class MainCardScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler, IBeginDragHandler
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
    public float isMovingSomewhereDuration = 0f;
    public bool isMovingSomewhere = false;
    public bool isDiscarded = false;
    public Vector3 targetPosition;
    bool isReleasedFromDrag = false;
    bool isDragging = false;
    bool isMarked = false;

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
        FetchFields();
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
        if (isMovingSomewhereDuration > 0 && isMovingSomewhere)
        {
            GetComponent<Image>().raycastTarget = false;
            Vector3 newPos = (targetPosition - transform.position) * (Time.deltaTime / isMovingSomewhereDuration);
            transform.position += newPos;
            isMovingSomewhereDuration -= Time.deltaTime;
        }
        else if (isMovingSomewhere)
        {
            GetComponent<Image>().raycastTarget = true;
            transform.position = targetPosition;
            isMovingSomewhere = false;
            if (isDiscarded)
            {
                CardManager.instance.AddCardToDiscardPile(myCardToken);
                Destroy(gameObject);
            }
        }

        if (isDissolving)
        {
            GetComponent<Image>().raycastTarget = false;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.parent.name== "HandCards" && !isMarked)
        {
            transform.localScale *= 1.15f;
            transform.localPosition += new Vector3(0f, 20.0f, 0f);
            isMarked = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (transform.parent.name == "HandCards" && isMarked)
        {
            transform.localScale = new Vector3(1f,1f,1f);

            if (isReleasedFromDrag) isReleasedFromDrag = false;
            else transform.localPosition -= new Vector3(0f, 20.0f, 0f);

            HandCardScript.instance.ScaleUIBasedOnCardCount();
            isMarked = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!TurnAndEnemyManager.instance.isPlayerTurn) return;
        if (transform.parent.name != "HandCards") return;

        if (isDragging)
        {
            if (transform.localPosition.y > 240f)
            {
                MouseClickAndGrabManager.instance.TryPlayCard();
            }
            else
            {
                MouseClickAndGrabManager.instance.ClearGrabbedItem();
                HandCardScript.instance.ScaleUIBasedOnCardCount();
            }
            isReleasedFromDrag = true;
            isDragging = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!TurnAndEnemyManager.instance.isPlayerTurn) return;
        if (transform.parent.name != "HandCards") return;

        if (MouseClickAndGrabManager.instance.TryGrabCardExtern(gameObject))
        {
            MouseClickAndGrabManager.instance.SetGrabbedItem(gameObject);
            transform.localEulerAngles = new Vector3(0, 0, 0);
            isDragging = true;
        }
    }

    public void MarkForDiscard()
    {
        GetComponent<Image>().raycastTarget = false;
    }
}