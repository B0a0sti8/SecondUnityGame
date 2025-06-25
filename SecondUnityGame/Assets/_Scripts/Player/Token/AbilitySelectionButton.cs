using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilitySelectionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Color myColor;
    public GameObject myAbilityObject;
    public bool isPassiveAbility;
    private GameObject abilityPreviewObject;

    private void Start()
    {
        abilityPreviewObject = MainCanvasSingleton.instance.transform.Find("PreviewSlot").Find("TokenAbilityPreview").gameObject;
        myColor = GetComponent<Image>().color;
        if (isPassiveAbility) myColor.a = 0.4f;
        else myColor.a = 0.65f;

        GetComponent<Image>().color = myColor;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isPassiveAbility) return;
        myColor.a = 0.9f;
        GetComponent<Image>().color = myColor;

        abilityPreviewObject.SetActive(true);
        abilityPreviewObject.transform.Find("AbilityName").GetComponent<TextMeshProUGUI>().text = myAbilityObject.GetComponent<PlayerTokenAbilityPrefab>().abilityName;
        abilityPreviewObject.transform.Find("AbilityDescription").GetComponent<TextMeshProUGUI>().text = myAbilityObject.GetComponent<PlayerTokenAbilityPrefab>().abilityDescription;
        abilityPreviewObject.transform.Find("AbilityIcon").GetComponent<Image>().sprite = myAbilityObject.GetComponent<PlayerTokenAbilityPrefab>().abilityIcon;
        abilityPreviewObject.transform.Find("TargetCount").GetComponent<TextMeshProUGUI>().text = "Target count: 0 / " + myAbilityObject.GetComponent<PlayerTokenAbilityPrefab>().abilityCheckPointsMax.ToString();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isPassiveAbility) myColor.a = 0.4f;
        else myColor.a = 0.65f;
        GetComponent<Image>().color = myColor;
        abilityPreviewObject.SetActive(false);
    }


}
