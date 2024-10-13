using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    public Game game;
    public Collider2D col;
    public TextMeshProUGUI UI_Subscript;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            game.StartBattle();
            UI_Subscript.text = "THE HORDE ARRIVES IN...";
        }
    }
}
