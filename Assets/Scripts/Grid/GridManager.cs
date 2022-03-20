using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class GridManager : MonoBehaviour, TileInterface
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private Tile tilePrefab;
    private Dictionary<Vector2, Tile> tiles;

    [SerializeField] int initialPlayerX;
    [SerializeField] int initialPlayerY;
    [SerializeField] PlayerController player;
    [SerializeField] private Transform cam;
    [SerializeField] private GameObject background;

    [SerializeField] List<Vector2> fieldsWithBoxes;
    [SerializeField] List<Vector2> fieldsWithPath;
    [SerializeField] List<Vector2> fieldsWithCoin;

    private int limitX = 0;
    private int limitY = 0;

    void Start()
    {
        player.gridManager = this;
        generateGrid();
        setupCameraPosition();
        setupBackgroundPosition();
        setTileBackgrounds();
        resetPlayerPosition();
        resetBoxesPositions();
        resetCoinsPositions();
    }

    private void Update()
    {
        handleClickReset();
    }

    void handleClickReset()
    {
        if (Input.GetButtonDown("ResetInput"))
        {
            removeGrid();
            Start();
        }
    }

    void removeGrid()
    {
        foreach (KeyValuePair<Vector2, Tile> entry in tiles)
        {
            if(entry.Value.currentBox != null)
            {
                entry.Value.destroyBox();
            }
            if(entry.Value.currentCoin != null)
            {
                entry.Value.removeCoin();
            }

            player.setCoinsAmount(0);

            DestroyImmediate(entry.Value.gameObject);
        }
    }

    void generateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.setPlayer(player);

                if (x > limitX) limitX = x;
                if (y > limitY) limitY = y;

                tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
    }

    void setupCameraPosition()
    {
        cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
    }

    void setupBackgroundPosition()
    {
        background.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f);

    }

    void setTileBackgrounds()
    {
        foreach (KeyValuePair<Vector2, Tile> entry in tiles)
        {
            Tile tile = getTileAtPosition(entry.Key);
            if (fieldsWithPath.Contains(entry.Key))
            {
                tile.setIsPath(true);
            }
            else
            {
                tile.setIsPath(false);
            }
        }
    }

    void resetPlayerPosition()
    {
        var firstPosition = getTileAtPosition(new Vector2(initialPlayerX, initialPlayerY));

        player.transform.position = new Vector3(
            firstPosition.gameObject.transform.position.x,
            firstPosition.gameObject.transform.position.y
            );
    }

    void resetBoxesPositions()
    {
        for (int i = 0; i < fieldsWithBoxes.Count; i++)
        {
            Vector2 boxesDirection = fieldsWithBoxes[i];

            Tile currentTile = getTileAtPosition(boxesDirection);

            if (currentTile != null)
            {
                currentTile.initBox(this, null);
            }
        }
    }

    void resetCoinsPositions()
    {
        for (int i = 0; i < fieldsWithCoin.Count; i++)
        {
            Vector2 coinsDirection = fieldsWithCoin[i];

            Tile currentTile = getTileAtPosition(coinsDirection);

            if (currentTile != null)
            {
                currentTile.initCoin();
            }
        }

        player.setCoinsAmount(0);
    }

    public void moveBoxFromTileTo(Tile oldTile, string direction)
    {

        float currentX = oldTile.transform.position.x;
        float currentY = oldTile.transform.position.y;

        if (checkIfCanMoveBox(direction, currentX, currentY))
        {
            Tile newBoxTile = null;

            switch (direction)
            {
                case CollisionUtils.TOP_SIDE:
                    newBoxTile = getTileAtPosition(new Vector2(currentX, currentY + 1));
                    break;
                case CollisionUtils.BOTTOM_SIDE:
                    newBoxTile = getTileAtPosition(new Vector2(currentX, currentY - 1));
                    break;
                case CollisionUtils.LEFT_SIDE:
                    newBoxTile = getTileAtPosition(new Vector2(currentX - 1, currentY));
                    break;
                case CollisionUtils.RIGHT_SIDE:
                    newBoxTile = getTileAtPosition(new Vector2(currentX + 1, currentY));
                    break;
            }

            oldTile.currentBox.transform.DOMove(newBoxTile.transform.position,0.7f);
            
            newBoxTile.initBox(this, oldTile.currentBox);
            oldTile.removeBox();
        }


    }

    internal bool checkIfCanMoveBox(string direction, float currentX, float currentY)
    {

        Tile tile;

        switch (direction)
        {
            case CollisionUtils.TOP_SIDE:
                tile = getTileAtPosition(new Vector2(currentX, currentY + 1));
                break;
            case CollisionUtils.BOTTOM_SIDE:
                tile = getTileAtPosition(new Vector2(currentX, currentY - 1));
                break;
            case CollisionUtils.LEFT_SIDE:
                tile = getTileAtPosition(new Vector2(currentX - 1, currentY));
                break;
            case CollisionUtils.RIGHT_SIDE:
                tile = getTileAtPosition(new Vector2(currentX + 1, currentY));
                break;
            default: return false;
        }

        if (tile != null)
        {
            if (tile.currentBox != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }

    }

    internal bool checkPlayerMovementLimit(Vector2 futurePosition)
    {

        Vector2 futureNormal = new Vector2(Mathf.Round(futurePosition.x), Mathf.Round(futurePosition.y));

        if (futurePosition.x < 0 || futurePosition.y < 0)
        {
            return false;
        } 
        else if (getTileAtPosition(futureNormal) == null)
        {
            return false;
        }
        else if (futureNormal.y > limitY || futureNormal.x > limitX)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    internal Tile getTileAtPosition(Vector2 pos)
    {
        if (tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    internal int getCoinTotal()
    {
        return fieldsWithCoin.Count;
    }
}