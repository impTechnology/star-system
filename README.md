# Star System - Tímový projekt 

--- 

## Kompilácia a build
### Potrebné
- .NET 8.0 SDK (https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Unity 2022.3.37f1 (https://unity.com/releases/editor/archive)
### Postup
1. V priečinku GameWatcher spustiť príkaz: `dotnet build -c Release`
2. Build Game Launcheru nájdeš v priečinku `GameWatcher/bin/Release/net8.0`
3. Buildneš Star-System v Unity
4. Do priečinku buildnutého Game Launcheru presunieš priečinok buildnutného Star System
5. Do priečinku buildnutého `StarSystem/Star System VR-sandbox_Data/StreamingAssets` vložíš priečinky buildov hier
6. Finito

--- 

## Štruktúra priečinku
GameLauncher:
- Gamelauncher.exe
- dll atď.
- StarSystem
  - MonoBleedingEdge atď.
  - StarSystem.exe
  - StarSystem_Data
    - StreamingAssets
      - gameshere.txt

---

## Dokumentácie
dokumentáciu a popis metód a tried pre Unity projekt nájdeš v StarSystem/Assets/_Scripts/readme.md
dokumentáciu pre Game Launcher nájdeš v GameWatcher/readme.md
