using UnityEngine;

public class Tiles : MonoBehaviour
{
    public enum TileType
    {
        Normal,
        Hazard,
        Exit,
        Conveyor
    }

    public TileType tileType = TileType.Normal;

    public TileType GetTileType()
    {
        return tileType;
    }
}