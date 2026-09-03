using System;

[Serializable]
public sealed class Meta
{
    public static Meta Instance;
    public bool IsCompleteTutorial;
    public bool IsCompleteDemo;

    public static Meta GetDefault()
    {
        return new Meta
        {
           
        };
    }
}
