using UnityEngine;
using TMPro;

public class RessourceManager : MonoBehaviour
{
    public static RessourceManager instance;

    public int woodAmount, stoneAmount, foodAmount, reagentsAmount, knowledgeAmount, coinAmount;
    [SerializeField] TextMeshProUGUI woodText, stoneText, foodText, reagentsText, knowledgeText, coinText;

    public enum ressourceType 
    {
        wood,
        stone,
        food,
        reagents,
        knowledge,
        coin,
        none
    }

    private void Awake()
    {
        instance = this;
    }

    public void UpdateUI()
    {
        woodText.text = woodAmount.ToString();
        stoneText.text = stoneAmount.ToString();
        foodText.text = foodAmount.ToString();
        reagentsText.text = reagentsAmount.ToString();
        knowledgeText.text = knowledgeAmount.ToString();
        coinText.text = coinAmount.ToString();
    }

    public void GatherRessource(ressourceType type, int amount)
    {
        switch (type)
        {
            case ressourceType.wood:
                woodAmount += amount;
                break;
            case ressourceType.stone:
                stoneAmount += amount;
                break;
            case ressourceType.food:
                foodAmount += amount;
                break;
            case ressourceType.reagents:
                reagentsAmount += amount;
                break;
            case ressourceType.knowledge:
                knowledgeAmount += amount;
                break;
            case ressourceType.coin:
                coinAmount += amount;
                break;
            default:
                break;
        }

        UpdateUI();
    }
}
