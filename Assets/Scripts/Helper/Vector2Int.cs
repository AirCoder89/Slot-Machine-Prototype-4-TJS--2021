using System;
using UnityEngine;

namespace Helper
{
    [Serializable]
    public struct Vector2Int
    {
        public int x;
        public int y;

        public Vector2Int(int inX, int inY)
        {
            x = inX;
            y = inY;
        }

        public Vector2Int(Vector2 inVector2)
        {
            x = Mathf.RoundToInt(inVector2.x);
            y = Mathf.RoundToInt(inVector2.y);
        }

        public Vector2 ToVector2()
            => new Vector2(x, y);

        public string ToString(char separator = 'x')
            => $"{x}{separator}{y}";
    }
}