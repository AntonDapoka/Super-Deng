using UnityEngine;

public class LevelIconScript : MonoBehaviour
{
    [SerializeField] private int levelID;
    [SerializeField] private int iconNumber;
    [SerializeField] private Spark[] sparks;

    public int GetLevelID()
    {
        return levelID;
    }

    public int GetIconNumber()
    {
        return iconNumber;
    }

    public Spark[] GetSparks()
    {
        return sparks;
    }
}
