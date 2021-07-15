using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClassDiagramDTO {
    public string eClass;
    public string _id;
    public string name;
    public List<Class> classes;
    public List<CDType> types;
    public Layout layout;
    // TODO add feedback(s) here
};


[System.Serializable]
public class Class {
    public string eClass;
    public string _id;
    public string name;
};

[System.Serializable]
public class CDType {
    public string eClass;
    public string _id;
};

[System.Serializable]
public class Layout {
    public string _id;
    public List<Container> containers;
};

[System.Serializable]
public class Container {
    public string _id;
    public string key;
    public List<Value> values;
};

[System.Serializable]
public class Value {
    public string _id;
    public string key;
    public Coordinate value;
};

[System.Serializable]
public class Coordinate {
    public string _id;
    public float x;
    public float y;
};