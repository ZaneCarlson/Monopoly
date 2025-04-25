# Monopoly Game (C# WinForms)

This is a simple implementation of the classic Monopoly board game as a Windows Forms application in C#. The solution includes the full game logic, UI rendering of the board, dice rolling, property transactions, and jail mechanics.

## Prerequisites

- **Operating System**: Windows 10 or later
- **IDE**: Visual Studio Community 2019/2022 (any edition with the **.NET Desktop Development** workload)
- **.NET Framework**: Target framework is **.NET Framework 4.7.2** (ensure the Developer Pack is installed)

## Getting Started

1. **Unzip the Project**  
   Extract the ZIP archive to a local folder on your machine, e.g., `C:\Projects\Monopoly`.

2. **Open the Solution**  
   - Launch Visual Studio.  
   - From the **File** menu, choose **Open > Project/Solution...**  
   - Navigate to the extracted folder and select **`Monopoly.sln`**.

3. **Restore Dependencies**  
   - If prompted, allow Visual Studio to **restore NuGet packages**.  
   - This project does not include external NuGet packages by default, but this step ensures any missing references are resolved.

4. **Build the Solution**  
   - Choose **Build > Build Solution** (or press **Ctrl+Shift+B**).  
   - Ensure there are no build errors in the **Error List** window.

5. **Run the Application**  
   - Press **F5** or click the **Start** button to launch the Monopoly game.  
   - A dialog will prompt you to select the number of players.  
   - Use the on-screen buttons to roll dice, view player info, buy properties, and manage turns. standard rules of monopoly apply. 

## Project Structure

```
Monopoly/            
├─ Monopoly.sln      # Visual Studio solution file
├─ Monopoly/         
│  ├─ Form1.cs       # Main Form and game logic
│  ├─ Form1.Designer.cs
│  ├─ Program.cs     
│  └─ Monopoly.csproj
└─ README.md         # This file
```

## Notes & Troubleshooting

- **.NET Target**: If you see a message about a missing .NET Framework, install the **.NET Framework 4.7.2 Developer Pack** from Microsoft.
- **Designer Errors**: If WinForms designer won’t load `Form1`, ensure all references to `System.Windows.Forms` and `System.Drawing` are intact and the target framework matches.

*Enjoy playing!*
