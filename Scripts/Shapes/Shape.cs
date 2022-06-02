
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shape : MonoBehaviour
{
    public virtual string Name 
    {
        get => Name;
        set
        {
            if (value.Length > 0)
            {
                sName = ValidateName(value);
                transform.GetChild(0).GetComponent<Image>().color = Color.clear;
            }
        }
    }

    public List<Connection> connections = new List<Connection>();
    protected string sName;

    public string GetName()
    {
        return sName;
    }

    protected string ValidateName(string val)
    {
        string ret = "";
        foreach(var c in val)
        {
            if (c != ' ') ret += c;
        }
        return ret;
    }
    public void ShowInputField()
    {
        transform.GetChild(0).GetComponent<Image>().color = Color.white;
    }
}
