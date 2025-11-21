namespace CharacterBuilder.Models;

public class Character
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "";
    public string Race { get; set; } = "";
    public string Class { get; set; } = "";
    public string Background { get; set; } = "";
    public string Alignment { get; set; } = "";
    public int Level { get; set; } = 1;
    public AbilityScores Abilities { get; set; } = new();
    public List<string> Spells { get; set; } = new();
    public List<Equipment> Equipment { get; set; } = new();
    public int HitPoints { get; set; }
    public int ArmorClass { get; set; }
    public int ProficiencyBonus { get; set; } = 2;
    public int Initiative { get; set; }
    public SavingThrows SavingThrows { get; set; } = new();
    public string Backstory { get; set; } = "";
    public string PhysicalDescription { get; set; } = "";
    public List<string> ClassFeatures { get; set; } = new();
}

public class AbilityScores
{
    public int Strength { get; set; } = 10;
    public int Dexterity { get; set; } = 10;
    public int Constitution { get; set; } = 10;
    public int Intelligence { get; set; } = 10;
    public int Wisdom { get; set; } = 10;
    public int Charisma { get; set; } = 10;
}

public class SavingThrows
{
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Constitution { get; set; }
    public int Intelligence { get; set; }
    public int Wisdom { get; set; }
    public int Charisma { get; set; }
}

public class Equipment
{
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public int Quantity { get; set; } = 1;
    public string Description { get; set; } = "";
    public string Damage { get; set; } = "";
    public string Properties { get; set; } = "";
    public int ArmorClass { get; set; } = 0;
}

public class Weapon
{
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public string Damage { get; set; } = "";
    public string DamageType { get; set; } = "";
    public string Properties { get; set; } = "";
    public string Category { get; set; } = "";
}

public class Armor
{
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public int ArmorClass { get; set; }
    public string Properties { get; set; } = "";
    public string Category { get; set; } = "";
}

public class Race
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public Dictionary<string, int> AbilityScoreIncrease { get; set; } = new();
    public List<string> Traits { get; set; } = new();
}

public class CharacterClass
{
    public string Name { get; set; } = "";
    public string HitDie { get; set; } = "";
    public List<string> PrimaryAbilities { get; set; } = new();
    public List<string> SavingThrows { get; set; } = new();
    public List<string> Skills { get; set; } = new();
    public bool Spellcaster { get; set; } = false;
    public Dictionary<int, List<string>> Features { get; set; } = new();
}

public class ClassFeature
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    
    public ClassFeature(string name, string description)
    {
        Name = name;
        Description = description;
    }
}

public class Spell
{
    public string Name { get; set; } = "";
    public int Level { get; set; }
    public string School { get; set; } = "";
    public string CastingTime { get; set; } = "";
    public string Range { get; set; } = "";
    public string Duration { get; set; } = "";
    public string Description { get; set; } = "";
    public string Damage { get; set; } = "";
    public string SpellSlot { get; set; } = "";
    public string Components { get; set; } = "";
}
