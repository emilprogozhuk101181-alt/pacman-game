# Pac-Man Game in Unity 6.5

A complete, fully functional Pac-Man game built from scratch in Unity 6.5.

## Features

✅ **Classic Pac-Man Gameplay**
- 28×31 tile-based maze (classic size)
- Pellet collection and scoring
- Power-up pellets with frightened mode
- 3 lives system with ghost respawning
- Win/Game Over conditions

✅ **4 Ghost AI Systems**
- **Blinky (Red)** - Direct chaser
- **Pinky (Pink)** - Ambush targeting
- **Inky (Cyan)** - Unpredictable behavior
- **Clyde (Orange)** - Chase/Scatter switching
- Chase and scatter modes with alternating patterns
- Frightened mode when eating power-ups

✅ **Complete Game Systems**
- Grid-based smooth movement
- Wall collision detection
- Tunnel wrapping at maze edges
- Score tracking and UI
- Lives management
- Level completion detection

## How to Run

1. Clone this repository
2. Open in Unity 6.5
3. Load the scene from `Assets/Scenes/PacmanGame.unity`
4. Press Play!

## Controls

- **Arrow Keys** - Move Pac-Man
- **Arrow Keys while running** - Queue next direction

## Game Mechanics

### Scoring
- Regular Pellet: 10 points
- Power Pellet: 50 points
- Eaten Ghost: 200 points

### Win Conditions
- Collect all pellets to win the level
- Get caught 3 times to lose

### Power-Ups
- Power Pellets make ghosts vulnerable
- Frightened mode lasts 8 seconds
- Eat ghosts during this time for bonus points

## Project Structure

```
Assets/
├── Scripts/
│   ├── GameManager.cs        # Main game controller
│   ├── MazeGenerator.cs      # Maze layout and spawning
│   ├── PacmanController.cs   # Player movement and input
│   ├── GhostController.cs    # Ghost AI behaviors
│   ├── Pellet.cs             # Pellet collection logic
│   └── PowerPellet.cs        # Power-up logic
├── Prefabs/
│   ├── Wall.prefab           # Wall tile prefab
│   ├── Pellet.prefab         # Regular pellet prefab
│   └── PowerPellet.prefab    # Power pellet prefab
└── Scenes/
    └── PacmanGame.unity      # Main game scene
```

## Future Enhancements

- Custom textures and sprites
- Multiple levels with increasing difficulty
- Sound effects and music
- High score leaderboard
- Different maze designs
- Smoother animations

## License

Free to use and modify!
