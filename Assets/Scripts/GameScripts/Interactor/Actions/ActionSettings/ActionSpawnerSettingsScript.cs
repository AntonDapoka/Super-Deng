public class ActionSpawnerSettingsScript : ActionSettingsScript
{
    public bool isRandom;
    public bool isPseudoRandom;
    public bool isCertain; // Will be certain faces activated?

    public bool isStableQuantity;
    public int quantityExact;
    public int quantityMin;
    public int quantityMax;

    public bool isRelativeToPlayer;
    public int[] arrayOfFacesRelativeToPlayer;
    public bool isRelativeToFigure;
    public int[] arrayOfFacesRelativeToFigure;

    public bool isProximityLimit;
    public int proximityLimit;
    public bool isDistanceLimit;
    public int distanceLimit;

    public bool isBasicSettingsChange;
}
