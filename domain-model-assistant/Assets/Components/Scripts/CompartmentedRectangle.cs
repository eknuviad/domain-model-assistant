
using System.Drawing;
//using System.Diagnostics;
//using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;

public class CompartmentedRectangle : Node
{
    public GameObject textbox; //this allows to instantiate textbox prefabs
    public GameObject section;
    public GameObject enumSign;
    public List<GameObject> sections = new();
    public GameObject popupMenu;
    public GameObject popupMenuEnum;
    public RectTransform rectangle;
    public GameObject abstractSign;
    
    public Color highlight = new(0.61f, 0.81f, 0.973f); // light blue
    public Color headerColor; // = transform.GetChild(1).gameObject.GetComponent<Image>().color;
    public Color sectionColor;
    float holdTimer = 0;
    bool hold = false;
    public State state { get; set; }

    private Diagram _diagram;

    private string id; //should move to node class later
    private Vector2 _prevPosition;
    private const int HeaderBackgroundHeight = 34;

    private float rectHeight;
    private float rectWidth;

    private int headerOffsetX = -31;
    private int headerOffsetY = 70;

    private int enumHeaderOffsetY = -24;

    private int enumSignOffsetX = -62;
    private int enumSignOffsetY = -8;

    private int sectionOffsetY = -30;
    private int popupMenuOffsetX = 138;
    private int abstractHeaderOffsetY = 63;
    private int abstractSignOffsetX = -50;
    private int abstractSignOffsetY = 64;
    public string ClassName { get; set;}
    public bool isEnum { get; set;}


    public bool IsHighlighted { get; set; }
    public bool isAbstract { get; set; }

    public enum State
    {
        Default,
        DraggingClass,
    }

    void Awake()
    {
        _diagram = GetComponentInParent<Diagram>();
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Default;
        NumOfConnectionPoints = 8; // default value
        for(int i = 0; i < NumOfConnectionPoints; i++)
        {
            connectionPointsAvailable.Add(true);
        }


        //CreateHeader();
        //CreateSection();

        if (isAbstract && isAbstract)
        {
            
            Debug.Log("Called abstract from CompRect");
            ToAbstractForm();
        }

        // Canvas.ForceUpdateCanvases();

        //RectTransform rtSection0 = (RectTransform)sections[0].GetComponent<Section>().transform;
        //RectTransform rtSection1 = (RectTransform)sections[1].GetComponent<Section>().transform;

        //var total_height = rtSection0.rect.height + rtSection1.rect.height;

        //rectangle = (RectTransform)transform;
        //rectangle.sizeDelta = new Vector2 (180, total_height+HeaderBackgroundHeight+3);


        // rectHeight = rectangle.rect.height;
        // rectWidth = rectangle.rect.width;

        id = GetComponent<CompartmentedRectangle>().ID;

        // for (int i = 0; i < transform.childCount; i++)
        // {
        //     var img = transform.GetChild(i).GetComponent<Image>();
        //     string color = img != null ? ("" + img.color) : "null";
        //     Debug.Log("CompRect transform child " + i + ": " + transform.GetChild(i) + " with image " + img
        //         + " and with color " + color);
        // }

        var headerIndex = transform.childCount - 4; // obtained by trial and error - make this more robust later
        var sectionIndex = headerIndex + 2;

        // the following changes header background to horizontal stretch middle anchor preset
        RectTransform headerBackground = (RectTransform) transform.GetChild(headerIndex);
        headerBackground.anchorMin = new Vector2(0, 1);
        headerBackground.anchorMax = new Vector2(1, 1);
        headerBackground.anchoredPosition = new Vector2(0, -17);
        headerBackground.sizeDelta = new Vector2 (0, HeaderBackgroundHeight);

        
        //transform.childCount
        Debug.Log("transform.childCount:"+transform.childCount);
        Debug.Log("headerIndex:"+headerIndex);
        headerIndex = 1;
        //headerColor = transform.GetChild(headerIndex).GetComponent<Image>().color;
        //sectionColor = transform.GetChild(sectionIndex).GetComponent<Image>().color;

    }

    // Update is called once per frame
    void Update()
    {
        if (hold)
        {
            OnBeginHold();
        }
        // RectTransform rtSection0 = (RectTransform)sections[0].GetComponent<Section>().transform;
        // RectTransform rtSection1 = (RectTransform)sections[1].GetComponent<Section>().transform;
        // Debug.Log("rtSection0 height =" + rtSection0.rect.height);
        //transform.GetChild(1).gameObject.GetComponent<Image>().material.color = Color.white;
    }

    // ************ BEGIN Controller Methods for Compartmented Rectangle ****************//

    public void CreateHeader()
    {
        var header = GameObject.Instantiate(textbox, transform);
        header.GetComponent<ClassHeaderTextBox>().SetHeaderOwner(gameObject);
        if (ClassName != null) 
        {
            header.GetComponent<ClassHeaderTextBox>().gameObject.GetComponent<InputField>().text = ClassName;
            header.GetComponent<ClassHeaderTextBox>().Name = ClassName;

        }
        AddHeader(header);
    }

    public void CreateSection()
    {
        Vector2 offset = new Vector2(0,-15) + new Vector2(0,-34) + new Vector2(0, -1);
        for (int i = 0; i < 2; i++) /*loop for a class with 2 sections*/
        {
            var sect = GameObject.Instantiate(section, transform);
            RectTransform rt_sect = (RectTransform) sect.transform; 
            rt_sect.anchoredPosition = offset + new Vector2(0, sectionOffsetY) * sections.Count;
            AddSection(sect);
        }
    }

    public void OnBeginHold()
    {
        hold = true;
        holdTimer += Time.deltaTime;
        _prevPosition = transform.position;
    }

    public void OnEndHold()
    {
        if (holdTimer > 1f - 5 && state == State.Default)
        {
            if(isEnum){
                SpawnEnumPopupMenu();
            }else{
                SpawnPopupMenu();
            }
            
        }
        holdTimer = 0;
        hold = false;
        //ToggleHighlight();
        
        //update class position
        WebCore.UpdateClassPosition(textbox, gameObject);
    }

    void SpawnPopupMenu()
    {
        if (popupMenu.GetComponent<PopupMenu>().GetCompartmentedRectangle() == null)
        {
            popupMenu = Instantiate(popupMenu);
            popupMenu.transform.position = transform.position + new Vector3(popupMenuOffsetX, 0, 0);
            popupMenu.GetComponent<PopupMenu>().SetCompartmentedRectangle(this);
            popupMenu.GetComponent<PopupMenu>().Open();
        }
        else
        {
            popupMenu.GetComponent<PopupMenu>().Open();
        }
    }

    void SpawnEnumPopupMenu()
    {
        if (popupMenuEnum.GetComponent<PopupMenuEnum>().GetCompartmentedRectangle() == null)
        {
            popupMenuEnum = Instantiate(popupMenuEnum);
            popupMenuEnum.transform.position = transform.position + new Vector3(popupMenuOffsetX, 0, 10);
            popupMenuEnum.GetComponent<PopupMenuEnum>().SetCompartmentedRectangle(this);
            popupMenuEnum.GetComponent<PopupMenuEnum>().Open();
        }
        else
        {
            popupMenuEnum.GetComponent<PopupMenuEnum>().Open();
        }
    }

    /// <summary>
    /// Destroy class when click on delete class.
    /// </summary>
    public override void Destroy()
    {
        if(isEnum){
            WebCore.DeleteEnum(gameObject);
            popupMenuEnum.GetComponent<PopupMenuEnum>().Destroy();
        }else{
            WebCore.DeleteClass(gameObject);
            popupMenu.GetComponent<PopupMenu>().Destroy();
        }
        
        
    }
    /// <summary>
    /// change class to abstraction class when click on to abstract.
    /// </summary>ßß
    public void ToAbstract()
    {
        WebCore.SwitchAbstract(gameObject);
        popupMenu.GetComponent<PopupMenu>().Close(); // close the popup menu
    }

    public void ToAbstractForm()
    {
        // var temp = GetHeader().GetComponent<InputField>().text;
        // string decor = "<<Abstraction>>";
        // string content = decor + temp;
        // GetHeader().GetComponent<InputField>().text = content;
        // Debug.Log("content is :"+GetHeader().GetComponent<InputField>().text);




        // CreateHeader();
        // var sign = GameObject.Instantiate(abstractSign, transform);
        // sign.GetComponent<InputField>().text = "<<Abstraction>>";
        // sign.transform.position = transform.position + new Vector3(abstractSignOffsetX, abstractSignOffsetY, 0);
        // //Debug.Log("position:"+GetHeader().GetComponent<AttributeTextBox>().transform.position);
        // GetHeader().GetComponent<AttributeTextBox>().transform.position = transform.position + new Vector3(headerOffsetX, abstractHeaderOffsetY, 0);
        // //Debug.Log("position:"+GetHeader().GetComponent<AttributeTextBox>().transform.position);
        // Vector3 oldPosition = transform.position + new Vector3(0, 18, 0);
        // var sect = GameObject.Instantiate(section, transform);
        // sect.GetComponent<RectTransform>().sizeDelta = new Vector2(176, 140);
        // sect.transform.position = oldPosition+ new Vector3(0, -36, 0) ;//* sections.Count;
        // //AddSection(sect);
        // //_diagram.AddLiteralsToSection(GetSection(0));
        // //header.GetComponent<RectTransform>().sizeDelta = new Vector2(176, 140);


        //CreateHeader();
        Debug.Log("transform position:"+transform.position);
        var sign = GameObject.Instantiate(abstractSign, transform);
        //sign.transform.position = transform.position + new Vector3(abstractSignOffsetX, abstractSignOffsetY, 0);
        RectTransform rt_sign = (RectTransform) sign.transform; 
        rt_sign.anchoredPosition =  new Vector3(abstractSignOffsetX, abstractSignOffsetY, 0);
        sign.GetComponent<InputField>().text = "<<Abstract>>";

        // //var header = GetHeader();//.GetComponent<ClassHeaderTextBox>();
        // //header.GetComponent<ClassHeaderTextBox>().isEnum = true;
        // //RectTransform rt_name = (RectTransform) header.transform; 
        // //rt_name.anchoredPosition =  new Vector3(0, 0, 0);
        // //Debug.Log("position:"+GetHeader().GetComponent<AttributeTextBox>().transform.position);
        // Vector3 oldPosition = transform.position + new Vector3(0, 18, 0);

        // Vector2 offset = new Vector2(0,-15) + new Vector2(0,-34) + new Vector2(0, -1);
        // var sect = GameObject.Instantiate(section, transform);
        // RectTransform rt_sect = (RectTransform) sect.transform; 
        // rt_sect.anchoredPosition = offset + new Vector2(0, sectionOffsetY) * sections.Count;
        // AddSection(sect);
        // RectTransform rtSection0 = (RectTransform)sections[0].GetComponent<Section>().transform;           
        // var total_height = rtSection0.rect.height;

        //rectangle = (RectTransform)transform;
        //rectangle.sizeDelta = new Vector2 (180, total_height+HeaderBackgroundHeight+3);
        //_diagram.AddLiteralsToSection(GetSection(0));
    }

    // ************ END Controller Methods for Compartmented Rectangle ****************//

    // ************ BEGIN UI model Methods for Compartmented Rectangle ****************//
    public bool AddSection(GameObject aSection)
    {
        if (sections.Contains(aSection))
        {
            return false;
        }
        sections.Add(aSection);
        aSection.GetComponent<Section>().SetCompartmentedRectangle(gameObject);
        return true;
    }

    public GameObject GetSection(int index)
    {
        if (index >= 0 && index < sections.Count)
        {
            return sections[index];
        }
        else
        {
            return null;
        }
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public override List<Vector2> GetConnectionPointsLocations()
    {
        // int debug_count = 0;
        // foreach (bool avail in connectionPointsAvailable)
        // {
        //     Debug.Log("avail " + debug_count++ + " :" + avail);
        // }
        RectTransform rectangle = (RectTransform)transform;
        var rectWidth = rectangle.rect.width;
        var rectHeight = rectangle.rect.height;

        List<Vector2> locations = new();
        var origin = GetPosition() + new Vector2(-rectWidth/2f, -rectHeight/2f);
        var hor_increment = (float)rectWidth / (NumOfConnectionPoints / 4);
        var ver_increment = (float)rectHeight / (NumOfConnectionPoints / 4);
        int index = 0;
        for (float x = 0; x <= rectWidth; x += hor_increment)
        {
            for (float y = rectHeight; y >= 0; y -= ver_increment)
            {
                if (y > 0 && y < rectHeight && x > 0 && x < rectWidth)
                {
                    continue;
                } 
                locations.Add(origin + new Vector2(x,y));
                index++;
            }
        }
        return locations;
    }

    public override void ReserveGeneralizationPt()
    {
        connectionPointsAvailable[NumOfConnectionPoints/2] = false;
    }

    public GameObject GetPopUpMenu()
    {
        return popupMenu;
    }

    public GameObject GetPopUpMenuEnum()
    {
        return popupMenuEnum;
    }
    
    public void SetLine()
    {
        Debug.Log("CompartmentedRectangle.SetLine() called");
        _diagram.gameObject.GetComponent<DrawLine>().AddCompartmentedRectangle(gameObject);
        popupMenu.GetComponent<PopupMenu>().Close(); // close the popup menu
    }

    public void ToggleHighlight()
    {
        if (IsHighlighted)
        {
            ResetColor();
        }
        else
        {
            IsHighlighted = true;
            Color newHeader = headerColor;
            Color newSection = sectionColor;
            newHeader = (newHeader + highlight) / 2;
            newSection = (newSection + highlight) / 2;
            Debug.Log("child count:"+transform.childCount);
            transform.GetChild(1).gameObject.GetComponent<Image>().color = newHeader;
            //transform.GetChild(3).gameObject.GetComponent<Image>().color = newSection;
            if(!isEnum){
                //transform.GetChild(4).gameObject.GetComponent<Image>().color = newSection;

            }
                    }
    }

    public void HighlightWith(Color color)
    {
        //IsHighlighted = true;
        Color newHeader = headerColor;
        Color newSection = sectionColor;
        newHeader = (newHeader + color) / 2;
        newSection = (newSection + color) / 2;
        transform.GetChild(1).gameObject.GetComponent<Image>().color = newHeader;
        transform.GetChild(3).gameObject.GetComponent<Image>().color = newSection;
        transform.GetChild(4).gameObject.GetComponent<Image>().color = newSection;
    }

    public void ResetColor()
    {
        IsHighlighted = false;
        transform.GetChild(1).gameObject.GetComponent<Image>().color = headerColor;
        transform.GetChild(3).gameObject.GetComponent<Image>().color = sectionColor;
        transform.GetChild(4).gameObject.GetComponent<Image>().color = sectionColor;
    }

    public void setSectionCount(int num){
        if(num == 2){
            //regular class
            isEnum = false;
            CreateHeader();
            var header = GetHeader();//.GetComponent<ClassHeaderTextBox>();
            header.GetComponent<ClassHeaderTextBox>().isEnum = false;

            Vector2 offset = new Vector2(0,-15) + new Vector2(0,-34) + new Vector2(0, -1);
            for (int i = 0; i < 2; i++) /*loop for a class with 2 sections*/
            {
                var sect = GameObject.Instantiate(section, transform);
                RectTransform rt_sect = (RectTransform) sect.transform; 
                rt_sect.anchoredPosition = offset + new Vector2(0, sectionOffsetY) * sections.Count;
                AddSection(sect);
            }

            RectTransform rtSection0 = (RectTransform)sections[0].GetComponent<Section>().transform;
            RectTransform rtSection1 = (RectTransform)sections[1].GetComponent<Section>().transform;

            var total_height = rtSection0.rect.height + rtSection1.rect.height;

            rectangle = (RectTransform)transform;
            rectangle.sizeDelta = new Vector2 (180, total_height+HeaderBackgroundHeight+3);

            _diagram.AddAttributesToSection(GetSection(0));//add atrributes to first section

        }
        if(num == 1){
            //enum class
            isEnum = true;
            CreateHeader();
            Debug.Log("transform position:"+transform.position);
            var sign = GameObject.Instantiate(enumSign, transform);
            RectTransform rt_sign = (RectTransform) sign.transform; 
            rt_sign.anchoredPosition =  new Vector3(0, enumSignOffsetY, 0);
            sign.GetComponent<InputField>().text = "<<Enumeration>>";
 
            var header = GetHeader();//.GetComponent<ClassHeaderTextBox>();
            header.GetComponent<ClassHeaderTextBox>().isEnum = true;
            RectTransform rt_name = (RectTransform) header.transform; 
            rt_name.anchoredPosition =  new Vector3(0, enumHeaderOffsetY, 0);
            //Debug.Log("position:"+GetHeader().GetComponent<AttributeTextBox>().transform.position);
            //Vector3 oldPosition = transform.position + new Vector3(0, 18, 0);

            Vector2 offset = new Vector2(0,-15) + new Vector2(0,-34) + new Vector2(0, -1);
            var sect = GameObject.Instantiate(section, transform);
            RectTransform rt_sect = (RectTransform) sect.transform; 
            rt_sect.anchoredPosition = offset + new Vector2(0, sectionOffsetY) * sections.Count;
            AddSection(sect);
            RectTransform rtSection0 = (RectTransform)sections[0].GetComponent<Section>().transform;           
            var total_height = rtSection0.rect.height;

            rectangle = (RectTransform)transform;
            rectangle.sizeDelta = new Vector2 (180, total_height+HeaderBackgroundHeight+3);
            _diagram.AddLiteralsToSection(GetSection(0));
            //header.GetComponent<RectTransform>().sizeDelta = new Vector2(176, 140);

        }
    }
    
    // ************ END UI model Methods for Compartmented Rectangle ****************//

}
