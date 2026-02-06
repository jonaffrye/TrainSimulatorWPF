# 🚂 Train Simulator - Railway Driving Simulator

> An educational train simulator developed in C# with WPF, offering a realistic railway driving experience with physics simulation.

![Train Simulator Interface](https://img.shields.io/badge/Platform-Windows-blue)
![.NET](https://img.shields.io/badge/.NET-WPF-purple)
![Language](https://img.shields.io/badge/Language-C%23-green)
![Status](https://img.shields.io/badge/Status-In%20Development-yellow)

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture](#architecture)
- [Installation](#installation)
- [Usage](#usage)
- [Physics Model](#physics-model)
- [Project Structure](#project-structure)
- [Contributing](#contributing)
- [Roadmap](#roadmap)

## 🎯 Overview

Train Simulator is a WPF desktop application designed to provide an educational and immersive railway driving experience. The simulator integrates a realistic physics engine modeling acceleration, braking, and resistance forces, while offering a user interface inspired by modern train cabins.

### Project Goals

- 🎓 **Educational**: Learn train startup procedures and driving operations
- ⚙️ **Realistic**: Physics simulation based on Davis equation
- 🖥️ **Interactive**: Intuitive user interface with screens and touch controls
- 🚉 **Complete**: Station system, schedules, and train selection

## ✨ Features

### User Interface

- **Main Navigation Screen**
  - Selection of departure station from 10 Belgian stations
  - Route and schedule selection
  - Train type selection

- **Interactive Startup Procedure**
  - Cabin activation
  - Pantograph raising
  - Power-up sequence
  - Parking brake release
  - Direction selector engagement

- **Realistic Dashboard**
  - Circular speed indicator (km/h)
  - Power gauge (kW)
  - Distance counter (m)
  - Traction and braking indicators (%)
  - Simulation clock
  - Status and control interface

### Physics Simulation

The simulator implements a complete physics model:

```
Basic equations:
- ΣF = ma
- a = (F_traction - F_braking - F_resistance) / m
- F_resistance = c₁ + c₂·v + c₃·v² (Davis Equation)
```

#### Simulated Parameters

- **Train mass**: Variable by train type
- **Maximum traction force**: Controlled by throttle
- **Maximum braking force**: Controlled by brake
- **Resistance coefficients**: Realistic friction modeling
- **Velocity and position**: Real-time calculation (Δt = 50ms)

### Controls

| Control        | Function                   |
| -------------- | -------------------------- |
| ⬆️ **UP**      | Menu navigation (up)       |
| ⬇️ **Down**    | Menu navigation (down)     |
| ✅ **Enter**   | Confirm selection          |
| 🛑 **Stop**    | Go back / Stop simulation  |
| 🔘 **ACQ**     | Acknowledge alarms         |
| 🔛 **ON**      | Cabin activation           |
| ⚡ **Pant**    | Pantograph                 |
| 🔌 **Power**   | Power on                   |
| 🅱️ **Brake**   | Parking brake              |
| ➡️ **Forward** | Forward direction          |
| 🎚️ **Slider**  | Traction/braking regulator |

## 🏗️ Architecture

### Main Class Structure

```
TrainSimulator/
│
├── Models/
│   ├── Train.cs           # Train class with physics model
│   ├── Station.cs         # Stations and their properties
│   └── Schedule.cs        # Timetables and routes
│
└── Views/
    ├── CenterPanel/
    │   ├── MainScreen.xaml.cs    # Initial selection screen
    │   ├── Screen.xaml.cs        # Main dashboard
    │   └── ScreenButton.xaml.cs  # Screen buttons
    │
    ├── LeftPanel/
    │   ├── LeftPan.xaml.cs       # Navigation panel
    │   └── RoundedBtn.xaml.cs    # Custom buttons
    │
    ├── RightPanel/
    │   └── RightPan.xaml.cs      # Regulator panel
    │
    └── BottomPanel/
        └── BottomPan.xaml.cs     # Control panel
```

### `Train` Class

```csharp
public class Train
{
    // Physical properties
    public double Mass { get; set; }
    public double MaxTractionForce { get; set; }
    public double MaxBrakeForce { get; set; }
    public List<double> ResCoef { get; set; }

    // Dynamic state
    public double Velocity { get; private set; }
    public double Position { get; private set; }

    // Simulation methods
    public void UpdateVelocity(double timeDT, double commandValue)
    public void UpdatePosition(double timeDT)
}
```

### Event System

The project uses a C# event system for inter-component communication:

```csharp
// Example of event subscription
leftPanel.EnterBtnClicked += OnEnterBtnEvent;
bottomPanel.OnBtnCLicked += OnTurnOnBtnEvent;
rightPanel.SliderChange += OnSliderChange;
```

## 🚀 Installation

### Prerequisites

- Windows 10/11
- .NET Framework 4.7.2 or higher / .NET 6.0+
- Visual Studio 2019 or later

### Installation Steps

1. **Clone the repository**

   ```bash
   git clone https://github.com/your-username/train-simulator.git
   cd train-simulator
   ```

2. **Open the project**

   ```bash
   # Open the solution in Visual Studio
   TrainSimulator.sln
   ```

3. **Restore NuGet packages**
   - Right-click on the solution → "Restore NuGet Packages"

4. **Build and run**
   - Press `F5` or click "Start"

## 📖 Usage

### Starting a Simulation

1. **Application Launch**
   - Welcome screen displays "Welcome to this simulation"

2. **Station Selection**
   - Use UP/DOWN buttons to navigate
   - Press ENTER to confirm
   - Available stations: Brussels-Midi, Antwerp-Central, Liège-Guillemins, etc.

3. **Schedule Selection**
   - Choose from available routes for the selected station
   - Display: Departure time, destination, platform number

4. **Train Selection**
   - Choose train type (IC, P, L, S)
   - View technical specifications

5. **Startup Procedure**
   - Follow the 5 steps in order:
     1. Activate cabin (ON button)
     2. Raise pantograph (Pant button)
     3. Power on (Power button)
     4. Release parking brake (Brake button)
     5. Select forward direction (Forward button)

6. **Driving**
   - Use the slider to control:
     - Positive values: Traction (acceleration)
     - Negative values: Braking (deceleration)
     - Center: Neutral mode (coasting)

### Reading the Dashboard

- **Circular indicator**: Current speed in km/h
- **Blue vertical bar**: Engine regime
- **Digital displays**:
  - `km/h`: Speed
  - `kW`: Power output
  - `m`: Distance traveled
- **Text indicators**:
  - `Traction: X%`: Acceleration level
  - `Brake: X%`: Braking level
- **Clock**: Elapsed time since departure (mm:ss)

## 🔬 Physics Model

### Equation of Motion

The simulator numerically solves the differential equations of motion:

```
dv/dt = (F_traction - F_braking - F_resistance) / m
dx/dt = v
```

Using Euler integration method:

```
v(t+Δt) = v(t) + a(t)·Δt
x(t+Δt) = x(t) + v(t)·Δt
```

### Resistance Forces (Davis)

```
F_resistance = c₁ + c₂·v + c₃·v²
```

Where:

- `c₁`: Constant mechanical resistance
- `c₂·v`: Linear resistance (friction)
- `c₃·v²`: Aerodynamic resistance (quadratic)

### Temporal Parameters

- **Time step**: Δt = 50 ms
- **Update frequency**: 20 Hz
- **Method**: Explicit Euler

## 📁 Project Structure

```
TrainSimulator/
│
├── TrainSimulator/              # Business logic
│   ├── Train.cs
│   ├── Station.cs
│   └── Schedule.cs
│
├── TrainSimulatorWPF/          # User interface
│   ├── View/
│   │   ├── CenterPanel/
│   │   ├── LeftPanel/
│   │   ├── RightPanel/
│   │   └── BottomPanel/
│   │
│   ├── Resources/              # Visual resources
│   └── App.xaml               # Application configuration
│
├── README.md
└── LICENSE
```

## 🗺️ Roadmap

### Current Version (v0.1-alpha)

- ✅ Station/schedule/train selection
- ✅ Startup procedure
- ✅ Basic physics simulation
- ✅ Dashboard interface

### Upcoming Features

#### Short Term

- [ ] Save/load configuration
- [ ] Alert and alarm system
- [ ] Variable weather conditions
- [ ] Railway signaling

#### Medium Term

- [ ] Complete railway network with routes
- [ ] Emergency braking system
- [ ] Realistic energy consumption
- [ ] Station stops with scheduled halts
- [ ] Multi-language support (FR/NL/EN)

#### Long Term

- [ ] Multiplayer mode (dispatcher/driver)
- [ ] Educational scenarios
- [ ] Random failure integration
- [ ] VR support (virtual reality headset)
- [ ] Custom route editor
- [ ] Driver statistics and performance

### Technical Improvements

- [ ] Physics engine optimization
- [ ] Graphics enhancement (3D)
- [ ] Realistic sound effects
- [ ] Database for schedules
- [ ] Comprehensive unit tests
- [ ] API documentation

## 🤝 Contributing

Contributions are welcome! Here's how to participate:

1. Fork the project
2. Create a branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Guidelines

- Follow C# naming conventions
- Comment code in English or French
- Test changes before submission
- Update documentation as needed

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Authors

- **Your Name** - _Initial development_ - [YourUsername](https://github.com/your-username)

## 🙏 Acknowledgments

- Inspiration: Real SNCB/NMBS train systems
- Physics model: Davis equation
- Framework: Microsoft WPF
- Community: Stack Overflow and C# forums

## 📞 Contact

For questions or suggestions:

- 📧 Email: your.email@example.com
- 🐛 Issues: [GitHub Issues](https://github.com/your-username/train-simulator/issues)
- 💬 Discussions: [GitHub Discussions](https://github.com/your-username/train-simulator/discussions)

---

**Note**: This project is in active development. Features and interface may evolve significantly.

⭐ If you find this project interesting, don't hesitate to give it a star on GitHub!

## 📜 License & Collaboration

This project is open source for **educational purposes only**.

**Want to contribute or use this code?**

- 🤝 Open an issue to discuss
- 📧 Email me at: your.email@example.com
- ⭐ Star the repo if you find it useful!

See [LICENSE.md](LICENSE.md) for full terms.
