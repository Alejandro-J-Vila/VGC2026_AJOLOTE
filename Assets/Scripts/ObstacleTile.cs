using UnityEngine.Tilemaps;

public class ObstacleTile : Tile
{
    // Enum that represents all of the different obstacles in game
    public enum Obstacles : byte { RedO, GreenO, BlueO, YellowO, PinkO, OrangeO }
    public Obstacles obstacleName; // Name of the obstacle
    public ObstacleTile() : base()
    {

    }
}
