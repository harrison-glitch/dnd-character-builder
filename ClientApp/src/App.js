import React, { useState, useEffect } from 'react';
import './App.css';

function App() {
  const [characters, setCharacters] = useState([]);
  const [currentCharacter, setCurrentCharacter] = useState(null);
  const [tempHP, setTempHP] = useState(0);
  const [races, setRaces] = useState([]);
  const [classes, setClasses] = useState([]);
  const [backgrounds, setBackgrounds] = useState([]);
  const [alignments, setAlignments] = useState([]);
  const [spells, setSpells] = useState([]);
  const [weapons, setWeapons] = useState([]);
  const [armor, setArmor] = useState([]);
  const [view, setView] = useState('list');

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    const [charactersRes, racesRes, classesRes, backgroundsRes, alignmentsRes, spellsRes, weaponsRes, armorRes] = await Promise.all([
      fetch('/api/character'),
      fetch('/api/character/races'),
      fetch('/api/character/classes'),
      fetch('/api/character/backgrounds'),
      fetch('/api/character/alignments'),
      fetch('/api/character/spells'),
      fetch('/api/character/weapons'),
      fetch('/api/character/armor')
    ]);

    setCharacters(await charactersRes.json());
    setRaces(await racesRes.json());
    setClasses(await classesRes.json());
    setBackgrounds(await backgroundsRes.json());
    setAlignments(await alignmentsRes.json());
    setSpells(await spellsRes.json());
    setWeapons(await weaponsRes.json());
    setArmor(await armorRes.json());
  };

  const createNewCharacter = () => {
    setCurrentCharacter({
      name: '',
      race: '',
      class: '',
      background: '',
      alignment: '',
      level: 1,
      abilities: { strength: 10, dexterity: 10, constitution: 10, intelligence: 10, wisdom: 10, charisma: 10 },
      spells: [],
      equipment: [],
      classFeatures: [],
      hitPoints: 0,
      armorClass: 10,
      proficiencyBonus: 2,
      initiative: 0,
      savingThrows: { strength: 0, dexterity: 0, constitution: 0, intelligence: 0, wisdom: 0, charisma: 0 },
      backstory: '',
      physicalDescription: ''
    });
    setView('create');
  };

  const saveCharacter = async () => {
    if (currentCharacter.id) {
      await fetch(`/api/character/${currentCharacter.id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(currentCharacter)
      });
    } else {
      await fetch('/api/character', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(currentCharacter)
      });
    }
    fetchData();
    setView('list');
  };

  const updateCharacterHealth = async (newHP) => {
    const updatedCharacter = { ...currentCharacter, hitPoints: newHP };
    setCurrentCharacter(updatedCharacter);
    
    if (currentCharacter.id) {
      await fetch(`/api/character/${currentCharacter.id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(updatedCharacter)
      });
    }
  };

  const generateStats = async () => {
    const response = await fetch('/api/character/generate-stats', { method: 'POST' });
    const stats = await response.json();
    setCurrentCharacter({ ...currentCharacter, abilities: stats });
  };

  const getClassAbilities = (className, level) => {
    const abilities = {
      Fighter: [
        { name: "Fighting Style", description: "Choose a fighting style: Archery, Defense, Dueling, Great Weapon Fighting, Protection, or Two-Weapon Fighting.", level: 1 },
        { name: "Second Wind", description: "Regain 1d10 + Fighter level hit points as a bonus action. Recharges on short or long rest.", level: 1 },
        { name: "Action Surge", description: "Take one additional action on your turn. Recharges on short or long rest.", level: 2 },
        { name: "Martial Archetype", description: "Choose your martial archetype: Champion, Battle Master, or Eldritch Knight.", level: 3 },
        { name: "Ability Score Improvement", description: "Increase one ability score by 2, or two ability scores by 1 each.", level: 4 },
        { name: "Extra Attack", description: "Attack twice, instead of once, when you take the Attack action on your turn.", level: 5 }
      ],
      Wizard: [
        { name: "Spellcasting", description: "Cast wizard spells using Intelligence as your spellcasting ability.", level: 1 },
        { name: "Arcane Recovery", description: "Recover some expended spell slots during a short rest.", level: 1 },
        { name: "Arcane Tradition", description: "Choose your arcane tradition: School of Evocation, Abjuration, or Divination.", level: 2 },
        { name: "Cantrip Formulas", description: "Replace one wizard cantrip with another from the wizard spell list.", level: 3 },
        { name: "Ability Score Improvement", description: "Increase one ability score by 2, or two ability scores by 1 each.", level: 4 }
      ],
      Rogue: [
        { name: "Expertise", description: "Double your proficiency bonus for two skills of your choice.", level: 1 },
        { name: "Sneak Attack", description: "Deal extra 1d6 damage when you have advantage or an ally is within 5 feet of target.", level: 1 },
        { name: "Thieves' Cant", description: "Know thieves' cant, a secret mix of dialect, jargon, and code.", level: 1 },
        { name: "Cunning Action", description: "Use Dash, Disengage, or Hide as a bonus action.", level: 2 },
        { name: "Roguish Archetype", description: "Choose your archetype: Thief, Assassin, or Arcane Trickster.", level: 3 },
        { name: "Ability Score Improvement", description: "Increase one ability score by 2, or two ability scores by 1 each.", level: 4 }
      ],
      Cleric: [
        { name: "Spellcasting", description: "Cast cleric spells using Wisdom as your spellcasting ability.", level: 1 },
        { name: "Divine Domain", description: "Choose a divine domain: Life, Light, Knowledge, Nature, Tempest, Trickery, or War.", level: 1 },
        { name: "Channel Divinity", description: "Channel divine energy to fuel magical effects, including Turn Undead.", level: 2 },
        { name: "Ability Score Improvement", description: "Increase one ability score by 2, or two ability scores by 1 each.", level: 4 },
        { name: "Destroy Undead", description: "When you successfully turn undead, creatures of CR 1/2 or lower are destroyed.", level: 5 }
      ],
      Barbarian: [
        { name: "Rage", description: "Enter a rage as a bonus action. Gain damage resistance and bonus damage. 2 uses per long rest.", level: 1 },
        { name: "Unarmored Defense", description: "While not wearing armor, AC equals 10 + Dex modifier + Con modifier.", level: 1 },
        { name: "Reckless Attack", description: "Gain advantage on melee weapon attacks, but enemies have advantage against you.", level: 2 },
        { name: "Danger Sense", description: "Advantage on Dexterity saving throws against effects you can see.", level: 2 },
        { name: "Primal Path", description: "Choose your primal path: Path of the Berserker or Path of the Totem Warrior.", level: 3 },
        { name: "Ability Score Improvement", description: "Increase one ability score by 2, or two ability scores by 1 each.", level: 4 }
      ],
      Ranger: [
        { name: "Favored Enemy", description: "Choose a favored enemy type. Gain bonuses when tracking and fighting them.", level: 1 },
        { name: "Natural Explorer", description: "Choose a favored terrain. Gain benefits when traveling in that terrain.", level: 1 },
        { name: "Fighting Style", description: "Choose a fighting style: Archery, Defense, Dueling, or Two-Weapon Fighting.", level: 2 },
        { name: "Spellcasting", description: "Cast ranger spells using Wisdom as your spellcasting ability.", level: 2 },
        { name: "Ranger Archetype", description: "Choose your archetype: Hunter or Beast Master.", level: 3 },
        { name: "Ability Score Improvement", description: "Increase one ability score by 2, or two ability scores by 1 each.", level: 4 }
      ]
    };

    const classAbilities = abilities[className] || [];
    return classAbilities.filter(ability => ability.level <= level);
  };

  const getModifier = (score) => Math.floor((score - 10) / 2);

  const addSpell = (spell) => {
    if (!currentCharacter.spells.includes(spell.name)) {
      setCurrentCharacter({
        ...currentCharacter,
        spells: [...currentCharacter.spells, spell.name]
      });
    }
  };

  const generateName = async () => {
    if (!currentCharacter.race) {
      alert('Please select a race first');
      return;
    }
    
    const response = await fetch(`/api/character/generate-name/${currentCharacter.race}`, { method: 'POST' });
    const name = await response.text();
    setCurrentCharacter({ ...currentCharacter, name: name.replace(/"/g, '') });
  };

  const [featureDescriptions] = useState({
    // Fighter Features
    "Fighting Style": "Choose a fighting style: Archery, Defense, Dueling, Great Weapon Fighting, Protection, or Two-Weapon Fighting.",
    "Second Wind": "Regain 1d10 + fighter level hit points as a bonus action once per short rest.",
    "Action Surge": "Take one additional action on your turn once per short rest.",
    "Martial Archetype": "Choose your archetype: Champion, Battle Master, or Eldritch Knight.",
    "Ability Score Improvement": "Increase one ability score by 2, or two ability scores by 1 each.",
    "Extra Attack": "Attack twice when you take the Attack action.",
    "Indomitable": "Reroll a failed saving throw once per long rest.",
    
    // Wizard Features
    "Spellcasting": "Cast spells using Intelligence as your spellcasting ability.",
    "Arcane Recovery": "Recover some expended spell slots during a short rest.",
    "Arcane Tradition": "Choose your school of magic specialization.",
    "Cantrip Formulas": "Replace one cantrip you know with another cantrip from the wizard spell list.",
    "Spell Mastery": "Choose a 1st-level and 2nd-level spell to cast at will.",
    "Signature Spells": "Choose two 3rd-level spells as signature spells.",
    
    // Rogue Features
    "Expertise": "Double your proficiency bonus for two skills of your choice.",
    "Sneak Attack": "Deal extra damage when you have advantage or an ally is nearby.",
    "Thieves' Cant": "Secret language known by rogues and criminals.",
    "Cunning Action": "Use Dash, Disengage, or Hide as a bonus action.",
    "Uncanny Dodge": "Halve damage from one attack per turn using your reaction.",
    "Evasion": "Take no damage on successful Dex saves, half damage on failures.",
    "Reliable Talent": "Treat d20 rolls of 9 or lower as 10 for ability checks you're proficient in.",
    
    // Common Features
    "3rd Level Spells": "Gain access to 3rd-level spells.",
    "4th Level Spells": "Gain access to 4th-level spells.",
    "5th Level Spells": "Gain access to 5th-level spells.",
    "6th Level Spells": "Gain access to 6th-level spells.",
    "7th Level Spells": "Gain access to 7th-level spells.",
    "8th Level Spells": "Gain access to 8th-level spells.",
    "9th Level Spells": "Gain access to 9th-level spells."
  });

  const deleteCharacter = async (id) => {
    if (window.confirm('Are you sure you want to delete this character?')) {
      await fetch(`/api/character/${id}`, { method: 'DELETE' });
      fetchData();
    }
  };

  const addEquipment = () => {
    const name = prompt('Equipment name:');
    if (name) {
      setCurrentCharacter({
        ...currentCharacter,
        equipment: [...currentCharacter.equipment, { name, type: 'Item', quantity: 1, description: '' }]
      });
    }
  };

  const addWeapon = (weapon) => {
    const weaponEquipment = {
      name: weapon.name,
      type: 'Weapon',
      quantity: 1,
      description: `${weapon.damage} ${weapon.damageType} damage. ${weapon.properties}`,
      damage: weapon.damage,
      properties: weapon.properties
    };
    setCurrentCharacter({
      ...currentCharacter,
      equipment: [...currentCharacter.equipment, weaponEquipment]
    });
  };

  const addArmor = (armorItem) => {
    const armorEquipment = {
      name: armorItem.name,
      type: 'Armor',
      quantity: 1,
      description: `AC ${armorItem.armorClass}. ${armorItem.properties}`,
      armorClass: armorItem.armorClass,
      properties: armorItem.properties
    };
    setCurrentCharacter({
      ...currentCharacter,
      equipment: [...currentCharacter.equipment, armorEquipment],
      armorClass: armorItem.category === 'Shield' ? currentCharacter.armorClass + armorItem.armorClass : armorItem.armorClass + Math.floor((currentCharacter.abilities.dexterity - 10) / 2)
    });
  };

  if (view === 'list') {
    return (
      <div className="App">
        <header>
          <h1>D&D 5e Character Builder</h1>
          <small style={{color: '#666', fontSize: '12px'}}>Version 2.1.0 - Nov 2025</small>
          <button onClick={createNewCharacter}>Create New Character</button>
        </header>
        
        <div className="character-list">
          {characters.map(char => (
            <div key={char.id} className="character-card">
              <h3>{char.name || 'Unnamed Character'}</h3>
              <p>Level {char.level} {char.race} {char.class}</p>
              <p>{char.alignment}</p>
              <button onClick={() => { setCurrentCharacter(char); setView('view'); }}>View</button>
              <button onClick={() => { setCurrentCharacter(char); setView('create'); }}>Edit</button>
              <button onClick={() => deleteCharacter(char.id)} className="delete-btn">Delete</button>
              <small style={{display: 'block', color: '#666'}}>v2025.11.20</small>
            </div>
          ))}
        </div>
      </div>
    );
  }

  if (view === 'view') {
    return (
      <div className="App">
        <header>
          <h1>{currentCharacter.name} - Character Sheet</h1>
          <button onClick={() => setView('list')}>Back to List</button>
          <button onClick={() => setView('create')}>Edit Character</button>
        </header>

        <div className="character-sheet">
          <div className="basic-info">
            <h2>Basic Information</h2>
            <p><strong>Race:</strong> {currentCharacter.race}</p>
            <p><strong>Class:</strong> {currentCharacter.class}</p>
            <p><strong>Background:</strong> {currentCharacter.background}</p>
            <p><strong>Alignment:</strong> {currentCharacter.alignment}</p>
            <p><strong>Level:</strong> {currentCharacter.level}</p>
            <p><strong>Hit Points:</strong> {currentCharacter.hitPoints}</p>
            <p><strong>Armor Class:</strong> {currentCharacter.armorClass}</p>
            <p><strong>Initiative:</strong> {currentCharacter.initiative >= 0 ? '+' : ''}{currentCharacter.initiative}</p>
            <p><strong>Proficiency Bonus:</strong> +{currentCharacter.proficiencyBonus}</p>
          </div>

          <div className="saving-throws">
            <h2>Saving Throws</h2>
            <p><strong>Strength:</strong> {currentCharacter.savingThrows?.strength >= 0 ? '+' : ''}{currentCharacter.savingThrows?.strength || 0}</p>
            <p><strong>Dexterity:</strong> {currentCharacter.savingThrows?.dexterity >= 0 ? '+' : ''}{currentCharacter.savingThrows?.dexterity || 0}</p>
            <p><strong>Constitution:</strong> {currentCharacter.savingThrows?.constitution >= 0 ? '+' : ''}{currentCharacter.savingThrows?.constitution || 0}</p>
            <p><strong>Intelligence:</strong> {currentCharacter.savingThrows?.intelligence >= 0 ? '+' : ''}{currentCharacter.savingThrows?.intelligence || 0}</p>
            <p><strong>Wisdom:</strong> {currentCharacter.savingThrows?.wisdom >= 0 ? '+' : ''}{currentCharacter.savingThrows?.wisdom || 0}</p>
            <p><strong>Charisma:</strong> {currentCharacter.savingThrows?.charisma >= 0 ? '+' : ''}{currentCharacter.savingThrows?.charisma || 0}</p>
          </div>

          <div className="abilities">
            <h2>Ability Scores</h2>
            <div className="ability-grid">
              {Object.entries(currentCharacter.abilities).map(([ability, score]) => (
                <div key={ability} className="ability-score">
                  <h4>{ability.charAt(0).toUpperCase() + ability.slice(1)}</h4>
                  <div className="score">{score}</div>
                  <div className="modifier">{getModifier(score) >= 0 ? '+' : ''}{getModifier(score)}</div>
                </div>
              ))}
            </div>
            
            <div className="combat-stats">
              <div className="health-section">
                <h3>Health</h3>
                <div className="health-bars">
                  <div className="health-bar-container">
                    <label>Current HP:</label>
                    <div className="health-bar">
                      <div 
                        className="health-fill current" 
                        style={{width: `${(currentCharacter.hitPoints / Math.max(currentCharacter.hitPoints, 1)) * 100}%`}}
                      ></div>
                      <span className="health-text">{currentCharacter.hitPoints}</span>
                    </div>
                    <input
                      type="number"
                      value={currentCharacter.hitPoints}
                      onChange={(e) => updateCharacterHealth(parseInt(e.target.value) || 0)}
                      className="health-input"
                      min="0"
                    />
                  </div>
                  <div className="health-bar-container">
                    <label>Max HP:</label>
                    <div className="health-bar">
                      <div className="health-fill max" style={{width: '100%'}}></div>
                      <span className="health-text">{currentCharacter.hitPoints}</span>
                    </div>
                    <span className="health-display">{currentCharacter.hitPoints}</span>
                  </div>
                  <div className="health-bar-container">
                    <label>Temp HP:</label>
                    <div className="health-bar">
                      <div className="health-fill temp" style={{width: tempHP > 0 ? '100%' : '0%'}}></div>
                      <span className="health-text">{tempHP}</span>
                    </div>
                    <input
                      type="number"
                      value={tempHP}
                      onChange={(e) => setTempHP(parseInt(e.target.value) || 0)}
                      className="health-input"
                      min="0"
                    />
                  </div>
                </div>
              </div>
              
              <div className="ac-section">
                <h3>Armor Class</h3>
                <div className="ac-display">
                  <div className="ac-value">{currentCharacter.armorClass}</div>
                  <div className="ac-label">AC</div>
                </div>
              </div>
            </div>
          </div>

          <div className="character-details">
            <h2>Character Details</h2>
            <div className="backstory">
              <h3>Backstory</h3>
              <p>{currentCharacter.backstory || 'No backstory provided.'}</p>
            </div>
            <div className="physical-description">
              <h3>Physical Description</h3>
              <p>{currentCharacter.physicalDescription || 'No physical description provided.'}</p>
            </div>
          </div>

          <div className="class-abilities">
            <h2>Class Abilities</h2>
            <div className="abilities-list">
              {getClassAbilities(currentCharacter.class, currentCharacter.level).map((ability, index) => (
                <div key={index} className="ability-item">
                  <h4>{ability.name}</h4>
                  <p>{ability.description}</p>
                </div>
              ))}
            </div>
          </div>

          <div className="spells">
            <h2>Spells</h2>
            <div className="spell-details-list">
              {currentCharacter.spells.map((spellName, index) => {
                const spell = spells.find(s => s.name === spellName);
                return spell ? (
                  <div key={index} className="selected-spell">
                    <h4>{spell.name}</h4>
                    <div className="spell-stats">
                      <p><strong>Level:</strong> {spell.level === 0 ? 'Cantrip' : spell.level}</p>
                      <p><strong>School:</strong> {spell.school}</p>
                      <p><strong>Casting Time:</strong> {spell.castingTime}</p>
                      <p><strong>Range:</strong> {spell.range}</p>
                      <p><strong>Duration:</strong> {spell.duration}</p>
                      <p><strong>Damage:</strong> {spell.damage}</p>
                      <p><strong>Spell Slot:</strong> {spell.spellSlot}</p>
                      <p><strong>Components:</strong> {spell.components}</p>
                    </div>
                    <p className="spell-description">{spell.description}</p>
                  </div>
                ) : (
                  <div key={index} className="selected-spell">
                    <h4>{spellName}</h4>
                    <p>Spell details not found</p>
                  </div>
                );
              })}
            </div>
          </div>
        </div>

        <div className="equipment">
          <h2>Equipment ({currentCharacter.equipment ? currentCharacter.equipment.length : 0} items)</h2>
          <div className="equipment-details-list">
            {currentCharacter.equipment && currentCharacter.equipment.map((item, index) => (
              <div key={index} className="selected-equipment">
                <h4>{item.name} ({item.quantity})</h4>
                <div className="equipment-stats">
                  <p><strong>Type:</strong> {item.type}</p>
                  <p><strong>Damage:</strong> {item.damage || 'N/A'}</p>
                  <p><strong>AC:</strong> {item.armorClass || 'N/A'}</p>
                  <p><strong>Properties:</strong> {item.properties || 'N/A'}</p>
                  <p><strong>Description:</strong> {item.description || 'N/A'}</p>
                </div>
              </div>
            ))}
          </div>
        </div>

        <div className="class-features">
          <h2>Class Features ({currentCharacter.classFeatures ? currentCharacter.classFeatures.length : 0} features)</h2>
          <div className="features-list">
            {currentCharacter.classFeatures && currentCharacter.classFeatures.map((feature, index) => (
              <div key={index} className="feature-item">
                <h4>{feature}</h4>
                {featureDescriptions[feature] && (
                  <p>{featureDescriptions[feature]}</p>
                )}
              </div>
            ))}
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="App">
      <header>
        <h1>{currentCharacter.id ? 'Edit Character' : 'Create Character'}</h1>
        <button onClick={() => setView('list')}>Back to List</button>
        <button onClick={saveCharacter}>Save Character</button>
      </header>

      <div className="character-form">
        <div className="form-section">
          <h2>Basic Information</h2>
          <div className="name-input-group">
            <input
              type="text"
              placeholder="Character Name"
              value={currentCharacter.name}
              onChange={(e) => setCurrentCharacter({ ...currentCharacter, name: e.target.value })}
            />
            <button type="button" onClick={generateName}>Generate Name</button>
          </div>
          
          <select
            value={currentCharacter.race}
            onChange={(e) => setCurrentCharacter({ ...currentCharacter, race: e.target.value })}
          >
            <option value="">Select Race</option>
            {races.map(race => <option key={race.name} value={race.name}>{race.name}</option>)}
          </select>

          <select
            value={currentCharacter.class}
            onChange={(e) => setCurrentCharacter({ ...currentCharacter, class: e.target.value })}
          >
            <option value="">Select Class</option>
            {classes.map(cls => <option key={cls.name} value={cls.name}>{cls.name}</option>)}
          </select>

          <select
            value={currentCharacter.background}
            onChange={(e) => setCurrentCharacter({ ...currentCharacter, background: e.target.value })}
          >
            <option value="">Select Background</option>
            {backgrounds.map(bg => <option key={bg} value={bg}>{bg}</option>)}
          </select>

          <select
            value={currentCharacter.alignment}
            onChange={(e) => setCurrentCharacter({ ...currentCharacter, alignment: e.target.value })}
          >
            <option value="">Select Alignment</option>
            {alignments.map(align => <option key={align} value={align}>{align}</option>)}
          </select>

          <input
            type="number"
            min="1"
            max="20"
            value={currentCharacter.level}
            onChange={(e) => setCurrentCharacter({ ...currentCharacter, level: parseInt(e.target.value) })}
          />
        </div>

        <div className="form-section">
          <h2>Ability Scores</h2>
          <button onClick={generateStats}>Generate Random Stats</button>
          <div className="ability-inputs">
            {Object.entries(currentCharacter.abilities).map(([ability, score]) => (
              <div key={ability}>
                <label>{ability.charAt(0).toUpperCase() + ability.slice(1)}</label>
                <input
                  type="number"
                  min="3"
                  max="20"
                  value={score}
                  onChange={(e) => setCurrentCharacter({
                    ...currentCharacter,
                    abilities: { ...currentCharacter.abilities, [ability]: parseInt(e.target.value) }
                  })}
                />
                <span>({getModifier(score) >= 0 ? '+' : ''}{getModifier(score)})</span>
              </div>
            ))}
          </div>
        </div>

        <div className="form-section">
          <h2>Spells</h2>
          <div className="spell-list">
            {spells.map(spell => (
              <div key={spell.name} className="spell-item">
                <h4>{spell.name}</h4>
                <div className="spell-details">
                  <p><strong>Level:</strong> {spell.level === 0 ? 'Cantrip' : spell.level}</p>
                  <p><strong>School:</strong> {spell.school}</p>
                  <p><strong>Casting Time:</strong> {spell.castingTime}</p>
                  <p><strong>Range:</strong> {spell.range}</p>
                  <p><strong>Duration:</strong> {spell.duration}</p>
                  <p><strong>Damage:</strong> {spell.damage}</p>
                  <p><strong>Spell Slot:</strong> {spell.spellSlot}</p>
                  <p><strong>Components:</strong> {spell.components}</p>
                  <p><strong>Description:</strong> {spell.description}</p>
                </div>
                <button onClick={() => addSpell(spell)}>Add Spell</button>
              </div>
            ))}
          </div>
          <h3>Selected Spells:</h3>
          <ul>
            {currentCharacter.spells.map((spell, index) => (
              <li key={index}>
                {spell}
                <button onClick={() => setCurrentCharacter({
                  ...currentCharacter,
                  spells: currentCharacter.spells.filter((_, i) => i !== index)
                })}>Remove</button>
              </li>
            ))}
          </ul>
        </div>

        <div className="form-section">
          <h2>Character Details</h2>
          <textarea
            placeholder="Backstory (leave blank for auto-generated)"
            value={currentCharacter.backstory}
            onChange={(e) => setCurrentCharacter({ ...currentCharacter, backstory: e.target.value })}
            rows="4"
          />
          <textarea
            placeholder="Physical Description (leave blank for auto-generated)"
            value={currentCharacter.physicalDescription}
            onChange={(e) => setCurrentCharacter({ ...currentCharacter, physicalDescription: e.target.value })}
            rows="3"
          />
        </div>

        <div className="form-section">
          <h2>Equipment</h2>
          
          <div className="equipment-dropdowns">
            <div className="dropdown-group">
              <label>Add Weapon:</label>
              <select onChange={(e) => {
                if (e.target.value) {
                  const weapon = weapons.find(w => w.name === e.target.value);
                  if (weapon) addWeapon(weapon);
                  e.target.value = '';
                }
              }}>
                <option value="">Select Weapon</option>
                <optgroup label="Simple Weapons">
                  {weapons.filter(w => w.category === 'Simple').map(weapon => (
                    <option key={weapon.name} value={weapon.name}>
                      {weapon.name} ({weapon.damage} {weapon.damageType})
                    </option>
                  ))}
                </optgroup>
                <optgroup label="Martial Weapons">
                  {weapons.filter(w => w.category === 'Martial').map(weapon => (
                    <option key={weapon.name} value={weapon.name}>
                      {weapon.name} ({weapon.damage} {weapon.damageType})
                    </option>
                  ))}
                </optgroup>
              </select>
            </div>

            <div className="dropdown-group">
              <label>Add Armor:</label>
              <select onChange={(e) => {
                if (e.target.value) {
                  const armorItem = armor.find(a => a.name === e.target.value);
                  if (armorItem) addArmor(armorItem);
                  e.target.value = '';
                }
              }}>
                <option value="">Select Armor</option>
                <optgroup label="Light Armor">
                  {armor.filter(a => a.category === 'Light').map(armorItem => (
                    <option key={armorItem.name} value={armorItem.name}>
                      {armorItem.name} (AC {armorItem.armorClass} + Dex)
                    </option>
                  ))}
                </optgroup>
                <optgroup label="Medium Armor">
                  {armor.filter(a => a.category === 'Medium').map(armorItem => (
                    <option key={armorItem.name} value={armorItem.name}>
                      {armorItem.name} (AC {armorItem.armorClass} + Dex max 2)
                    </option>
                  ))}
                </optgroup>
                <optgroup label="Heavy Armor">
                  {armor.filter(a => a.category === 'Heavy').map(armorItem => (
                    <option key={armorItem.name} value={armorItem.name}>
                      {armorItem.name} (AC {armorItem.armorClass})
                    </option>
                  ))}
                </optgroup>
                <optgroup label="Shield">
                  {armor.filter(a => a.category === 'Shield').map(armorItem => (
                    <option key={armorItem.name} value={armorItem.name}>
                      {armorItem.name} (+{armorItem.armorClass} AC)
                    </option>
                  ))}
                </optgroup>
              </select>
            </div>

            <div className="dropdown-group">
              <button onClick={addEquipment}>Add Custom Equipment</button>
            </div>
          </div>

          <h3>Current Equipment:</h3>
          <ul>
            {currentCharacter.equipment.map((item, index) => (
              <li key={index}>
                <strong>{item.name}</strong> ({item.quantity}) - {item.type}
                {item.damage && <span> - Damage: {item.damage}</span>}
                {item.armorClass && <span> - AC: {item.armorClass}</span>}
                <button onClick={() => setCurrentCharacter({
                  ...currentCharacter,
                  equipment: currentCharacter.equipment.filter((_, i) => i !== index)
                })}>Remove</button>
              </li>
            ))}
          </ul>
        </div>
      </div>
    </div>
  );
}

export default App;
