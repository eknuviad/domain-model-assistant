using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClassDiagramDTO {
    public string eClass;
    public string _id;
    public string name;
    public Classes[] classes;
    public Types[] types;
    public Layout layout;
    // TODO add feedback(s) here
};


[System.Serializable]
public class Classes{
    public string eClass;
    public string _id;
    public string name;
};

[System.Serializable]
public class Types{
    public string eClass;
    public string _id;
};

[System.Serializable]
public class Layout{
    public string _id;
    public Containers[] containers;
};

[System.Serializable]
public class Containers{
    public string _id;
    public string key;
    public Value[] value;
};

[System.Serializable]
public class Value{
    public string _id;
    public string x;
    public Cordinate value;
};

[System.Serializable]
public class Cordinate{
    public string _id;
    public float x;
    public float y;
};