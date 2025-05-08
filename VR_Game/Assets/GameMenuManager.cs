using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Diagnostics;
using System.IO;

public class GameUIManager : MonoBehaviour
{
    [System.Serializable]
    public class GameInfo
    {
        public string title;
        public string description;
        public string exeFileName;
    }

    public List<GameInfo> games = new List<GameInfo>();

    public TMP_Text gameTitleText;
    public TMP_Text gameDescriptionText;
    public Button startButton;
    public List<Button> gameButtons;

    private GameInfo selectedGame;

    void Start()
    {
        ClearGameDetails();

        games.Add(new GameInfo
        {
            title = "Svet solárnych článkov",
            description = "Vitaj na solárnej stanici! V tejto hre sa naučíš rozoznávať obnoviteľné a neobnoviteľné zdroje energie, objavíš výhody a nevýhody rôznych spôsobov výroby elektriny a zistíš, ako fungujú solárne články. Budeš experimentovať s nastaveniami osvetlenia a záťaže, aby si dosiahol čo najlepší výkon solárneho systému. Nakoniec si vyskúšaš zostaviť funkčný elektrický obvod, ktorý dokáže napájať spotrebič. Pripravený na misiu plnú objavov a energie? Poďme na to!",
            exeFileName = "SvetSolarnychClankov/SvetSolarnychClankov.exe"
        });

        games.Add(new GameInfo
        {
            title = "Skryté hrozby internetu",
            description = "Vitaj v kybernetickom svete! V tejto hre sa naučíš rozpoznávať rôzne hrozby internetu, ako sú vírusy, trojské kone či spyware. Spoznáš princípy phishingových útokov, naučíš sa odhaľovať podvodné e-maily a zostavíš silné heslo, ktoré ochráni tvoje dáta. Každý level ťa zavedie do iného prostredia – od bludiska plného vírusov až po oceán plný nástrah. Získaj vedomosti, ktoré ťa ochránia v online svete, a staň sa majstrom kyberbezpečnosti!",
            exeFileName = "SkryteHrozbyInternetu/SkryteHrozbyInternetu.exe"
        });

        games.Add(new GameInfo
        {
            title = "Svet Elektromagnetizmu",
            description = "Vitaj v elektromagnetickom svete!\r\nV tejto hre sa naučíš, ako fungujú elektromotory, akumulátory a elektromagnetické javy v praxi. Pomôž opustenému vlaku získať energiu, poskladaj elektrický obvod a otvor si cestu ďalej pomocou frekvenčného ladiča. Čakajú ťa logické úlohy, praktické výzvy a zábavné prekvapenie na záver. Využi svoje vedomosti a zručnosti a objav silu elektromagnetizmu vo virtuálnom svete!",
            exeFileName = "SvetElektromagnetizmu/Game.exe"
        });

        games.Add(new GameInfo
        {
            title = "Planéta CMOS",
            description = "V interaktívnom prostredí technologického strediska si vyskúšaš, ako poskladať PMOS a NMOS tranzistory podľa návodov, pričom sa ti meria čas – zvládneš to rýchlejšie ako ostatní? Potom začne skutočná zábava: meníš napätia, sleduješ, ako sa pohybujú nosiče, a objavuješ, čo sa deje vo vnútri tranzistorov. Na záver spojíš PMOS a NMOS do jedného – CMOS – a pochopíš, ako spolu fungujú pri prepínaní signálu. Všetko je hravé, názorné a vysvetlené priamo v priestore, takže nepotrebuješ žiadne predchádzajúce znalosti – stačí chuť objavovať!",
            exeFileName = "PlanetaCMOS/Planeta CMOS.exe"
        });

        games.Add(new GameInfo
        {
            title = "Planéta logických obvodov",
            description = "V tejto vesmírnej hre sa ponoríš do sveta logických hradiel, kde je tvojím cieľom správne nastaviť kombinácie hradiel (AND, OR, NAND, NOR, XOR) a vytvárať obvody, ktoré ovládajú prúd. Naučíš sa, ako rôzne logické operácie ovplyvňujú výsledky a použiješ ich na riešenie zábavných výziev. Postupne sa naučíš, ako efektívne pracovať s logickými hradlami a získaš skúsenosti, ktoré ti pomôžu prekonať všetky úlohy. Cestuj vesmírom, vytváraj obvody a zbieraj hviezdičky!",
            exeFileName = "PlanetaLogickychObvodov/New Unity Project (2).exe"
        });

        games.Add(new GameInfo
        {
            title = "Svet tyristorov",
            description = "Vydaj sa na interaktívne dobrodružstvo, v ktorom sa naučíš, ako fungujú tyristory a IGBT tranzistory. Tvojím cieľom je opraviť auto, aby si stihol párty, no čaká ťa cesta plná elektrónov, napätia a zábavy. Vyskúšaš si spínanie tyristora, budeš triafať elektróny v hre na presnosť a pozorovať rekombináciu. V závere nahradíš tyristor úspornejším IGBT a správne nastavíš jeho parametre. Hra spája fyziku a akciu do pútavého zážitku, ktorý ťa doslova vtiahne dovnútra súčiastok.",
            exeFileName = "SvetTyristorov/mhn.exe"
        });

        games.Add(new GameInfo
        {
            title = "Organická Elektronika",
            description = "Vstúp do prísne stráženého podzemného bunkra, kde technológia a veda ožívajú. Postupne odhaľuj tajomstvá organickej fotovoltiky a OLED technológie, aby si zachránil malého robota, ktorému došla energia aj zrak. Čakajú ťa skryté miestnosti, logické úlohy, skladanie elektronických vrstiev a interaktívny kvíz, kde si overíš, čo si sa naučil. Všetko, čo nájdeš a pochopíš, ti pomôže rozsvietiť svetlo vo svete organickej elektroniky. Pripravený na misiu, kde sú vedomosti tvojím najväčším nástrojom?",
            exeFileName = "OrganickaElektronika/OrganickaElektronika.exe"
        });

        games.Add(new GameInfo
        {
            title = "Svet led diód",
            description = "Pristál si na planéte, kde LED diódy stratili svoju žiaru – a tvojou misiou je zistiť prečo. V interaktívnom vesmírnom dobrodružstve sa naučíš, ako fungujú LED diódy, čo ovplyvňuje ich svietenie a aké látky a podmienky sú pre ne nevyhnutné. Budeš hádzať elektróny, pracovať s napätím a skladať funkčné obvody – no pozor, zlá kombinácia môže spôsobiť výbuch! S každou splnenou úlohou sa planéta znovu rozžiari a ty sa posunieš bližšie k vyriešeniu záhady. Priprav sa rozsvietiť celý svet... doslova",
            exeFileName = "SvetLEDDiod/HraVR.exe"
        });

        for (int i = 0; i < gameButtons.Count; i++)
        {
            int index = i;
            gameButtons[i].onClick.AddListener(() => ShowGameDetails(index));
        }

        startButton.onClick.AddListener(OnStartClicked);
    }

    void ShowGameDetails(int index)
    {
        selectedGame = games[index];
        gameTitleText.text = selectedGame.title;
        gameDescriptionText.text = selectedGame.description;

        startButton.gameObject.SetActive(true);
    }

    void OnStartClicked()
    {
        if (selectedGame == null || string.IsNullOrEmpty(selectedGame.exeFileName))
        {
            UnityEngine.Debug.LogError("Žiadna hra nebola vybraná alebo chýba exe súbor.");
            return;
        }

        string exePath = Path.Combine(Application.dataPath, "../Games/" + selectedGame.exeFileName);

        if (File.Exists(exePath))
        {
            string launcherPath = Path.Combine(Application.dataPath, "../Games/Launcher.bat");
            string vrGameExePath = Path.Combine(Application.dataPath, "../VR_Game.exe");

            File.WriteAllText(launcherPath,
                "@echo off\n" +
                "start \"\" \"" + exePath + "\"\n" +
                "timeout /t 2 > nul\n" +
                ":waitloop\n" +
                "timeout /t 2 > nul\n" +
                "tasklist /fi \"imagename eq " + Path.GetFileName(exePath) + "\" | find /i \"" + Path.GetFileNameWithoutExtension(exePath) + "\" >nul\n" +
                "if not errorlevel 1 goto waitloop\n" +
                "start \"\" \"" + vrGameExePath + "\"\n"
            );

            Process.Start(launcherPath);

            StartCoroutine(DelayedQuit());
        }
        else
        {
            UnityEngine.Debug.LogError("EXE súbor sa nenašiel: " + exePath);
        }
    }

    System.Collections.IEnumerator DelayedQuit()
    {
        yield return new WaitForSeconds(1.5f);
        Application.Quit();
    }

    void ClearGameDetails()
    {
        gameTitleText.text = "";
        gameDescriptionText.text = "";
        startButton.gameObject.SetActive(false);
        selectedGame = null;
    }
}
