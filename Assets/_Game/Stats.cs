using System.Collections.Generic;
using UnityEngine;

public sealed class Stats
{
    public static Stats Instance;
    
    public string Name;
    public int HP;
    public int MaxHP;
    public int LV;
    public int RUB;
    public int EXP;
    public string Weapon;
    public string Armor;
    public string[] Items = new string[8];
    public float Time;
    public string LevelName;
    public Vector2 Position;
    public int Kills;
    public int Spared;
    public bool IsGenocide;
    public int Fun;
    
    public int TomaraCutscene;
    public int GermanState; // 0 идет катсцена 1 прошла катмцена 2 закончили бой пощадив 3 убили
    public bool[] SpikePuzzle;
    public bool[] MashaShop; // 0 Jam 1 Pies
    public bool[] PlatePuzzle;
    public int DJNikolayState; // 0 ничего не делали 1 победили мирно 2 убили 3 поговорили 4 поговорили убили
    public bool IsTomaraDead;
    public List<string> LayItemIDs = new();
    public bool IsGasterEgg;

    public static int GetNextEXP(int lv)
    {
        return lv switch
        {
            1 => 10,
            2 => 20,
            3 => 40,
            4 => 50,
            5 => 80,
            6 => 100,
            7 => 200,
            8 => 300,
            9 => 400,
            10 => 500,
            11 => 800,
            12 => 1000,
            13 => 1500,
            14 => 2000,
            15 => 3000,
            16 => 5000,
            17 => 10000,
            18 => 25000,
            19 => 49999,
            _ => 0,
        };
    }

    public static int GetBaseATK(int lv)
    {
        return lv switch
        {
            1 => 0,
            2 => 2,
            3 => 4,
            4 => 6,
            5 => 8,
            _ => 0,
        };
    }

    public static int GetWeaponATK(string weaponName)
    {
        return weaponName switch
        {
           Constants.ROZOCHKA => 0,
            _ => 0
        };
    }
    
    public static int GetBaseDEF(int lv)
    {
        return lv switch
        {
            1 => 0,
            2 => 0,
            3 => 0,
            4 => 0,
            5 => 1,
            _ => 0,
        };
    }
    
    public static int GetArmorDEF(string armor)
    {
        return armor switch
        {
            Constants.K_PAL => 0,
            Constants.K_NIKE => 3,
            Constants.K_ADIDAS => 99,
            _ => 0
        };
    }
    
    public Stats GetLoad()
    {
        var loadData = SaveSystem.Load();

        if (loadData != null)
            return loadData;
        
        return new Stats()
        {
            Name = "???",
            LevelName = "???",
        };
    }
    
    public static Stats GetDefault()
    {
        return new Stats
        {
            HP = 20,
            MaxHP = 20,
            EXP = 0,
            LV = 1,
            LevelName = "Level 1",
            Name = "Федя",
            Position = new Vector2(),
            Armor = Constants.K_PAL,
            Weapon = Constants.ROZOCHKA,
            Items = new []
            {
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
            },
            
            TomaraCutscene = 0,
            GermanState = 0,
            SpikePuzzle = new bool[2],
            MashaShop = new bool[2],
            PlatePuzzle = new bool[5],
            IsGenocide = false,
            LayItemIDs = new List<string>(),
            Fun = Random.Range(0, 100),
        };
    }
    
    public static string GetLevelName(string loadDataLevelName)
    {
        return loadDataLevelName switch
        {
            "Level 1" => "Руинино",
            "Level 2" => "Руинино",
            "Level 3" => "Руинино",
            "Level 4" => "Руинино",
            "Level 5" => "Руинино",
            "Level 6" => "Руинино",
            "Level 7" => "Чертаново",
            "Level 8" => "Чертаново",
            "Level 9" => "Чертаново",
            "Level 10" => "Чертаново",
            "Level 11" => "Чертаново",
            "Level 11_2" => "Чертаново",
            "Level 12" => "Чертаново",
            "Level 13" => "Чертаново",
            "Level 14" => "Чертаново",
            "Level 14_1" => "Чертаново",
            "Level 14_2" => "Чертаново",
            "Level 15" => "Руинино-Падик",
            "Level 15_1" => "Руинино",
            "Level 15_2" => "Руинино",
            "Level 15_2_1" => "Руинино",
            "Level 15_2_2" => "Руинино",
            "Level 16" => "Руинино",
            "Level 17" => "Руинино",
            "Level 18" => "Руинино",
            "Level 19" => "Руинино",
            "Level 20" => "Руинино",
            _ => "???"
        };
    }

    public bool TryAddItem(string item)
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] != string.Empty)
                continue;
            
            Items[i] = item;
            return true;
        }
        
        return false;
    }

    public static bool IsWeapon(string item)
    {
        return item switch
        {
            Constants.ROZOCHKA => true,
            _ => false
        };
    }

    public static bool IsArmor(string item)
    {
        return item switch
        {
            Constants.K_PAL => true,
            Constants.K_NIKE => true,
            Constants.K_ADIDAS => true,
            _ => false
        };
    }

    public static bool IsComida(string item)
    {
        return item switch
        {
            Constants.JAGUAR => true,
            Constants.ANTIPOHMELIN => true,
            Constants.MASHA_JAM => true,
            Constants.MASHA_PIES => true,
            Constants.BELASH => true,
            Constants.NASTOYKA_GASTERA => true,
            _ => false
        };
    }
    
    public static int GetItemHP(string item)
    {
        return item switch
        {
            Constants.JAGUAR => 5,
            Constants.ANTIPOHMELIN => 10,
            Constants.MASHA_JAM => 24,
            Constants.MASHA_PIES => 12,
            Constants.BELASH => 100,
            Constants.NASTOYKA_GASTERA => 1,
            _ => 0
        };
    }
}
