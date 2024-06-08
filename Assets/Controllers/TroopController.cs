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

    // Start is called before the first frame update
    void Start()
    {
        quantityText = GameObject.Find("TextQuantity").GetComponent<TextMeshProUGUI>();
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
    // Inicializa el territorio con datos
    public void Init()
    {
        //storage the information about the territory (Id only)
        this.territoryId = territory.Id;

        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component not found on prefab!");
            return;
        }

        originalMaterial = lineMaterial;

        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.loop = true;

        // Set the sorting layer and order
        lineRenderer.sortingLayerName = MapManager.Instance.sortingLayerNameMap;
        lineRenderer.sortingOrder = MapManager.Instance.sortingOrderLines;

        // Set the color of the line renderer
        //lineRenderer.startColor = territoryColor;
        //lineRenderer.endColor = territoryColor;

        lineRenderer.positionCount = territory.TerritoryBoundary.Count;
        for (int i = 0; i < territory.TerritoryBoundary.Count; i++)
        {
            Vector2 mapPosition = territory.TerritoryBoundary[i];
            Vector3 worldPosition = new Vector3(mapPosition.x, mapPosition.y, 0);
            lineRenderer.SetPosition(i, worldPosition);
        }

        polygonCollider = GetComponent<PolygonCollider2D>();
        if (polygonCollider == null)
        {
            Debug.LogError("PolygonCollider2D component not found on prefab!");
            return;
            //polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
        }

        polygonCollider.points = territory.TerritoryBoundary.ToArray();

        // Fill the territory with the desired color
        FillTerritory(territoryColor, territory.TerritoryBoundary);
    }
    */

    void OnMouseDown()
    {
        Debug.Log("Troop clicked: " + territoryId);
        //TroopManager.Instance.SelectTroop(this);
    }

    /*
    public void Select()
    {
        lineRenderer.material = MapManager.Instance.selectedTerritoryMaterial;
    }

    public void Deselect()
    {
        lineRenderer.material = originalMaterial;
    }
    */
}
