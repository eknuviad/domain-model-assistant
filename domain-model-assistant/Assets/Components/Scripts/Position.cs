using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PositionDTO
{
    public float xPosition;
    public float yPosition;

    public PositionDTO(float positionX, float positionY)
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

public class AddAttributeBodyDTO
{
    public int rankIndex;

    public int typeId;

    public string attributeName;

    public AddAttributeBodyDTO(int rIndex, int aTypeId, string name)
    {
        rankIndex = rIndex;
        typeId = aTypeId;
        attributeName = name;
    }

}

public class RenameClassDTO
{
    public string newName;
    
    public RenameClassDTO(string name)
    {
        newName = name;
    }

}

public class SetMultiplicityDTO
{
    public int lowerBound;
    public int upperBound;

    public SetMultiplicityDTO(int lB, int uB)
    {
        lowerBound = lB;
        upperBound = uB;
    }
}
