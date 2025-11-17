# D&D 5e Character Builder

A comprehensive web-based character builder for Dungeons & Dragons 5th Edition, built with C# ASP.NET Core and React.

## Features

### Core Character Creation
- **Race Selection**: Choose from 9 core D&D races (Human, Elf, Dwarf, Halfling, Dragonborn, Gnome, Half-Elf, Half-Orc, Tiefling)
- **Class Selection**: All 12 core classes (Fighter, Wizard, Rogue, Cleric, Ranger, Paladin, Barbarian, Bard, Druid, Monk, Sorcerer, Warlock)
- **Background Selection**: 12 classic backgrounds (Acolyte, Criminal, Folk Hero, Noble, Sage, Soldier, etc.)
- **Alignment Selection**: All 9 D&D alignments
- **Level Progression**: Character levels 1-20

### Ability Score Management
- **Manual Entry**: Set ability scores manually (3-20 range)
- **Random Generation**: 4d6 drop lowest method
- **Modifier Calculation**: Automatic modifier calculation and display
- **Race Bonuses**: Automatic application of racial ability score increases

### Spell Management
- **Spell Library**: Curated list of D&D 5e spells with full descriptions
- **Spellcaster Detection**: Automatic spell access based on class selection
- **Spell Details**: Level, school, casting time, range, duration, and description
- **Custom Spell Lists**: Add/remove spells for each character

### Equipment & Inventory
- **Equipment Management**: Add custom equipment with quantities
- **Item Categories**: Organize equipment by type
- **Inventory Tracking**: Keep track of all character possessions

### Character Sheet Display
- **Complete Character Sheet**: View all character information in organized format
- **Health & AC Tracking**: Hit points and armor class display
- **Proficiency Bonus**: Automatic calculation based on level
- **Character Export**: Save and load characters

## Technology Stack

### Backend (C# ASP.NET Core)
- **.NET 9.0**: Latest .NET framework
- **ASP.NET Core Web API**: RESTful API endpoints
- **In-Memory Storage**: Character data persistence during session
- **Model-Based Architecture**: Clean separation of concerns

### Frontend (React)
- **React 18**: Modern React with hooks
- **Responsive Design**: Mobile-friendly interface
- **Component-Based**: Modular UI components
- **Real-Time Updates**: Instant character updates

## API Endpoints

### Character Management
- `GET /api/character` - Get all characters
- `GET /api/character/{id}` - Get specific character
- `POST /api/character` - Create new character
- `PUT /api/character/{id}` - Update character
- `DELETE /api/character/{id}` - Delete character

### Game Data
- `GET /api/character/races` - Get all races with traits
- `GET /api/character/classes` - Get all classes with details
- `GET /api/character/backgrounds` - Get all backgrounds
- `GET /api/character/alignments` - Get all alignments
- `GET /api/character/spells` - Get spell library

### Utilities
- `POST /api/character/generate-stats` - Generate random ability scores

## Installation & Setup

### Prerequisites
- .NET 9.0 SDK
- Node.js (for React frontend)
- Git

### Running the Application
1. Clone the repository
2. Navigate to the project directory
3. Restore .NET packages: `dotnet restore`
4. Install React dependencies: `cd ClientApp && npm install`
5. Run the application: `dotnet run`
6. Access at `http://localhost:8000`

## Project Structure

```
character-builder/
├── Controllers/           # API Controllers
│   └── CharacterController.cs
├── Models/               # Data Models
│   └── Character.cs
├── ClientApp/            # React Frontend
│   ├── src/
│   │   ├── App.js       # Main React Component
│   │   ├── App.css      # Styling
│   │   └── index.js     # React Entry Point
│   ├── public/
│   │   └── index.html   # HTML Template
│   └── package.json     # React Dependencies
├── Program.cs            # Application Entry Point
├── CharacterBuilder.csproj # Project Configuration
└── README.md            # This file
```

## Character Creation Workflow

1. **Basic Information**: Enter name, select race, class, background, alignment, and level
2. **Ability Scores**: Generate random stats or enter manually
3. **Spell Selection**: Choose spells (for spellcasting classes)
4. **Equipment**: Add weapons, armor, and other gear
5. **Save Character**: Store character for future use
6. **View Character Sheet**: See complete character information

## D&D 5e Data Included

### Races (9)
- Human, Elf, Dwarf, Halfling, Dragonborn, Gnome, Half-Elf, Half-Orc, Tiefling
- Each with racial traits and ability score increases

### Classes (12)
- Fighter, Wizard, Rogue, Cleric, Ranger, Paladin, Barbarian, Bard, Druid, Monk, Sorcerer, Warlock
- Hit dice, primary abilities, saving throws, and skill proficiencies

### Spells
- Cantrips and leveled spells
- Complete spell descriptions with mechanics

## Contributing

This is a personal project for D&D character creation. Feel free to fork and modify for your own campaigns!

## License

This project is for educational and personal use. D&D 5e content is property of Wizards of the Coast.
