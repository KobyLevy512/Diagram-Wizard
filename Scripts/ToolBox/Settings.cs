using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject Content;
    public Slider ZoomSlider;

    [Space(20)]
    public GameObject Camera;
    public Dropdown BgColor;

    public int Quality
    {
        set
        {
            QualitySettings.SetQualityLevel(value);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void ChangeZoom()
    {
        Content.GetComponent<RectTransform>().localScale = new Vector3(0.25f + ZoomSlider.value, 0.25f + ZoomSlider.value, 1);
    }
    public void ChangeBackground()
    {
        Color c;
        switch(BgColor.value)
        {
            case 0:
                c = new Color(1f, 1f, 1f);
                break;

            case 1:
                c = new Color(0.6320754f, 0.1401299f, 0.1982487f);
                break;

            case 2:
                c = new Color(0.1459594f, 0.3789692f, 0.754717f);
                break;

            case 3:
                c = new Color(0.7306421f, 0.745283f, 0.2214756f);
                break;

            case 4:
                c = new Color(0.2f, 0.2f, 0.2f);
                break;

            default:
                c = new Color(0.1921569f, 0.4162878f, 0.4745098f);
                break;
        }
        Camera.GetComponent<Camera>().backgroundColor = c;
    }
}
