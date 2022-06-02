using Microsoft.Win32;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CreateDB : MonoBehaviour
{
    public InputField DbName;
    public Dropdown ServerList;
    public GameObject MsgBox;
    public bool Query { get; set; }
    public bool Database { get; set; }
    public bool Classes { get; set; }
    public bool Server { get; set; }
    public int Language {get;set;}
    private void Start()
    {
        ServerList.options.Clear();
        string ServerName = System.Environment.MachineName;
        RegistryView registryView = System.Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
        using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
        {
            RegistryKey instanceKey = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL", false);
            if (instanceKey != null)
            {
                foreach (var instanceName in instanceKey.GetValueNames())
                {
                    ServerList.options.Add(new Dropdown.OptionData(ServerName + "\\" + instanceName));
                }
            }
        }
        ServerList.value = 0;
        ServerList.captionText.text = ServerList.options[0].text;
        Query = true;
        Database = true;
        Classes = true;
        Server = true;
        Language = 0;
    }
    public void CompileDB()
    {
        if (DbName.text.Length > 0) SqlConnect.DbName = DbName.text;
        else SqlConnect.DbName = RandomString();
        if (Database)
        {
            SqlConnect.ServerName = ServerList.options[ServerList.value].text;
            SqlConnect.Compile();
        }
        if(Query || Classes || Server)
        {
            var f = File.CreateText("pat.txt");
            f.Write(SqlConnect.DbName);
            f.Close();
            var p = System.Diagnostics.Process.Start("SaveDialog.exe", "pat.txt");
            while (!p.HasExited) ;
            if (File.Exists("pat.txt"))
            {
                var path = File.ReadAllText("pat.txt");
                File.Delete("pat.txt");
                if (path.Length != 0)
                {
                    if (Query)
                    {
                        QueryMaker.Path = path;
                        QueryMaker.Make();
                    }
                    if (Classes)
                    {
                        ClassMaker.Language = (ClassMaker.ProgramingLanguage)Language;
                        ClassMaker.Path = path;
                        ClassMaker.Make();
                    }
                }
            }
        }
        if (File.Exists("eror.txt"))
        {
            string erors = File.ReadAllText("eror.txt");
            MsgBox.GetComponent<MsgBox>().TextColor = Color.red;
            MsgBox.GetComponent<MsgBox>().Title = "Compile Eror !";
            MsgBox.GetComponent<MsgBox>().Text = "Export Failed\n" + erors;
            File.Delete("eror.txt");
        }
        else
        {
            MsgBox.GetComponent<MsgBox>().TextColor = Color.green;
            MsgBox.GetComponent<MsgBox>().Text = "Export Success";
        }
        Instantiate(MsgBox, transform.parent);
    }
    private string RandomString()
    {
        string ret = "";
        int len = Random.Range(5, 10);
        for (int i = 0; i < len; i++)
        {
            ret += (char)Random.Range(97, 122);
        }
        return ret;
    }
}
