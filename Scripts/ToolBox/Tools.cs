using UnityEngine;
using UnityEngine.UI;

public class Tools : MonoBehaviour
{
    public GameObject Entity, Atribute, Relation, Multy, Conection;
    public GameObject ConnectionButton;
    Transform content;
    ColorBlock basic;

    private void Start()
    {
        content = transform.parent.GetChild(0).GetChild(0).GetChild(0);
        ConnectRef.Tools = this;
        basic = ConnectionButton.GetComponent<Button>().colors; 
    }
    public void AddEntity()
    {
        var e = Instantiate(Entity, content);
        SqlConnect.entities.Add(e.GetComponent<Entity>());
    }
    public void AddAttribute()
    {
        Instantiate(Atribute, content);
    }
    public void AddRelation()
    {
        Instantiate(Relation, content);
    }
    public void AddMulty()
    {
        Instantiate(Multy, content);
    }
    private Connection InstanceConnection(GameObject a, GameObject b)
    {
        var c = Instantiate(Conection, content);
        c.transform.SetAsFirstSibling();
        c.GetComponent<Connection>().Init(a, b);
        var con = c.GetComponent<Connection>();
        con.shape1 = a.GetComponent<Shape>();
        con.shape2 = b.GetComponent<Shape>();
        return con;
    }
    public void AddConnection(GameObject a, GameObject b)
    {
        ConnectionButton.GetComponent<Button>().colors = basic;
        var acomp = a.GetComponent<Shape>();
        var bcomp = b.GetComponent<Shape>();
        if (acomp is Entity)
        {
            Entity ae = (Entity)acomp;
            if (bcomp is Atribute)
            {
                if (((Atribute)bcomp).Multy)
                {
                    if(((Atribute)bcomp).connections.FindAll(x=>x.shape1 is Entity || x.shape2 is Entity).Count == 0)
                    {
                        var i = InstanceConnection(a, b);
                        ae.multis.Add((Atribute)bcomp);
                        ae.connections.Add(i);
                        ((Atribute)bcomp).connections.Add(i);
                    }
                }
                else if (((Atribute)bcomp).connections.Count == 0)
                {
                    var i = InstanceConnection(a, b);
                    ae.atributes.Add((Atribute)bcomp);
                    ae.connections.Add(i);
                    ((Atribute)bcomp).connections.Add(i);
                }
            }
            else if (bcomp is Relation && ((Relation)bcomp).AddConnected())
            {
                var i = InstanceConnection(a, b);
                ae.relations.Add((Relation)bcomp);
                ae.connections.Add(i);
                ((Relation)bcomp).connections.Add(i);
            }
            else return;
        }
        else if (bcomp is Entity)
        {
            Entity ae = (Entity)bcomp;
            if (acomp is Atribute)
            {
                if (((Atribute)acomp).Multy)
                {
                    if (((Atribute)acomp).connections.FindAll(x => x.shape1 is Entity || x.shape2 is Entity).Count == 0)
                    {
                        var i = InstanceConnection(a, b);
                        ae.multis.Add((Atribute)acomp);
                        ae.connections.Add(i);
                        ((Atribute)acomp).connections.Add(i);
                    }
                }
                else if (((Atribute)acomp).connections.Count == 0)
                {
                    var i = InstanceConnection(a, b);
                    ae.atributes.Add((Atribute)acomp);
                    ae.connections.Add(i);
                    ((Atribute)acomp).connections.Add(i);
                }
            }
            else if (acomp is Relation && ((Relation)acomp).AddConnected())
            {
                var i = InstanceConnection(a, b);
                ae.relations.Add((Relation)acomp);
                ae.connections.Add(i);
                ((Relation)acomp).connections.Add(i);
            }
            else return;
        }
        else if (acomp is Relation)
        {
            Relation ae = (Relation)acomp;
            if (bcomp is Atribute && ((Atribute)bcomp).connections.Count == 0)
            {
                var i = InstanceConnection(a,b);
                ae.atributes.Add((Atribute)bcomp);
                ae.connections.Add(i);
                ((Atribute)bcomp).connections.Add(i);
            }
            else return;
        }
        else if (bcomp is Relation)
        {
            Relation ae = (Relation)bcomp;
            if (acomp is Atribute && ((Atribute)acomp).connections.Count == 0)
            {
                var i = InstanceConnection(a, b);
                ae.atributes.Add((Atribute)acomp);
                ae.connections.Add(i);
                ((Atribute)acomp).connections.Add(i);
            }
            else return;
        }
        else if (acomp is Atribute && ((Atribute)acomp).Multy)
        {
            var ae = (Atribute)acomp;
            if(bcomp is Atribute && ((Atribute)bcomp).connections.Count ==0)
            {
                var i = InstanceConnection(a, b);
                ae.connections.Add(i);
                ((Atribute)bcomp).connections.Add(i);
            }
        }
        else if(bcomp is Atribute && ((Atribute)bcomp).Multy)
        {
            var ae = (Atribute)acomp;
            if (acomp is Atribute && ((Atribute)acomp).connections.Count == 0)
            {
                var i = InstanceConnection(a, b);
                ae.connections.Add(i);
                ((Atribute)acomp).connections.Add(i);
            }
        }
        else return;
    }
    public void StartConnection()
    {
        ConnectRef.ConnectionMode = true;
        ColorBlock b = new ColorBlock();
        b.selectedColor = Color.cyan;
        b.colorMultiplier = 1;
        ConnectionButton.GetComponent<Button>().colors = b;
    } 
}
