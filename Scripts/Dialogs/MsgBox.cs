using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MsgBox : MonoBehaviour
{
    public string Title, Text;
    public Color TextColor = Color.black;
    private void Start()
    {
        var field = transform.GetChild(5).GetComponent<Text>();
        field.text = Text;

        field = transform.GetChild(4).GetComponent<Text>();
        field.text = Title;
        field.color = TextColor;
    }
    public void Destroy(float delay)
    {
        Destroy(gameObject, delay);
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
