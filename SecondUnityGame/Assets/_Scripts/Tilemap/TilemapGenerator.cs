using UnityEngine;

public class TilemapGenerator : MonoBehaviour
{
    #region Generation-params
    int xMax = 80;                   // Breite der Map in Anzahl der Tiles
    int yMax = 55;                   // H�he der Map " " " "

    // Die Anfangsh�he und offsets der einzelnen Streifen lassen sich im Nachhinein noch durch funktionen beeinflussen
    int waterHeight = 3;             // Dicke des Wasserstreifens
    int[] waterHeightArray;

    int sandHeight = 6;              // Dicke des Sandstreifens
    int[] sandHeightArray;
    int sandOffset = 9;              // Offset des Sandstreifens
    int[] sandOffsetArray;

    int lightGrassHeight = 7;        // Dicke des d�nnen Grasstreifens
    int[] lightGrassHeightArray;
    int lightGrassOffset = 21;       // Offset des d�nnen Grasstreifens
    int[] lightGrassOffsetArray;

    int darkGrassHeight = 19;        // Dicke des dicken Grasstreifens
    int[] darkGrassHeightArray;
    int darkGrassOffset = 38;        // Offset des dicken Grasstreifens
    int[] darkGrassOffsetArray;
    #endregion

    int[,] tileArray;       // Bedeutung des Arrays: 0 = Wasser, 1 = Sand, 2 = d�nnes Gras, 3 = dickes Gras
    GameObject[,] tokenSlotReferenceArray;

    [SerializeField] Sprite waterTile, grassLightTile, grassDarkTile, sandTile;
    [SerializeField] Sprite waterBottomSandTop1_tile, waterBottomSandTop2_tile, threeSandOneWaterRight_tile, threeSandOneWaterLeft_tile, threeWaterOneSandRight_tile, threeWaterOneSandLeft_tile;
    [SerializeField] Sprite sandBottomLightGrassTop1_tile, sandBottomLightGrassTop2_tile, sandBottomLightGrassTop3_tile, threeGrassLightOneSandRight_tile, threeGrassLightOneSandLeft_tile, threeSandOneGrassLightRight_tile, threeSandOneGrassLightLeft_tile;
    [SerializeField] GameObject tokenSlot;
    [SerializeField] Vector3 generationVector;

    private void Start()
    {
        tileArray = new int[xMax, yMax];
        tokenSlotReferenceArray = new GameObject[xMax, yMax];

        tokenSlot = transform.parent.Find("TokenSlot_prefabs").Find("TokenSlot").gameObject;

        GenerateMap();
    }

    public void GenerateMap()
    {
        ResetArraysAndDeleteTiles();
        StructureOffsetsAndHeight();

        GenerateRoughTilemap();
        CreateRoughBorders();
        RemoveArtefacts1();

        FillWaterSandBorder();
        FillSandGrassLightBorder();
    }

    void ResetArraysAndDeleteTiles()
    {
        tileArray = new int[xMax, yMax];


        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = 0; yValue < yMax; yValue++)
            {
                Destroy(tokenSlotReferenceArray[xValue, yValue]);
            }
        }

        tokenSlotReferenceArray = new GameObject[xMax, yMax];
    }

    public void PaintTile( Vector3 position, int type)
    {
        switch (type)
        {
            case 100:     // Wasser
                GameObject tSlot1 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot1.GetComponent<SpriteRenderer>().sprite = waterTile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot1;
                break;
            case 200:     // Sand
                GameObject tSlot2 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot2.GetComponent<SpriteRenderer>().sprite = sandTile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot2;
                break;
            case 300:     // D�nnes Gras
                GameObject tSlot3 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot3.GetComponent<SpriteRenderer>().sprite = grassLightTile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot3;
                break;
            case 400:     // Dickes Gras
                GameObject tSlot4 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot4.GetComponent<SpriteRenderer>().sprite = grassDarkTile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot4;
                break;
            default:
                break;
        }

    }

    public void PaintSandToLightGrassTile(Vector3 position, int type)
    {
        switch (type)
        {
            case 201:     // Wasser unten Sand oben 1
                GameObject tSlot201 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot201.GetComponent<SpriteRenderer>().sprite = sandBottomLightGrassTop1_tile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot201;
                break;
            case 202:     // Wasser unten Sand oben 2
                GameObject tSlot202 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot202.GetComponent<SpriteRenderer>().sprite = sandBottomLightGrassTop2_tile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot202;
                break;
            case 203:     // Drei Sand, Wasser Rechts
                GameObject tSlot203 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot203.GetComponent<SpriteRenderer>().sprite = sandBottomLightGrassTop3_tile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot203;
                break;
            case 204:     // Drei Sand Wasser Links
                GameObject tSlot204 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot204.GetComponent<SpriteRenderer>().sprite = threeGrassLightOneSandRight_tile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot204;
                break;
            case 205:     // Drei Sand Wasser Links
                GameObject tSlot205 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot205.GetComponent<SpriteRenderer>().sprite = threeGrassLightOneSandLeft_tile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot205;
                break;
            case 206:     // Drei Sand Wasser Links
                GameObject tSlot206 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot206.GetComponent<SpriteRenderer>().sprite = threeSandOneGrassLightRight_tile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot206;
                break;
            case 207:     // Drei Sand Wasser Links
                GameObject tSlot207 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot207.GetComponent<SpriteRenderer>().sprite = threeSandOneGrassLightLeft_tile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot207;
                break;
            default:
                break;
        }

    }

    public void PaintWaterToSandTile(Vector3 position, int type)
    {
        switch (type)
        {
            case 101:     // Wasser unten Sand oben 1
                GameObject tSlot101 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot101.GetComponent<SpriteRenderer>().sprite = waterBottomSandTop1_tile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot101;
                break;
            case 102:     // Wasser unten Sand oben 2
                GameObject tSlot102 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot102.GetComponent<SpriteRenderer>().sprite = waterBottomSandTop2_tile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot102;
                break;
            case 103:     // Drei Sand, Wasser Rechts
                GameObject tSlot103 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot103.GetComponent<SpriteRenderer>().sprite = threeSandOneWaterRight_tile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot103;
                break;
            case 104:     // Drei Sand Wasser Links
                GameObject tSlot104 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot104.GetComponent<SpriteRenderer>().sprite = threeSandOneWaterLeft_tile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot104;
                break;
            case 105:     // Drei Sand Wasser Links
                GameObject tSlot105 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot105.GetComponent<SpriteRenderer>().sprite = threeWaterOneSandRight_tile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot105;
                break;
            case 106:     // Drei Sand Wasser Links
                GameObject tSlot106 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot106.GetComponent<SpriteRenderer>().sprite = threeWaterOneSandLeft_tile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot106;
                break;
            default:
                break;
        }

    }

    void StructureOffsetsAndHeight()
    {
        // No Function just fix offset and height as starting value
        int[] modifiedValues = new int[xMax];

        int modifierNumber = 2;

        switch (modifierNumber)
        {
            case 1:
                // Linear zunehmend
                for (int i = 0; i < modifiedValues.Length; i++) modifiedValues[i] = (int)Mathf.Round(i * 0.20f);
                break;
            case 2:
                // Linear abnehmend
                for (int i = 0; i < modifiedValues.Length; i++) modifiedValues[i] = (int)Mathf.Round((xMax - i) * 0.20f);
                break;
            case 3:
                // Parabolisch zunehmend
                for (int i = 0; i < modifiedValues.Length; i++) modifiedValues[i] = (int)Mathf.Round(0 + Mathf.Pow(i * Mathf.Sqrt(10) / xMax, 2));
                break;
            case 4:
                // Parabolisch abnehmend
                for (int i = 0; i < modifiedValues.Length; i++) modifiedValues[i] = (int)Mathf.Round(10 - Mathf.Pow(i * Mathf.Sqrt(10) / xMax, 2));
                break;
            case 5:
                // Parabolisch mittig zu seiten hin zunehmend
                for (int i = 0; i < modifiedValues.Length; i++) modifiedValues[i] = (int)Mathf.Round(0 + Mathf.Pow((i - xMax / 2) * Mathf.Sqrt(29) / xMax, 2f));
                break;
            case 6:
                // Parabolisch mittig zu seiten hin abnehmend
                for (int i = 0; i < modifiedValues.Length; i++) modifiedValues[i] = (int)Mathf.Round(6 - Mathf.Pow((i - xMax / 2) * Mathf.Sqrt(29) / xMax, 2f));
                break;
            default:
                break;
        }


        // Anpassen der H�hen und der Offsets basierend auf der gegebenen modifiedValues Funktion
        waterHeightArray = new int[xMax];
        for (int i = 0; i < waterHeightArray.Length; i++) waterHeightArray[i] = waterHeight + modifiedValues[i];

        sandHeightArray = new int[xMax];
        for (int i = 0; i < sandHeightArray.Length; i++) sandHeightArray[i] = sandHeight;
        sandOffsetArray = new int[xMax];
        for (int i = 0; i < sandOffsetArray.Length; i++) sandOffsetArray[i] = sandOffset + modifiedValues[i];

        lightGrassHeightArray = new int[xMax];
        for (int i = 0; i < lightGrassHeightArray.Length; i++) lightGrassHeightArray[i] = lightGrassHeight;
        lightGrassOffsetArray = new int[xMax];
        for (int i = 0; i < lightGrassOffsetArray.Length; i++) lightGrassOffsetArray[i] = lightGrassOffset + modifiedValues[i];

        darkGrassHeightArray = new int[xMax];
        for (int i = 0; i < darkGrassHeightArray.Length; i++) darkGrassHeightArray[i] = darkGrassHeight;
        darkGrassOffsetArray = new int[xMax];
        for (int i = 0; i < darkGrassOffsetArray.Length; i++) darkGrassOffsetArray[i] = darkGrassOffset + modifiedValues[i];
    }

    void GenerateRoughTilemap()
    {
        // Erstelle Wasser mit einer H�he von "waterHeight"
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = 0; yValue < waterHeightArray[xValue]; yValue++)
            {
                Vector3 position = new Vector3(xValue, yValue, 0);
                PaintTile(position, 100); // Wasser
                tileArray[xValue, yValue] = 100;
            }
        }

        // Erstelle Sand mit einer H�he von "sandHeight"
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = sandOffsetArray[xValue]; yValue < sandOffsetArray[xValue] + sandHeightArray[xValue]; yValue++)
            {
                Vector3 position = new Vector3(xValue, yValue, 0);
                PaintTile(position, 200); // Sand
                tileArray[xValue, yValue] = 200;
            }
        }

        // Erstelle d�nnes Gras mit einer H�he von "lightGrassHeight"
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = lightGrassOffsetArray[xValue]; yValue < lightGrassOffsetArray[xValue] + lightGrassHeightArray[xValue]; yValue++)
            {
                Vector3 position = new Vector3(xValue, yValue,  0);
                PaintTile(position, 300); // D�nnes Gras
                tileArray[xValue, yValue] = 300;
            }
        }

        // Erstelle dickes Gras mit einer H�he von "darkGrassHeight"
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = darkGrassOffsetArray[xValue]; yValue < Mathf.Min(darkGrassOffsetArray[xValue] + darkGrassHeightArray[xValue], yMax); yValue++)
            {
                Vector3 position = new Vector3(xValue, yValue, 0);
                PaintTile(position, 400); // Dickes Gras
                tileArray[xValue, yValue] = 400;
            }
        }
    }

    void CreateRoughBorders()
    {
        // Grober �bergang zwischen Wasser und Sand
        int borderValue = Random.Range(waterHeightArray[0] + 1, sandOffsetArray[0]); // Zuf�lliger Wert f�r den �bergang zwischen den harten Grenzen
        int borderValueBefore = borderValue;                            // Vergleichswert um zu sehen ob der zuf�llige Wert gr��er oder kleiner wurde.
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            // Wenn der zuf�llige Wert nicht an den Grenzen anst��t und sich in der vorherigen Runde nicht ge�ndert hat, darf er um eins gr��er oder kleiner werden oder gleich bleiben.
            if (borderValue > waterHeightArray[xValue] + 1 && borderValue < sandOffsetArray[xValue] - 1 && borderValue - borderValueBefore == 0) { borderValueBefore = borderValue; borderValue += Random.Range(-1, 2); }

            // Wenn der zuf�llige Wert nicht an der unteren Grenze anst��t und er in vorherigen Runde nicht gr��er wurde, darf er kleiner werden oder gleich bleiben
            else if (borderValue > waterHeightArray[xValue] + 1 && borderValue - borderValueBefore != 1) { borderValueBefore = borderValue; borderValue += Random.Range(-1, 1); }

            // Wenn der zuf�llige Wert nicht an der oberen Grenze anst��t und er in vorherigen Runde nicht kleiner wurde, darf er gr��er werden oder gleich bleiben
            else if (borderValue < sandOffsetArray[xValue] - 1 && borderValue - borderValueBefore != -1) { borderValueBefore = borderValue; borderValue += Random.Range(0, 2); }

            // Wenn keines der Kriterien erf�llt ist, bleibt er gleich
            else borderValueBefore = borderValue;

            // Alle Felder unterhalb des Zufallswertes werden Wasser
            for (int yValue = waterHeightArray[xValue]; yValue < borderValue - 1; yValue++)
            {
                if (yValue < sandOffsetArray[xValue] - 2)
                {
                    Vector3 position = new Vector3(xValue, yValue, 0);
                    PaintTile(position, 100); // Wasser
                    tileArray[xValue, yValue] = 100;
                }
            }

            // Alle �berhalb des Zufallswertes werden Sand
            for (int yValue = borderValue + 1; yValue < sandOffsetArray[xValue]; yValue++)
            {
                if (yValue > waterHeightArray[xValue] + 1)
                {
                    Vector3 position = new Vector3(xValue, yValue, 0);
                    PaintTile(position, 200); // Sand
                    tileArray[xValue, yValue] = 200;
                }
            }
        }

        // Grober �bergang zwischen Sand und d�nnem Gras
        borderValue = Random.Range(sandOffsetArray[0] + sandHeightArray[0] + 1, lightGrassOffsetArray[0]); // Zuf�lliger Wert f�r den �bergang zwischen den harten Grenzen
        borderValueBefore = borderValue;                                             // Vergleichswert um zu sehen ob der zuf�llige Wert gr��er oder kleiner wurde.
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            // Wenn der zuf�llige Wert nicht an den Grenzen anst��t und sich in der vorherigen Runde nicht ge�ndert hat, darf er um eins gr��er oder kleiner werden oder gleich bleiben.
            if (borderValue > sandOffsetArray[xValue] + sandHeightArray[xValue] + 1 && borderValue < lightGrassOffsetArray[xValue] - 1 && borderValue - borderValueBefore == 0) { borderValueBefore = borderValue; borderValue += Random.Range(-1, 2); }

            // Wenn der zuf�llige Wert nicht an der unteren Grenze anst��t und er in vorherigen Runde nicht gr��er wurde, darf er kleiner werden oder gleich bleiben
            else if (borderValue > sandOffsetArray[xValue] + sandHeightArray[xValue] + 1 && borderValue - borderValueBefore != 1) { borderValueBefore = borderValue; borderValue += Random.Range(-1, 1); }

            // Wenn der zuf�llige Wert nicht an der oberen Grenze anst��t und er in vorherigen Runde nicht kleiner wurde, darf er gr��er werden oder gleich bleiben
            else if (borderValue < lightGrassOffsetArray[xValue] - 1 && borderValue - borderValueBefore != -1) { borderValueBefore = borderValue; borderValue += Random.Range(0, 2); }

            // Wenn keines der Kriterien erf�llt ist, bleibt er gleich
            else borderValueBefore = borderValue;

            // Alle Felder unterhalb des Zufallswertes werden Sand
            for (int yValue = sandOffsetArray[xValue] + sandHeightArray[xValue]; yValue < borderValue - 1; yValue++)
            {
                if (yValue < lightGrassOffsetArray[xValue] - 2)
                {
                    Vector3 position = new Vector3(xValue, yValue, 0);
                    PaintTile(position, 200); // Sand
                    tileArray[xValue, yValue] = 200;
                }
            }

            // Alle �berhalb des Zufallswertes werden D�nnes Gras
            for (int yValue = borderValue + 1; yValue < lightGrassOffsetArray[xValue]; yValue++)
            {
                if (yValue > sandOffsetArray[xValue] + sandHeightArray[xValue] + 1)
                {
                    Vector3 position = new Vector3(xValue, yValue, 0);
                    PaintTile(position, 300); // D�nnes Gras
                    tileArray[xValue, yValue] = 300;
                }
            }
        }

        // Grober �bergang zwischen d�nnem und dickem Gras
        borderValue = Random.Range(lightGrassOffsetArray[0] + lightGrassHeightArray[0] + 1, darkGrassOffsetArray[0]);       // Zuf�lliger Wert f�r den �bergang zwischen den harten Grenzen
        borderValueBefore = borderValue;                                                                // Vergleichswert um zu sehen ob der zuf�llige Wert gr��er oder kleiner wurde.
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            // Wenn der zuf�llige Wert nicht an den Grenzen anst��t und sich in der vorherigen Runde nicht ge�ndert hat, darf er um eins gr��er oder kleiner werden oder gleich bleiben.
            if (borderValue > lightGrassOffsetArray[xValue] + lightGrassHeightArray[xValue] + 1 && borderValue < darkGrassOffsetArray[xValue] - 1 && borderValue - borderValueBefore == 0) { borderValueBefore = borderValue; borderValue += Random.Range(-1, 2); }

            // Wenn der zuf�llige Wert nicht an der unteren Grenze anst��t und er in vorherigen Runde nicht gr��er wurde, darf er kleiner werden oder gleich bleiben
            else if (borderValue > lightGrassOffsetArray[xValue] + lightGrassHeightArray[xValue] + 1 && borderValue - borderValueBefore != 1) { borderValueBefore = borderValue; borderValue += Random.Range(-1, 1); }

            // Wenn der zuf�llige Wert nicht an der oberen Grenze anst��t und er in vorherigen Runde nicht kleiner wurde, darf er gr��er werden oder gleich bleiben
            else if (borderValue < darkGrassOffsetArray[xValue] - 1 && borderValue - borderValueBefore != -1) { borderValueBefore = borderValue; borderValue += Random.Range(0, 2); }

            // Wenn keines der Kriterien erf�llt ist, bleibt er gleich
            else borderValueBefore = borderValue;

            // Alle Felder unterhalb des Zufallswertes werden d�nnes Gras
            for (int yValue = lightGrassOffsetArray[xValue] + lightGrassHeightArray[xValue]; yValue < borderValue - 1; yValue++)
            {
                if (yValue < darkGrassOffsetArray[xValue] - 2)
                {
                    Vector3 position = new Vector3(xValue, yValue, 0);
                    PaintTile(position, 300); // D�nnes Gras
                    tileArray[xValue, yValue] = 300;
                }
            }

            // Alle �berhalb des Zufallswertes werden dickes Gras
            for (int yValue = borderValue + 1; yValue < darkGrassOffsetArray[xValue]; yValue++)
            {
                if (yValue > lightGrassOffsetArray[xValue] + lightGrassHeightArray[xValue] + 1)
                {
                    Vector3 position = new Vector3(xValue, yValue, 0);
                    PaintTile(position, 400); // Dickes Gras
                    tileArray[xValue, yValue] = 400;
                }
            }
        }
    }

    void RemoveArtefacts1()
    {
        for (int xValue = 1; xValue < xMax - 1; xValue++)
        {
            for (int yValue = 1; yValue < yMax - 1; yValue++)
            {
                if (tileArray[xValue, yValue] == 0) // Noch kein Tile an der entsprechenden Stelle
                {

                    int counter = 0;
                    if (tileArray[xValue + 1, yValue] != 0) counter += 1;
                    if (tileArray[xValue -1, yValue] != 0) counter += 1;
                    if (tileArray[xValue, yValue + 1] != 0) counter += 1;
                    if (tileArray[xValue, yValue - 1] != 0) counter += 1;

                    if (counter >=3)
                    {
                        tileArray[xValue, yValue] = tileArray[xValue + 1, yValue];
                        PaintTile(new Vector3(xValue, yValue, 0), tileArray[xValue, yValue]);
                    }

                }
                else // Es gibt ein Tile an der entsprechenden Stelle
                {
                    int counter = 0;
                    if (tileArray[xValue + 1, yValue] == 0) counter += 1;
                    if (tileArray[xValue - 1, yValue] == 0) counter += 1;
                    if (tileArray[xValue, yValue + 1] == 0) counter += 1;
                    if (tileArray[xValue, yValue - 1] == 0) counter += 1;

                    if (counter >= 3)
                    {
                        tileArray[xValue, yValue] = 0;
                        Destroy(tokenSlotReferenceArray[xValue, yValue].gameObject);
                        tokenSlotReferenceArray[xValue, yValue] = null;
                    }

                }


            }
        }
    }

    void FillWaterSandBorder()
    {
        // Es werden immer zwei Linien Platz gelassen, da es bei schr�ger Tile-F�hrung n�tig ist. 
        // Wenn die Wasser und Sand Linie lokal horizontal verl�uft, braucht man nur eine Reihe Platz. --> An diesen Stellen wird mit Sand aufgef�llt.
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = 0; yValue < yMax; yValue++)
            {
                if (tileArray[xValue, yValue] == 0) // Alle Tiles die noch nicht belegt sind
                {
                    if (tileArray[xValue, yValue + 1] == 200) // �ber dem Tile ist Sand
                    {
                        if ((xValue < xMax - 1 && tileArray[xValue + 1, yValue - 1] == 0) && (xValue > 0 && tileArray[xValue - 1, yValue - 1] == 0)) // Nicht am Rand und Links und rechts unter dem Tile ist leer.
                        {
                            PaintTile(new Vector3(xValue, yValue, 0), 200);
                            tileArray[xValue, yValue] = 200;
                            
                        }
                        else if (xValue == xMax - 1 && tileArray[xValue - 1, yValue - 1] == 0) // Am Rechten Rand, Links unten ist leer
                        {
                            PaintTile(new Vector3(xValue, yValue, 0), 200);
                            tileArray[xValue, yValue] = 200;
                        }
                        else if (xValue == 0 && tileArray[xValue + 1, yValue - 1] == 0) // Am linken Rand, rechts unten ist leer
                        {
                             PaintTile(new Vector3(xValue, yValue, 0), 200);
                             tileArray[xValue, yValue] = 200;
                        }
                    }
                }
            }
        }

        //Andersrum noch mit wasser auff�llen
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = 0; yValue < yMax; yValue++)
            {
                if (tileArray[xValue, yValue] == 0) // Tile ist noch leer
                {
                    if (tileArray[xValue, yValue - 1] == 100) // Unter dem Tile ist Wasser
                    {
                        if ((xValue < xMax - 1 && tileArray[xValue + 1, yValue + 1] == 0) && (xValue > 0 && tileArray[xValue - 1, yValue + 1] == 0) && tileArray[xValue, yValue + 1] == 0) // Links und rechts unter dem Tile ist leer.
                        {
                            PaintTile(new Vector3(xValue, yValue, 0), 100);
                            tileArray[xValue, yValue] = 100;

                        }
                        else if (xValue == xMax - 1 && tileArray[xValue - 1, yValue + 1] == 0 && tileArray[xValue, yValue + 1] == 0) // Am Rechten Rand, Links oben ist leer
                        {
                            PaintTile(new Vector3(xValue, yValue, 0), 100);
                            tileArray[xValue, yValue] = 100;
                        }
                        else if (xValue == 0 && tileArray[xValue + 1, yValue + 1] == 0 && tileArray[xValue, yValue + 1] == 0) // Am linken Rand, rechts oben ist leer
                        {
                            PaintTile(new Vector3(xValue, yValue, 0), 100);
                            tileArray[xValue, yValue] = 100;
                        }
                    }
                }

            }
        }

        // Als n�chstes werden die einfachen �bergange im lokal horizontalen Bereich gemacht.
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = 0; yValue < yMax; yValue++)
            {
                if (tileArray[xValue, yValue] == 0)
                {
                    if (tileArray[xValue, yValue - 1] == 100 && tileArray[xValue, yValue + 1] == 200) // Alle Tiles die �ber einem Wasser-Tile und unter einem Sand-Tile liegen
                    {
                        if (Random.Range(0, 10) > 1)
                        {
                            PaintWaterToSandTile(new Vector3(xValue, yValue), 101);
                            tileArray[xValue, yValue] = 101;
                        }
                        else
                        {
                            PaintWaterToSandTile(new Vector3(xValue, yValue), 102);
                            tileArray[xValue, yValue] = 102;
                        }
                    }
                }
            }
        }

        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = 0; yValue < yMax; yValue++)
            {
                if (tileArray[xValue, yValue] == 0)
                {
                    if (tileArray[xValue, yValue + 1] == 200)
                    {
                        if (xValue > 0 && tileArray[xValue - 1, yValue] == 200) // Alle Tiles die rechts von einem Sand-Tile liegen
                        {
                            PaintWaterToSandTile(new Vector3(xValue, yValue), 103);
                            tileArray[xValue, yValue] = 103;
                        }
                        else if (xValue < xMax - 1 && tileArray[xValue + 1, yValue] == 200)
                        {
                            PaintWaterToSandTile(new Vector3(xValue, yValue), 104);
                            tileArray[xValue, yValue] = 104;
                        }
                    }
                }
            }
        }

        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = 0; yValue < yMax; yValue++)
            {
                if (tileArray[xValue, yValue] == 0)
                {
                    if (tileArray[xValue, yValue - 1] == 100)
                    {
                        if (xValue > 0 && tileArray[xValue - 1, yValue] == 100) // Alle Tiles die rechts von einem Sand-Tile liegen
                        {
                            PaintWaterToSandTile(new Vector3(xValue, yValue), 105);
                            tileArray[xValue, yValue] = 105;
                        }
                        else if (xValue < xMax - 1 && tileArray[xValue + 1, yValue] == 100)
                        {
                            PaintWaterToSandTile(new Vector3(xValue, yValue), 106);
                            tileArray[xValue, yValue] = 106;
                        }
                    }
                }
            }
        }
    }

    void FillSandGrassLightBorder()
    {
        // Funktion sehr �hnlich wie FillWaterSandBorder, die Kommentare hier sind nur copy und paste und z�hlen nicht. Schaue die andere Funktion an f�r die Doku :)
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = 0; yValue < yMax; yValue++)
            {
                if (tileArray[xValue, yValue] == 0) // Alle Tiles die noch nicht belegt sind
                {
                    if (tileArray[xValue, yValue + 1] == 300) // �ber dem Tile ist Sand
                    {
                        if ((xValue < xMax - 1 && tileArray[xValue + 1, yValue - 1] == 0) && (xValue > 0 && tileArray[xValue - 1, yValue - 1] == 0)) // Nicht am Rand und Links und rechts unter dem Tile ist leer.
                        {
                            PaintTile(new Vector3(xValue, yValue, 0), 300);
                            tileArray[xValue, yValue] = 300;

                        }
                        else if (xValue == xMax - 1 && tileArray[xValue - 1, yValue - 1] == 0) // Am Rechten Rand, Links unten ist leer
                        {
                            PaintTile(new Vector3(xValue, yValue, 0), 300);
                            tileArray[xValue, yValue] = 300;
                        }
                        else if (xValue == 0 && tileArray[xValue + 1, yValue - 1] == 0) // Am linken Rand, rechts unten ist leer
                        {
                            PaintTile(new Vector3(xValue, yValue, 0), 300);
                            tileArray[xValue, yValue] = 300;
                        }
                    }
                }
            }
        }

        //Andersrum noch mit wasser auff�llen
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = 0; yValue < yMax; yValue++)
            {
                if (tileArray[xValue, yValue] == 0) // Tile ist noch leer
                {
                    if (tileArray[xValue, yValue - 1] == 200) // Unter dem Tile ist Wasser
                    {
                        if ((xValue < xMax - 1 && tileArray[xValue + 1, yValue + 1] == 0) && (xValue > 0 && tileArray[xValue - 1, yValue + 1] == 0) && tileArray[xValue, yValue + 1] == 0) // Links und rechts unter dem Tile ist leer.
                        {
                            PaintTile(new Vector3(xValue, yValue, 0), 200);
                            tileArray[xValue, yValue] = 200;

                        }
                        else if (xValue == xMax - 1 && tileArray[xValue - 1, yValue + 1] == 0 && tileArray[xValue, yValue + 1] == 0) // Am Rechten Rand, Links oben ist leer
                        {
                            PaintTile(new Vector3(xValue, yValue, 0), 200);
                            tileArray[xValue, yValue] = 200;
                        }
                        else if (xValue == 0 && tileArray[xValue + 1, yValue + 1] == 0 && tileArray[xValue, yValue + 1] == 0) // Am linken Rand, rechts oben ist leer
                        {
                            PaintTile(new Vector3(xValue, yValue, 0), 200);
                            tileArray[xValue, yValue] = 200;
                        }
                    }
                }

            }
        }

        // Als n�chstes werden die einfachen �bergange im lokal horizontalen Bereich gemacht.
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = 0; yValue < yMax; yValue++)
            {
                if (tileArray[xValue, yValue] == 0)
                {
                    if (tileArray[xValue, yValue - 1] == 200 && tileArray[xValue, yValue + 1] == 300) // Alle Tiles die �ber einem Wasser-Tile und unter einem Sand-Tile liegen
                    {
                        if (Random.Range(0, 10) > 1)
                        {
                            PaintSandToLightGrassTile(new Vector3(xValue, yValue), 201);
                            tileArray[xValue, yValue] = 201;
                        }
                        else
                        {
                            PaintSandToLightGrassTile(new Vector3(xValue, yValue), 202);
                            tileArray[xValue, yValue] = 202;
                        }
                    }
                }
            }
        }

        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = 0; yValue < yMax; yValue++)
            {
                if (tileArray[xValue, yValue] == 0)
                {
                    if (tileArray[xValue, yValue + 1] == 300)
                    {
                        if (xValue > 0 && tileArray[xValue - 1, yValue] == 300) // Alle Tiles die rechts von einem Sand-Tile liegen
                        {
                            PaintSandToLightGrassTile(new Vector3(xValue, yValue), 204);
                            tileArray[xValue, yValue] = 204;
                        }
                        else if (xValue < xMax - 1 && tileArray[xValue + 1, yValue] == 300)
                        {
                            PaintSandToLightGrassTile(new Vector3(xValue, yValue), 205);
                            tileArray[xValue, yValue] = 205;
                        }
                    }
                }
            }
        }

        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = 0; yValue < yMax; yValue++)
            {
                if (tileArray[xValue, yValue] == 0)
                {
                    if (tileArray[xValue, yValue - 1] == 200)
                    {
                        if (xValue > 0 && tileArray[xValue - 1, yValue] == 200) // Alle Tiles die rechts von einem Sand-Tile liegen
                        {
                            PaintSandToLightGrassTile(new Vector3(xValue, yValue), 206);
                            tileArray[xValue, yValue] = 206;
                        }
                        else if (xValue < xMax - 1 && tileArray[xValue + 1, yValue] == 200)
                        {
                            PaintSandToLightGrassTile(new Vector3(xValue, yValue), 207);
                            tileArray[xValue, yValue] = 207;
                        }
                    }
                }
            }
        }
    }
}
