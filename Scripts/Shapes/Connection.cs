using UnityEngine;
using UnityEngine.UI;

public class Connection : MonoBehaviour
{
    public GameObject Object1, Object2;
    private RectTransform object1, object2;
    private Image image;
    public RectTransform rectTransform;
    [HideInInspector]
    public Shape shape1, shape2;
    public void Init(GameObject a, GameObject b)
    {
        Object1 = a;
        Object2 = b;
        object1 = Object1.GetComponent<RectTransform>();
        object2 = Object2.GetComponent<RectTransform>();
    }
    public void Destroy()
    {
        if (Input.GetMouseButton(1))
        {
            if(shape1 is Entity)
            {
                Entity s1 = (Entity)shape1;
                if (shape2 is Relation)
                    s1.relations.Remove((Relation)shape2);
                else if (shape2 is Atribute)
                    s1.atributes.Remove((Atribute)shape2);
            }
            if (shape1 is Relation)
            {
                Relation s1 = (Relation)shape1;
                if (shape2 is Entity)
                    ((Entity)shape2).relations.Remove(s1);
                else if (shape2 is Atribute)
                    s1.atributes.Remove((Atribute)shape2);
            }
            if (shape2 is Entity)
            {
                Entity s1 = (Entity)shape2;
                if (shape1 is Relation)
                    s1.relations.Remove((Relation)shape1);
                else if (shape1 is Atribute)
                    s1.atributes.Remove((Atribute)shape1);
            }
            if (shape2 is Relation)
            {
                Relation s1 = (Relation)shape2;
                if (shape1 is Entity)
                    ((Entity)shape1).relations.Remove(s1);
                else if (shape1 is Atribute)
                    s1.atributes.Remove((Atribute)shape1);
            }
            shape1.connections.Remove(this);
            shape2.connections.Remove(this);
            Destroy(transform.gameObject);
        }
    }
    void Start()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (object1.gameObject.activeSelf && object2.gameObject.activeSelf)
        {
            rectTransform.localPosition = (object1.localPosition + object2.localPosition) / 2;
            Vector3 dif = object2.localPosition - object1.localPosition;
            rectTransform.sizeDelta = new Vector3(dif.magnitude, 3);
            rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI));
        }
    }
}
