using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static Atribute;

public static class ClassMaker
{
    public enum ProgramingLanguage
    {
        cs,
        cpp,
        dart,
        jar,
        js,
        py,
        ruby,
        swift,
        ts
    }
    public static string Path;
    public static ProgramingLanguage Language;
    static string code = "";
    public static void Make()
    {
        code = "";
        Path += @"\\" + SqlConnect.DbName + "Classes." + Language;
        List<Relation> relations = new List<Relation>();
        List<Atribute> multis = new List<Atribute>();
        foreach (var e in SqlConnect.entities)
        {
            code += "class " + e.GetName();
            if (Language != ProgramingLanguage.py)
            {
                code += "\n{\n";
            }
            else code += ":\n";
            foreach(var a in e.atributes)
            {
                if(!a.Multy)
                {
                    if (a.type != PropType.SQL_VARIANT)
                    {
                        code += "\t" + Variable(a.GetName(), a.type);
                    }
                    else code += "\t" + Variable(a.GetName());
                }
                else
                {
                    if (!multis.Contains(a))
                        multis.Add(a);
                }
            }
            if (Language != ProgramingLanguage.py)
            {
                code += "}\n";
            }
            foreach (var r in e.relations)
            {
                if (!relations.Contains(r))
                    relations.Add(r);
            }
        }
        foreach(var r in relations)
        {
            code += "class " + r.GetName();
            if (Language != ProgramingLanguage.py)
            {
                code += "\n{\n";
            }
            else code += ":\n";
            foreach (var a in r.atributes)
            {
                if (a.type != PropType.SQL_VARIANT)
                {
                    code += "\t" + Variable(a.GetName(), a.type);
                }
                else code += "\t" + Variable(a.GetName());
            }
            var l = r.connections.Where(o => o.shape1 is Entity || o.shape2 is Entity);
            foreach (var e in l)
            {
                string name = e.shape1 is Entity ? e.shape1.GetName() : e.shape2.GetName();
                code += "\t" + Variable(name + "Ref", name);
            }
            if (Language != ProgramingLanguage.py)
            {
                code += "}\n";
            }
        }
        foreach(var r in multis)
        {
            code += "class " + r.GetName();
            if (Language != ProgramingLanguage.py)
            {
                code += "\n{\n";
            }
            else code += ":\n";
            foreach (var a in r.connections)
            {
                Atribute at;
                if (a.shape1 is Atribute && !((Atribute)a.shape1).Multy)
                {
                    at = (Atribute)a.shape1;
                }
                else if (a.shape2 is Atribute && !((Atribute)a.shape2).Multy)
                    at = (Atribute)a.shape1;
                else continue;
                if (at.type != PropType.SQL_VARIANT)
                {
                    code += "\t" + Variable(at.GetName(), at.type);
                }
                else code += "\t" + Variable(at.GetName());
            }
        }
        File.WriteAllText(Path, code);
    }
    static string Variable(string name)
    {
        switch(Language)
        {
            case ProgramingLanguage.cs:
                return "public object " + name + "{get;set;}\n";
            case ProgramingLanguage.cpp:
                return "void* " + name + ";\n";
            case ProgramingLanguage.dart:
                return "var " + name + ";\n";
            case ProgramingLanguage.jar:
                return "public Object " + name + ";\n";
            case ProgramingLanguage.js:
                return name + ";\n";
            case ProgramingLanguage.py:
                return name + " = None\n";
            case ProgramingLanguage.ruby:
                return name + " = nil\n";
            case ProgramingLanguage.swift:
                return name + ": Optional\n";
            default:
                return name + "?:any";

        }
    }
    static string Variable(string name, string type)
    {
        switch (Language)
        {
            case ProgramingLanguage.cs:
                return "public " + type + " " + name + "{get;set;}\n";
            case ProgramingLanguage.cpp:
                return type + "* " + name + ";\n";
            case ProgramingLanguage.dart:
                return type + " " + name + ";\n";
            case ProgramingLanguage.jar:
                return "public " + type + " " + name + ";\n";
            case ProgramingLanguage.js:
                return name + ";\n";
            case ProgramingLanguage.py:
                return name + " = " + type + "()\n";
            case ProgramingLanguage.ruby:
                return name + " = " + type +"\n";
            case ProgramingLanguage.swift:
                return name + ":" + type +"\n";
            default:
                return name + "?:" + type +"\n";

        }
    }
    static string Variable(string name, PropType type)
    {
        switch(Language)
        {
            case ProgramingLanguage.cs:
                switch(type)
                {
                    case PropType.BIGINT:
                        return Variable(name, "long");

                    case PropType.BIT:
                        return Variable(name, "bool");

                    case PropType.DATE:
                    case PropType.DATETIME:
                    case PropType.TIME:
                        return Variable(name, "DateTime");

                    case PropType.DECIMAL:
                        return Variable(name, "decimal");

                    case PropType.FLOAT:
                        return Variable(name, "float");

                    case PropType.IMAGE:
                        return Variable(name, "Image");

                    case PropType.INT:
                    case PropType.MONEY:
                        return Variable(name, "int");

                    default:
                        return Variable(name, "string");

                }
            case ProgramingLanguage.cpp:
                switch (type)
                {
                    case PropType.BIGINT:
                        return Variable(name, "long");

                    case PropType.BIT:
                        return Variable(name, "bool");

                    case PropType.DECIMAL:
                        return Variable(name, "decimal");

                    case PropType.FLOAT:
                        return Variable(name, "float");

                    case PropType.INT:
                    case PropType.MONEY:
                        return Variable(name, "int");

                    default:
                        return Variable(name, "string");

                }
            case ProgramingLanguage.dart:
                switch (type)
                {

                    case PropType.BIT:
                        return Variable(name, "bool");

                    case PropType.DECIMAL:
                        return Variable(name, "double");

                    case PropType.FLOAT:
                        return Variable(name, "float");

                    case PropType.INT:
                    case PropType.MONEY:
                    case PropType.BIGINT:
                        return Variable(name, "int");

                    default:
                        return Variable(name, "string");

                }
            case ProgramingLanguage.jar:
                switch (type)
                {
                    case PropType.BIGINT:
                        return Variable(name, "long");

                    case PropType.BIT:
                        return Variable(name, "boolean");

                    case PropType.DATE:
                        return Variable(name, "LocalDate");

                    case PropType.DATETIME:
                        return Variable(name, "LocalDateTime");

                    case PropType.TIME:
                        return Variable(name, "LocalTime");

                    case PropType.DECIMAL:
                        return Variable(name, "double");

                    case PropType.FLOAT:
                        return Variable(name, "float");

                    case PropType.IMAGE:
                        return Variable(name, "Image");

                    case PropType.INT:
                    case PropType.MONEY:
                        return Variable(name, "int");

                    default:
                        return Variable(name, "String");

                }
            case ProgramingLanguage.swift:
                switch (type)
                {
                    case PropType.BIGINT:
                    case PropType.INT:
                    case PropType.MONEY:
                        return Variable(name, "Int");

                    case PropType.BIT:
                        return Variable(name, "Bool");

                    case PropType.DECIMAL:
                        return Variable(name, "Double");

                    case PropType.FLOAT:
                        return Variable(name, "Float");

                    default:
                        return Variable(name, "String");

                }
            case ProgramingLanguage.ts:
                switch(type)
                {
                    case PropType.BIGINT:
                    case PropType.DECIMAL:
                    case PropType.FLOAT:
                    case PropType.INT:
                    case PropType.MONEY:
                        return Variable(name, "number");

                    case PropType.IMAGE:
                        return Variable(name, "HTMLImageElement");

                    case PropType.DATE:
                    case PropType.DATETIME:
                    case PropType.TIME:
                        return Variable(name, "Date");

                    default:
                        return Variable(name, "string");
                        
                }

            case ProgramingLanguage.ruby:
                switch (type)
                {
                    case PropType.BIT:
                        return Variable(name, "false");

                    case PropType.BIGINT:
                    case PropType.INT:
                    case PropType.MONEY:
                        return Variable(name, "0");

                    case PropType.DECIMAL:
                    case PropType.FLOAT:
                        return Variable(name, "0.0");

                    default:
                        return Variable(name,"" + '"' + '"'); ;
                }
            default:
                switch(type)
                {
                    case PropType.BIT:
                        return Variable(name, "bool");

                    case PropType.BIGINT:
                    case PropType.INT:
                    case PropType.MONEY:
                        return Variable(name, "int");

                    case PropType.DECIMAL:
                    case PropType.FLOAT:
                        return Variable(name, "float");

                    default:
                        return Variable(name, "str"); ;
                }

        }
    }
}
