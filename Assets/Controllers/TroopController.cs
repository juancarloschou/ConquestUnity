using Assets;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;

public class TroopController : MonoBehaviour
{
    // Where is the troop
    public string territoryId;

    public TextMeshProUGUI quantityText; // Assign this in the Inspector
    private int defendersStrength;

    private SpriteRenderer outlineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        quantityText = GameObject.Find("TextQuantity").GetComponent<TextMeshProUGUI>();

        // Find the outline SpriteRenderer component within the child
        outlineRenderer = transform.Find("OutlineSelected").GetComponent<SpriteRenderer>(); // Assuming the outline sprite is a child named "Outline"

        // Ensure outline is initially inactive
        outlineRenderer.gameObject.SetActive(false);
    }

    public void SetDefendersStrength(int strength)
    {
        defendersStrength = strength;
        UpdateQuantityText();
    }

    private void UpdateQuantityText()
    {
        if (quantityText != null)
        {
            quantityText.text = defendersStrength.ToString();
        }
    }

    /*
    void OnMouseDown()
    {
        Debug.Log("Troop clicked: " + territoryId);
        TroopManager.Instance.SelectTroop(this);
    }
    */

    public void Select()
    {
        outlineRenderer.gameObject.SetActive(true);
    }

    public void Deselect()
    {
        outlineRenderer.gameObject.SetActive(false);
    }
}
