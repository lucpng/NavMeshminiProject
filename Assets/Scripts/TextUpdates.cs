using UnityEngine;
using TMPro;

public class TextUpdates : MonoBehaviour
{
    private string visitorText = "Visitors : ";
    private string wanderersText = " Wanderers : ";
    private TextMeshProUGUI TextMesh;
    

    private void Start()
    {
        TextMesh = gameObject.GetComponent<TextMeshProUGUI>();
    }
    public void UpdateText(string nbrVisitor, string nbrWanderers)
    {

        TextMesh.SetText(visitorText + nbrVisitor + wanderersText + nbrWanderers);
    }
}
