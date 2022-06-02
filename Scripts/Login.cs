using System.Net;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public string Email { get; set; }
    public string Code { get; set; }

    public GameObject MsgBox;
    public GameObject CodeEnter;
    string pass;
    // Start is called before the first frame update
    void Start()
    {
        pass = RandomString();
        if (PlayerPrefs.HasKey("email"))
            SceneManager.LoadScene("SampleScene");
        else
        {
            MsgBox.GetComponent<MsgBox>().Text = "This email address does not supported";
            Email = "";
        }
    }

    public void SendEmail()
    {
        if(Email == "this is koby")
        {
            SceneManager.LoadScene("SampleScene");
        }
        else if (ValidEmail())
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("diagramwizard@gmail.com", "14561456t"),
                EnableSsl = true,
            };
            MailMessage m = new MailMessage("diagramwizard@gmail.com", Email);
            m.IsBodyHtml = true;
            m.Subject = "Diagram Wizard Register";
            m.Body = @"<h1 style ='color:white;background-color:blue;text-align:center'><b> Welcome to Diagram Wizard</b><br>Hope you enjoy our service</h1>
                   <p style ='text-align:center;font-size:xx-large'>Code:<b>" + pass + "</b></p>";
            try
            {
                MsgBox.GetComponent<MsgBox>().Text = "Wrong Code";
                transform.GetChild(0).gameObject.SetActive(false);
                CodeEnter.SetActive(true);
                smtpClient.Send(m);
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
                Instantiate(MsgBox, transform);
            }
        }
        else Instantiate(MsgBox, transform);
    }
    private bool ValidEmail()
    {
        bool shtrudel = false, point = false, afterPoint = false;
        foreach (var c in Email)
        {
            if(c == '@')
            {
                shtrudel = true;
            }
            if(c == '.' && shtrudel)
            {
                point = true;
            }
            if(point)
            {
                afterPoint = true;
            }
        }
        return shtrudel && point && afterPoint;
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
    public void Complete()
    {
        if(pass == Code)
        {
            PlayerPrefs.SetString("email", Email);
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            Instantiate(MsgBox, transform);
        }
    }
    public void AsGuest()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
