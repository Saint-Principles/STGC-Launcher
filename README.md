## Description

**STGC Launcher**

This project is a launcher application for the game Slendytubbies Guardian Collection. It provides a user-friendly interface to download, update, configure, and launch the game with various customization options.

### Key Features:
- **Game Management**: Download, install, update, and launch Slendytubbies Guardian Collection
- **Auto-Updates**: Automatic checking for game and launcher updates
- **Multi-language Support**: English and Russian localization with easy extension
- **Customizable Settings**: Configure graphics quality, resolution, mouse sensitivity, and installation folder
- **News Integration**: Display game news
- **Font Customization**: Support for custom fonts with proper rendering

### How it works:
1. **Initialization**: The launcher checks for updates and loads user settings on startup
2. **Game Status Detection**: Automatically detects if the game is installed and checks for available updates
3. **User Interaction**: Provides clear buttons for downloading, updating, or playing the game based on current status
4. **Configuration**: Users can customize game and launcher settings through the settings window
5. **Game Launch**: Configures Unity game settings via registry and launches the game with proper parameters

### Adding Custom Languages:
The launcher supports adding custom languages through text files in the `Localizations` folder. Each language file follows this structure:
- First line: `Language: [Display Name]`
- Second line: `Font: [Path to font file]`
- Following lines: Translations in format `"controlName": "translation", fontSize: [size]` or `"KEY": "translation"`

Example language file structure:
```
Language: Español
Font: Fonts\ldslender.ttf

"startButton": "ACTUALIZAR AHORA", fontSize: 18
"statusLabel": "¡Nueva actualización disponible!", fontSize: 16
"PLAY": "JUGAR"
```

The launcher automatically detects and loads all `.txt` files from the Localizations folder.

### Requirements:
- Windows 7 or higher
- Administrator privileges (for game installation and registry access)