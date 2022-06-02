using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Relation : Shape
{
    public List<Atribute> atributes = new List<Atribute>();
    GameObject dropDown1, dropDown2;
    private void Start()
    {
        dropDown1 = transform.GetChild(1).gameObject;
        dropDown2 = transform.GetChild(2).gameObject;
    }
    private void Update()
    {
        ScreenHandle.GetWindowRect(ScreenHandle.FindWindow(null, "Erd"), out RECT r);
        int w = (r.Right - r.Left) / 10, h = (r.Top - r.Bottom)/10;
        var c = connections.Where(c => (c.shape1 != this && c.shape1 is Entity) || (c.shape2 != this && c.shape2 is Entity)).ToList();
        if (c != null && c.Count>0)
        {
            dropDown1.SetActive(true);
            float degree = Mathf.Atan2(c[0].transform.position.y - transform.position.y, c[0].transform.position.x - transform.position.x);
            dropDown1.transform.position = new Vector2(Mathf.Cos(degree) * w + transform.position.x, -Mathf.Sin(degree) * h + transform.position.y);
            if (c.Count > 1)
            {
                dropDown2.SetActive(true);
                degree = Mathf.Atan2(c[1].transform.position.y - transform.position.y, c[1].transform.position.x - transform.position.x);
                dropDown2.transform.position = new Vector2(Mathf.Cos(degree) * w + transform.position.x,-Mathf.Sin(degree) * h + transform.position.y);
            }
        }
        else
        {
            dropDown1.SetActive(false);
            dropDown2.SetActive(false);
        }
    }
    public bool AddConnected()
    {
        var l = connections.Where(o => o.shape1 is Entity || o.shape2 is Entity);
        return l.Count()<2;
    }
    public bool Side1IsMany()
    {
        return dropDown1.GetComponent<Dropdown>().value == 0;
    }
    public bool Side2IsMany()
    {
        return dropDown2.GetComponent<Dropdown>().value == 0;
    }
}
