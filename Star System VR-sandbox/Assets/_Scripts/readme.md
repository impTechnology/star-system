# StarSystemController
**Táto trieda musí vždy byť iba RAZ v scéne, ináč sa budú diať zlé veci**

**StarSystemController** komponent je daný na objekt `StarSystemController` je to child pod skupinou `==== MANAGERS ====`, tento objekt sa správa ako singleton, teda vieš sa naň v iných skriptoch dotazovať pomocou: 
```csharp
StarSystemController.Instance.MetódaTriedy()
```
**Pridanie hier**, pridáš prvok do zoznamu ako je vidieť na sreenshote, pozor niektoré hry majú ináč pomenovaný game folder ako exečko, preto sú tu tieto 2 string polia. Funkcia si ".exe" už doplní. V Debugu/unity editore máš tam zopár výpisov či našlo tú hru, ináč sa to nedá testovať pokiaľ to nebuildneš.

![vstupnabrana-hry.png](https://github.com/impTechnology/star-system/blob/main/images/vstupnabrana-hry.png)

---
## Metódy 
### public void LaunchSubgame(string subgameName)
- za platný argument subgameName považujeme meno exečka hry bez prípony, metóda si následne pohľadá folderName cez dictionary pre túto požadovanú hru
- v editore ti vypíše či sa hra našla alebo nie, k tomu je tam niekoľko výpisov čo sa stalo
- ak to buildneš metóda vytvára v priečinku hry textový dokument, ktorý potom prečíta GameLauncher a spustí tú hru, tento texťák sa vymaže po vypnutí hry
- túto funkciu vieš zavolať cez `StarSystemController.Instance.LaunchSubgame("hra")` v hocijakom skripte, štandardne je volaná v `PlanetInteractible` triede

---

### public void ExitGame()
- túto metódu treba zavolať, keď vypíname vstupnú bránu, nakoľko komunikuje s GameLauncherom aby sa aj on vypol, nakoľko on manažuje, vypínanie a zapínanie vstupnej brány a hier
- funguje na tom istom princípe ako `LaunchSubgame` s tým, že vytvára texťák, na ktorý čaká GameLauncher
- zatiaľ ak vypneš vstupnú bránu bez volania tejto metódy ti GameLauncher opätovne spustí vstupnú bránu (volanie metódy je na ESC, teda ALTF4 zatiaľ nefunguje :P)

# PlanetInteractable
## Parametre
**POTREBNÝ PARAMATER** - subgameName - meno hry, ktorú chceš spustiť bez prípony
interactable - základný komponent pre XR interactables
## Metódy
### OnClick()
Táto metóda je zavolaná pri štandardnej interakcií `onclick`, volá metódu `StarSystemController` aby spustil požadovanú hru argument je náš potrebný parameter triedy

Túto triedu pridáš na hocijaký objekt pravdepodobne z colliderom, ktorý nepatrí medzi UI, stačí pridať tento skript, ten XR základ sa pridá automaticky, nič extra netreba nastavovať okrem potrebného parametru.
![vstupnabrana-portal](https://github.com/impTechnology/star-system/blob/main/images/vstupnabrana-portal.png)

---

