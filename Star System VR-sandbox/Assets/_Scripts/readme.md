### StarSystemController
Escape je ofiko vypnutie hry, toto zaručí aby sa vypol aj ten GameLauncher čo kontroluje, ktorá hra má byť spustená.

V triede máš list dictionaries pre hry, foldername môže bývať iný ako názov exečka, preto sú tu obe, .exe si script doplní. V Debugu/unity editore máš tam zopár výpisov či našlo tú hru, ináč sa to nedá testovať pokiaľ to nebuildneš.
```csharp
[System.Serializable]
public class StringDictionary
{
    [field: SerializeField] public string FolderName { get; set; }
    [field: SerializeField] public string GameName { get; set; }
}
```

### PlanetInteractable

Toto dávaš na objekt, s ktorým chceš spustiť hru, napíšeš potom do políčka v komponente názov exečka, je to síce redundanté, ale neviem ako to lepšie na teraz spraviť. 