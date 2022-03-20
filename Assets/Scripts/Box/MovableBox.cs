using System;
using UnityEngine;


class MovableBox : MonoBehaviour
{
    [SerializeField] internal Sprite boxSprite;
    BoxInterface boxInterface;
    internal bool isTouchingPlayer = false;

    internal void addBoxInterface(BoxInterface boxInterface)
    {
        this.boxInterface = boxInterface;
    }

    internal void setBoxLayout()
    {
        GetComponent<SpriteRenderer>().sprite = boxSprite;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            boxInterface.changePlayerTouchStatus(true, collision);
            isTouchingPlayer = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            boxInterface.changePlayerTouchStatus(true, collision);
            isTouchingPlayer = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            boxInterface.changePlayerTouchStatus(false, collision);
            isTouchingPlayer = false;
        }
    }
}

