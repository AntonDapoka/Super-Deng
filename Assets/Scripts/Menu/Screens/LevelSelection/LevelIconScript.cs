using UnityEngine;

public class LevelIconScript : MonoBehaviour
{
    [SerializeField] private int levelID;

    public int GetLevelID()
    {
        return levelID;
    }
}
