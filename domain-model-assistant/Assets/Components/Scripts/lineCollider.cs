using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Edge),typeof(PolygonCollider2D))]
public class lineCollider : MonoBehaviour
{

    Ray ray;
    RaycastHit hit;

    Edge edge;

    //The collider for the line
    PolygonCollider2D polygonCollider2D;

    //The points to draw a collision shape between
    List<Vector2> colliderPoints = new List<Vector2>(); 

    // Start is called before the first frame update
    void Start()
    {
        edge = GetComponent<Edge>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
    
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        if (colliderPoints != null) colliderPoints.ForEach(p => Gizmos.DrawSphere(p, 0.1f));
    }

    // Update is called once per frame
    void Update()
    {
        colliderPoints = CalculateColliderPoints();
        foreach (var item in colliderPoints)
        {
            //Debug.Log(item.ToString());
        }
        polygonCollider2D.SetPath(0, colliderPoints.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));
        // if (Input.GetMouseButtonDown (0)){
        //     Debug.Log("mouse clicked");
        //     edge.SpawnPopupLineMenu();
        // }

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit))
        {
            Debug.Log("mouse hovering");
          
        }
    
    }

    
    //mouse hover, not clicked, set color to blue
    void OnMouseEnter()
    {  
        Debug.Log("mouse enter");
        edge.setColor(1);
    }
    //mouse exits, set color back to black, never triggered for some reason
    void onMouseExit()
    {
        Debug.Log("mouse exit");
        edge.setColor(0);
    }

    void onMouseUp()
    {
        Debug.Log("mouse up");
        edge.SpawnPopupLineMenu();
    }

    // public void OnPointerDown(EventSystems.PointerEventData eventData)
    // {
    //    Debug.Log ("pointer down while over the Collider!");
    // }
    //mouse exit, set back to black
    
    private List<Vector2> CalculateColliderPoints() {
        //Get All positions on the line renderer
        Vector3[] positions = edge.GetPositions();

        //Get the Width of the Line
       // float width = 0.5f;
        float width = 5f * edge.GetWidth();

        //m = (y2 - y1) / (x2 - x1)
        float m = (positions[1].y - positions[0].y) / (positions[1].x - positions[0].x);
        float deltaX = (width / 2f) * (m / Mathf.Pow(m * m + 1, 0.5f));
        float deltaY = (width / 2f) * (1 / Mathf.Pow(1 + m * m, 0.5f));

        //Calculate the Offset from each point to the collision vertex
        Vector3[] offsets = new Vector3[2];
        offsets[0] = new Vector3(-deltaX, deltaY);
        offsets[1] = new Vector3(deltaX, -deltaY);

        //Generate the Colliders Vertices
        List<Vector2> colliderPositions = new List<Vector2> {
            positions[0] + offsets[0],
            positions[1] + offsets[0],
            positions[1] + offsets[1],
            positions[0] + offsets[1]
        };

        return colliderPositions;
    }
}
