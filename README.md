# Star System (Vstupná brána) - Tímový projekt 24/25

## Unity
- [2022.3.61f1](https://unity.com/releases/editor/archive)
- [openXR (XR Interaction Toolkit)](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.1/manual/index.html)
- [TextMeshPro](https://docs.unity3d.com/Packages/com.unity.textmeshpro@3.2/manual/index.html)

## Práca s projektom
- pullni si tento repozitár a stiahni požadovanú Unity verziu (potrebuješ UWP a Windows IL2CPP Build support moduly)
- prečítaj si dokumentáciu a používateľskú príručku poskytnutú od Vedúcej tímového projektu/záverčnej práce
- na prácu potrebuješ ľubovoľné VRko, ktoré má podporu openXR api (potrebné pridať do modulov openXR v projekte)
- na prácu bez VRka je možné doinštalovať "simulátor VR" [MOCKMHD](https://docs.unity3d.com/Packages/com.unity.xr.mock-hmd@1.0/manual/index.html)

## O Aplikácii
Aplikácia slúži ako spojka, tzv. centrálny hub, kde si používateľ vie zvoliť zo zoznamu ponúknutých edukačných hier. Každá hra je separátna Unity aplikácia, ktorá beží nezávisle od Star Systemu (táto apka).

## Integrácia novej hry do vstupnej brány

### 1.Duplikovanie tlačidla v Scroll View
Zoznam hier je vizualizovaný prostredníctvom komponentu `Scroll View`, v ktorom každá hra je reprezentovaná samostatným tlačidlom. Nové tlačidlo je možné pridať nasledovne:

- Otvorte scénu vstupnej brány vo Unity.
- V Hierarchy nájdite objekt Scroll View > Viewport > Content.
- Vyberte existujúce tlačidlo (Button), ktoré reprezentuje hru.
- Stlačte `Ctrl + D` (alebo `Cmd + D` na macOS) pre jeho duplikovanie.
- Posuňte novo vytvorené tlačidlo pozdĺž osi Y tak, aby sa zachoval vertikálny odstup 20 jednotiek od predchádzajúceho tlačidla.

Odporúčanie: Skontrolujte rovnomernosť rozostupov medzi všetkými tlačidlami, aby ste zachovali vizuálnu konzistentnosť a dobrú používateľskú skúsenosť.

--- 

### 2. Aktualizácia GameMenuManager.cs

- Logika spustenia hier je centralizovaná v skripte `GameMenuManager.cs`. 
- Každá hra je evidovaná v zozname games, ktorý sa napĺňa prostredníctvom inštancií triedy `GameInfo`.

Novú hru zaregistrujete nasledovne:
- Otvorte súbor `GameMenuManager.cs`.
- Vyhľadajte časť kódu, kde sa nachádza inicializácia zoznamu games pomocou `games.Add(new GameInfo { ... });`
- Pridajte nový blok v nasledovnom formáte:
```csharp
games.Add(new GameInfo
{
    title = "Názov hry",
    description = "Krátky popis čo je cieľ hry.",
    exeFileName = "NazovHry/NazovHry.exe"
});
```
**Popis atribútov:**

`title:` Zobrazovaný názov hry v používateľskom rozhraní.

`description:` Popis hry, ktorý sa zobrazí v detailnom paneli.

`exeFileName:` Relatívna cesta k spustiteľnému súboru hry z priečinka Games.

--- 

### 3.Pridanie herných súborov do projektu
- Každá hra musí byť fyzicky prítomná v štruktúre projektu v adresári Games, aby ju bolo možné spustiť z hlavného menu.
- Otvorte priečinok projektu, kde sa nachádza build vstupnej brány
- Nájdite zložku Games.
- Vložte sem nový podpriečinok (napr. SvetSolarnychClankov).
- Skopírujte do neho celý build hry vrátane .exe súboru a všetkých dátových priečinkov.

Poznámka: Dodržanie správnej štruktúry priečinkov je nevyhnutné pre funkčnosť spúšťania hier.

--- 

### 4.Buildnutie aktualizovanej verzie vstupnej brány
Po úspešnom pridaní novej hry do kódu a používateľského rozhrania je potrebné vytvoriť nový build vstupnej brány. Aby sa zachoval existujúci priečinok Games s hernými súbormi, postupujte nasledovne:

- Vymaž pôvodný build vstupnej brány, okrem priečinka Games:
- Vytvor nový build vstupnej brány do toho istého adresára:
- V Unity otvorte menu File > Build Settings.
- Kliknite na tlačidlo Build.
- Ako cieľový priečinok build-u zvoľte ten istý priečinok, z ktorého boli predtým zmazané súbory.
- Unity následne vytvorí nový build aplikácie, pričom priečinok Games, ktorý už v tomto adresári existuje, zostane zachovaný a nebude prepísaný.

Týmto spôsobom je možné efektívne rozširovať aplikáciu o nové edukačné hry bez potreby zásahu do infraštruktúry samotných hier. Celý proces podporuje jednoduchú údržbu, škálovateľnosť a opätovnú použiteľnosť základného kódu aplikácie.
