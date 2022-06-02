using UnityEngine;
using UnityEngine.UI;


public class Atribute : Shape
{
    public enum PropType
    {
        SQL_VARIANT,
        BIGINT,
        BIT,
        DATE,
        DATETIME,
        DECIMAL,
        FLOAT,
        IMAGE,
        INT,
        MONEY,
        TEXT,
        TIME,
        VARBINARY,
        VARCHAR,
        XML
    }
    [HideInInspector]
    public bool primaryKey,notNull,autoIncrement,unique;
    public bool Multy;
    public GameObject UnderLine;
    [HideInInspector]
    public bool Connected;
    [HideInInspector]
    public PropType type;

    public bool PrimaryKey
    {
        set
        {
            primaryKey = value;
            notNull = primaryKey;
            unique = primaryKey;
            UnderLine.SetActive(primaryKey);
            transform.GetChild(2).GetChild(3).GetComponent<Toggle>().isOn = primaryKey;
            transform.GetChild(2).GetChild(4).GetComponent<Toggle>().isOn = primaryKey;
            transform.GetChild(2).GetChild(4).GetComponent<Toggle>().interactable = !primaryKey;
            transform.GetChild(2).GetChild(3).GetComponent<Toggle>().interactable = !primaryKey;
        }
    }
    public bool NotNull
    {
        set
        {
            notNull = value;
        }
    }
    public bool AutoIncrement
    {
        set => autoIncrement = value;
    }
    public bool Unique
    {
        set => unique = value;
    }
    public int TypeChange
    {
        set
        {
            type = (PropType)value;
            if(type == PropType.INT || type == PropType.DECIMAL || type == PropType.BIGINT)
            {
                transform.GetChild(2).GetChild(5).GetComponent<Toggle>().interactable = true;
            }
            else
            {
                transform.GetChild(2).GetChild(5).GetComponent<Toggle>().interactable = false;
            }
        }
    }
    public override string Name
    {
        get => base.Name; 
        set
        {
            if (value.Length > 0)
            {
                sName = ValidateName(value);
                transform.GetChild(0).GetComponent<Image>().color = Color.clear;
                if (!Multy)
                {
                    primaryKey = sName.ToLower().Contains("id");
                    notNull = primaryKey;
                    unique = primaryKey;
                    UnderLine.SetActive(primaryKey);
                    transform.GetChild(2).GetChild(3).GetComponent<Toggle>().isOn = primaryKey;
                    transform.GetChild(2).GetChild(4).GetComponent<Toggle>().isOn = primaryKey;
                    transform.GetChild(2).GetChild(2).GetComponent<Toggle>().isOn = primaryKey;
                    transform.GetChild(2).GetChild(4).GetComponent<Toggle>().interactable = !primaryKey;
                    transform.GetChild(2).GetChild(3).GetComponent<Toggle>().interactable = !primaryKey;
                }
            }
        }
    }
}
