
public class Node: MonoBehaviour, BaseComponent {

    private String id;
    private Canvas canvas;
    private Textbox header;
    private List<Node> composites;
    private List<Edge> connections;

    public String getId(){
        return id;
    }

    public boolean setId(int id){
        boolean wasSet = false;
        this.id = id;
        wasSet = true;
        return wasSet; 

    }

    public Canvas getCanvas(){
        return canvas;
    }

    public boolean setCanvas(Canvas aCanvas){
        boolean wasSet = false;
        if(aCanvas == null){
            return wasSet;
        }
        canvas = aCanvas;
        canvas.addNode(this);
        wasSet = true;
        return wasSet;
    }

    public Textbox getTextbox(){
        return header;
    }
    public boolean setTextbox(Texbox aheader){
        boolean wasSet = false;
        if(aheader == null){
            return wasSet;
        }
        header = aheader;
        header.addNode(this);
        wasSet = true;
        return wasSet;
    }
    public Node getNode(int index){
        Node aNode = composites[index];
        return aNode;
    }

    public List<Node> getNodes(){
        List<Node> aComposites = Collections.unmodifiableList(composites);
        return aComposites;
    }

    public boolean addNode(Node aNode){
        boolean wasSet = false;
        if(composites.contains(aNode)){
            return false;
        }
        composites.add(aNode);
        wasSet = true;
        return wasSet;
    }

    public Edge getEdge(int index){
        Edge aEdge = connections[index];
        return aEdge; 
    }
    public List<Edge> getEdges(){
        List<Edge> aEdges = Collections.unmodifiableList(connections);
        return aEdges;  
    }
    public boolean addEdge(Edge aEdge){
        boolean wasSet = false;
        if(connections.contains(aEdge)){
            return wasSet;
        }
        connections.add(aEdge);
        wasSet = true;
        return wasSet;
    }
}