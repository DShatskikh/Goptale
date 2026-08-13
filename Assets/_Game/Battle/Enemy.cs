using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public string Name;
    public int Health;
    public int MaxHealth;
    public int Defence;
    public int Relationship;
    public int EXP;
    public int RUB;
    public List<string> Actions =  new();
    public List<string[]> ActionAnswers =  new();
    public virtual bool IsActive => Health > 0 && !IsMercy;
    public bool IsMercy;
}
