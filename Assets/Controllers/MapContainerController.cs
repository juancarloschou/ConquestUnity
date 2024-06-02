using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapContainerController : MonoBehaviour
{
    /*
    // Reference to the MapManager
    private MapManager mapManager;

    public Material territoryLineMaterial; // Material for the territory lines
    public Material selectedTerritoryMaterial; // Material for selected territory

    private float thickLines = 0.25f; // Grosor de las líneas
    private string sortingLayerNameLines = "Foreground"; // Set the desired sorting layer
    private int sortingOrderLines = 1; // Set the desired sorting order
    */

    // Start is called before the first frame update
    void Start()
    {
        /*
        // Find the GameManager in the scene
        mapManager = FindFirstObjectByType<MapManager>();

        // Ensure the GameManager is found before starting the game
        if (mapManager != null)
        {
            DrawTerritoryLines(mapManager.territories);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    // Draw territory lines using LineRenderer
    void DrawTerritoryLines(List<Territory> territories)
    {
        Debug.Log("DrawTerritoryLines " + territories.Count);
        foreach (Territory territory in territories)
        {
            // Create a LineRenderer component on the MapContainer GameObject
            GameObject territoryObject = new GameObject(territory.territoryID + "Lines");
            territoryObject.transform.SetParent(transform);

            // Log the name of the parent
            Debug.Log($"{territoryObject.name} parent: {territoryObject.transform.parent.name}");

            // Add LineRenderer component to the GameObject
            LineRenderer lineRenderer = territoryObject.AddComponent<LineRenderer>();

            // Set LineRenderer properties
            lineRenderer.material = territoryLineMaterial; // Assign the line material
            lineRenderer.startWidth = thickLines;
            lineRenderer.endWidth = thickLines;
            lineRenderer.loop = true; // Connect the last point to the first one to form a closed loop

            // Set the sorting layer and order
            lineRenderer.sortingLayerName = sortingLayerNameLines;
            lineRenderer.sortingOrder = sortingOrderLines;

            // Set LineRenderer positions based on territoryBoundary points
            lineRenderer.positionCount = territory.territoryBoundary.Count;
            for (int i = 0; i < territory.territoryBoundary.Count; i++)
            {
                Vector2 mapPosition = territory.territoryBoundary[i];
                Vector3 worldPosition = new Vector3(mapPosition.x, mapPosition.y, 0);
                lineRenderer.SetPosition(i, worldPosition);
            }

            //PolygonCollider2D collider = territoryObject.AddComponent<PolygonCollider2D>();
            //collider.points = territory.territoryBoundary.ToArray();
            //territoryObject.AddComponent<TerritoryManager>().Init(mapManager, territory);

            // Log the creation of the territory object
            Debug.Log($"Created territory object: {territoryObject.name}");
        }
    }
    */

    /*
    void LogParent(GameObject obj)
    {
        if (obj.transform.parent != null)
        {
            Debug.Log($"{obj.name} parent: {obj.transform.parent.name}");
        }
        else
        {
            Debug.Log($"{obj.name} has no parent");
        }
    }
    */
}
