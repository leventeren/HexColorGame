using UnityEngine;

namespace vertigoGames.hexColorGame
{
    public static class HexMetrics
    {
        public const float OuterRadius = 10f;
        public const float InnerRadius = OuterRadius * 0.866025404f;
        public const float solidFactor = 0.75f;
        public const float blendFactor = 1f - solidFactor;
        public const float hexDrawFactor = 17f;        
        /* corners(x,y[hexagon içerisinde gameobject kullanılmayacaksa mesh ile derinlik verilebilir],z) */
        public static Vector3[] corners = {
            new Vector3(-5f, GameManager.Instance.hexagonCellHeight, 8.6f),
            new Vector3(5f, GameManager.Instance.hexagonCellHeight, 8.6f),
            new Vector3(8.6f, GameManager.Instance.hexagonCellHeight, 0f),
            new Vector3(5f, GameManager.Instance.hexagonCellHeight, -8.6f),
            new Vector3(-5f, GameManager.Instance.hexagonCellHeight, -8.6f),
            new Vector3(-8.6f, GameManager.Instance.hexagonCellHeight, 0f),
            new Vector3(-5f, GameManager.Instance.hexagonCellHeight, 8.6f)
        };
    }
}