using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShapeSave 
{
    public float[] Position;
    public string Name;
    public ShapeSave(Shape s)
    {
        Position = new float[3];
        Position[0] = s.gameObject.transform.position.x;
        Position[1] = s.gameObject.transform.position.y;
        Position[2] = s.gameObject.transform.position.z;

        Name = s.Name;
    }
}
