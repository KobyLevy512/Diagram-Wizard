using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MoveButton : MonoBehaviour
{
    bool clicked = true;

    GameObject inputField;
    private void Start()
    {
        inputField = transform.GetChild(0).gameObject;
        inputField.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (clicked)
        {
            if (Input.GetMouseButton(1))
            {
                var c = GetComponent<Shape>();
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    int len = c.connections.Count;
                    for (int i = 0; i < len; i++)
                    {
                        c.connections[0].Destroy();
                    }
                    if (c is Entity)
                        SqlConnect.entities.Remove((Entity)c);
                    Destroy(gameObject);
                }
                else if(c is Atribute && !((Atribute)c).Multy)
                {
                    c.gameObject.transform.GetChild(2).gameObject.SetActive(true);
                }
            }
            else transform.position = Input.mousePosition;
        }
    }

    public void Move()
    {
        if(ConnectRef.ConnectionMode)
        {
            if (ConnectRef.Object1 != null && ConnectRef.Object1 != gameObject)
            {
                ConnectRef.Object2 = gameObject;
                ConnectRef.SpawnConnection();
            }
            else ConnectRef.Object1 = gameObject;
        }
        else clicked = true;
    }
    public void StopMove()
    {
        clicked = false;
        inputField.SetActive(true);
    }
}
