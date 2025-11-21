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
        new Race { Name = "Tiefling", Description = "Infernal heritage", AbilityScoreIncrease = new() { {"Intelligence", 1}, {"Charisma", 2} }, Traits = new() { "Darkvision", "Hellish Resistance", "Infernal Legacy" } },
        new Race { Name = "Aarakocra", Description = "Bird-like humanoids", AbilityScoreIncrease = new() { {"Dexterity", 2}, {"Wisdom", 1} }, Traits = new() { "Flight", "Talons", "Wind Caller" } },
        new Race { Name = "Aasimar", Description = "Celestial-touched", AbilityScoreIncrease = new() { {"Charisma", 2} }, Traits = new() { "Darkvision", "Celestial Resistance", "Healing Hands", "Light Bearer" } },
        new Race { Name = "Bugbear", Description = "Goblinoid brutes", AbilityScoreIncrease = new() { {"Strength", 2}, {"Dexterity", 1} }, Traits = new() { "Darkvision", "Long-Limbed", "Powerful Build", "Sneaky", "Surprise Attack" } },
        new Race { Name = "Firbolg", Description = "Giant-kin forest dwellers", AbilityScoreIncrease = new() { {"Wisdom", 2}, {"Strength", 1} }, Traits = new() { "Firbolg Magic", "Hidden Step", "Powerful Build", "Speech of Beast and Leaf" } },
        new Race { Name = "Genasi", Description = "Elemental-touched", AbilityScoreIncrease = new() { {"Constitution", 2} }, Traits = new() { "Elemental Resistance", "Elemental Magic" } },
        new Race { Name = "Githyanki", Description = "Astral plane warriors", AbilityScoreIncrease = new() { {"Strength", 2}, {"Intelligence", 1} }, Traits = new() { "Githyanki Psionics", "Martial Prodigy", "Medium Armor Proficiency" } },
        new Race { Name = "Goblin", Description = "Small and cunning", AbilityScoreIncrease = new() { {"Dexterity", 2}, {"Constitution", 1} }, Traits = new() { "Darkvision", "Fury of the Small", "Nimble Escape" } },
        new Race { Name = "Goliath", Description = "Mountain-dwelling giants", AbilityScoreIncrease = new() { {"Strength", 2}, {"Constitution", 1} }, Traits = new() { "Natural Athlete", "Stone's Endurance", "Powerful Build", "Mountain Born" } },
        new Race { Name = "Hobgoblin", Description = "Militaristic goblinoids", AbilityScoreIncrease = new() { {"Constitution", 2}, {"Intelligence", 1} }, Traits = new() { "Darkvision", "Martial Training", "Saving Face" } },
        new Race { Name = "Kenku", Description = "Flightless bird-folk", AbilityScoreIncrease = new() { {"Dexterity", 2}, {"Wisdom", 1} }, Traits = new() { "Expert Forgery", "Kenku Training", "Mimicry" } },
        new Race { Name = "Kobold", Description = "Small draconic humanoids", AbilityScoreIncrease = new() { {"Dexterity", 2}, {"Intelligence", 1} }, Traits = new() { "Darkvision", "Grovel, Cower, and Beg", "Pack Tactics", "Sunlight Sensitivity" } },
        new Race { Name = "Lizardfolk", Description = "Reptilian survivalists", AbilityScoreIncrease = new() { {"Constitution", 2}, {"Wisdom", 1} }, Traits = new() { "Bite", "Cunning Artisan", "Hold Breath", "Hunter's Lore", "Natural Armor", "Hungry Jaws" } },
        new Race { Name = "Orc", Description = "Savage and strong", AbilityScoreIncrease = new() { {"Strength", 2}, {"Constitution", 1} }, Traits = new() { "Darkvision", "Aggressive", "Menacing", "Relentless Endurance" } },
        new Race { Name = "Tabaxi", Description = "Curious cat-folk", AbilityScoreIncrease = new() { {"Dexterity", 2}, {"Charisma", 1} }, Traits = new() { "Darkvision", "Feline Agility", "Cat's Claws", "Cat's Talents" } },
        new Race { Name = "Tortle", Description = "Turtle-like wanderers", AbilityScoreIncrease = new() { {"Strength", 2}, {"Wisdom", 1} }, Traits = new() { "Claws", "Hold Breath", "Natural Armor", "Shell Defense", "Survival Instinct" } },
        new Race { Name = "Triton", Description = "Sea-dwelling guardians", AbilityScoreIncrease = new() { {"Strength", 1}, {"Constitution", 1}, {"Charisma", 1} }, Traits = new() { "Amphibious", "Control Air and Water", "Emissary of the Sea", "Guardians of the Depths" } },
        new Race { Name = "Yuan-ti Pureblood", Description = "Serpentine and cunning", AbilityScoreIncrease = new() { {"Charisma", 2}, {"Intelligence", 1} }, Traits = new() { "Darkvision", "Innate Spellcasting", "Magic Resistance", "Poison Immunity" } }
    };

    private static readonly List<Spell> _spells = new()
    {
        // Cantrips (Level 0)
        new Spell { Name = "Cantrip - Fire Bolt", Level = 0, School = "Evocation", CastingTime = "1 action", Range = "120 feet", Duration = "Instantaneous", Description = "Ranged spell attack for fire damage", Damage = "1d10 fire", SpellSlot = "Cantrip", Components = "V, S" },
        new Spell { Name = "Cantrip - Mage Hand", Level = 0, School = "Conjuration", CastingTime = "1 action", Range = "30 feet", Duration = "1 minute", Description = "Create a spectral hand to manipulate objects", Damage = "None", SpellSlot = "Cantrip", Components = "V, S" },
        new Spell { Name = "Cantrip - Prestidigitation", Level = 0, School = "Transmutation", CastingTime = "1 action", Range = "10 feet", Duration = "Up to 1 hour", Description = "Minor magical trick", Damage = "None", SpellSlot = "Cantrip", Components = "V, S" },
        new Spell { Name = "Cantrip - Eldritch Blast", Level = 0, School = "Evocation", CastingTime = "1 action", Range = "120 feet", Duration = "Instantaneous", Description = "Beam of crackling energy", Damage = "1d10 force", SpellSlot = "Cantrip", Components = "V, S" },
        new Spell { Name = "Cantrip - Sacred Flame", Level = 0, School = "Evocation", CastingTime = "1 action", Range = "60 feet", Duration = "Instantaneous", Description = "Flame-like radiance descends on target", Damage = "1d8 radiant", SpellSlot = "Cantrip", Components = "V, S" },
        new Spell { Name = "Cantrip - Vicious Mockery", Level = 0, School = "Enchantment", CastingTime = "1 action", Range = "60 feet", Duration = "Instantaneous", Description = "Insults deal psychic damage", Damage = "1d4 psychic", SpellSlot = "Cantrip", Components = "V" },
        new Spell { Name = "Cantrip - Minor Illusion", Level = 0, School = "Illusion", CastingTime = "1 action", Range = "30 feet", Duration = "1 minute", Description = "Create a sound or image", Damage = "None", SpellSlot = "Cantrip", Components = "S, M" },
        
        // 1st Level Spells
        new Spell { Name = "Magic Missile", Level = 1, School = "Evocation", CastingTime = "1 action", Range = "120 feet", Duration = "Instantaneous", Description = "Three darts of magical force", Damage = "3 × (1d4+1) force", SpellSlot = "1st level", Components = "V, S" },
        new Spell { Name = "Healing Word", Level = 1, School = "Evocation", CastingTime = "1 bonus action", Range = "60 feet", Duration = "Instantaneous", Description = "Heal a creature", Damage = "1d4 + spell mod healing", SpellSlot = "1st level", Components = "V" },
        new Spell { Name = "Shield", Level = 1, School = "Abjuration", CastingTime = "1 reaction", Range = "Self", Duration = "1 round", Description = "+5 bonus to AC", Damage = "None", SpellSlot = "1st level", Components = "V, S" },
        new Spell { Name = "Cure Wounds", Level = 1, School = "Evocation", CastingTime = "1 action", Range = "Touch", Duration = "Instantaneous", Description = "Heal by touch", Damage = "1d8 + spell mod healing", SpellSlot = "1st level", Components = "V, S" },
        new Spell { Name = "Detect Magic", Level = 1, School = "Divination", CastingTime = "1 action", Range = "Self", Duration = "Concentration, up to 10 minutes", Description = "Sense presence of magic", Damage = "None", SpellSlot = "1st level", Components = "V, S" },
        new Spell { Name = "Thunderwave", Level = 1, School = "Evocation", CastingTime = "1 action", Range = "Self (15-foot cube)", Duration = "Instantaneous", Description = "Thunder damage and push", Damage = "2d8 thunder", SpellSlot = "1st level", Components = "V, S" },
        new Spell { Name = "Faerie Fire", Level = 1, School = "Evocation", CastingTime = "1 action", Range = "60 feet", Duration = "Concentration, up to 1 minute", Description = "Outline creatures in light", Damage = "None", SpellSlot = "1st level", Components = "V" },
        new Spell { Name = "Sleep", Level = 1, School = "Enchantment", CastingTime = "1 action", Range = "90 feet", Duration = "1 minute", Description = "Put creatures to sleep", Damage = "5d8 hit points affected", SpellSlot = "1st level", Components = "V, S, M" },
        
        // 2nd Level Spells
        new Spell { Name = "Misty Step", Level = 2, School = "Conjuration", CastingTime = "1 bonus action", Range = "Self", Duration = "Instantaneous", Description = "Teleport up to 30 feet", Damage = "None", SpellSlot = "2nd level", Components = "V" },
        new Spell { Name = "Scorching Ray", Level = 2, School = "Evocation", CastingTime = "1 action", Range = "120 feet", Duration = "Instantaneous", Description = "Three rays of fire", Damage = "3 × 2d6 fire", SpellSlot = "2nd level", Components = "V, S" },
        new Spell { Name = "Hold Person", Level = 2, School = "Enchantment", CastingTime = "1 action", Range = "60 feet", Duration = "Concentration, up to 1 minute", Description = "Paralyze a humanoid", Damage = "None", SpellSlot = "2nd level", Components = "V, S, M" },
        new Spell { Name = "Invisibility", Level = 2, School = "Illusion", CastingTime = "1 action", Range = "Touch", Duration = "Concentration, up to 1 hour", Description = "Make a creature invisible", Damage = "None", SpellSlot = "2nd level", Components = "V, S, M" },
        new Spell { Name = "Web", Level = 2, School = "Conjuration", CastingTime = "1 action", Range = "60 feet", Duration = "Concentration, up to 1 hour", Description = "Fill area with sticky webs", Damage = "None", SpellSlot = "2nd level", Components = "V, S, M" },
        
        // 3rd Level Spells
        new Spell { Name = "Fireball", Level = 3, School = "Evocation", CastingTime = "1 action", Range = "150 feet", Duration = "Instantaneous", Description = "20-foot radius explosion", Damage = "8d6 fire", SpellSlot = "3rd level", Components = "V, S, M" },
        new Spell { Name = "Lightning Bolt", Level = 3, School = "Evocation", CastingTime = "1 action", Range = "Self (100-foot line)", Duration = "Instantaneous", Description = "100-foot line of lightning", Damage = "8d6 lightning", SpellSlot = "3rd level", Components = "V, S, M" },
        new Spell { Name = "Counterspell", Level = 3, School = "Abjuration", CastingTime = "1 reaction", Range = "60 feet", Duration = "Instantaneous", Description = "Stop a creature from casting", Damage = "None", SpellSlot = "3rd level", Components = "S" },
        new Spell { Name = "Fly", Level = 3, School = "Transmutation", CastingTime = "1 action", Range = "Touch", Duration = "Concentration, up to 10 minutes", Description = "Grant flying speed of 60 feet", Damage = "None", SpellSlot = "3rd level", Components = "V, S, M" },
        new Spell { Name = "Dispel Magic", Level = 3, School = "Abjuration", CastingTime = "1 action", Range = "120 feet", Duration = "Instantaneous", Description = "End spells on target", Damage = "None", SpellSlot = "3rd level", Components = "V, S" },
        
        // 4th Level Spells
        new Spell { Name = "Greater Invisibility", Level = 4, School = "Illusion", CastingTime = "1 action", Range = "Touch", Duration = "Concentration, up to 1 minute", Description = "Invisible and can attack", Damage = "None", SpellSlot = "4th level", Components = "V, S" },
        new Spell { Name = "Polymorph", Level = 4, School = "Transmutation", CastingTime = "1 action", Range = "60 feet", Duration = "Concentration, up to 1 hour", Description = "Transform creature into beast", Damage = "None", SpellSlot = "4th level", Components = "V, S, M" },
        new Spell { Name = "Wall of Fire", Level = 4, School = "Evocation", CastingTime = "1 action", Range = "120 feet", Duration = "Concentration, up to 1 minute", Description = "Create wall of fire", Damage = "5d8 fire", SpellSlot = "4th level", Components = "V, S, M" },
        
        // 5th Level Spells
        new Spell { Name = "Cone of Cold", Level = 5, School = "Evocation", CastingTime = "1 action", Range = "Self (60-foot cone)", Duration = "Instantaneous", Description = "60-foot cone of cold", Damage = "8d8 cold", SpellSlot = "5th level", Components = "V, S, M" },
        new Spell { Name = "Teleport", Level = 5, School = "Conjuration", CastingTime = "1 action", Range = "10 feet", Duration = "Instantaneous", Description = "Transport up to 8 creatures", Damage = "None", SpellSlot = "5th level", Components = "V" },
        new Spell { Name = "Dominate Person", Level = 5, School = "Enchantment", CastingTime = "1 action", Range = "60 feet", Duration = "Concentration, up to 1 minute", Description = "Control a humanoid's actions", Damage = "None", SpellSlot = "5th level", Components = "V, S" }
    };

    private static readonly List<CharacterClass> _classes = new()
    {
        new CharacterClass { 
            Name = "Fighter", 
            HitDie = "d10", 
            PrimaryAbilities = new() { "Strength", "Dexterity" }, 
            SavingThrows = new() { "Strength", "Constitution" }, 
            Skills = new() { "Acrobatics", "Animal Handling", "Athletics", "History", "Insight", "Intimidation", "Perception", "Survival" },
            Features = new() {
                { 1, new() { "Fighting Style", "Second Wind" } },
                { 2, new() { "Action Surge" } },
                { 3, new() { "Martial Archetype" } },
                { 4, new() { "Ability Score Improvement" } },
                { 5, new() { "Extra Attack" } },
                { 6, new() { "Ability Score Improvement" } },
                { 7, new() { "Martial Archetype Feature" } },
                { 8, new() { "Ability Score Improvement" } },
                { 9, new() { "Indomitable" } },
                { 10, new() { "Martial Archetype Feature" } },
                { 11, new() { "Extra Attack (2)" } },
                { 12, new() { "Ability Score Improvement" } },
                { 13, new() { "Indomitable (2)" } },
                { 14, new() { "Ability Score Improvement" } },
                { 15, new() { "Martial Archetype Feature" } },
                { 16, new() { "Ability Score Improvement" } },
                { 17, new() { "Action Surge (2)", "Indomitable (3)" } },
                { 18, new() { "Martial Archetype Feature" } },
                { 19, new() { "Ability Score Improvement" } },
                { 20, new() { "Extra Attack (3)" } }
            }
        },
        new CharacterClass { 
            Name = "Wizard", 
            HitDie = "d6", 
            PrimaryAbilities = new() { "Intelligence" }, 
            SavingThrows = new() { "Intelligence", "Wisdom" }, 
            Skills = new() { "Arcana", "History", "Insight", "Investigation", "Medicine", "Religion" }, 
            Spellcaster = true,
            Features = new() {
                { 1, new() { "Spellcasting", "Arcane Recovery" } },
                { 2, new() { "Arcane Tradition" } },
                { 3, new() { "Cantrip Formulas" } },
                { 4, new() { "Ability Score Improvement" } },
                { 5, new() { "3rd Level Spells" } },
                { 6, new() { "Arcane Tradition Feature" } },
                { 7, new() { "4th Level Spells" } },
                { 8, new() { "Ability Score Improvement" } },
                { 9, new() { "5th Level Spells" } },
                { 10, new() { "Arcane Tradition Feature" } },
                { 11, new() { "6th Level Spells" } },
                { 12, new() { "Ability Score Improvement" } },
                { 13, new() { "7th Level Spells" } },
                { 14, new() { "Arcane Tradition Feature" } },
                { 15, new() { "8th Level Spells" } },
                { 16, new() { "Ability Score Improvement" } },
                { 17, new() { "9th Level Spells" } },
                { 18, new() { "Spell Mastery" } },
                { 19, new() { "Ability Score Improvement" } },
                { 20, new() { "Signature Spells" } }
            }
        },
        new CharacterClass { 
            Name = "Rogue", 
            HitDie = "d8", 
            PrimaryAbilities = new() { "Dexterity" }, 
            SavingThrows = new() { "Dexterity", "Intelligence" }, 
            Skills = new() { "Acrobatics", "Athletics", "Deception", "Insight", "Intimidation", "Investigation", "Perception", "Performance", "Persuasion", "Sleight of Hand", "Stealth" },
            Features = new() {
                { 1, new() { "Expertise", "Sneak Attack", "Thieves' Cant" } },
                { 2, new() { "Cunning Action" } },
                { 3, new() { "Roguish Archetype" } },
                { 4, new() { "Ability Score Improvement" } },
                { 5, new() { "Uncanny Dodge" } },
                { 6, new() { "Expertise" } },
                { 7, new() { "Evasion" } },
                { 8, new() { "Ability Score Improvement" } },
                { 9, new() { "Roguish Archetype Feature" } },
                { 10, new() { "Ability Score Improvement" } },
                { 11, new() { "Reliable Talent" } },
                { 12, new() { "Ability Score Improvement" } },
                { 13, new() { "Roguish Archetype Feature" } },
                { 14, new() { "Blindsense" } },
                { 15, new() { "Slippery Mind" } },
                { 16, new() { "Ability Score Improvement" } },
                { 17, new() { "Roguish Archetype Feature" } },
                { 18, new() { "Elusive" } },
                { 19, new() { "Ability Score Improvement" } },
                { 20, new() { "Stroke of Luck" } }
            }
        },
        new CharacterClass { 
            Name = "Cleric", 
            HitDie = "d8", 
            PrimaryAbilities = new() { "Wisdom" }, 
            SavingThrows = new() { "Wisdom", "Charisma" }, 
            Skills = new() { "History", "Insight", "Medicine", "Persuasion", "Religion" }, 
            Spellcaster = true,
            Features = new() {
                { 1, new() { "Spellcasting", "Divine Domain" } },
                { 2, new() { "Channel Divinity", "Divine Domain Feature" } },
                { 3, new() { "2nd Level Spells" } },
                { 4, new() { "Ability Score Improvement" } },
                { 5, new() { "Destroy Undead (CR 1/2)", "3rd Level Spells" } },
                { 6, new() { "Channel Divinity (2/rest)", "Divine Domain Feature" } },
                { 7, new() { "4th Level Spells" } },
                { 8, new() { "Ability Score Improvement", "Destroy Undead (CR 1)", "Divine Domain Feature" } },
                { 9, new() { "5th Level Spells" } },
                { 10, new() { "Divine Intervention" } },
                { 11, new() { "Destroy Undead (CR 2)", "6th Level Spells" } },
                { 12, new() { "Ability Score Improvement" } },
                { 13, new() { "7th Level Spells" } },
                { 14, new() { "Destroy Undead (CR 3)" } },
                { 15, new() { "8th Level Spells" } },
                { 16, new() { "Ability Score Improvement" } },
                { 17, new() { "Destroy Undead (CR 4)", "9th Level Spells", "Divine Domain Feature" } },
                { 18, new() { "Channel Divinity (3/rest)" } },
                { 19, new() { "Ability Score Improvement" } },
                { 20, new() { "Divine Intervention Improvement" } }
            }
        },
        new CharacterClass { 
            Name = "Ranger", 
            HitDie = "d10", 
            PrimaryAbilities = new() { "Dexterity", "Wisdom" }, 
            SavingThrows = new() { "Strength", "Dexterity" }, 
            Skills = new() { "Animal Handling", "Athletics", "Insight", "Investigation", "Nature", "Perception", "Stealth", "Survival" }, 
            Spellcaster = true,
            Features = new() {
                { 1, new() { "Favored Enemy", "Natural Explorer" } },
                { 2, new() { "Fighting Style", "Spellcasting" } },
                { 3, new() { "Ranger Archetype", "Primeval Awareness" } },
                { 4, new() { "Ability Score Improvement" } },
                { 5, new() { "Extra Attack", "2nd Level Spells" } },
                { 6, new() { "Favored Enemy and Natural Explorer improvements" } },
                { 7, new() { "Ranger Archetype Feature" } },
                { 8, new() { "Ability Score Improvement", "Land's Stride" } },
                { 9, new() { "3rd Level Spells" } },
                { 10, new() { "Natural Explorer improvement", "Hide in Plain Sight" } },
                { 11, new() { "Ranger Archetype Feature" } },
                { 12, new() { "Ability Score Improvement" } },
                { 13, new() { "4th Level Spells" } },
                { 14, new() { "Favored Enemy improvement", "Vanish" } },
                { 15, new() { "Ranger Archetype Feature" } },
                { 16, new() { "Ability Score Improvement" } },
                { 17, new() { "5th Level Spells" } },
                { 18, new() { "Feral Senses" } },
                { 19, new() { "Ability Score Improvement" } },
                { 20, new() { "Foe Slayer" } }
            }
        },
        new CharacterClass { 
            Name = "Paladin", 
            HitDie = "d10", 
            PrimaryAbilities = new() { "Strength", "Charisma" }, 
            SavingThrows = new() { "Wisdom", "Charisma" }, 
            Skills = new() { "Athletics", "Insight", "Intimidation", "Medicine", "Persuasion", "Religion" }, 
            Spellcaster = true,
            Features = new() {
                { 1, new() { "Divine Sense", "Lay on Hands" } },
                { 2, new() { "Fighting Style", "Spellcasting", "Divine Smite" } },
                { 3, new() { "Divine Health", "Sacred Oath" } },
                { 4, new() { "Ability Score Improvement" } },
                { 5, new() { "Extra Attack", "2nd Level Spells" } },
                { 6, new() { "Aura of Protection" } },
                { 7, new() { "Sacred Oath Feature" } },
                { 8, new() { "Ability Score Improvement" } },
                { 9, new() { "3rd Level Spells" } },
                { 10, new() { "Aura of Courage" } },
                { 11, new() { "Improved Divine Smite" } },
                { 12, new() { "Ability Score Improvement" } },
                { 13, new() { "4th Level Spells" } },
                { 14, new() { "Cleansing Touch" } },
                { 15, new() { "Sacred Oath Feature" } },
                { 16, new() { "Ability Score Improvement" } },
                { 17, new() { "5th Level Spells" } },
                { 18, new() { "Aura improvements" } },
                { 19, new() { "Ability Score Improvement" } },
                { 20, new() { "Sacred Oath Feature" } }
            }
        },
        new CharacterClass { 
            Name = "Barbarian", 
            HitDie = "d12", 
            PrimaryAbilities = new() { "Strength" }, 
            SavingThrows = new() { "Strength", "Constitution" }, 
            Skills = new() { "Animal Handling", "Athletics", "Intimidation", "Nature", "Perception", "Survival" },
            Features = new() {
                { 1, new() { "Rage", "Unarmored Defense" } },
                { 2, new() { "Reckless Attack", "Danger Sense" } },
                { 3, new() { "Primal Path" } },
                { 4, new() { "Ability Score Improvement" } },
                { 5, new() { "Extra Attack", "Fast Movement" } },
                { 6, new() { "Path Feature" } },
                { 7, new() { "Feral Instinct" } },
                { 8, new() { "Ability Score Improvement" } },
                { 9, new() { "Brutal Critical (1 die)" } },
                { 10, new() { "Path Feature" } },
                { 11, new() { "Relentless Rage" } },
                { 12, new() { "Ability Score Improvement" } },
                { 13, new() { "Brutal Critical (2 dice)" } },
                { 14, new() { "Path Feature" } },
                { 15, new() { "Persistent Rage" } },
                { 16, new() { "Ability Score Improvement" } },
                { 17, new() { "Brutal Critical (3 dice)" } },
                { 18, new() { "Indomitable Might" } },
                { 19, new() { "Ability Score Improvement" } },
                { 20, new() { "Primal Champion" } }
            }
        },
        new CharacterClass { 
            Name = "Bard", 
            HitDie = "d8", 
            PrimaryAbilities = new() { "Charisma" }, 
            SavingThrows = new() { "Dexterity", "Charisma" }, 
            Skills = new() { "Any three" }, 
            Spellcaster = true,
            Features = new() {
                { 1, new() { "Spellcasting", "Bardic Inspiration" } },
                { 2, new() { "Jack of All Trades", "Song of Rest" } },
                { 3, new() { "Bard College", "Expertise" } },
                { 4, new() { "Ability Score Improvement" } },
                { 5, new() { "Bardic Inspiration (d8)", "Font of Inspiration", "3rd Level Spells" } },
                { 6, new() { "Countercharm", "Bard College Feature" } },
                { 7, new() { "4th Level Spells" } },
                { 8, new() { "Ability Score Improvement" } },
                { 9, new() { "Song of Rest (d8)", "5th Level Spells" } },
                { 10, new() { "Bardic Inspiration (d10)", "Expertise", "Magical Secrets" } },
                { 11, new() { "6th Level Spells" } },
                { 12, new() { "Ability Score Improvement" } },
                { 13, new() { "Song of Rest (d10)", "7th Level Spells" } },
                { 14, new() { "Magical Secrets", "Bard College Feature" } },
                { 15, new() { "Bardic Inspiration (d12)", "8th Level Spells" } },
                { 16, new() { "Ability Score Improvement" } },
                { 17, new() { "Song of Rest (d12)", "9th Level Spells" } },
                { 18, new() { "Magical Secrets" } },
                { 19, new() { "Ability Score Improvement" } },
                { 20, new() { "Superior Inspiration" } }
            }
        },
        new CharacterClass { 
            Name = "Druid", 
            HitDie = "d8", 
            PrimaryAbilities = new() { "Wisdom" }, 
            SavingThrows = new() { "Intelligence", "Wisdom" }, 
            Skills = new() { "Arcana", "Animal Handling", "Insight", "Medicine", "Nature", "Perception", "Religion", "Survival" }, 
            Spellcaster = true,
            Features = new() {
                { 1, new() { "Druidcraft", "Spellcasting" } },
                { 2, new() { "Wild Shape", "Druid Circle" } },
                { 3, new() { "2nd Level Spells" } },
                { 4, new() { "Wild Shape improvement", "Ability Score Improvement" } },
                { 5, new() { "3rd Level Spells" } },
                { 6, new() { "Druid Circle Feature" } },
                { 7, new() { "4th Level Spells" } },
                { 8, new() { "Wild Shape improvement", "Ability Score Improvement" } },
                { 9, new() { "5th Level Spells" } },
                { 10, new() { "Druid Circle Feature" } },
                { 11, new() { "6th Level Spells" } },
                { 12, new() { "Ability Score Improvement" } },
                { 13, new() { "7th Level Spells" } },
                { 14, new() { "Druid Circle Feature" } },
                { 15, new() { "8th Level Spells" } },
                { 16, new() { "Ability Score Improvement" } },
                { 17, new() { "9th Level Spells" } },
                { 18, new() { "Timeless Body", "Beast Spells" } },
                { 19, new() { "Ability Score Improvement" } },
                { 20, new() { "Archdruid" } }
            }
        },
        new CharacterClass { 
            Name = "Monk", 
            HitDie = "d8", 
            PrimaryAbilities = new() { "Dexterity", "Wisdom" }, 
            SavingThrows = new() { "Strength", "Dexterity" }, 
            Skills = new() { "Acrobatics", "Athletics", "History", "Insight", "Religion", "Stealth" },
            Features = new() {
                { 1, new() { "Unarmored Defense", "Martial Arts" } },
                { 2, new() { "Ki", "Unarmored Movement" } },
                { 3, new() { "Monastic Tradition", "Deflect Missiles" } },
                { 4, new() { "Ability Score Improvement", "Slow Fall" } },
                { 5, new() { "Extra Attack", "Stunning Strike" } },
                { 6, new() { "Ki-Empowered Strikes", "Monastic Tradition Feature" } },
                { 7, new() { "Evasion", "Stillness of Mind" } },
                { 8, new() { "Ability Score Improvement" } },
                { 9, new() { "Unarmored Movement improvement" } },
                { 10, new() { "Purity of Body" } },
                { 11, new() { "Monastic Tradition Feature" } },
                { 12, new() { "Ability Score Improvement" } },
                { 13, new() { "Tongue of the Sun and Moon" } },
                { 14, new() { "Diamond Soul" } },
                { 15, new() { "Timeless Body" } },
                { 16, new() { "Ability Score Improvement" } },
                { 17, new() { "Monastic Tradition Feature" } },
                { 18, new() { "Empty Body" } },
                { 19, new() { "Ability Score Improvement" } },
                { 20, new() { "Perfect Self" } }
            }
        },
        new CharacterClass { 
            Name = "Sorcerer", 
            HitDie = "d6", 
            PrimaryAbilities = new() { "Charisma" }, 
            SavingThrows = new() { "Constitution", "Charisma" }, 
            Skills = new() { "Arcana", "Deception", "Insight", "Intimidation", "Persuasion", "Religion" }, 
            Spellcaster = true,
            Features = new() {
                { 1, new() { "Spellcasting", "Sorcerous Origin" } },
                { 2, new() { "Font of Magic" } },
                { 3, new() { "Metamagic", "2nd Level Spells" } },
                { 4, new() { "Ability Score Improvement" } },
                { 5, new() { "3rd Level Spells" } },
                { 6, new() { "Sorcerous Origin Feature" } },
                { 7, new() { "4th Level Spells" } },
                { 8, new() { "Ability Score Improvement" } },
                { 9, new() { "5th Level Spells" } },
                { 10, new() { "Metamagic" } },
                { 11, new() { "6th Level Spells" } },
                { 12, new() { "Ability Score Improvement" } },
                { 13, new() { "7th Level Spells" } },
                { 14, new() { "Sorcerous Origin Feature" } },
                { 15, new() { "8th Level Spells" } },
                { 16, new() { "Ability Score Improvement" } },
                { 17, new() { "Metamagic", "9th Level Spells" } },
                { 18, new() { "Sorcerous Origin Feature" } },
                { 19, new() { "Ability Score Improvement" } },
                { 20, new() { "Sorcerous Restoration" } }
            }
        },
        new CharacterClass { 
            Name = "Warlock", 
            HitDie = "d8", 
            PrimaryAbilities = new() { "Charisma" }, 
            SavingThrows = new() { "Wisdom", "Charisma" }, 
            Skills = new() { "Arcana", "Deception", "History", "Intimidation", "Investigation", "Nature", "Religion" }, 
            Spellcaster = true,
            Features = new() {
                { 1, new() { "Otherworldly Patron", "Pact Magic" } },
                { 2, new() { "Eldritch Invocations" } },
                { 3, new() { "Pact Boon", "2nd Level Spells" } },
                { 4, new() { "Ability Score Improvement" } },
                { 5, new() { "3rd Level Spells" } },
                { 6, new() { "Otherworldly Patron Feature" } },
                { 7, new() { "4th Level Spells" } },
                { 8, new() { "Ability Score Improvement" } },
                { 9, new() { "5th Level Spells" } },
                { 10, new() { "Otherworldly Patron Feature" } },
                { 11, new() { "Mystic Arcanum (6th level)" } },
                { 12, new() { "Ability Score Improvement" } },
                { 13, new() { "Mystic Arcanum (7th level)" } },
                { 14, new() { "Otherworldly Patron Feature" } },
                { 15, new() { "Mystic Arcanum (8th level)" } },
                { 16, new() { "Ability Score Improvement" } },
                { 17, new() { "Mystic Arcanum (9th level)" } },
                { 18, new() { "Eldritch Master" } },
                { 19, new() { "Ability Score Improvement" } },
                { 20, new() { "Eldritch Master" } }
            }
        }
    };

    private static readonly List<string> _backgrounds = new()
    {
        "Acolyte", "Criminal", "Folk Hero", "Noble", "Sage", "Soldier", "Charlatan", "Entertainer", "Guild Artisan", "Hermit", "Outlander", "Sailor"
    };

    private static readonly List<string> _alignments = new()
    {
        "Lawful Good", "Neutral Good", "Chaotic Good", "Lawful Neutral", "True Neutral", "Chaotic Neutral", "Lawful Evil", "Neutral Evil", "Chaotic Evil"
    };

    private static readonly List<Weapon> _weapons = new()
    {
        // Simple Melee Weapons
        new Weapon { Name = "Club", Type = "Simple Melee", Damage = "1d4", DamageType = "Bludgeoning", Properties = "Light", Category = "Simple" },
        new Weapon { Name = "Dagger", Type = "Simple Melee", Damage = "1d4", DamageType = "Piercing", Properties = "Finesse, Light, Thrown (20/60)", Category = "Simple" },
        new Weapon { Name = "Handaxe", Type = "Simple Melee", Damage = "1d6", DamageType = "Slashing", Properties = "Light, Thrown (20/60)", Category = "Simple" },
        new Weapon { Name = "Javelin", Type = "Simple Melee", Damage = "1d6", DamageType = "Piercing", Properties = "Thrown (30/120)", Category = "Simple" },
        new Weapon { Name = "Mace", Type = "Simple Melee", Damage = "1d6", DamageType = "Bludgeoning", Properties = "", Category = "Simple" },
        new Weapon { Name = "Quarterstaff", Type = "Simple Melee", Damage = "1d6", DamageType = "Bludgeoning", Properties = "Versatile (1d8)", Category = "Simple" },
        new Weapon { Name = "Spear", Type = "Simple Melee", Damage = "1d6", DamageType = "Piercing", Properties = "Thrown (20/60), Versatile (1d8)", Category = "Simple" },
        
        // Simple Ranged Weapons
        new Weapon { Name = "Light Crossbow", Type = "Simple Ranged", Damage = "1d8", DamageType = "Piercing", Properties = "Ammunition (80/320), Loading, Two-handed", Category = "Simple" },
        new Weapon { Name = "Shortbow", Type = "Simple Ranged", Damage = "1d6", DamageType = "Piercing", Properties = "Ammunition (80/320), Two-handed", Category = "Simple" },
        new Weapon { Name = "Sling", Type = "Simple Ranged", Damage = "1d4", DamageType = "Bludgeoning", Properties = "Ammunition (30/120)", Category = "Simple" },
        
        // Martial Melee Weapons
        new Weapon { Name = "Battleaxe", Type = "Martial Melee", Damage = "1d8", DamageType = "Slashing", Properties = "Versatile (1d10)", Category = "Martial" },
        new Weapon { Name = "Greatsword", Type = "Martial Melee", Damage = "2d6", DamageType = "Slashing", Properties = "Heavy, Two-handed", Category = "Martial" },
        new Weapon { Name = "Longsword", Type = "Martial Melee", Damage = "1d8", DamageType = "Slashing", Properties = "Versatile (1d10)", Category = "Martial" },
        new Weapon { Name = "Rapier", Type = "Martial Melee", Damage = "1d8", DamageType = "Piercing", Properties = "Finesse", Category = "Martial" },
        new Weapon { Name = "Scimitar", Type = "Martial Melee", Damage = "1d6", DamageType = "Slashing", Properties = "Finesse, Light", Category = "Martial" },
        new Weapon { Name = "Shortsword", Type = "Martial Melee", Damage = "1d6", DamageType = "Piercing", Properties = "Finesse, Light", Category = "Martial" },
        new Weapon { Name = "Warhammer", Type = "Martial Melee", Damage = "1d8", DamageType = "Bludgeoning", Properties = "Versatile (1d10)", Category = "Martial" },
        new Weapon { Name = "Greataxe", Type = "Martial Melee", Damage = "1d12", DamageType = "Slashing", Properties = "Heavy, Two-handed", Category = "Martial" },
        new Weapon { Name = "Maul", Type = "Martial Melee", Damage = "2d6", DamageType = "Bludgeoning", Properties = "Heavy, Two-handed", Category = "Martial" },
        new Weapon { Name = "Pike", Type = "Martial Melee", Damage = "1d10", DamageType = "Piercing", Properties = "Heavy, Reach, Two-handed", Category = "Martial" },
        new Weapon { Name = "Glaive", Type = "Martial Melee", Damage = "1d10", DamageType = "Slashing", Properties = "Heavy, Reach, Two-handed", Category = "Martial" },
        
        // Martial Ranged Weapons
        new Weapon { Name = "Heavy Crossbow", Type = "Martial Ranged", Damage = "1d10", DamageType = "Piercing", Properties = "Ammunition (100/400), Heavy, Loading, Two-handed", Category = "Martial" },
        new Weapon { Name = "Longbow", Type = "Martial Ranged", Damage = "1d8", DamageType = "Piercing", Properties = "Ammunition (150/600), Heavy, Two-handed", Category = "Martial" }
    };

    private static readonly List<Armor> _armor = new()
    {
        // Light Armor
        new Armor { Name = "Padded", Type = "Light Armor", ArmorClass = 11, Properties = "Stealth Disadvantage", Category = "Light" },
        new Armor { Name = "Leather", Type = "Light Armor", ArmorClass = 11, Properties = "", Category = "Light" },
        new Armor { Name = "Studded Leather", Type = "Light Armor", ArmorClass = 12, Properties = "", Category = "Light" },
        
        // Medium Armor
        new Armor { Name = "Hide", Type = "Medium Armor", ArmorClass = 12, Properties = "", Category = "Medium" },
        new Armor { Name = "Chain Shirt", Type = "Medium Armor", ArmorClass = 13, Properties = "", Category = "Medium" },
        new Armor { Name = "Scale Mail", Type = "Medium Armor", ArmorClass = 14, Properties = "Stealth Disadvantage", Category = "Medium" },
        new Armor { Name = "Breastplate", Type = "Medium Armor", ArmorClass = 14, Properties = "", Category = "Medium" },
        new Armor { Name = "Half Plate", Type = "Medium Armor", ArmorClass = 15, Properties = "Stealth Disadvantage", Category = "Medium" },
        
        // Heavy Armor
        new Armor { Name = "Ring Mail", Type = "Heavy Armor", ArmorClass = 14, Properties = "Stealth Disadvantage", Category = "Heavy" },
        new Armor { Name = "Chain Mail", Type = "Heavy Armor", ArmorClass = 16, Properties = "Stealth Disadvantage", Category = "Heavy" },
        new Armor { Name = "Splint", Type = "Heavy Armor", ArmorClass = 17, Properties = "Stealth Disadvantage", Category = "Heavy" },
        new Armor { Name = "Plate", Type = "Heavy Armor", ArmorClass = 18, Properties = "Stealth Disadvantage", Category = "Heavy" },
        
        // Shields
        new Armor { Name = "Shield", Type = "Shield", ArmorClass = 2, Properties = "+2 AC", Category = "Shield" }
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
        
        // Calculate derived stats
        CalculateCharacterStats(character);
        
        // Generate backstory and description if not provided
        if (string.IsNullOrEmpty(character.Backstory))
            character.Backstory = GenerateBackstory(character);
        if (string.IsNullOrEmpty(character.PhysicalDescription))
            character.PhysicalDescription = GeneratePhysicalDescription(character);
        
        _characters.Add(character);
        
        // Log character to file
        LogCharacterToFile(character, "Created");
        
        return character;
    }

    [HttpPut("{id}")]
    public ActionResult UpdateCharacter(string id, [FromBody] Character character)
    {
        var existing = _characters.FirstOrDefault(c => c.Id == id);
        if (existing == null) return NotFound();
        
        character.Id = id;
        
        // Recalculate derived stats
        CalculateCharacterStats(character);
        
        var index = _characters.IndexOf(existing);
        _characters[index] = character;
        
        // Log character update to file
        LogCharacterToFile(character, "Updated");
        
        return Ok();
    }

    [HttpGet("class-features/{className}/{level}")]
    public ActionResult<List<string>> GetClassFeatures(string className, int level)
    {
        var characterClass = _classes.FirstOrDefault(c => c.Name.Equals(className, StringComparison.OrdinalIgnoreCase));
        if (characterClass == null)
            return NotFound($"Class '{className}' not found");

        var features = new List<string>();
        for (int i = 1; i <= level; i++)
        {
            if (characterClass.Features.ContainsKey(i))
            {
                features.AddRange(characterClass.Features[i]);
            }
        }

        return Ok(features);
    }

    private static void CalculateCharacterStats(Character character)
    {
        // Calculate ability modifiers
        int GetModifier(int score) => (score - 10) / 2;
        
        // Calculate Initiative (Dex modifier)
        character.Initiative = GetModifier(character.Abilities.Dexterity);
        
        // Calculate base AC (10 + Dex modifier, can be overridden by armor)
        if (character.ArmorClass == 0)
            character.ArmorClass = 10 + GetModifier(character.Abilities.Dexterity);
        
        // Calculate proficiency bonus based on level
        character.ProficiencyBonus = 2 + ((character.Level - 1) / 4);
        
        // Calculate saving throws (base modifier + proficiency if proficient)
        var classInfo = _classes.FirstOrDefault(c => c.Name == character.Class);
        
        // Populate class features based on level
        character.ClassFeatures.Clear();
        if (classInfo != null)
        {
            for (int i = 1; i <= character.Level; i++)
            {
                if (classInfo.Features.ContainsKey(i))
                {
                    character.ClassFeatures.AddRange(classInfo.Features[i]);
                }
            }
        }
        
        character.SavingThrows.Strength = GetModifier(character.Abilities.Strength) + 
            (classInfo?.SavingThrows.Contains("Strength") == true ? character.ProficiencyBonus : 0);
        character.SavingThrows.Dexterity = GetModifier(character.Abilities.Dexterity) + 
            (classInfo?.SavingThrows.Contains("Dexterity") == true ? character.ProficiencyBonus : 0);
        character.SavingThrows.Constitution = GetModifier(character.Abilities.Constitution) + 
            (classInfo?.SavingThrows.Contains("Constitution") == true ? character.ProficiencyBonus : 0);
        character.SavingThrows.Intelligence = GetModifier(character.Abilities.Intelligence) + 
            (classInfo?.SavingThrows.Contains("Intelligence") == true ? character.ProficiencyBonus : 0);
        character.SavingThrows.Wisdom = GetModifier(character.Abilities.Wisdom) + 
            (classInfo?.SavingThrows.Contains("Wisdom") == true ? character.ProficiencyBonus : 0);
        character.SavingThrows.Charisma = GetModifier(character.Abilities.Charisma) + 
            (classInfo?.SavingThrows.Contains("Charisma") == true ? character.ProficiencyBonus : 0);
        
        // Calculate HP based on class, level, constitution, and race
        if (character.HitPoints == 0 && classInfo != null)
        {
            int hitDie = int.Parse(classInfo.HitDie.Substring(1)); // Remove 'd' from "d10"
            int conModifier = GetModifier(character.Abilities.Constitution);
            
            // Racial HP bonuses
            int racialBonus = GetRacialHPBonus(character.Race);
            
            // Level 1: Max hit die + Con modifier + racial bonus
            // Additional levels: Average of hit die + Con modifier per level
            int baseHP = hitDie + conModifier + racialBonus;
            int additionalHP = (character.Level - 1) * ((hitDie / 2) + 1 + conModifier);
            
            character.HitPoints = baseHP + additionalHP;
        }
    }

    private static int GetRacialHPBonus(string race)
    {
        return race switch
        {
            "Dwarf" => 1,      // Hardy constitution
            "Half-Orc" => 1,   // Relentless endurance
            "Goliath" => 1,    // Stone's endurance
            "Dragonborn" => 1, // Draconic resilience
            _ => 0
        };
    }

    private static string GenerateBackstory(Character character)
    {
        var random = new Random();
        var stories = new Dictionary<string, Dictionary<string, List<string>>>
        {
            ["Fighter"] = new()
            {
                ["Human"] = new() { 
                    "Born in the farming village of Millhaven, you were the eldest of four children. When orc raiders began terrorizing the countryside, you took up your grandfather's sword and rallied the village militia. After successfully defending your home, a traveling knight named Sir Gareth recognized your natural leadership and tactical mind. He offered to train you in the ways of combat and chivalry. For three years, you learned swordplay, mounted combat, and the code of honor. Now you venture forth as a knight-errant, seeking to protect the innocent and prove yourself worthy of the title. Your family's farm prospers under your siblings' care, but they know you were meant for greater things than tilling soil.",
                    "You served as a city guard in the great port city of Seahaven for seven years, rising through the ranks to become a sergeant. Your unit was known for their discipline and effectiveness in keeping the peace among the diverse population of merchants, sailors, and travelers. Everything changed when corrupt officials began taking bribes from smugglers and pirates. When you refused to look the other way, you were framed for theft and barely escaped with your life. Now an exile from the only home you've known, you wander the roads as a sellsword, your reputation preceding you. Though bitter about the injustice, you've found freedom in your new life and a chance to fight for causes you truly believe in." 
                },
                ["Elf"] = new() { 
                    "For over a century, you served in the Silverleaf Guard, protectors of the ancient Moonwood Forest. Your keen eyes and steady sword arm made you a respected ranger among your people. You witnessed the slow encroachment of civilization, the felling of sacred groves, and the pollution of crystal streams. When a vision from the forest spirits showed you a great darkness spreading across the land, you knew your destiny lay beyond the woodland borders. Taking only your ancestral blade and a blessing from the Elder Council, you ventured into the world of shorter-lived races. Though you miss the eternal twilight of your forest home, you've found purpose in protecting the natural world wherever your travels take you.",
                    "Born into the noble House Starweaver, you were trained from childhood in the elegant combat arts of your people. Your family's ancient castle overlooked the Whispering Vale, where you learned to fight with grace and precision. When shadow creatures began emerging from a tear in reality near your ancestral lands, you led a company of elven warriors against them. Though victorious, the battle cost you many friends and left you questioning the isolationist ways of your people. Seeking to understand the threats facing all races, not just elves, you've taken up the life of an adventurer. Your noble bearing and centuries of training make you a formidable ally and a natural leader." 
                },
                ["Dwarf"] = new() { 
                    "Raised in the mountain stronghold of Ironpeak, you learned the warrior traditions of your clan from your father, a veteran of the Goblin Wars. Your people have defended the mountain passes for generations, and you proved yourself in countless skirmishes against orc raiders and giant spiders. When ancient dwarven ruins were discovered deep in the Underdark, your clan elders chose you to lead an expedition to reclaim them. The journey revealed not only lost treasures but also a growing threat of dark creatures stirring in the depths. Now you seek surface allies to help your people face this ancient evil. Your hammer bears the runes of your ancestors, and your beard is braided with the traditional knots of a mountain warrior.",
                    "As the youngest child of Clan Ironforge, you were expected to follow family tradition and become a master smith. While you learned the basics of metalworking, your heart yearned for adventure beyond the forge fires. When a dragon attacked the nearby human settlement of Goldbrook, you defied your family's wishes and joined the defense. Your courage in battle and skill with axe and shield earned you recognition, but also your father's disappointment. Choosing honor over family approval, you've set out to prove that a dwarf's worth isn't measured only by the quality of their craftsmanship, but by their deeds in defense of others." 
                }
            },
            ["Wizard"] = new()
            {
                ["Human"] = new() { 
                    "Your magical awakening came at age twelve when you accidentally set your family's barn on fire while trying to light a candle with your mind. Recognizing your potential, the village wise woman, Grandmother Willow, took you as her apprentice. For eight years, you studied the fundamental principles of magic in her tower library, learning to harness your raw talent through discipline and study. When she passed away, she left you her spellbook and a cryptic message about a 'great convergence' that would require your skills. Now you travel the world, seeking ancient knowledge and magical artifacts while trying to decipher her final prophecy. Your familiar, a raven named Quill, was her parting gift to you.",
                    "Born into a family of merchants, you showed an unusual aptitude for numbers and patterns that your parents hoped would serve you well in trade. However, your true calling revealed itself when you discovered a hidden cache of magical tomes in your family's warehouse. Self-taught through years of careful study and dangerous experimentation, you've mastered several schools of magic. Your unconventional education means you sometimes approach spellcasting in unorthodox ways, but your practical experience has made you resourceful and adaptable. You've left the family business to pursue magical research, though you maintain your merchant's eye for valuable opportunities." 
                },
                ["Elf"] = new() { 
                    "Born into the prestigious Moonwhisper family, you were enrolled in the Celestial Academy of Arcane Arts at the age of fifty—still a child by elven standards. For decades, you excelled in theoretical magic, particularly divination and enchantment. Your thesis on 'Temporal Resonance in Scrying Magic' earned you recognition among the academy's masters. However, your studies were interrupted when you experienced a prophetic vision of a great calamity threatening all the realms. The academy elders dismissed your vision as the product of an overactive imagination, but you knew better. Taking your most precious spellbooks and research notes, you've ventured into the wider world to prevent the disaster you foresaw.",
                    "As a member of the ancient Starweaver bloodline, magic flows through your veins like starlight. Your family has served as court wizards to the Elven High King for over a millennium, and you were groomed from birth to continue this tradition. Your specialty in illusion and transmutation magic made you invaluable in diplomatic missions and court intrigue. Everything changed when you uncovered evidence of a conspiracy against the throne involving several noble houses. Rather than become embroiled in elven politics, you chose exile, taking with you only your spellbook and your conscience. Now you use your considerable magical talents to help those who cannot help themselves." 
                },
                ["Gnome"] = new() { 
                    "Your insatiable curiosity about how magic works led to numerous 'accidents' in your childhood—including the infamous incident where you turned your neighbor's prized roses into singing mushrooms. Recognizing that formal training might prevent future disasters, your parents enrolled you in the Tinkertown Academy of Practical Magic. There, you excelled at combining magic with mechanical devices, creating wonderful and occasionally explosive inventions. Your graduation project, a self-stirring cauldron that could brew potions automatically, worked perfectly until it achieved sentience and tried to take over the academy. Now you travel the world, seeking new magical phenomena to study and hopefully avoiding any more sentient kitchen appliances." 
                }
            },
            ["Rogue"] = new()
            {
                ["Halfling"] = new() { 
                    "Growing up in the bustling trade city of Riverside, you learned early that small size and quick fingers could mean the difference between eating and going hungry. Your family ran a modest bakery, but when a corrupt tax collector began demanding impossible payments, you took matters into your own hands. Your first 'job' was liberating the tax money from the collector's strongbox and returning it to the struggling merchants of your district. Word of your skills spread, and soon you were running a network of information brokers and 'problem solvers.' When the city guard got too close to your operation, you decided it was time to see what opportunities awaited beyond the city walls. Your reputation as someone who helps the underdog has followed you into your new life as an adventurer.",
                    "Born into the Lightfoot clan, you were raised on stories of your ancestors' adventures and their legendary luck. Your natural stealth and nimble fingers made you an excellent scout for your extended family's traveling merchant caravan. You learned to read people's intentions, spot danger from afar, and find hidden paths through dangerous territory. When bandits attacked your caravan and killed several family members, including your beloved grandmother, you swore an oath of vengeance. After tracking down and dealing with the bandits' leader, you discovered a taste for justice that couldn't be satisfied by a peaceful merchant's life. Now you use your skills to protect other travelers and hunt down those who prey on the innocent." 
                },
                ["Human"] = new() { 
                    "The streets of Shadowport raised you after your parents died in a plague when you were eight. Taken in by the Crimson Daggers thieves' guild, you learned to survive through stealth, cunning, and quick reflexes. Your mentor, a grizzled halfling named Pip, taught you that the best thieves steal from those who can afford it and help those who can't. For years, you walked the line between criminal and folk hero, redistributing wealth from corrupt nobles to struggling families. When Pip was murdered by a rival guild, you realized the criminal life would eventually consume you. You've left the guild behind, but your skills in stealth, lockpicking, and reading people's motivations serve you well in your new career as an adventurer.",
                    "As a member of the King's Shadow Guard, you served as a spy and infiltrator for the crown, gathering intelligence on threats to the realm. Your missions took you from noble courts to criminal underworlds, and you became skilled at adopting different identities and blending into any social situation. Your world shattered when you discovered that your handler was selling state secrets to enemy nations. Framed as a traitor when you tried to expose the truth, you barely escaped execution. Now a fugitive from the very kingdom you served, you've learned to trust only yourself. Your skills in deception and combat serve you well, but you're always looking over your shoulder for agents of your former employers." 
                },
                ["Elf"] = new() { 
                    "Trained as a scout for the Silverleaf Rangers, you spent decades patrolling the borders of the great forest, watching for threats to your people. Your keen senses and natural stealth made you invaluable for reconnaissance missions deep into hostile territory. You witnessed the gradual encroachment of civilization on the natural world and the corruption it brought with it. When you discovered that some of your own people were secretly trading with slavers and poachers, you took it upon yourself to gather evidence. Your investigation revealed a conspiracy that reached the highest levels of elven society. Rather than see your people torn apart by scandal, you chose exile, but not before ensuring the guilty parties faced justice. Now you use your ranger skills to protect the innocent, regardless of their race." 
                }
            }
        };
        
        var classStories = stories.GetValueOrDefault(character.Class, new());
        var raceStories = classStories.GetValueOrDefault(character.Race, new() { 
            $"You are a {character.Race.ToLower()} {character.Class.ToLower()} with a complex past that has shaped you into the person you are today. Born into a world of conflict and opportunity, you've learned to rely on your skills and instincts to survive. Your journey has taken you far from your origins, and you've seen both the best and worst that civilization has to offer. Whether driven by a desire for justice, knowledge, power, or redemption, you've chosen the dangerous path of an adventurer. Your unique background gives you insights that others lack, and your determination has carried you through challenges that would break lesser individuals. Now you stand ready to face whatever dangers and opportunities await, armed with hard-won experience and an unshakeable resolve to forge your own destiny in a world that rarely offers second chances." 
        });
        
        return raceStories[random.Next(raceStories.Count)];
    }

    private static string GeneratePhysicalDescription(Character character)
    {
        var random = new Random();
        var raceDescriptions = new Dictionary<string, List<string>>
        {
            ["Human"] = new() { "Average height with weathered features and determined eyes.", "Tall and lean with calloused hands from hard work.", "Stocky build with kind eyes and a ready smile." },
            ["Elf"] = new() { "Graceful and tall with pointed ears and ethereal beauty.", "Slender frame with silver hair and piercing green eyes.", "Elegant bearing with delicate features and ancient wisdom in their gaze." },
            ["Dwarf"] = new() { "Short and sturdy with a magnificent braided beard.", "Broad shoulders and strong arms, with intricate tattoos.", "Compact frame with bright eyes and a booming laugh." },
            ["Halfling"] = new() { "Small and cheerful with curly hair and bare feet.", "Round face with rosy cheeks and twinkling eyes.", "Diminutive stature but with surprising courage in their stance." },
            ["Dragonborn"] = new() { "Tall and imposing with scales that shimmer in the light.", "Draconic features with a powerful build and noble bearing.", "Reptilian appearance with fierce eyes and a commanding presence." },
            ["Gnome"] = new() { "Small with a large nose and mischievous grin.", "Tiny frame but with bright, intelligent eyes full of curiosity.", "Compact build with wild hair and ink-stained fingers." },
            ["Half-Elf"] = new() { "Human height with subtle elven features and grace.", "Caught between two worlds, with both human warmth and elven elegance.", "Tall and lean with slightly pointed ears and expressive eyes." },
            ["Half-Orc"] = new() { "Large frame with prominent tusks and green-tinged skin.", "Imposing stature but with surprisingly gentle eyes.", "Muscular build with orcish features softened by human heritage." },
            ["Tiefling"] = new() { "Devilish appearance with horns and a long tail.", "Exotic beauty with red skin and glowing eyes.", "Infernal heritage evident in their sharp features and confident demeanor." }
        };
        
        var defaultDescriptions = new List<string> { "Distinctive appearance that draws attention.", "Memorable features that reflect their unique heritage.", "Striking presence that commands respect." };
        var descriptions = raceDescriptions.GetValueOrDefault(character.Race, defaultDescriptions);
        return descriptions[random.Next(descriptions.Count)];
    }

    private static void LogCharacterToFile(Character character, string action)
    {
        try
        {
            var logEntry = $"\n### {character.Name ?? "Unnamed Character"} - {action} on {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n" +
                          $"- **Race**: {character.Race}\n" +
                          $"- **Class**: {character.Class}\n" +
                          $"- **Level**: {character.Level}\n" +
                          $"- **Background**: {character.Background}\n" +
                          $"- **Alignment**: {character.Alignment}\n" +
                          $"- **Ability Scores**: STR {character.Abilities.Strength} DEX {character.Abilities.Dexterity} CON {character.Abilities.Constitution} INT {character.Abilities.Intelligence} WIS {character.Abilities.Wisdom} CHA {character.Abilities.Charisma}\n" +
                          $"- **Spells**: {string.Join(", ", character.Spells)}\n" +
                          $"- **Equipment**: {string.Join(", ", character.Equipment.Select(e => e.Name))}\n" +
                          $"- **Character ID**: {character.Id}\n\n---\n";

            System.IO.File.AppendAllText("SAVED CHARACTERS.md", logEntry);
        }
        catch (Exception ex)
        {
            // Log error but don't fail the character creation
            Console.WriteLine($"Failed to log character: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteCharacter(string id)
    {
        var character = _characters.FirstOrDefault(c => c.Id == id);
        if (character == null)
            return NotFound();

        _characters.RemoveAll(c => c.Id == id);
        
        // Remove from saved characters file
        try
        {
            var filePath = "SAVED CHARACTERS.md";
            if (System.IO.File.Exists(filePath))
            {
                var content = System.IO.File.ReadAllText(filePath);
                var lines = content.Split('\n').ToList();
                
                // Find and remove the character entry
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Contains($"**Character ID**: {id}"))
                    {
                        // Remove from character name line backwards to separator
                        int startIndex = i;
                        while (startIndex > 0 && !lines[startIndex - 1].StartsWith("## "))
                            startIndex--;
                        
                        // Remove to next separator or end
                        int endIndex = i + 1;
                        while (endIndex < lines.Count && !lines[endIndex].StartsWith("---"))
                            endIndex++;
                        if (endIndex < lines.Count) endIndex++; // Include separator
                        
                        lines.RemoveRange(startIndex, endIndex - startIndex);
                        break;
                    }
                }
                
                System.IO.File.WriteAllText(filePath, string.Join('\n', lines));
            }
        }
        catch (Exception ex)
        {
            // Log error but don't fail the delete operation
            Console.WriteLine($"Error removing character from file: {ex.Message}");
        }
        
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

    [HttpGet("weapons")]
    public ActionResult<List<Weapon>> GetWeapons() => _weapons;

    [HttpGet("armor")]
    public ActionResult<List<Armor>> GetArmor() => _armor;

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

    private static readonly Dictionary<string, List<string>> _raceNames = new()
    {
        ["Human"] = new() { "Aerdrie", "Ahvak", "Aramil", "Berris", "Cithreth", "Dayereth", "Enna", "Galinndan", "Hadarai", "Halimath", "Heian", "Himo", "Immeral", "Ivellios", "Korfel", "Lamlis", "Laucian", "Mindartis", "Naal", "Nutae", "Paelynn", "Peren", "Quarion", "Riardon", "Rolen", "Silvyr", "Suhnab", "Thamior", "Theriatis", "Therivan", "Uthemar", "Vanuath", "Varis" },
        ["Elf"] = new() { "Adran", "Aelar", "Aramil", "Aranea", "Berris", "Dayereth", "Enna", "Galinndan", "Hadarai", "Immeral", "Ivellios", "Laucian", "Mindartis", "Naal", "Nutae", "Paelynn", "Peren", "Quarion", "Riardon", "Rolen", "Silvyr", "Suhnab", "Thamior", "Theriatis", "Therivan", "Uthemar", "Vanuath", "Varis", "Adrie", "Althaea", "Anastrianna", "Andraste", "Antinua", "Bethrynna", "Birel", "Caelynn", "Dara" },
        ["Dwarf"] = new() { "Adrik", "Alberich", "Baern", "Barendd", "Brottor", "Bruenor", "Dain", "Darrak", "Delg", "Eberk", "Einkil", "Fargrim", "Flint", "Gardain", "Harbek", "Kildrak", "Morgran", "Orsik", "Oskar", "Rangrim", "Rurik", "Taklinn", "Thoradin", "Thorek", "Thorfin", "Tordek", "Traubon", "Travok", "Ulfgar", "Veit", "Vondal", "Amber", "Bardryn", "Diesa", "Eldeth", "Gunnloda", "Greta", "Helja", "Hlin", "Kathra", "Kristryd", "Ilde", "Liftrasa", "Mardred", "Riswynn", "Sannl", "Torbera", "Torgga", "Vistra" },
        ["Halfling"] = new() { "Alton", "Ander", "Cade", "Corrin", "Eldon", "Errich", "Finnan", "Garret", "Lindal", "Lyle", "Merric", "Milo", "Osborn", "Perrin", "Reed", "Roscoe", "Wellby", "Andry", "Chenna", "Dee", "Euphemia", "Jillian", "Kithri", "Lavinia", "Lidda", "Merla", "Nedda", "Paela", "Portia", "Seraphina", "Shaena", "Trym", "Vani", "Verna" },
        ["Dragonborn"] = new() { "Arjhan", "Balasar", "Bharash", "Donaar", "Ghesh", "Heskan", "Kriv", "Medrash", "Mehen", "Nadarr", "Pandjed", "Patrin", "Rhogar", "Shamash", "Shedinn", "Tarhun", "Torinn", "Akra", "Biri", "Daar", "Farideh", "Harann", "Havilar", "Jheri", "Kava", "Korinn", "Mishann", "Nala", "Perra", "Raiann", "Sora", "Surina", "Thava", "Uadjit" },
        ["Gnome"] = new() { "Alston", "Alvyn", "Boddynock", "Brocc", "Burgell", "Dimble", "Eldon", "Erky", "Fonkin", "Frug", "Gerbo", "Gimble", "Glim", "Jebeddo", "Kellen", "Namfoodle", "Orryn", "Roondar", "Seebo", "Sindri", "Warryn", "Wrenn", "Zook", "Bimpnottin", "Breena", "Caramip", "Carlin", "Donella", "Duvamil", "Ella", "Ellyjoybell", "Ellywick", "Lilli", "Loopmottin", "Lorilla", "Mardnab", "Nissa", "Nyx", "Oda", "Orla", "Roywyn", "Shamil", "Tana", "Waywocket", "Zanna" },
        ["Half-Elf"] = new() { "Abel", "Caleb", "Corwin", "Felix", "Immerial", "Lamlis", "Mindartis", "Naal", "Nutae", "Paelynn", "Peren", "Quarion", "Riardon", "Rolen", "Silvyr", "Suhnab", "Thamior", "Theriatis", "Therivan", "Uthemar", "Vanuath", "Varis", "Adrie", "Althaea", "Anastrianna", "Andraste", "Antinua", "Bethrynna", "Birel", "Caelynn", "Dara" },
        ["Half-Orc"] = new() { "Dench", "Feng", "Gell", "Henk", "Holg", "Imsh", "Keth", "Krusk", "Mhurren", "Ront", "Shump", "Thokk", "Baggi", "Emen", "Engong", "Kansif", "Myev", "Neega", "Ovak", "Ownka", "Shautha", "Sutha", "Vola", "Volen", "Yevelda" },
        ["Tiefling"] = new() { "Akmenos", "Amnon", "Barakas", "Damakos", "Ekemon", "Iados", "Kairon", "Leucis", "Melech", "Mordai", "Morthos", "Pelaios", "Skamos", "Therai", "Akta", "Anakir", "Bryseis", "Criella", "Damaia", "Ea", "Kallista", "Lerissa", "Makaria", "Nemeia", "Orianna", "Phelaia", "Rieta" },
        ["Aarakocra"] = new() { "Aera", "Aial", "Aur", "Deekek", "Errk", "Heehk", "Ikki", "Kleeck", "Oorr", "Ouss", "Quaf", "Quierk", "Salleek", "Urreek", "Zeed" },
        ["Aasimar"] = new() { "Aritian", "Beltin", "Cernan", "Cronwier", "Eran", "Ilamin", "Maudril", "Okrin", "Parant", "Tural", "Wyran", "Zaigan", "Arken", "Arsinoe", "Davina", "Drinma", "Imesah", "Lamlis", "Laumei", "Mialee", "Nijena", "Nutae", "Paelynn", "Peren", "Quarion", "Selise", "Silvyr" },
        ["Bugbear"] = new() { "Batbat", "Drubbus", "Ghukliak", "Ghurran", "Grandar", "Hruggek", "Klarg", "Korga", "Krusk", "Lugdush", "Mauhur", "Mhurren", "Muzgash", "Ront", "Shump", "Thokk", "Ugreth", "Ushak" },
        ["Firbolg"] = new() { "Adalbern", "Aerdrie", "Ahvak", "Aramil", "Aranea", "Berris", "Cithreth", "Dayereth", "Enna", "Galinndan", "Hadarai", "Halimath", "Heian", "Himo", "Immeral", "Ivellios", "Korfel", "Lamlis", "Laucian", "Mindartis", "Naal", "Nutae", "Paelynn", "Peren", "Quarion", "Riardon", "Rolen", "Silvyr", "Suhnab", "Thamior", "Theriatis", "Therivan", "Uthemar", "Vanuath", "Varis" },
        ["Genasi"] = new() { "Abyss", "Arc", "Blaze", "Cinder", "Crystal", "Dust", "Ember", "Flame", "Flow", "Gust", "Haze", "Igneous", "Jet", "Kindle", "Lava", "Mist", "Obsidian", "Pebble", "Quartz", "Reed", "Salt", "Scorch", "Steam", "Tempest", "Torrent", "Umbra", "Vapor", "Wave", "Zephyr" },
        ["Githyanki"] = new() { "Baeloth", "Dak'kon", "Gish", "Githzerai", "Kith'rak", "Lae'zel", "Orpheus", "Qudenos", "Sarth", "Taman", "Vlaakith", "Zerthimon" },
        ["Goblin"] = new() { "Booyahg", "Droop", "Eek", "Fik", "Gleep", "Greez", "Hurk", "Klarg", "Meepo", "Nix", "Obb", "Poog", "Quet", "Runt", "Snik", "Tig", "Urt", "Yeek", "Zook" },
        ["Goliath"] = new() { "Aukan", "Eglath", "Gae-Al", "Gauthak", "Ilikan", "Keothi", "Kuori", "Lo-Kag", "Manneo", "Maveith", "Nalla", "Orilo", "Paavu", "Pethani", "Thalai", "Thotham", "Uthal", "Vaunea", "Vimak" },
        ["Hobgoblin"] = new() { "Aruget", "Bertak", "Dargol", "Dencig", "Feng", "Gell", "Henk", "Holg", "Imsh", "Keth", "Krusk", "Mhurren", "Ront", "Shump", "Thokk" },
        ["Kenku"] = new() { "Booming", "Bunny", "Croaking", "Drumming", "Echo", "Flapper", "Hammer", "Ink", "Jinx", "Kick", "Laugh", "Mimic", "Nod", "Oink", "Peck", "Quill", "Rustle", "Screech", "Thud", "Whistle" },
        ["Kobold"] = new() { "Arix", "Eks", "Ett", "Galax", "Garu", "Hakka", "Irtos", "Kashak", "Krik", "Lamlis", "Mepo", "Molo", "Ohsoss", "Rotom", "Sagin", "Sik", "Sniv", "Taklak", "Tes", "Urak", "Varn", "Vex", "Yik" },
        ["Lizardfolk"] = new() { "Achuak", "Aryte", "Baeshra", "Darastrix", "Garurt", "Ihtos", "Kepesk", "Mirik", "Obash", "Pras", "Semuanya", "Sess", "Shedinn", "Thessarian", "Trisk", "Usk", "Valignat", "Verthisathurgiesh", "Wuuthrad", "Xanka" },
        ["Orc"] = new() { "Dench", "Feng", "Gell", "Henk", "Holg", "Imsh", "Keth", "Krusk", "Mhurren", "Ront", "Shump", "Thokk", "Ugreth", "Ushak", "Baggi", "Emen", "Engong", "Kansif", "Myev", "Neega", "Ovak", "Ownka", "Shautha", "Sutha", "Vola", "Volen", "Yevelda" },
        ["Tabaxi"] = new() { "Cloud on the Mountaintop", "Five Timber", "Jade Shoe", "Left-Handed Hummingbird", "Seven Thundercloud", "Skirt of Snakes", "Smoking Mirror", "Two Sticks", "Distant Rain", "Falling Star", "Laughing Brook", "Nimble Fingers", "Quick Blade", "Singing Flame", "Swift River", "Wandering Star" },
        ["Tortle"] = new() { "Aranck", "Bambosk", "Damu", "Gar", "Gura", "Ini", "Jappa", "Kinlek", "Krull", "Lim", "Lop", "Nortle", "Nulka", "Olo", "Plonk", "Quee", "Rattle", "Shappa", "Slyp", "Ubo", "Uhok", "Wabu", "Xelbuk", "Yog" },
        ["Triton"] = new() { "Aryn", "Belthyn", "Delnis", "Duthyn", "Feloren", "Otanyn", "Shalryn", "Vlaryn", "Wolyn", "Zunis", "Arista", "Belthyris", "Dara", "Halryn", "Kailani", "Lyr", "Moana", "Naida", "Nerida", "Thalassa" },
        ["Yuan-ti Pureblood"] = new() { "Asutali", "Eztli", "Hessatal", "Hitotee", "Issahu", "Itstli", "Manuya", "Meztli", "Nesalli", "Otlaca", "Shalkashlah", "Sisava", "Sitlali", "Soakosh", "Ssimalli", "Suisatal", "Talash", "Teoshi", "Tlazohtlala", "Yaotal", "Yaxkin", "Zyanya" }
    };

    [HttpPost("generate-name/{race}")]
    public ActionResult<string> GenerateName(string race)
    {
        if (!_raceNames.ContainsKey(race))
            return BadRequest("Race not found");

        var random = new Random();
        var names = _raceNames[race];
        return names[random.Next(names.Count)];
    }
}
