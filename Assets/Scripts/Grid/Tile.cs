using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, BoxInterface, CoinInterface
{
    [SerializeField] Sprite grassSprite;
    [SerializeField] Sprite pathSprite;

    [SerializeField] MovableBox boxprefab;
    [SerializeField] CoinItem coinPrefab;

    private bool movePressed = false;
    private bool isCollidingWithPlayer = false;
    private Collision2D playerCollision;

    TileInterface tileInterface;

    internal PlayerController player;
    internal MovableBox currentBox;
    internal CoinItem currentCoin;

    private void Update()
    {
        if (Input.GetButtonDown("MoveBoxInput"))
        {
            this.movePressed = true;
        }
    }

    private void FixedUpdate()
    {
        if (movePressed)
        {
            handleClick();
            this.movePressed = false;
        }
    }

    public void setIsPath(bool isPath)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = isPath ? pathSprite : grassSprite;
    }
    public void setPlayer(PlayerController player)
    {
        this.player = player;
    }

    internal void initBox(TileInterface tileInterface, MovableBox box)
    {
        if (box == null)
        {
            box = Instantiate(boxprefab, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
            box.setBoxLayout();
        }

        box.isTouchingPlayer = false;
        this.tileInterface = tileInterface;
        currentBox = box;
        currentBox.name = $"Box {transform.position.x} {transform.position.y}";
        currentBox.addBoxInterface(this);
    }

    internal void removeBox()
    {
        this.tileInterface = null;
        currentBox = null;
        isCollidingWithPlayer = false;
        playerCollision = null;
    }

    internal void destroyBox()
    {
        DestroyImmediate(currentBox.gameObject);
        DestroyImmediate(currentBox);
        this.tileInterface = null;
        currentBox = null;
        isCollidingWithPlayer = false;
        playerCollision = null;
    }

    void handleClick()
    {
        if (isCollidingWithPlayer)
        {
            if (currentBox != null)
            {
                if (currentBox.isTouchingPlayer)
                {
                    moveBox();
                }
            }
        }
    }

    void moveBox()
    {
        currentBox.GetComponent<AudioSource>().Play();
        string collisionAngle = CollisionUtils.getAngleIsColliding(playerCollision);
        tileInterface.moveBoxFromTileTo(this, CollisionUtils.getOpositeDirection(collisionAngle));
    }

    public void changePlayerTouchStatus(bool isTouching, Collision2D collision)
    {
        isCollidingWithPlayer = isTouching;
        this.playerCollision = collision;
    }

    internal void initCoin()
    {
        currentCoin = Instantiate(coinPrefab, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
        currentCoin.initCoinInterface(this);
        currentCoin.initCoinSprite();
        currentCoin.name = $"Coin {transform.position.x} {transform.position.y}";
    }

    public void onCoinObtained(CoinItem obtainedCoin)
    {
        obtainedCoin.GetComponent<AudioSource>().Play();
        currentCoin.GetComponent<CircleCollider2D>().enabled = false;
        player.setCoinsAmount(player.coinsCount + 1);
        removeCoin();
    }

    internal void removeCoin()
    {
        Destroy(currentCoin.gameObject,0.4f);
        currentCoin = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.currentTile = this;
        }
    }

}