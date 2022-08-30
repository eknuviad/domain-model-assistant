using System;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script instantiates a polygonCollider2D with the same shape as the line renderer 
/// object in edge, and move along with the edge. It detects mouse hovering, mouse exit and mouse clicks activities
/// which allows highlighting upon left click and linePopupMenu spawning upon right click.
/// </summary>
[RequireComponent(typeof(Edge), typeof(PolygonCollider2D))]
public class LineCollider : MonoBehaviour
{

    Edge edge;

    //The collider for the line
    PolygonCollider2D polygonCollider2D;

    LineRenderer highlightBox;

    bool isSelected = false;


    //The points to draw a collision shape between
    List<Vector2> colliderPoints = new(); 

    // Start is called before the first frame update
    void Start()
    {
        edge = GetComponent<Edge>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        colliderPoints = CalculateColliderPoints();
        polygonCollider2D.SetPath(0, colliderPoints.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));
    }

    
    // mouse hover, not clicked, set color to blue
    void OnMouseEnter()
    {  
        if (!isSelected)
        {
            edge.SetColor(Color.blue);
        }
    }

    // mouse exits, set color back to black
    void OnMouseExit()
    {
        if (!isSelected)
        {
            edge.SetColor(Color.black);
        }
    }

    void OnMouseUp()
    {
        if (isSelected)
        {
            isSelected = false;
            edge.SetColor(Color.blue);
        }
        else
        {
            edge.SetColor(Color.magenta);
            isSelected = true;
        }
        
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            edge.mousePos = Input.mousePosition;
            edge.SpawnPopupLineMenu();
        }
    }
    
    private List<Vector2> CalculateColliderPoints() 
    {
        // Get all positions on the line renderer
        Vector3 linePos1 = edge.GetPosition1();
        Vector3 linePos2 = edge.GetPosition2();

        // Get the width of the line
        // float width = 0.5f;
        float width = 2f * edge.GetWidth();

        // m = (y2 - y1) / (x2 - x1)
        // slope for vertical lines are undefined
        float slope = linePos2.x - linePos1.x !=0 ? (linePos2.y - linePos1.y) / (linePos2.x - linePos1.x) : 0;
        float deltaX = (width / 2f) * (slope / Mathf.Pow(slope * slope + 1, 0.5f));
        float deltaY = (width / 2f) * (1 / Mathf.Pow(1 + slope * slope, 0.5f));

        // Calculate the Offset from each point to the collision vertex
        Vector3[] offsets = new Vector3[2];
        offsets[0] = new Vector3(-deltaX, deltaY);
        offsets[1] = new Vector3(deltaX, -deltaY);

        // Generate the colliders vertices
        var colliderPositions = new List<Vector2> {
            linePos1 + offsets[0],
            linePos2 + offsets[0],
            linePos2 + offsets[1],
            linePos1 + offsets[1]
        };

        return colliderPositions;
    }
    
}
