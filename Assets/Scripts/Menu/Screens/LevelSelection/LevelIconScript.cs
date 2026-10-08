using UnityEngine;

public class LevelIconScript : MonoBehaviour
{
    [SerializeField] private int levelID;
    [SerializeField] private Spark[] sparks;

    public int GetLevelID()
    {
        return levelID;
    }

    public Spark[] GetSparks()
    {
        return sparks;
    }
}
