using UnityEngine;
using System;

public abstract class CollisionUtils
{

    public const string TOP_SIDE = "top";
    public const string BOTTOM_SIDE = "bottom";
    public const string RIGHT_SIDE = "right";
    public const string LEFT_SIDE = "left";
    public const string NONE_SIDE = "none";

    public static string getAngleIsColliding(Collision2D collision)
    {

        Vector2 direction = collision.GetContact(0).normal;
        if (direction.x == 1) return LEFT_SIDE;
        if (direction.x == -1) return RIGHT_SIDE;
        if (direction.y == 1) return BOTTOM_SIDE;
        if (direction.y == -1) return TOP_SIDE;

        return NONE_SIDE;
    }
    public static String getOpositeDirection(string playerPosition)
    {

        switch (playerPosition)
        {
            case TOP_SIDE:
                return BOTTOM_SIDE;
            case BOTTOM_SIDE:
                return TOP_SIDE;
            case LEFT_SIDE:
                return RIGHT_SIDE;
            case RIGHT_SIDE:
                return LEFT_SIDE;
            default: return NONE_SIDE;
        }
    }
}

