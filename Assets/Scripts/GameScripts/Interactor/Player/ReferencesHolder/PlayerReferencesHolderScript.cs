
using UnityEngine;

public abstract class PlayerReferencesHolderScript : MonoBehaviour
{
    [SerializeField] protected GameObject[] heartParts;
    [SerializeField] protected GameObject[] beatShutters;
    [SerializeField] protected GameObject[] navigationHints;
    [SerializeField] protected GameObject frame;
    [SerializeField] protected Transform positionCenter;
    [SerializeField] protected Transform positionCameraFollow;

    public GameObject[] HeartParts => heartParts;
    public GameObject[] BeatShutters => beatShutters;
    public GameObject[] NavigationHints => navigationHints;
    public GameObject Frame => frame;
    public Transform PositionCenter => positionCenter;
    public Transform PositionCameraFollow => positionCameraFollow;
}
