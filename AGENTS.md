# IntervalTimer Agent Context

Welcome! If you are an AI agent working in this repository, here is the essential context and rules you need to know:

## Project Overview
- **Technology Stack:** .NET 10 MAUI
- **Target Platform:** Mobile primarily (specifically optimized for Android).
- **Purpose:** An Interval Timer app that lets users create, run, and track custom walk/run interval presets.

## Architecture
- **Pattern:** MVVM (Model-View-ViewModel)
- **MVVM Toolkit:** We use the `CommunityToolkit.Mvvm` package heavily. You should use `[ObservableProperty]`, `[RelayCommand]`, and `ObservableObject` source generators for ViewModels.
- **Dependency Injection:** Services and ViewModels are registered in `MauiProgram.cs`. `DatabaseService` is a Singleton, while ViewModels and Views are Transients.
- **Routing:** Shell navigation is configured in `AppShell.xaml`. The application defaults to the `PresetsPage`. 

## Key Components
- **Models:**
  - `Preset`: Defines intervals (RunTime, WalkTime, limits) and tracks `LastUsed` so the default preset list is sorted by most recently used.
  - `RunHistory`: Logs completed runs with start/completion status.
- **Data Persistence:** Handled via SQLite using the `sqlite-net-pcl` package. All database logic lives in `Data/DatabaseService.cs`.
- **UI Custom Controls:**
  - `CircularProgressBar` (`Controls/CircularProgressBar.cs`): A custom `GraphicsView` rendering a dynamic circular arc using `Microsoft.Maui.Graphics`. It relies on an absolute `endAngle` for `DrawArc` to smoothly deplete clockwise.

## Design Aesthetic & Guidelines
- **Theme:** Forced Dark Mode (`AppTheme.Dark`).
- **Colors:** Deep, off-black surfaces (`#121212`, `#1E1E1E`) paired with **neon** accents.
  - Run state is **Neon Green** (`#39FF14`).
  - Walk state is **Neon Blue** (`#00FFFF`).
- **Style:** Clean, athletic, modern, flat 2D styling. No overly generic colors. We use bold typography and a central circular progress ring for the active timer.

## Common Workflows & Commands
- **Building for Android:** `dotnet build -t:Run -f net10.0-android`
- **Known Quirks:** When building, if you encounter `java.nio.file.NoSuchFileException` in the `obj/` directory or `Microsoft.Maui.Resizetizer` errors regarding duplicate file names, clean the `obj/` and `bin/` directories completely before rebuilding.

## Agent Instructions
- Ensure UI components adhere to the existing dark/neon aesthetic.
- Do not introduce UI frameworks or CSS-based systems (e.g. Tailwind) into this MAUI XAML project.
- Always use `CommunityToolkit.Mvvm` source generators over manual `INotifyPropertyChanged` boilerplate.
