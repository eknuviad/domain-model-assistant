using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClassDiagramDTO {
    public string eClass; // is this really necessary?
    public string _id;
    public string name; // NamedElement?
    public List<Class> classes;
    public List<CDType> types;
    public Layout layout;
    // TODO add feedback(s) here?
}


[System.Serializable]
public class Class {
    public string eClass;
    public string _id;
    public string name;
}

[System.Serializable]
public class CDType {
    // name? (this is a NamedElement in the MM)
    public string eClass;
    public string _id;
}

[System.Serializable]
public class Layout {
    public string _id;
    public List<ContainerMap> containers;
}

[System.Serializable]
public class ContainerMap {
    public string _id;
    public string key; // reference to EObject?
    public List<ElementMap> values; // value in MM
}

[System.Serializable]
public class ElementMap {
    public string _id;
    public string key; // reference to EObject?
    public LayoutElement value;
}

[System.Serializable]
public class LayoutElement {
    public string _id;
    public float x;
    public float y;
}
