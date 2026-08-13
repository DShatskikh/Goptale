using TMPro;

public struct ShakingSymbolData
{
    public int StartIndex;
    public int EndIndex;
    public TMP_Text Label;
    public float Force;

    public ShakingSymbolData(int startIndex, int endIndex, TMP_Text label, float force)
    {
        StartIndex = startIndex;
        EndIndex = endIndex;
        Label = label;
        Force = force;
    }
}
