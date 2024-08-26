using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public Vector2 playerRotation;

    public GameData()
    {
        this.playerPosition = Vector3.zero;
        this.playerRotation = Vector2.zero;
    }
}
