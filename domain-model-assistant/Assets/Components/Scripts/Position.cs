using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Position
{
    public float xPosition;
    public float yPosition;

    public Position(float positionX, float positionY)
    {
        this.xPosition = positionX;
        this.yPosition = positionY;
    }
}

public class AddClassDTO
{
    public string className;

    public bool dataType;

    public bool isInterface;

    public float x;

    public float y; 
}

public class AddEnumClassDTO
{
    public string enumName;

    public float x;

    public float y; 
}

public class AddAttributeBody
{
    public int rankIndex;

    public int typeId;

    public string attributeName;

    
    public AddAttributeBody(int rIndex, int aTypeId, string name)
    {
        rankIndex = rIndex;
        typeId = aTypeId;
        attributeName = name;
    }

}

public class AddLiteralBody
{
    public int rankIndex;

    public string literalName;

    
    public AddLiteralBody(int rIndex, string name)
    {
        rankIndex = rIndex;
        literalName = name;
    }

}
