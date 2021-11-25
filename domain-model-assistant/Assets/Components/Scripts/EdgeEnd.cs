using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class EdgeEnd : MonoBehaviour
{
    public string ID
    { get; set; }
    
    public GameObject edgeEndTitle;
    public GameObject edgeEndNumber;
    public GameObject textbox;
    // public GameObject popupMenu;
    void Start()
    {
        // this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
        // edgeEndTitle = GameObject.Instantiate(textbox, this.transform);
        edgeEndTitle.GetComponent<InputField>().text = "Enter Text ...";
        // edgeEndNumber = GameObject.Instantiate(textbox, this.transform);
        // edgeEndNumber.transform.position += new Vector3(0,20,0);
        Debug.Log("instatntiate");
    }

    void Update()
    {
        // if (nodes != null)
        // {
        //     var pos1 = Camera.main.ScreenToWorldPoint(nodes[0].transform.position + new Vector3(0,-95,0));
        //     pos1.z = 0;
        //     var pos2 = Camera.main.ScreenToWorldPoint(nodes[1].transform.position + new Vector3(0,95,0));
        //     pos2.z = 0;
        //     line.SetPosition(0, pos1);
        //     line.SetPosition(1, pos2);
        // }
    }

    // void createEdge()
    // {
    //     this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
    //     line = this.gameObject.GetComponent<LineRenderer>();
    //     // line.material = material;
    //     line.material = new Material (Shader.Find ("Sprites/Default"));
    //     line.material.color = Color.black; 
    //     line.positionCount = 2; //straightline with 2 end points
    //     line.startWidth = 0.1f; //line width
    //     line.endWidth = 0.1f; //line width
    //     line.useWorldSpace = true; //set to true so lines defined in world space
    //     line.numCapVertices = 50;
    //     Debug.Log("line created");
    // }

    void Destroy(){
         Destroy(this.gameObject);
    }

//     public bool AddNode(GameObject aNode){
//          if(nodes.Contains(aNode)){
//             return false;
//         }
//         nodes.Add(aNode);
//         aNode.GetComponent<Node>().AddEdge(this.gameObject);
//         Debug.Log("node added to edge");
//         return true;
//     }
}