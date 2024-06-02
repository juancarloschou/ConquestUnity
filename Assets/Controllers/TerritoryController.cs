using Assets;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryController : MonoBehaviour
{
    // Dont storage the information about the territory. Only the Id (the info is in MapManager -> territories)
    public string territoryId;

    private Material originalMaterial; // the material for draw the lines when its not selected

    private string sortingLayerNameLines = "Foreground"; // Set the desired sorting layer
    private int sortingOrderLines = 1; // Set the desired sorting order

    private LineRenderer lineRenderer;
    private PolygonCollider2D polygonCollider;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Inicializa el territorio con datos
    public void Init(Territory territory, Material lineMaterial, float lineWidth)
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
        lineRenderer.sortingLayerName = sortingLayerNameLines;
        lineRenderer.sortingOrder = sortingOrderLines;

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
    }

    void OnMouseDown()
    {
        Debug.Log("Territory clicked: " + territoryId);
        TerritoryManager.Instance.SelectTerritory(this);
    }

    public void Select()
    {
        lineRenderer.material = MapManager.Instance.selectedTerritoryMaterial;
    }

    public void Deselect()
    {
        lineRenderer.material = originalMaterial;
    }
}
