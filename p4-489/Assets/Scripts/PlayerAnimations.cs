using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    // Start is called before the first frame update
    Game game;
    GameObject player;
    PlayerMovement playerMovement;
    SpriteRenderer spriteRenderer;

    [SerializeField] Sprite up;
    [SerializeField] Sprite down;
    [SerializeField] Sprite left;
    [SerializeField] Sprite right;
    [SerializeField] Sprite topLeft;
    [SerializeField] Sprite topRight;
    [SerializeField] Sprite bottomLeft;
    [SerializeField] Sprite bottomRight;

    Vector2 playerDirection;
    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        player = GameObject.Find("Player");
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;

        playerDirection = playerMovement.movementDirection;

        FaceDirection();
    }

    void FaceDirection()
    {
        float directionY = playerDirection.y;
        float directionX = playerDirection.x;
        bool movingUp = directionY > 0 ? true : false;
        bool movingDown = directionY < 0 ? true : false;
        bool movingLeft = directionX < 0 ? true : false;
        bool movingRight = directionX > 0 ? true : false;

        // upwards movement
        if (movingUp)
        {
            if (movingLeft) { spriteRenderer.sprite = topLeft; }
            else if (movingRight) { spriteRenderer.sprite = topRight; }
            else { spriteRenderer.sprite = up; }
        }
        // downwards movement
        else if (movingDown)
        {
            if (movingLeft) { spriteRenderer.sprite = bottomLeft; }
            else if (movingRight) { spriteRenderer.sprite = bottomRight; }
            else { spriteRenderer.sprite = down; }
        }
        else
        {
            if (movingLeft) { spriteRenderer.sprite = left; }
            else if (movingRight) { spriteRenderer.sprite = right; }
        }
    }

    public void PlayerDied()
    {
        spriteRenderer.enabled = false;
    }
}
