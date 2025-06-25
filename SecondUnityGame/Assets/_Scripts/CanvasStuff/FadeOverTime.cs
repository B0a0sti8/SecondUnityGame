using UnityEngine;
using TMPro;

public class FadeOverTime : MonoBehaviour
{
    public float myDuration = 1f;
    float elapsed = 0f;
    public Color myTextColor;
    TextMeshProUGUI myTextField;

    private void Start()
    {
        myTextField = GetComponent<TextMeshProUGUI>();
        if (myTextField == null) myTextField = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        transform.position += new Vector3(0, 10 * Time.deltaTime / myDuration, 0);
        elapsed += Time.deltaTime;
        //myTextColor.a = (1 - elapsed / myDuration);
        myTextField.color = myTextColor;

        if (elapsed > myDuration) Destroy(gameObject);
    }
}
