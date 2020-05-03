[System.Serializable]
public class Cmd
{
    public string cmd;
    public Hand hand;
}

[System.Serializable]
public class Hand
{
    public int id;
    public string type;
    public int[] palmPosition;
    public int grabStrength;
    // fingers
}