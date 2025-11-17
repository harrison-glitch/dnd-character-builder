import React, { useState, useEffect } from 'react';
import './App.css';

function App() {
  const [characters, setCharacters] = useState([]);
  const [currentCharacter, setCurrentCharacter] = useState(null);
  const [races, setRaces] = useState([]);
  const [classes, setClasses] = useState([]);
  const [backgrounds, setBackgrounds] = useState([]);
  const [alignments, setAlignments] = useState([]);
  const [spells, setSpells] = useState([]);
  const [view, setView] = useState('list');

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    const [charactersRes, racesRes, classesRes, backgroundsRes, alignmentsRes, spellsRes] = await Promise.all([
      fetch('/api/character'),
      fetch('/api/character/races'),
      fetch('/api/character/classes'),
      fetch('/api/character/backgrounds'),
      fetch('/api/character/alignments'),
      fetch('/api/character/spells')
    ]);

    setCharacters(await charactersRes.json());
    setRaces(await racesRes.json());
    setClasses(await classesRes.json());
    setBackgrounds(await backgroundsRes.json());
    setAlignments(await alignmentsRes.json());
    setSpells(await spellsRes.json());
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
      hitPoints: 0,
      armorClass: 10,
      proficiencyBonus: 2
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

  const generateStats = async () => {
    const response = await fetch('/api/character/generate-stats', { method: 'POST' });
    const stats = await response.json();
    setCurrentCharacter({ ...currentCharacter, abilities: stats });
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

  const addEquipment = () => {
    const name = prompt('Equipment name:');
    if (name) {
      setCurrentCharacter({
        ...currentCharacter,
        equipment: [...currentCharacter.equipment, { name, type: 'Item', quantity: 1, description: '' }]
      });
    }
  };

  if (view === 'list') {
    return (
      <div className="App">
        <header>
          <h1>D&D 5e Character Builder</h1>
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
          </div>

          <div className="spells">
            <h2>Spells</h2>
            <ul>
              {currentCharacter.spells.map((spell, index) => (
                <li key={index}>{spell}</li>
              ))}
            </ul>
          </div>

          <div className="equipment">
            <h2>Equipment</h2>
            <ul>
              {currentCharacter.equipment.map((item, index) => (
                <li key={index}>{item.name} ({item.quantity})</li>
              ))}
            </ul>
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
          <input
            type="text"
            placeholder="Character Name"
            value={currentCharacter.name}
            onChange={(e) => setCurrentCharacter({ ...currentCharacter, name: e.target.value })}
          />
          
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
                <p>{spell.description}</p>
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
          <h2>Equipment</h2>
          <button onClick={addEquipment}>Add Equipment</button>
          <ul>
            {currentCharacter.equipment.map((item, index) => (
              <li key={index}>
                {item.name} ({item.quantity})
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
