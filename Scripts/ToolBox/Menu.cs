using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject FileList,EditList,ToolsList,WindowList,HelpList;
    public GameObject MsgBox,Dialog;
    bool isActive;
    string text = "Diagram Wizard made for my final project at ort college.\n"
            + "You can convert your erd diagram to a database and server and classes at any programing language.\n" +
            "All right reserved to Koby levy and ort college.";

    public void Deactivate() => isActive = false;
    /*    PRESS EVENTS     */
    public void FilePress()
    {
        FileList.SetActive(!FileList.activeSelf);
        isActive = FileList.activeSelf;
        ToolsList.SetActive(false);
        HelpList.SetActive(false);
        EditList.SetActive(false);
        WindowList.SetActive(false);
    }
    public void EditPress()
    {
        EditList.SetActive(!EditList.activeSelf);
        isActive = EditList.activeSelf;
        FileList.SetActive(false);
        HelpList.SetActive(false);
        ToolsList.SetActive(false);
        WindowList.SetActive(false);
    }
    public void ToolsPress()
    {
        ToolsList.SetActive(!ToolsList.activeSelf);
        isActive = ToolsList.activeSelf;
        FileList.SetActive(false);
        HelpList.SetActive(false);
        EditList.SetActive(false);
        WindowList.SetActive(false);
    }
    public void WindowPress()
    {
        WindowList.SetActive(!WindowList.activeSelf);
        isActive = WindowList.activeSelf;
        FileList.SetActive(false);
        HelpList.SetActive(false);
        EditList.SetActive(false);
        ToolsList.SetActive(false);
    }
    public void HelpPress()
    {
        HelpList.SetActive(!HelpList.activeSelf);
        isActive = HelpList.activeSelf;
        FileList.SetActive(false);
        ToolsList.SetActive(false);
        EditList.SetActive(false);
        WindowList.SetActive(false);
    }
    /********************/
    /*     OVERLAP      */
    public void FileOverlap()
    {
        if(isActive)
        {
            FileList.SetActive(true);
            ToolsList.SetActive(false);
            HelpList.SetActive(false);
            EditList.SetActive(false);
            WindowList.SetActive(false);
        }
    }
    public void EditOverlap()
    {
        if (isActive)
        {
            FileList.SetActive(false);
            ToolsList.SetActive(false);
            HelpList.SetActive(false);
            EditList.SetActive(true);
            WindowList.SetActive(false);
        }
    }
    public void ToolsOverlap()
    {
        if (isActive)
        {
            FileList.SetActive(false);
            ToolsList.SetActive(true);
            HelpList.SetActive(false);
            EditList.SetActive(false);
            WindowList.SetActive(false);
        }
    }
    public void WindowOverlap()
    {
        if (isActive)
        {
            FileList.SetActive(false);
            ToolsList.SetActive(false);
            HelpList.SetActive(false);
            EditList.SetActive(false);
            WindowList.SetActive(true);
        }
    }
    public void HelpOverlap()
    {
        if (isActive)
        {
            FileList.SetActive(false);
            ToolsList.SetActive(false);
            HelpList.SetActive(true);
            EditList.SetActive(false);
            WindowList.SetActive(false);
        }
    }
    /*        FILE      */
    public void NewProject()
    {
        Dialog.GetComponent<DialogBox>().Title = "New Project";
        Dialog.GetComponent<DialogBox>().Msg = "Are you sure? this operation will quit from this project";
        var ins = Instantiate(Dialog, transform);
        ins.GetComponent<DialogBox>().YesClick += NewProjectYes;
    }
    public void NewProjectYes()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void Quit()
    {
        Dialog.GetComponent<DialogBox>().Title = "Quit";
        Dialog.GetComponent<DialogBox>().Msg = "Are you sure?";
        var ins = Instantiate(Dialog, transform);
        ins.GetComponent<DialogBox>().YesClick += QuitYes;
    }
    public void QuitYes()
    {
        Application.Quit(0);
    }
    /*******************/
    /*        HELP     */
    public void Info()
    {
        MsgBox.GetComponent<MsgBox>().Text = text;
        MsgBox.GetComponent<MsgBox>().TextColor = Color.black;
        MsgBox.GetComponent<MsgBox>().Title = "Info";
        var i = Instantiate(MsgBox, transform);
    }    
    /******************/
}
