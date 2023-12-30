using UnityEngine;

public class GridPlacement : MonoBehaviour
{
    public Grid grid;
    public GameObject prefabToPlace;
    public Color hoverColor = Color.green;
    public LayerMask gridLayer;
    public GameObject prefabPlaceHolderForBuilding;

    private GameObject attachedPrefab;
    private Renderer attachedPrefabRenderer;
    private Color originalColor;

    void Update()
    {
        HandleInput();
        HandleGridHighlight();
    }

    void HandleInput()
    {
        // Check for "B" key press to attach prefab to the mouse
        if (Input.GetKeyDown(KeyCode.B))
        {
            AttachPrefabToMouse();
        }

        // Check for mouse click to place the prefab on the grid
        if (Input.GetMouseButtonDown(0) && attachedPrefab != null)
        {
            PlacePrefabOnGrid();
        }
    }

    void AttachPrefabToMouse()
    {
        if (prefabToPlace != null)
        {
            // Instantiate the prefab and attach it to the mouse
            attachedPrefab = Instantiate(prefabToPlace);
            attachedPrefabRenderer = attachedPrefab.GetComponent<Renderer>();
            originalColor = attachedPrefabRenderer.material.color;
        }
    }

    void PlacePrefabOnGrid()
    {
        attachedPrefab.SetActive(false);
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, gridLayer))
        {
            Vector3 gridPosition = GetGridPosition(hit.point, grid);
            float terrainHeight = GetTerrainHeightAtPosition(gridPosition);
            
            GameObject placeHolderPrefab = Instantiate(prefabPlaceHolderForBuilding);

            placeHolderPrefab.transform.position =  new Vector3(
                Mathf.Floor(gridPosition.x / grid.tileSizeX) * grid.tileSizeX + grid.tileSizeX / 2f,
                terrainHeight + 0.25f, // Keep the Y position as is
                Mathf.Floor(gridPosition.z / grid.tileSizeZ) * grid.tileSizeZ + grid.tileSizeZ / 2f
            );
            
            TaskManager.Instance.AddTask(new BuildTask()
            {
                buildLocation = new Vector3(
                    Mathf.Floor(gridPosition.x / grid.tileSizeX) * grid.tileSizeX + grid.tileSizeX / 2f,
                    terrainHeight, // Keep the Y position as is
                    Mathf.Floor(gridPosition.z / grid.tileSizeZ) * grid.tileSizeZ + grid.tileSizeZ / 2f
                ),
                prefabToBuild = attachedPrefab
            });
            
            attachedPrefab = null;
        }
    }

    Vector3 GetGridPosition(Vector3 hitPoint, Grid grid)
    {
        // Convert the hit point to local space
        Vector3 localHitPoint = grid.transform.InverseTransformPoint(hitPoint);

        // Calculate the adjusted position to be in the center of the grid tile
        Vector3 gridPosition = new Vector3(
            Mathf.Floor(localHitPoint.x / grid.tileSizeX) * grid.tileSizeX + grid.tileSizeX / 2f,
            localHitPoint.y, // Keep the Y position as is
            Mathf.Floor(localHitPoint.z / grid.tileSizeZ) * grid.tileSizeZ + grid.tileSizeZ / 2f
        );

        // Convert the adjusted position back to world space
        return grid.transform.TransformPoint(gridPosition);
    }

    float GetTerrainHeightAtPosition(Vector3 position)
    {
        Ray ray = new Ray(new Vector3(position.x, position.y + 1000f, position.z), Vector3.down);
        RaycastHit hit;

        // Raycast down to find the terrain
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Buildable")))
        {
            return hit.point.y;
        }

        // If no terrain found, return the original Y position
        return position.y;
    }

    void HandleGridHighlight()
    {
        if (attachedPrefab != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the raycast against the grid layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, gridLayer))
            {
                // Highlight the grid position
                HighlightGridPosition(hit.point);
            }
        }
    }

    void HighlightGridPosition(Vector3 position)
    {
        float terrainHeight = GetTerrainHeightAtPosition(position);
        
        // Snap the position to the grid
        Vector3 gridPosition = new Vector3(
            Mathf.Floor(position.x / grid.tileSizeX) * grid.tileSizeX + grid.tileSizeX / 2f,
            terrainHeight, // Keep the Y position as is
            Mathf.Floor(position.z / grid.tileSizeZ) * grid.tileSizeZ + grid.tileSizeZ / 2f
        );

        // Set the color of the attached prefab to the hover color
        attachedPrefabRenderer.material.color = hoverColor;

        // Move the attached prefab to the snapped grid position for visualization
        attachedPrefab.transform.position = gridPosition;
    }
}
