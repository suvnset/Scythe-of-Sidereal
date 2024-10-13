using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI killCount;
    public TextMeshProUGUI timeLeft;
    public TextMeshProUGUI alert;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI subscript;

    public Image weapon;
    public Image special;
    public Image dash;
    public Sprite scytheSprite;
    public Sprite swordSprite;


    Color faded;
    void Start()
    {
        faded = new Color(0.1603774f, 0.1603774f, 0.1603774f, 1f);
    }
    public void HP(int h)
    {
        hp.text = "HP: " + h.ToString();
    }
    public void KillCount(int k)
    {
        killCount.text = "KILLS: " + k.ToString();
    }
    public void TimeLeft(int t)
    {
        if (t != -1)
        {
            timeLeft.text = t.ToString();
        }
        else
        {
            timeLeft.text = "";
        }
    }
    public void Subscript(string txt)
    {
        subscript.text = txt;
    }

    public void Alert(string txt)
    {
        alert.text = txt;
    }

    public void WeaponUI(bool active)
    {
        if (active)
        {
            weapon.sprite = scytheSprite;
        }
        else
        {
            weapon.sprite = swordSprite;
        }
    }

    public void SpecialUI(bool active)
    {
        if (active)
        {
            special.color = Color.white;
        }
        else
        {
            Debug.Log("special UI");
            special.color = faded;
        }
    }

    public void DashUI(bool active)
    {
        if (active)
        {
            dash.color = Color.white;
        }
        else
        {
            dash.color = faded;
        }
    }
}
