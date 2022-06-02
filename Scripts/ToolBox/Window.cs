using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public GameObject Edits, Manipulate, ToolBox;
    public void EditsPress()
    {
        Edits.SetActive(!Edits.activeSelf); ;
    }
    public void ManipulatePress()
    {
        Manipulate.SetActive(!Manipulate.activeSelf); ;
    }
    public void ToolBoxPress()
    {
        ToolBox.SetActive(!ToolBox.activeSelf); ;
    }
}
