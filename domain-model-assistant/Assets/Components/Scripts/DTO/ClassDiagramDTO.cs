using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClassDiagramDTO
{
    public ClassDiagram classDiagram;
}

[System.Serializable]
public class ClassDiagram
{
    public string eClass;
    public string _id;
    public string name;
    public List<Class> classes;
    public List<CDType> types;
    public List<Association> associations;
    public Layout layout;
}

[System.Serializable]
public class Class
{
    public string eClass;
    public string _id;
    public string name;
    public string isAbstract;
    public List<Attribute> attributes;
    public List<AssociationEnd> associationEnds;
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
public class AssociationEnd
{
    public string _id;
    public string name;
    public string assoc;
    public string lowerBound;
    public string upperBound;
}

[System.Serializable]
public class Association
{
    public string _id;
    public string name;
    public List<string> ends;
}

[System.Serializable]
public class ContainerMap
{
    public string _id;
    public string key;
    public List<ElementMap> value; // Not "values" in plural from because of Ecore conventions 
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
