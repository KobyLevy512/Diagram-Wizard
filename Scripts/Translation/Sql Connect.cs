using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public static class SqlConnect
{
    public static List<Entity> entities = new List<Entity>();
    public static string DbName { get; set; }
    public static string ServerName { get; set; }
    private static SqlConnection sqlCon;
    private static string code;
    public static int fIndex = 0;
    public static void Compile()
    {
        code = "";
        fIndex = 0;
        AddLine("CREATE DATABASE " + DbName);
        Execute();
        code = "";
        AddLine("USE " + DbName);
        List<Relation> relations = new List<Relation>();
        List<Atribute> multis = new List<Atribute>();
        foreach (var i in entities)
        {
            string line = "CREATE TABLE ";
            string prim = "PRIMARY KEY CLUSTERED (";
            line += i.GetName() + "(";
            foreach (var j in i.atributes)
            {
                line += j.GetName() + " " + j.type.ToString();
                if (j.primaryKey) prim += j.GetName() + ",";
                else if (j.unique) line += " UNIQUE";
                if (j.notNull) line += " NOT NULL";
                if (j.autoIncrement) line += " IDENTITY(1,1)";
                line += ",";
            }
            if(prim.Length>23)
            {
                prim = prim.Remove(prim.Length - 1, 1);
                line += prim + ")";
            }
            else
            {
                line = line.Remove(line.Length - 1);
            }
            line += ")\n";
            foreach (var j in i.relations)
            {
                if (!relations.Contains(j))
                    relations.Add(j);
            }
            foreach(var j in i.multis)
            {
                if (!multis.Contains(j))
                    multis.Add(j);
            }
            AddLine(line);
        }
        foreach (var i in relations)
        {
            bool many1 = i.Side1IsMany();
            bool many2 = i.Side2IsMany();
            if(many1 == many2)
            {
                string prim = "PRIMARY KEY CLUSTERED (";
                string line = "CREATE TABLE ";
                line += i.GetName() + "\n(";
                foreach (var j in i.atributes)
                {
                    line += j.GetName() + " " + j.type.ToString();
                    if (j.primaryKey) prim += j.GetName() + ",";
                    else if (j.unique) line += " UNIQUE";
                    if (j.notNull) line += " NOT NULL";
                    if (j.autoIncrement) line += " IDENTITY(1,1)";
                    line += ",\n";
                }
                var l = i.connections.Where(o => o.shape1 is Entity || o.shape2 is Entity);
                string foriegn = "";
                foreach (var j in l)
                {
                    if (j.shape1 is Entity)
                    {
                        prim += j.shape1.GetName() + "Id,";
                        foriegn += "ALTER TABLE " + i.GetName() + " ADD CONSTRAINT FK_" + j.shape1.GetName() + fIndex++ + " FOREIGN KEY(" + j.shape1.GetName() + "Id) REFERENCES " + j.shape1.GetName() + "(";
                        foreach (var k in ((Entity)j.shape1).atributes)
                        {
                            if (k.primaryKey)
                            {
                                line += j.shape1.GetName() + "Id " + k.type.ToString() + ",\n";
                                foriegn += k.GetName() + ")\n";
                                break;
                            }
                        }
                    }
                    else
                    {
                        prim += j.shape2.GetName() + "Id,";
                        foriegn += "ALTER TABLE " + i.GetName() + " ADD CONSTRAINT FK_" + j.shape2.GetName() + fIndex++ + " FOREIGN KEY(" + j.shape2.GetName() + "Id) REFERENCES " + j.shape2.GetName() + "(";
                        foreach (var k in ((Entity)j.shape2).atributes)
                        {
                            if (k.primaryKey)
                            {
                                line += j.shape2.GetName() + "Id " + k.type.ToString() + ",\n";
                                foriegn += k.GetName() + ")\n";
                                break;
                            }
                        }
                    }
                }
                prim = prim.Remove(prim.Length - 1, 1);
                line += prim + ")\n";
                line += ")\n";
                line += foriegn;
                AddLine(line);
            }
            else if(many1 && !many2)
            {
                var l = i.connections.Where(o => o.shape1 is Entity || o.shape2 is Entity).ToList();
                string line = "ALTER TABLE ";
                string fieldName = "ADD ";
                if(l[1].shape1 is Entity)
                {
                    foreach (var k in ((Entity)l[1].shape1).atributes)
                    {
                        if (k.primaryKey)
                        {
                            fieldName += l[1].shape1.GetName() + k.GetName() + " " + k.type.ToString() + ";\n";
                            break;
                        }
                    }
                }
                else
                {
                    foreach (var k in ((Entity)l[1].shape2).atributes)
                    {
                        if (k.primaryKey)
                        {
                            fieldName += l[1].shape2.GetName() + k.GetName() + " " + k.type.ToString() + ";\n";
                            break;
                        }
                    }
                }
                if(l[0].shape1 is Entity)
                {
                    line += l[0].shape1.GetName() + "\n";
                }
                else
                {
                    line += l[0].shape2.GetName() + "\n";
                }
                line += fieldName;
                AddLine(line);
            }
            else if(!many1 && many2)
            {
                var l = i.connections.Where(o => o.shape1 is Entity || o.shape2 is Entity).ToList();
                string line = "ALTER TABLE ";
                string fieldName = "ADD ";
                if (l[0].shape1 is Entity)
                {
                    foreach (var k in ((Entity)l[0].shape1).atributes)
                    {
                        if (k.primaryKey)
                        {
                            fieldName += l[0].shape1.GetName() + k.GetName() + " " + k.type.ToString() + ";\n";
                            break;
                        }
                    }
                }
                else
                {
                    foreach (var k in ((Entity)l[1].shape2).atributes)
                    {
                        if (k.primaryKey)
                        {
                            fieldName += l[1].shape2.GetName() + k.GetName() + " " + k.type.ToString() + ";\n";
                            break;
                        }
                    }
                }
                if (l[1].shape1 is Entity)
                {
                    line += l[1].shape1.GetName() + "\n";
                }
                else
                {
                    line += l[1].shape2.GetName() + "\n";
                }
                line += fieldName;
                AddLine(line);
            }
        }
        foreach(var i in multis)
        {
            string prim = "PRIMARY KEY CLUSTERED (";
            string line = "CREATE TABLE ";
            line += i.GetName() + "\n(";
            var shape1 = i.connections.Where(f => f.shape1 is Atribute && !((Atribute)f.shape1).Multy);
            var shape2 = i.connections.Where(f => f.shape2 is Atribute && !((Atribute)f.shape2).Multy);
            foreach (var j in shape1)
            {
                var k = (Atribute)j.shape1;
                line += k.GetName() + " " + k.type.ToString();
                if (k.primaryKey) prim += k.GetName() + ",";
                else if (k.unique) line += " UNIQUE";
                if (k.notNull) line += " NOT NULL";
                if (k.autoIncrement) line += " IDENTITY(1,1)";
                line += ",\n";
            }
            foreach (var j in shape2)
            {
                var k = (Atribute)j.shape2;
                line += k.GetName() + " " + k.type.ToString();
                if (k.primaryKey) prim += k.GetName() + ",";
                else if (k.unique) line += " UNIQUE";
                if (k.notNull) line += " NOT NULL";
                if (k.autoIncrement) line += " IDENTITY(1,1)";
                line += ",\n";
            }
            var l = i.connections.Where(o => o.shape1 is Entity || o.shape2 is Entity).ToList();
            string foriegn = "";
            if(l[0].shape1 is Entity)
            {
                prim += l[0].shape1.GetName() + "Id,";
                foriegn += "ALTER TABLE " + i.GetName() + " ADD CONSTRAINT FK_" + l[0].shape1.GetName() + fIndex++ + " FOREIGN KEY(" + l[0].shape1.GetName() + "Id) REFERENCES " + l[0].shape1.GetName() + "(";
                foreach (var k in ((Entity)l[0].shape1).atributes)
                {
                    if (k.primaryKey)
                    {
                        line += l[0].shape1.GetName() + "Id " + k.type.ToString() + ",\n";
                        foriegn += k.GetName() + ")\n";
                        break;
                    }
                }
            }
            else
            {
                prim += l[0].shape2.GetName() + "Id,";
                foriegn += "ALTER TABLE " + i.GetName() + " ADD CONSTRAINT FK_" + l[0].shape2.GetName() + fIndex++ + " FOREIGN KEY(" + l[0].shape2.GetName() + "Id) REFERENCES " + l[0].shape2.GetName() + "(";
                foreach (var k in ((Entity)l[0].shape2).atributes)
                {
                    if (k.primaryKey)
                    {
                        line += l[0].shape2.GetName() + "Id " + k.type.ToString() + ",\n";
                        foriegn += k.GetName() + ")\n";
                        break;
                    }
                }
            }
            prim = prim.Remove(prim.Length - 1, 1);
            line += prim + ")\n";
            line += ")\n";
            line += foriegn;
            AddLine(line);
        }
        Execute();

    }
    private static void AddLine(string line)
    {
        code += line + ";";
    }
    private static void Execute()
    {
        var f = File.CreateText("tmp.txt");
        f.Write(code);
        f.Close();
        var p = System.Diagnostics.Process.Start("SqlCmd.exe", "tmp.txt" + " " + ServerName);
        p.WaitForExit();
        File.Delete("tmp.txt");
        code = "";
    }
}
