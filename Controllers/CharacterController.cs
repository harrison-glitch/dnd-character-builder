using Microsoft.AspNetCore.Mvc;
using CharacterBuilder.Models;

namespace CharacterBuilder.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CharacterController : ControllerBase
{
    private static List<Character> _characters = new();
    
    private static readonly List<Race> _races = new()
    {
        new Race { Name = "Human", Description = "Versatile and ambitious", AbilityScoreIncrease = new() { {"All", 1} }, Traits = new() { "Extra Language", "Extra Skill" } },
        new Race { Name = "Elf", Description = "Graceful and long-lived", AbilityScoreIncrease = new() { {"Dexterity", 2} }, Traits = new() { "Darkvision", "Fey Ancestry", "Trance" } },
        new Race { Name = "Dwarf", Description = "Hardy and resilient", AbilityScoreIncrease = new() { {"Constitution", 2} }, Traits = new() { "Darkvision", "Dwarven Resilience", "Stonecunning" } },
        new Race { Name = "Halfling", Description = "Small but brave", AbilityScoreIncrease = new() { {"Dexterity", 2} }, Traits = new() { "Lucky", "Brave", "Halfling Nimbleness" } },
        new Race { Name = "Dragonborn", Description = "Draconic heritage", AbilityScoreIncrease = new() { {"Strength", 2}, {"Charisma", 1} }, Traits = new() { "Breath Weapon", "Damage Resistance" } },
        new Race { Name = "Gnome", Description = "Small and clever", AbilityScoreIncrease = new() { {"Intelligence", 2} }, Traits = new() { "Darkvision", "Gnome Cunning" } },
        new Race { Name = "Half-Elf", Description = "Between two worlds", AbilityScoreIncrease = new() { {"Charisma", 2} }, Traits = new() { "Darkvision", "Fey Ancestry", "Two Skills" } },
        new Race { Name = "Half-Orc", Description = "Strength and struggle", AbilityScoreIncrease = new() { {"Strength", 2}, {"Constitution", 1} }, Traits = new() { "Darkvision", "Relentless Endurance", "Savage Attacks" } },
        new Race { Name = "Tiefling", Description = "Infernal heritage", AbilityScoreIncrease = new() { {"Intelligence", 1}, {"Charisma", 2} }, Traits = new() { "Darkvision", "Hellish Resistance", "Infernal Legacy" } }
    };

    private static readonly List<CharacterClass> _classes = new()
    {
        new CharacterClass { Name = "Fighter", HitDie = "d10", PrimaryAbilities = new() { "Strength", "Dexterity" }, SavingThrows = new() { "Strength", "Constitution" }, Skills = new() { "Acrobatics", "Animal Handling", "Athletics", "History", "Insight", "Intimidation", "Perception", "Survival" } },
        new CharacterClass { Name = "Wizard", HitDie = "d6", PrimaryAbilities = new() { "Intelligence" }, SavingThrows = new() { "Intelligence", "Wisdom" }, Skills = new() { "Arcana", "History", "Insight", "Investigation", "Medicine", "Religion" }, Spellcaster = true },
        new CharacterClass { Name = "Rogue", HitDie = "d8", PrimaryAbilities = new() { "Dexterity" }, SavingThrows = new() { "Dexterity", "Intelligence" }, Skills = new() { "Acrobatics", "Athletics", "Deception", "Insight", "Intimidation", "Investigation", "Perception", "Performance", "Persuasion", "Sleight of Hand", "Stealth" } },
        new CharacterClass { Name = "Cleric", HitDie = "d8", PrimaryAbilities = new() { "Wisdom" }, SavingThrows = new() { "Wisdom", "Charisma" }, Skills = new() { "History", "Insight", "Medicine", "Persuasion", "Religion" }, Spellcaster = true },
        new CharacterClass { Name = "Ranger", HitDie = "d10", PrimaryAbilities = new() { "Dexterity", "Wisdom" }, SavingThrows = new() { "Strength", "Dexterity" }, Skills = new() { "Animal Handling", "Athletics", "Insight", "Investigation", "Nature", "Perception", "Stealth", "Survival" }, Spellcaster = true },
        new CharacterClass { Name = "Paladin", HitDie = "d10", PrimaryAbilities = new() { "Strength", "Charisma" }, SavingThrows = new() { "Wisdom", "Charisma" }, Skills = new() { "Athletics", "Insight", "Intimidation", "Medicine", "Persuasion", "Religion" }, Spellcaster = true },
        new CharacterClass { Name = "Barbarian", HitDie = "d12", PrimaryAbilities = new() { "Strength" }, SavingThrows = new() { "Strength", "Constitution" }, Skills = new() { "Animal Handling", "Athletics", "Intimidation", "Nature", "Perception", "Survival" } },
        new CharacterClass { Name = "Bard", HitDie = "d8", PrimaryAbilities = new() { "Charisma" }, SavingThrows = new() { "Dexterity", "Charisma" }, Skills = new() { "Any three" }, Spellcaster = true },
        new CharacterClass { Name = "Druid", HitDie = "d8", PrimaryAbilities = new() { "Wisdom" }, SavingThrows = new() { "Intelligence", "Wisdom" }, Skills = new() { "Arcana", "Animal Handling", "Insight", "Medicine", "Nature", "Perception", "Religion", "Survival" }, Spellcaster = true },
        new CharacterClass { Name = "Monk", HitDie = "d8", PrimaryAbilities = new() { "Dexterity", "Wisdom" }, SavingThrows = new() { "Strength", "Dexterity" }, Skills = new() { "Acrobatics", "Athletics", "History", "Insight", "Religion", "Stealth" } },
        new CharacterClass { Name = "Sorcerer", HitDie = "d6", PrimaryAbilities = new() { "Charisma" }, SavingThrows = new() { "Constitution", "Charisma" }, Skills = new() { "Arcana", "Deception", "Insight", "Intimidation", "Persuasion", "Religion" }, Spellcaster = true },
        new CharacterClass { Name = "Warlock", HitDie = "d8", PrimaryAbilities = new() { "Charisma" }, SavingThrows = new() { "Wisdom", "Charisma" }, Skills = new() { "Arcana", "Deception", "History", "Intimidation", "Investigation", "Nature", "Religion" }, Spellcaster = true }
    };

    private static readonly List<string> _backgrounds = new()
    {
        "Acolyte", "Criminal", "Folk Hero", "Noble", "Sage", "Soldier", "Charlatan", "Entertainer", "Guild Artisan", "Hermit", "Outlander", "Sailor"
    };

    private static readonly List<string> _alignments = new()
    {
        "Lawful Good", "Neutral Good", "Chaotic Good", "Lawful Neutral", "True Neutral", "Chaotic Neutral", "Lawful Evil", "Neutral Evil", "Chaotic Evil"
    };

    private static readonly List<Spell> _spells = new()
    {
        new Spell { Name = "Cantrip - Fire Bolt", Level = 0, School = "Evocation", CastingTime = "1 action", Range = "120 feet", Duration = "Instantaneous", Description = "Ranged spell attack for 1d10 fire damage" },
        new Spell { Name = "Cantrip - Mage Hand", Level = 0, School = "Conjuration", CastingTime = "1 action", Range = "30 feet", Duration = "1 minute", Description = "Create a spectral hand to manipulate objects" },
        new Spell { Name = "Magic Missile", Level = 1, School = "Evocation", CastingTime = "1 action", Range = "120 feet", Duration = "Instantaneous", Description = "Three darts of magical force, each dealing 1d4+1 damage" },
        new Spell { Name = "Healing Word", Level = 1, School = "Evocation", CastingTime = "1 bonus action", Range = "60 feet", Duration = "Instantaneous", Description = "Heal 1d4 + spellcasting modifier hit points" },
        new Spell { Name = "Shield", Level = 1, School = "Abjuration", CastingTime = "1 reaction", Range = "Self", Duration = "1 round", Description = "+5 bonus to AC until start of next turn" },
        new Spell { Name = "Fireball", Level = 3, School = "Evocation", CastingTime = "1 action", Range = "150 feet", Duration = "Instantaneous", Description = "20-foot radius explosion dealing 8d6 fire damage" }
    };

    [HttpGet]
    public ActionResult<List<Character>> GetCharacters() => _characters;

    [HttpGet("{id}")]
    public ActionResult<Character> GetCharacter(string id)
    {
        var character = _characters.FirstOrDefault(c => c.Id == id);
        return character == null ? NotFound() : character;
    }

    [HttpPost]
    public ActionResult<Character> CreateCharacter([FromBody] Character character)
    {
        character.Id = Guid.NewGuid().ToString();
        _characters.Add(character);
        return character;
    }

    [HttpPut("{id}")]
    public ActionResult UpdateCharacter(string id, [FromBody] Character character)
    {
        var existing = _characters.FirstOrDefault(c => c.Id == id);
        if (existing == null) return NotFound();
        
        character.Id = id;
        var index = _characters.IndexOf(existing);
        _characters[index] = character;
        return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteCharacter(string id)
    {
        _characters.RemoveAll(c => c.Id == id);
        return Ok();
    }

    [HttpGet("races")]
    public ActionResult<List<Race>> GetRaces() => _races;

    [HttpGet("classes")]
    public ActionResult<List<CharacterClass>> GetClasses() => _classes;

    [HttpGet("backgrounds")]
    public ActionResult<List<string>> GetBackgrounds() => _backgrounds;

    [HttpGet("alignments")]
    public ActionResult<List<string>> GetAlignments() => _alignments;

    [HttpGet("spells")]
    public ActionResult<List<Spell>> GetSpells() => _spells;

    [HttpPost("generate-stats")]
    public ActionResult<AbilityScores> GenerateStats()
    {
        var random = new Random();
        return new AbilityScores
        {
            Strength = RollStat(random),
            Dexterity = RollStat(random),
            Constitution = RollStat(random),
            Intelligence = RollStat(random),
            Wisdom = RollStat(random),
            Charisma = RollStat(random)
        };
    }

    private static int RollStat(Random random)
    {
        var rolls = new[] { random.Next(1, 7), random.Next(1, 7), random.Next(1, 7), random.Next(1, 7) };
        return rolls.OrderByDescending(x => x).Take(3).Sum();
    }
}
