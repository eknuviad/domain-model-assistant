// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class Edge : MonoBehaviour
// {

//     public string ID
//     { get; set; }
//     // public EdgeType edgeType 
//     // {get; set;}
//     public GameObject node;
//     public List<EdgeEnd> edgeEnds;
//     public LineRenderer lineRenderer;
//     public Transform[] points;

//      void Start()
//     {
//         this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
//     }

//     void Awake(){
//         lineRenderer = GetComponent<LineRenderer>();
//     }

//     void Update(){
//         for (int i =0; i< points.Length; i++){
//            lineRenderer.SetPosition(i, points[i].position);
//         }
//     }

//     public void SetUpLine(Transform[] points){
//         lineRenderer.positionCount = points.Length;
//         this.points = points;
//     }
//     // public bool SetEdgeEnd(GameObject aEdgeEnd)
//     // {
//     //     if (edgeEnds.Contains(aEdgeEnd))
//     //     {
//     //         return false;
//     //     }
//     //     edgeEnds.Add(aEdgeEnd);
//     //     aEdgeEnd.GetComponent<EdgeEnd>().SetEdge(this.gameObject);
//     //     return true;
//     // }
// }
/////////////////////////////////////////////////////////////////////////////////////////////////////////
//Part2

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Rendering;

// public class Edge : MonoBehaviour
// {
//     private LineRenderer line;
//     private Vector3 mousePos;
//     public Material material;
//     private int currLines = 0;//ciunter for lines drawn

//     void Start()
//     {

//     }

//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {
//             if (line == null)
//             {
//                 createLine();
//             }

//             mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//mouse postiion is Vecot3, mouse cordinates are 2d so convert
//             mousePos.z = 0; //so lines are drawn on xy plan
//             line.SetPosition(0, mousePos);
//             line.SetPosition(1, mousePos);
//         }
//         else if (Input.GetMouseButtonUp(0) && line) //check if mouse has been lifted
//         {
//             mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//             mousePos.z = 0;
//             line.SetPosition(1, mousePos);
//             line = null; //if line is not drawn
//             currLines++;
//         }
//         else if (Input.GetMouseButton(0) && line) //if mouse is pressed, conrinuously set position to cur mouse pos
//         {
//             mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//             mousePos.z = 0;
//             line.SetPosition(1, mousePos);
//         }
//     }

//     void createLine()
//     {
//         line = new GameObject("Line" + currLines).AddComponent<LineRenderer>();//new line gameobject
//         line.material = material;
//         line.positionCount = 2; //straightline with 2 end points
//         line.startWidth = 0.15f; //line width
//         line.endWidth = 0.15f; //line width
//         line.useWorldSpace = false; //set to true so lines defined in world space
//         line.numCapVertices = 50;
//     }
// }
