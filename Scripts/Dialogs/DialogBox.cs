using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    public string Title = "Quit";
    public string Msg = "Are you sure?";

    public delegate void Click();
    public event Click YesClick,NoClick;
    private void Start()
    {
        transform.GetChild(4).GetComponent<Text>().text = Msg;
        transform.GetChild(3).GetComponent<Text>().text = Title;
    }
    public void Yes()
    {
        Destroy(gameObject);
        YesClick?.Invoke();
    }
    public void No()
    {
        Destroy(gameObject);
        NoClick?.Invoke();
    }
}
