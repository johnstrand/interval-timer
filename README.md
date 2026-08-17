# IntervalTimer

IntervalTimer is a sleek, modern .NET 10 MAUI application designed specifically for Android mobile devices. It helps you manage custom run/walk interval workouts with an intuitive, athletic, dark-themed UI featuring vibrant neon indicators and haptic feedback.

## Features
- **Custom Presets:** Create configurable interval presets, defining your own run duration, walk duration, starting phase, and optional total workout limits (by duration or interval count).
- **Smart Sorting:** Presets automatically sort by the ones you've used most recently.
- **Dynamic Visuals:** A large, central custom circular progress ring smoothly depletes to track your current interval state, shifting from Neon Green (Run) to Neon Blue (Walk).
- **Run History:** A dedicated history tab logs your completed workouts, duration, and the preset you used.
- **Haptic & Audio Cues:** Built-in haptic vibration alerts notify you when it's time to switch between running and walking.

## Technology Stack
- **Framework:** .NET 10 MAUI
- **Architecture:** MVVM (Model-View-ViewModel) utilizing `CommunityToolkit.Mvvm` source generators
- **Local Database:** SQLite (`sqlite-net-pcl`)
- **Graphics:** Custom rendering via `Microsoft.Maui.Graphics`

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- MAUI Workload (`dotnet workload install maui`)
- An Android device or emulator (Android API 21+)

### Build & Run
To compile and deploy the app directly to an attached Android device or running emulator:
```bash
dotnet build -t:Run -f net10.0-android
```

*(Note: If you encounter build caching issues or duplicate file name errors from the MAUI Resizetizer, it is recommended to clean your `bin/` and `obj/` directories before rebuilding.)*

## Project Structure
- **`/Models`:** SQLite database schemas (`Preset.cs`, `RunHistory.cs`).
- **`/ViewModels`:** MVVM view models managing application state.
- **`/Views`:** XAML pages representing the user interface.
- **`/Controls`:** Custom graphics controls, including the `CircularProgressBar`.
- **`/Data`:** The local persistence layer (`DatabaseService.cs`).
- **`/Resources`:** Application visual assets, splash screens, icons, and styling (`Colors.xaml`).
