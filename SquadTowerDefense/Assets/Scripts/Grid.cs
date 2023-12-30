using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int gridSizeX = 10;
    public int gridSizeZ = 10;
    public Color gridColor = Color.green;
    public bool showGizmos = true;
    public LayerMask hitLayers; 
    public float tileSizeX = 1.0f; // Adjustable grid tile size in the X direction
    public float tileSizeZ = 1.0f;

    void Update()
    {
        // Check for mouse input
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, hitLayers))
            {
                Debug.Log("Hit grid at: " + hit.point);
                // Do something with the hit point on the grid
            }
        }
    }

    void OnDrawGizmos()
    {
        if (showGizmos)
        {
            DrawGrid();
        }
    }

    void DrawGrid()
    {
        Gizmos.color = gridColor;

        // Calculate the offset based on the grid size
        Vector3 centerOffset = new Vector3(gridSizeX * tileSizeX / 2f, 0, gridSizeZ * tileSizeZ / 2f);

        // Draw grid lines in the X direction
        for (float x = 0; x <= gridSizeX; x += 1)
        {
            Gizmos.DrawLine(new Vector3(x * tileSizeX, 0, 0) - centerOffset, new Vector3(x * tileSizeX, 0, gridSizeZ * tileSizeZ) - centerOffset);
        }

        // Draw grid lines in the Z direction
        for (float z = 0; z <= gridSizeZ; z += 1)
        {
            Gizmos.DrawLine(new Vector3(0, 0, z * tileSizeZ) - centerOffset, new Vector3(gridSizeX * tileSizeX, 0, z * tileSizeZ) - centerOffset);
        }
    }
}
