## IGDL2

### Arbeiten mit Git (auf dem main Branch)

- Wichtig: Immer zuerst den main Branch **pullen**!
- Erst dann können Änderungen gemacht werden, ohne Konflikte hervorzurufen.
- Wenn ihr Änderungen gemacht habt und sie speichern bzw. auf Github hochladen wollt, speichert zuerst alles in Unity und fügt alle geänderten (und auch potenziell neuen) Dateien zum Repository (z.B. mit GitKraken) hinzu.
- Hinweis: Auch wenn ihr Dateien gelöscht habt, muss diese Änderung Git mitgeteilt werden, sonst ist diese Änderung nur bei euch lokal. (Das wird aber alles in dem einen Befehl gemacht.)
- Danach muss ein **Commit** erstellt werden (mit einer Nachricht, was geändert wurde, um den Überblick über die gemachten Änderungen zu behalten).
- Dann muss der **Commit** noch **gepusht** werden.
- Fertig :-)

### Ordnerstruktur (main Branch) 

    Assets 
        Animation 
        Artwork (hier sind alle *.png gespeichert, sowie (wenn bereits in Unity erstellt) die entsprechenden Tilesets, d.h. Pallet- und Tiles-Ordner zu jedem *.png)
            DoorsAndLever (Beispiel)
                Pallets_Door_1
                Tiles_Door_1
                door_1.png
            Ground
            Pillar
            Player
            Wall
        Gizmos (verschiedene Icons)
        Images (Bilder zum Testen, z.B. hebel.png für den Hebel oder white.png für den weißen Hintergrund)
        Prefabs (Elemente, die in mehreren Szenen gebraucht werden, z.B. Player, Firefly, ...)
        Presets
        Sample (Beispiel vom Anfang, was wir zum Testen genutzt haben mit Artwork, Scenes, ...)
        Scenes (unsere Szenen, die wir bisher erstellt haben)
        Scripts (die C#-Skripte, die für verschiedene Funktionen in Unity geschrieben wurden)
        Settings
    Packages
    ProjektSettings
    .gitignore
    README.md
