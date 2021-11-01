using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClassDiagramDTO
{
    public string eClass;
    public string _id;
    public string name; // NamedElement?
    public List<Class> classes;
    public List<CDType> types;
    public Layout layout;
    // TODO add feedback here
}

[System.Serializable]
public class Class
{
    public string eClass;
    public string _id;
    public string name;
    public List<Attribute> attributes;
}

[System.Serializable]
public class CDType
{
    public string eClass;
    public string _id;
}

[System.Serializable]
public class Layout
{
    public string _id;
    public List<ContainerMap> containers;
}

[System.Serializable]
public class Attribute
{
    public string _id;
    public string name;
    public string type;

}
[System.Serializable]
public class ContainerMap
{
    public string _id;
    public string key;
    public List<ElementMap> value/*s*/; // TODO Change this after WebCORE is updated
}

[System.Serializable]
public class ElementMap
{
    public string _id;
    public string key;
    public LayoutElement value;
}

[System.Serializable]
public class LayoutElement
{
    public string _id;
    public float x;
    public float y;
}
