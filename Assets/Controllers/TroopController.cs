using Assets;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;

public class TroopController : MonoBehaviour
{
    // data of the troop
    public int? territoryId; // Where is the troop
    public bool isMoving; // is in a movement
    public int defendersStrength; // quantity

    public TextMeshProUGUI quantityText; // Assign this in the Inspector

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

    // Initialize the territory with data
    public void Init(int territoryId)
    {
        this.territoryId = territoryId;
        this.isMoving = false;
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

    public IEnumerator MoveTo(TerritoryController destination, float duration)
    {
        if (isMoving) yield break;
        isMoving = true;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = destination.transform.position;
        float elapsed = 0;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        isMoving = false;

        // Handle arrival at destination
        ArriveAtDestination(destination);
    }

    private void ArriveAtDestination(TerritoryController destinationController)
    {
        /*
        // Check for enemies
        if (destination.HasEnemyTroops(GameManager.Instance.currentPlayer))
        {
            // Handle battle
            BattleManager.Instance.HandleBattle(this, destination.GetEnemyTroops(GameManager.Instance.currentPlayer));
        }
        else if (destination.IsNeutral() || destination.IsOwnedBy(GameManager.Instance.currentPlayer))
        {
        */
            // add troops to destination territory
            Territory destinationTerritory = MapManager.Instance.GetTerritoryById(destinationController.territoryId);
            if (destinationTerritory != null)
            {
                destinationTerritory.DefendersStrength = defendersStrength;
                destinationTerritory.TroopController = this;
            }
        //}

    }
}
