# Dialogue System

A flexible and extensible dialogue system for Unity that features a typewriter effect, inline token support for interactive prompts, input device detection, and complete demo scenes with playable characters and interactive NPCs.

## Overview

This dialogue system is designed to be easy to integrate into any Unity project while providing a robust foundation for managing character conversations, dialogue UI, and player interactions. It supports multiple input methods (keyboard, mouse, Xbox, PlayStation gamepads) and dynamically displays the appropriate interaction prompts based on the detected input device.

## Features

### Core Features

- **Dialogue Manager**: Central system for managing all dialogue interactions and serving as a bridge between the dialogue system and other game systems
- **Typewriter Effect**: Smooth, character-by-character text reveal with adjustable speed and audio feedback
- **Input Device Detection**: Automatically detects and switches between keyboard/mouse (with customizable icon sets) and various gamepads (Xbox, PlayStation)
- **Inline Tokens**: Support for special tokens like `[interact]`, `[next]`, and `[skip]` that are replaced with context-aware glyphs based on the detected input method.
  Easy to adde more!
- **Glyph Mapping**: Data structure (`SO_TokenMap`) for seamlessly mapping raw tokens to sprite assets
- **Character Information System**: Stores and displays character icons, names, and colors with smooth transitions

### UI and Visual Features

- Smooth fade-in and fade-out animations for dialogue windows
- Character icon and background color display
- Dialogue text area with TextMesh Pro integration
- Interactive buttons for continuing and skipping dialogue

### Demo Scenes

- **Simple Demo** (`Demo.unity`): Basic testing scene for the dialogue system without character movement
- **Extended Demo** (`ExtendedDemo.unity`): Full-featured demo with:
  - Playable character with movement and animation controls
  - Multiple interactive NPCs (Skeleton, Zombie)
  - Interaction detection and prompts
  - Character collision and movement

## How to run the project

- Clone the repository `git clone git@github.com:Filippooss/Dialogue-System.git`
- Download the 6000.3.9.f1 version of unity throw the official Unity Hub
- Open the project from Unity Hub
- Open and run the ExtendedDemo or Demo scene

## Project Structure

```
Assets/
├── Scripts/
│   ├── Dialogue System/          # Core dialogue system scripts
│   │   ├── DialogueManager.cs    # Main dialogue manager
│   │   ├── DialogueUI.cs         # UI controller with typewriter effect
│   │   ├── SO_DialogueData.cs    # Dialogue data structure
│   │   ├── SO_TokenMap.cs        # Token to glyph mapping
│   │   └── ScriptableObjects     # Dialogue assets
|   ├── ExtendedDemo/
|   │   ├── Player.cs             # Player movement and interaction script
│   |   ├── NPC.cs                # NPC interaction script
│   │   └── InteractionPrompt.cs  # Simple script for world interaction prompt
│   ├── Input/                    # Input handling system
│   │   ├── InputReader.cs        # Input device detection and event broadcasting
│   │   ├── E_InputMethod.cs      # Input method enumeration
│   │   └── GameInput.cs          # Generated input action map
│   ├── UI/                       # UI components
│   │   ├── CustomButton.cs       # Custom button implementation
│   │   └── IInputChange.cs       # Interface for input-aware UI
│   ├── CharacterInfo.cs          # Character data structure
│   └── ...
├── Scenes/
│   ├── Demo.unity                # Simple testing demo
│   └── ExtendedDemo.unity        # Full-featured demo scene
├── ScriptableObjects/
│   ├── Characters/               # Character assets (Skeleton, Zombie, Keeper)
│   └── InputReader.asset         # Input system configuration
└── ...
```

## Key Components

### DialogueManager

Singleton manager that coordinates dialogue playback and acts as a bridge between the dialogue system and other game systems. Handles input events and manages the dialogue lifecycle.

### DialogueUI

Controls all visual aspects of dialogue display including:

- Typewriter effect with configurable character reveal speed
- Punctuation delays for natural reading pacing
- Audio feedback during typing
- Dynamic sprite asset switching based on input method
- Token parsing and glyph replacement

### InputReader

Detects the active input device and broadcasts input events:

- Mouse/Keyboard detection
- Xbox Gamepad detection
- PlayStation DualShock detection
- Generic Gamepad fallback
- Events: `ContinueEvent`, `SkipEvent`, `InputControllerChangeEvent`

### SO_TokenMap

Maps inline tokens to sprite assets:

```csharp
[interact]  → Display interact icon
[next]      → Display next icon
[skip]      → Display skip icon
```

These tokens are replaced with sprite glyphs from a `TMP_SpriteAsset` that corresponds to the current input method.

### SO_DialogueData

Scriptable object containing a conversation as a list of `CharacterLine` entries:

```csharp
public class CharacterLine
{
    public string Line;                    // Dialogue text with optional tokens
    public CharacterInfo CharacterInfo;    // Speaker information
}
```

## Inline Tokens

The dialogue system supports inline tokens that are replaced with context-aware glyphs:

- **[interact]**: Displays the interaction button (E on keyboard, X on Xbox, Square on PlayStation)
- **[continue]**: Displays the continue button
- **[skip]**: Displays the skip button

Example dialogues:

```
"Press [interact] to pick up the sword!"
"Click [next] to continue."
"Press [skip] to skip this dialogue."
```

## Usage

### Setting Up a Dialogue

1. Create a `SO_DialogueData` asset:
   - Right-click in Project → Create → Scriptable Objects/Dialogue System/DialogueModel

2. Create character assets (if needed):
   - Right-click in Project → Create → Scriptable Objects/CharacterInfo

3. Add dialogue lines:
   - Assign characters to each line
   - Write dialogue text with optional inline tokens
   - Use TextArea for multi-line dialogue

### Playing Dialogue

```csharp
DialogueManager.Instance.PlayDialogue(dialogueData, () =>
{
    // Callback when dialogue finishes
    Debug.Log("Dialogue finished!");
});
```

### Input Controls

- **Continue**: Press Space or A to continue to the next line
- **Skip**: Press Tab or RB to skip entire dialogue
- **Interact** (in extended demo): Press E or X to interact with NPCs
- **Move** (in extended demo): Use WASD or left stick to move the character

### Customizing the Typewriter Effect

In the DialogueUI component inspector:

- **Char Reveal Speed**: Time between character reveals (lower = faster)
- **Punctuation Wait Time**: Delay after punctuation marks
- **Punctuation Chars**: Characters that trigger delays (default: ",.:")

### Changing Sprite Assets by Input Method

Configure sprite assets in DialogueUI:

- Add entries to the Sprite Asset Pack List
- Assign TMP_SpriteAsset for each input method
- System automatically switches when input device changes

## Extended Demo Features

### Player System

- **Movement**: Use WASD keys or left stick to move
- **Interaction**: Press E or X button to interact with nearby NPCs
- **Detection Radius**: 2 units (configurable)
- Smooth character rotation and animation blending

### NPC System

- **Interaction Prompts**: Shows "[interact]" prompt when player is in range
- **Dialogue Integration**: Plays dialogue from assigned SO_DialogueData
- **Input Disabling**: Prevents player movement during dialogue
- **Multiple NPCs**: Skeleton, Zombie with unique dialogues

## Audio

The typewriter effect includes audio feedback:

- Random typing sounds play as characters are revealed
- Audio is played through the Audio Channel Event

## Advanced Customization

### Creating Custom Input Methods

Edit `E_InputMethod.cs` to add new input methods:

```csharp
public enum E_InputMethod
{
    MouseKeyboard,
    PlayStation,
    Xbox,
    Gamepad,
    YourCustomMethod  // Add here
}
```

### Extending the Dialogue System

The system is designed for extension:

- Implement `IInputChange` interface for input-aware components
- Listen to `InputControllerChangeEvent` for input method changes
- Extend `SO_DialogueData` for custom dialogue properties
- Create custom `SO_GameAction` derived classes for special actions

## Limitations

- System cannot detect combined punctuation characters like "...". Write now the system will detect them separately, which will result in 3 wait times

## Dependencies

- Unity's Input System (new Input System)
- TextMesh Pro

## Extra

Emphasis was given to the Dialogue System, anything else like Player Controller is designed to support and demonstrate the key futures of the system, they are not represent in any way a final game product.

## Credits

UI icons and 3D assets are from Kenney's store: https://kenney.nl/assets/category:Audio
Halloween font from Google Fonts: https://fonts.google.com/
