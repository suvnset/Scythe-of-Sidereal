using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    Collider2D m_collider;
    public SpriteRenderer m_spriteRenderer;
    bool m_isOpen;

    void Start()
    {
        m_collider = GetComponent<Collider2D>();
        m_isOpen = false;
    }

    private void OnCollision2D(Collider2D collision)
    {
        if (!(m_isOpen) && ((collision.CompareTag("Weapon")) || (collision.CompareTag("Enemy"))))
        {
            Debug.Log("DOOR OPENING");
            m_collider.enabled = false;
            m_spriteRenderer.color = Color.green;
            m_isOpen = true;
            Invoke("Open", 2.0f);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(m_isOpen) && (collision.CompareTag("Weapon")))
        {
            StartCoroutine(Open());
        }
    }

    IEnumerator Open()
    {
        Debug.Log("DOOR OPENING");
        m_collider.enabled = false;
        m_spriteRenderer.enabled = false;
        m_isOpen = true;

        yield return new WaitForSeconds(2f);

        Debug.Log("DOOR CLOSING");
        m_collider.enabled = true;
        m_spriteRenderer.enabled = true;
        m_isOpen = false;
    }
}
