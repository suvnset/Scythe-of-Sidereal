using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaShadow : MonoBehaviour
{
    // Start is called before the first frame update

    public SpriteRenderer[] sprites;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.tag == "Player")
        {
            Debug.Log("LIGHT ON");
            LightsOn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.tag == "Player")
        {
            Debug.Log("LIGHT OFF");
            LightsOff();
        }
    }

    void LightsOn()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].enabled = false;
        }
    }

    void LightsOff()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].enabled = true;
        }
    }
}
