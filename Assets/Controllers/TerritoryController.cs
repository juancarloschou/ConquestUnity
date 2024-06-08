using Assets;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryController : MonoBehaviour
{
    // Dont storage the information about the territory. Only the Id (the info is in MapManager -> territories)
    public string territoryId;

    private Material originalMaterial; // the material for draw the lines when its not selected

    // layer and order of lines (boundaries)
    //private string sortingLayerNameLines;
    //private int sortingOrderLines;

    // layer and order of filling (territory)
    //private string sortingLayerNameFilling;
    //private int sortingOrderFilling;

    private LineRenderer lineRenderer;
    private PolygonCollider2D polygonCollider;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Initialize the territory with data
    public void Init(Territory territory, Material lineMaterial, float lineWidth, Color territoryColor)
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

    void FillTerritory(Color territoryColor, List<Vector2> territoryBoundary)
    {
        Debug.Log("FillTerritory");

        // Create a new GameObject for filling the territory
        GameObject fillObject = new GameObject("TerritoryFill");
        fillObject.transform.SetParent(transform);

        // Add a SpriteRenderer component to the fill object
        SpriteRenderer spriteRenderer = fillObject.AddComponent<SpriteRenderer>();

        // Create a new texture that matches the size of the territory
        Texture2D texture = new Texture2D(100, 100); // Adjust size as necessary
        Color transparentColor = new Color(territoryColor.r, territoryColor.g, territoryColor.b, 0.75f); // 50% transparency
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                texture.SetPixel(x, y, transparentColor);
            }
        }
        texture.Apply();

        // Create a sprite from the texture
        spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        // Set the sorting layer and order for the fill object
        spriteRenderer.sortingLayerName = MapManager.Instance.sortingLayerNameMap;
        spriteRenderer.sortingOrder = MapManager.Instance.sortingOrderFilling; // Ensure the fill is rendered below the borders

        // Set the position of the fill object to the center of the territory
        Vector2 center = MapManager.Instance.GetTerritoryCenter(territoryBoundary);
        fillObject.transform.position = new Vector3(center.x, center.y, 0);

        // Adjust the scale of the fill object to match the territory size
        fillObject.transform.localScale = MapManager.Instance.CalculateScaleFromTerritory(territoryBoundary, spriteRenderer, 1f);
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
