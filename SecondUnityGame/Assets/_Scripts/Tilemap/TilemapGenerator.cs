using UnityEngine;

public class TilemapGenerator : MonoBehaviour
{
    #region Generation-params
    int xMax = 80;                   // Breite der Map in Anzahl der Tiles
    int yMax = 55;                   // Höhe der Map " " " "

    // Die Anfangshöhe und offsets der einzelnen Streifen lassen sich im Nachhinein noch durch funktionen beeinflussen
    int waterHeight = 3;             // Dicke des Wasserstreifens
    int[] waterHeightArray;

    int sandHeight = 6;              // Dicke des Sandstreifens
    int[] sandHeightArray;
    int sandOffset = 9;              // Offset des Sandstreifens
    int[] sandOffsetArray;

    int lightGrassHeight = 7;        // Dicke des dünnen Grasstreifens
    int[] lightGrassHeightArray;
    int lightGrassOffset = 21;       // Offset des dünnen Grasstreifens
    int[] lightGrassOffsetArray;

    int darkGrassHeight = 19;        // Dicke des dicken Grasstreifens
    int[] darkGrassHeightArray;
    int darkGrassOffset = 38;        // Offset des dicken Grasstreifens
    int[] darkGrassOffsetArray;
    #endregion

    int[,] tileArray;       // Bedeutung des Arrays: 100 = Wasser, 200 = Sand, 300 = dünnes Gras, 400 = dickes Gras
    int[,] extrasArray;     // Bedeutung: 100 = Baum (Meistens 2 Felder Hoch), 200 = Stein / Erz
    GameObject[,] tokenSlotReferenceArray;

    [SerializeField] GameObject tokenSlot;

    // Main Tiles
    [SerializeField] Sprite waterTile, grassLightTile, grassDarkTile, sandTile;

    // Border Tiles
    [SerializeField] Sprite waterBottomSandTop1_tile, waterBottomSandTop2_tile, threeSandOneWaterRight_tile, threeSandOneWaterLeft_tile, threeWaterOneSandRight_tile, threeWaterOneSandLeft_tile;
    [SerializeField] Sprite sandBottomLightGrassTop1_tile, sandBottomLightGrassTop2_tile, sandBottomLightGrassTop3_tile, threeGrassLightOneSandRight_tile, threeGrassLightOneSandLeft_tile, threeSandOneGrassLightRight_tile, threeSandOneGrassLightLeft_tile;
    [SerializeField] Sprite lightGrassBottomdarkGrassTop1_tile, threeLightGrassOneDarkGrassRight_tile, threeLightGrassOneDarkGrassLeft_tile, threeDarkGrassOneLightGrassLeft_tile, threeDarkGrassOneLightGrassRight_tile;


    // Extras Tiles
    [SerializeField]
    Sprite tree1Sprite0, tree1Sprite1, tree2Sprite0, tree2Sprite1, tree3Sprite0, tree3Sprite1, tree4Sprite0, tree4Sprite1, tree5Sprite0, tree5Sprite1,
        tree6Sprite0, tree6Sprite1, tree7Sprite0, tree7Sprite1, tree8Sprite0, tree8Sprite1;

    [SerializeField] Sprite bitStone0Sprite00, bigStone0Sprite10, bigStone0Sprite01, bigStone0Sprite11, bitStone1Sprite00, bigStone1Sprite10, bigStone1Sprite01, bigStone1Sprite11;

    [SerializeField] Sprite crystal0Sprite0, crystal0Sprite1, crystal1Sprite0, crystal1Sprite1;




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
        FillLightGrassDarkGrassBorder();

        // BaumAnzahl Chance, Gebiet (400 = dunkles Gras, 300 = helles Gras)
        AddTrees(80, 1000, 400);
        AddTrees(35, 300, 300);

        AddBigStones(5, 1000, 400);
        AddBigStones(3, 1000, 300);
        AddBigStones(2, 1000, 200);

        AddCrystals(10, 1000, 400);
        AddCrystals(3, 1000, 300);
        AddCrystals(1, 1000, 200);
    }

    void ResetArraysAndDeleteTiles()
    {
        tileArray = new int[xMax, yMax];
        extrasArray = new int[xMax, yMax];

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
            case 300:     // Dünnes Gras
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

    public void PaintLightGrassToDarkGrassTile(Vector3 position, int type)
    {
        switch (type)
        {
            case 301:    
                GameObject tSlot301 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot301.GetComponent<SpriteRenderer>().sprite = lightGrassBottomdarkGrassTop1_tile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot301;
                break;
            case 302:    
                GameObject tSlot302 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot302.GetComponent<SpriteRenderer>().sprite = threeDarkGrassOneLightGrassRight_tile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot302;
                break;
            case 303:    
                GameObject tSlot303 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot303.GetComponent<SpriteRenderer>().sprite = threeDarkGrassOneLightGrassLeft_tile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot303;
                break;
            case 304:  
                GameObject tSlot304 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot304.GetComponent<SpriteRenderer>().sprite = threeLightGrassOneDarkGrassRight_tile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot304;
                break;
            case 305:    
                GameObject tSlot305 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot305.GetComponent<SpriteRenderer>().sprite = threeLightGrassOneDarkGrassLeft_tile;
                tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)] = tSlot305;
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

    public void PaintTree(Vector3 position, int type)
    {
        Sprite mySprite1 = waterTile;
        Sprite mySprite2 = waterTile;

        switch (type)
        {
            case 101:
                mySprite1 = tree1Sprite0;
                mySprite2 = tree1Sprite1;
                break;
            case 102:
                mySprite1 = tree2Sprite0;
                mySprite2 = tree2Sprite1;
                break;
            case 103:
                mySprite1 = tree3Sprite0;
                mySprite2 = tree3Sprite1;
                break;
            case 104:
                mySprite1 = tree4Sprite0;
                mySprite2 = tree4Sprite1;
                break;
            case 105:
                mySprite1 = tree5Sprite0;
                mySprite2 = tree5Sprite1;
                break;
            case 106:
                mySprite1 = tree6Sprite0;
                mySprite2 = tree6Sprite1;
                break;
            case 107:
                mySprite1 = tree7Sprite0;
                mySprite2 = tree7Sprite1;
                break;
            case 108:
                mySprite1 = tree8Sprite0;
                mySprite2 = tree8Sprite1;
                break;
            default:
                break;
        }

        tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)].transform.Find("ExtraFeature").GetComponent<SpriteRenderer>().sprite = mySprite1;
        tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y)].transform.Find("ExtraFeature").gameObject.SetActive(true);
        tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y) + 1].transform.Find("ExtraFeature").GetComponent<SpriteRenderer>().sprite = mySprite2;
        tokenSlotReferenceArray[(int)Mathf.Round(position.x), (int)Mathf.Round(position.y) + 1].transform.Find("ExtraFeature").gameObject.SetActive(true);

    }

    void StructureOffsetsAndHeight()
    {
        // No Function just fix offset and height as starting value
        int[] modifiedValues = new int[xMax];

        int modifierNumber = 2 ;

        modifierNumber = Random.Range(0, 7);

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
                for (int i = 0; i < modifiedValues.Length; i++) modifiedValues[i] = (int)Mathf.Round(0 + Mathf.Pow((i - xMax) * Mathf.Sqrt(10) / xMax, 2));
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


        // Anpassen der Höhen und der Offsets basierend auf der gegebenen modifiedValues Funktion
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
        // Erstelle Wasser mit einer Höhe von "waterHeight"
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = 0; yValue < waterHeightArray[xValue]; yValue++)
            {
                Vector3 position = new Vector3(xValue, yValue, 0);
                PaintTile(position, 100); // Wasser
                tileArray[xValue, yValue] = 100;
            }
        }

        // Erstelle Sand mit einer Höhe von "sandHeight"
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = sandOffsetArray[xValue]; yValue < sandOffsetArray[xValue] + sandHeightArray[xValue]; yValue++)
            {
                Vector3 position = new Vector3(xValue, yValue, 0);
                PaintTile(position, 200); // Sand
                tileArray[xValue, yValue] = 200;
            }
        }

        // Erstelle dünnes Gras mit einer Höhe von "lightGrassHeight"
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = lightGrassOffsetArray[xValue]; yValue < lightGrassOffsetArray[xValue] + lightGrassHeightArray[xValue]; yValue++)
            {
                Vector3 position = new Vector3(xValue, yValue,  0);
                PaintTile(position, 300); // Dünnes Gras
                tileArray[xValue, yValue] = 300;
            }
        }

        // Erstelle dickes Gras mit einer Höhe von "darkGrassHeight"
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
        // Grober Übergang zwischen Wasser und Sand
        int borderValue = Random.Range(waterHeightArray[0] + 1, sandOffsetArray[0]); // Zufälliger Wert für den Übergang zwischen den harten Grenzen
        int borderValueBefore = borderValue;                            // Vergleichswert um zu sehen ob der zufällige Wert größer oder kleiner wurde.
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            // Wenn der zufällige Wert nicht an den Grenzen anstößt und sich in der vorherigen Runde nicht geändert hat, darf er um eins größer oder kleiner werden oder gleich bleiben.
            if (borderValue > waterHeightArray[xValue] + 1 && borderValue < sandOffsetArray[xValue] - 1 && borderValue - borderValueBefore == 0) { borderValueBefore = borderValue; borderValue += Random.Range(-1, 2); }

            // Wenn der zufällige Wert nicht an der unteren Grenze anstößt und er in vorherigen Runde nicht größer wurde, darf er kleiner werden oder gleich bleiben
            else if (borderValue > waterHeightArray[xValue] + 1 && borderValue - borderValueBefore != 1) { borderValueBefore = borderValue; borderValue += Random.Range(-1, 1); }

            // Wenn der zufällige Wert nicht an der oberen Grenze anstößt und er in vorherigen Runde nicht kleiner wurde, darf er größer werden oder gleich bleiben
            else if (borderValue < sandOffsetArray[xValue] - 1 && borderValue - borderValueBefore != -1) { borderValueBefore = borderValue; borderValue += Random.Range(0, 2); }

            // Wenn keines der Kriterien erfüllt ist, bleibt er gleich
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

            // Alle überhalb des Zufallswertes werden Sand
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

        // Grober Übergang zwischen Sand und dünnem Gras
        borderValue = Random.Range(sandOffsetArray[0] + sandHeightArray[0] + 1, lightGrassOffsetArray[0]); // Zufälliger Wert für den Übergang zwischen den harten Grenzen
        borderValueBefore = borderValue;                                             // Vergleichswert um zu sehen ob der zufällige Wert größer oder kleiner wurde.
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            // Wenn der zufällige Wert nicht an den Grenzen anstößt und sich in der vorherigen Runde nicht geändert hat, darf er um eins größer oder kleiner werden oder gleich bleiben.
            if (borderValue > sandOffsetArray[xValue] + sandHeightArray[xValue] + 1 && borderValue < lightGrassOffsetArray[xValue] - 1 && borderValue - borderValueBefore == 0) { borderValueBefore = borderValue; borderValue += Random.Range(-1, 2); }

            // Wenn der zufällige Wert nicht an der unteren Grenze anstößt und er in vorherigen Runde nicht größer wurde, darf er kleiner werden oder gleich bleiben
            else if (borderValue > sandOffsetArray[xValue] + sandHeightArray[xValue] + 1 && borderValue - borderValueBefore != 1) { borderValueBefore = borderValue; borderValue += Random.Range(-1, 1); }

            // Wenn der zufällige Wert nicht an der oberen Grenze anstößt und er in vorherigen Runde nicht kleiner wurde, darf er größer werden oder gleich bleiben
            else if (borderValue < lightGrassOffsetArray[xValue] - 1 && borderValue - borderValueBefore != -1) { borderValueBefore = borderValue; borderValue += Random.Range(0, 2); }

            // Wenn keines der Kriterien erfüllt ist, bleibt er gleich
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

            // Alle überhalb des Zufallswertes werden Dünnes Gras
            for (int yValue = borderValue + 1; yValue < lightGrassOffsetArray[xValue]; yValue++)
            {
                if (yValue > sandOffsetArray[xValue] + sandHeightArray[xValue] + 1)
                {
                    Vector3 position = new Vector3(xValue, yValue, 0);
                    PaintTile(position, 300); // Dünnes Gras
                    tileArray[xValue, yValue] = 300;
                }
            }
        }

        // Grober Übergang zwischen dünnem und dickem Gras
        borderValue = Random.Range(lightGrassOffsetArray[0] + lightGrassHeightArray[0] + 1, darkGrassOffsetArray[0]);       // Zufälliger Wert für den Übergang zwischen den harten Grenzen
        borderValueBefore = borderValue;                                                                // Vergleichswert um zu sehen ob der zufällige Wert größer oder kleiner wurde.
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            // Wenn der zufällige Wert nicht an den Grenzen anstößt und sich in der vorherigen Runde nicht geändert hat, darf er um eins größer oder kleiner werden oder gleich bleiben.
            if (borderValue > lightGrassOffsetArray[xValue] + lightGrassHeightArray[xValue] + 1 && borderValue < darkGrassOffsetArray[xValue] - 1 && borderValue - borderValueBefore == 0) { borderValueBefore = borderValue; borderValue += Random.Range(-1, 2); }

            // Wenn der zufällige Wert nicht an der unteren Grenze anstößt und er in vorherigen Runde nicht größer wurde, darf er kleiner werden oder gleich bleiben
            else if (borderValue > lightGrassOffsetArray[xValue] + lightGrassHeightArray[xValue] + 1 && borderValue - borderValueBefore != 1) { borderValueBefore = borderValue; borderValue += Random.Range(-1, 1); }

            // Wenn der zufällige Wert nicht an der oberen Grenze anstößt und er in vorherigen Runde nicht kleiner wurde, darf er größer werden oder gleich bleiben
            else if (borderValue < darkGrassOffsetArray[xValue] - 1 && borderValue - borderValueBefore != -1) { borderValueBefore = borderValue; borderValue += Random.Range(0, 2); }

            // Wenn keines der Kriterien erfüllt ist, bleibt er gleich
            else borderValueBefore = borderValue;

            // Alle Felder unterhalb des Zufallswertes werden dünnes Gras
            for (int yValue = lightGrassOffsetArray[xValue] + lightGrassHeightArray[xValue]; yValue < borderValue - 1; yValue++)
            {
                if (yValue < darkGrassOffsetArray[xValue] - 2)
                {
                    Vector3 position = new Vector3(xValue, yValue, 0);
                    PaintTile(position, 300); // Dünnes Gras
                    tileArray[xValue, yValue] = 300;
                }
            }

            // Alle überhalb des Zufallswertes werden dickes Gras
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
        // Es werden immer zwei Linien Platz gelassen, da es bei schräger Tile-Führung nötig ist. 
        // Wenn die Wasser und Sand Linie lokal horizontal verläuft, braucht man nur eine Reihe Platz. --> An diesen Stellen wird mit Sand aufgefüllt.
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = 0; yValue < yMax; yValue++)
            {
                if (tileArray[xValue, yValue] == 0) // Alle Tiles die noch nicht belegt sind
                {
                    if (tileArray[xValue, yValue + 1] == 200) // über dem Tile ist Sand
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

        //Andersrum noch mit wasser auffüllen
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

        // Als nächstes werden die einfachen Übergange im lokal horizontalen Bereich gemacht.
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = 0; yValue < yMax; yValue++)
            {
                if (tileArray[xValue, yValue] == 0)
                {
                    if (tileArray[xValue, yValue - 1] == 100 && tileArray[xValue, yValue + 1] == 200) // Alle Tiles die über einem Wasser-Tile und unter einem Sand-Tile liegen
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
        // Funktion sehr ähnlich wie FillWaterSandBorder, die Kommentare hier sind nur copy und paste und zählen nicht. Schaue die andere Funktion an für die Doku :)
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = 0; yValue < yMax; yValue++)
            {
                if (tileArray[xValue, yValue] == 0) // Alle Tiles die noch nicht belegt sind
                {
                    if (tileArray[xValue, yValue + 1] == 300) // über dem Tile ist Sand
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

        //Andersrum noch mit wasser auffüllen
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

        // Als nächstes werden die einfachen Übergange im lokal horizontalen Bereich gemacht.
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = 0; yValue < yMax; yValue++)
            {
                if (tileArray[xValue, yValue] == 0)
                {
                    if (tileArray[xValue, yValue - 1] == 200 && tileArray[xValue, yValue + 1] == 300) // Alle Tiles die über einem Wasser-Tile und unter einem Sand-Tile liegen
                    {
                        if (Random.Range(0, 10) > 1)
                        {
                            PaintSandToLightGrassTile(new Vector3(xValue, yValue), 201);
                            tileArray[xValue, yValue] = 201;
                        }
                        else if(Random.Range(0, 10) > 1)
                        {
                            PaintSandToLightGrassTile(new Vector3(xValue, yValue), 203);
                            tileArray[xValue, yValue] = 203;
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

    void FillLightGrassDarkGrassBorder()
    {
        // Funktion sehr ähnlich wie FillWaterSandBorder, die Kommentare hier sind nur copy und paste und zählen nicht. Schaue die andere Funktion an für die Doku :)
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = 0; yValue < yMax; yValue++)
            {
                if (tileArray[xValue, yValue] == 0) // Alle Tiles die noch nicht belegt sind
                {
                    if (tileArray[xValue, yValue + 1] == 400) // über dem Tile ist Sand
                    {
                        if ((xValue < xMax - 1 && tileArray[xValue + 1, yValue - 1] == 0) && (xValue > 0 && tileArray[xValue - 1, yValue - 1] == 0)) // Nicht am Rand und Links und rechts unter dem Tile ist leer.
                        {
                            PaintTile(new Vector3(xValue, yValue, 0), 400);
                            tileArray[xValue, yValue] = 400;

                        }
                        else if (xValue == xMax - 1 && tileArray[xValue - 1, yValue - 1] == 0) // Am Rechten Rand, Links unten ist leer
                        {
                            PaintTile(new Vector3(xValue, yValue, 0), 400);
                            tileArray[xValue, yValue] = 400;
                        }
                        else if (xValue == 0 && tileArray[xValue + 1, yValue - 1] == 0) // Am linken Rand, rechts unten ist leer
                        {
                            PaintTile(new Vector3(xValue, yValue, 0), 400);
                            tileArray[xValue, yValue] = 400;
                        }
                    }
                }
            }
        }

        //Andersrum noch mit wasser auffüllen
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = 0; yValue < yMax; yValue++)
            {
                if (tileArray[xValue, yValue] == 0) // Tile ist noch leer
                {
                    if (tileArray[xValue, yValue - 1] == 300) // Unter dem Tile ist Wasser
                    {
                        if ((xValue < xMax - 1 && tileArray[xValue + 1, yValue + 1] == 0) && (xValue > 0 && tileArray[xValue - 1, yValue + 1] == 0) && tileArray[xValue, yValue + 1] == 0) // Links und rechts unter dem Tile ist leer.
                        {
                            PaintTile(new Vector3(xValue, yValue, 0), 300);
                            tileArray[xValue, yValue] = 300;

                        }
                        else if (xValue == xMax - 1 && tileArray[xValue - 1, yValue + 1] == 0 && tileArray[xValue, yValue + 1] == 0) // Am Rechten Rand, Links oben ist leer
                        {
                            PaintTile(new Vector3(xValue, yValue, 0), 300);
                            tileArray[xValue, yValue] = 300;
                        }
                        else if (xValue == 0 && tileArray[xValue + 1, yValue + 1] == 0 && tileArray[xValue, yValue + 1] == 0) // Am linken Rand, rechts oben ist leer
                        {
                            PaintTile(new Vector3(xValue, yValue, 0), 300);
                            tileArray[xValue, yValue] = 300;
                        }
                    }
                }

            }
        }

        // Als nächstes werden die einfachen Übergange im lokal horizontalen Bereich gemacht.
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = 0; yValue < yMax; yValue++)
            {
                if (tileArray[xValue, yValue] == 0)
                {
                    if (tileArray[xValue, yValue - 1] == 300 && tileArray[xValue, yValue + 1] == 400) // Alle Tiles die über einem Wasser-Tile und unter einem Sand-Tile liegen
                    {
                        PaintLightGrassToDarkGrassTile(new Vector3(xValue, yValue), 301);
                        tileArray[xValue, yValue] = 301;
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
                    if (tileArray[xValue, yValue + 1] == 400)
                    {
                        if (xValue > 0 && tileArray[xValue - 1, yValue] == 400) // Alle Tiles die links von einem Sand-Tile liegen
                        {
                            PaintLightGrassToDarkGrassTile(new Vector3(xValue, yValue), 302);
                            tileArray[xValue, yValue] = 302;
                        }
                        else if (xValue < xMax - 1 && tileArray[xValue + 1, yValue] == 400)
                        {
                            PaintLightGrassToDarkGrassTile(new Vector3(xValue, yValue), 303);
                            tileArray[xValue, yValue] = 303;
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
                    if (tileArray[xValue, yValue - 1] == 300)
                    {
                        if (xValue > 0 && tileArray[xValue - 1, yValue] == 300) // Alle Tiles die links von einem Sand-Tile liegen
                        {
                            PaintLightGrassToDarkGrassTile(new Vector3(xValue, yValue), 304);
                            tileArray[xValue, yValue] = 304;
                        }
                        else if (xValue < xMax - 1 && tileArray[xValue + 1, yValue] == 300)
                        {
                            PaintLightGrassToDarkGrassTile(new Vector3(xValue, yValue), 305);
                            tileArray[xValue, yValue] = 305;
                        }
                    }
                }
            }
        }
    }

    void AddTrees(int treeCount, int chanceModifier = 1000, int area = 400)
    {
        if (treeCount == 0) return;
        int newTreeCount = treeCount;

        for (int yValue = yMax - 1; yValue >= 0; yValue--)
        {
            for (int xValue = 0; xValue < xMax; xValue++)
            {
                if (yValue < yMax - 1 && tileArray[xValue,yValue] == area && (tileArray[xValue, yValue + 1] == area || (tileArray[xValue, yValue + 1] == 301))) // Wenn dicker Grasboden auf einem Feld und dem darüber ist
                {
                    if (extrasArray[xValue, yValue] == 0 && extrasArray[xValue, yValue + 1] == 0)
                    {
                        if (Random.Range(0, chanceModifier) == 0)
                        {
                            int treeType = Random.Range(1,9);
                            switch (treeType)
                            {
                                case 1:
                                    PaintTree(new Vector3(xValue, yValue, 0), 101);
                                    extrasArray[xValue, yValue] = 101;
                                    extrasArray[xValue, yValue + 1] = 101;
                                    break;
                                case 2:
                                    PaintTree(new Vector3(xValue, yValue, 0), 102);
                                    extrasArray[xValue, yValue] = 102;
                                    extrasArray[xValue, yValue + 1] = 102;
                                    break;
                                case 3:
                                    PaintTree(new Vector3(xValue, yValue, 0), 103);
                                    extrasArray[xValue, yValue] = 103;
                                    extrasArray[xValue, yValue + 1] = 103;
                                    break;
                                case 4:
                                    PaintTree(new Vector3(xValue, yValue, 0), 104);
                                    extrasArray[xValue, yValue] = 104;
                                    extrasArray[xValue, yValue + 1] = 104;
                                    break;
                                case 5:
                                    PaintTree(new Vector3(xValue, yValue, 0), 105);
                                    extrasArray[xValue, yValue] = 105;
                                    extrasArray[xValue, yValue + 1] = 105;
                                    break;
                                case 6:
                                    PaintTree(new Vector3(xValue, yValue, 0), 106);
                                    extrasArray[xValue, yValue] = 106;
                                    extrasArray[xValue, yValue + 1] = 106;
                                    break;
                                case 7:
                                    PaintTree(new Vector3(xValue, yValue, 0), 107);
                                    extrasArray[xValue, yValue] = 107;
                                    extrasArray[xValue, yValue + 1] = 107;
                                    break;
                                case 8:
                                    PaintTree(new Vector3(xValue, yValue, 0), 108);
                                    extrasArray[xValue, yValue] = 108;
                                    extrasArray[xValue, yValue + 1] = 108;
                                    break;
                                default:
                                    break;
                            }

                            newTreeCount -= 1;

                            goto ThisLoopEnds;
                        }

                    }
                }
            }
        }
        ThisLoopEnds:
            AddTrees(newTreeCount, chanceModifier, area);
    }

    void AddBigStones(int count, int chanceModifier = 1000, int area = 400)
    {
        if (count == 0) return;
        int newCount = count;

        for (int yValue = yMax - 1; yValue >= 0; yValue--)
        {
            for (int xValue = 0; xValue < xMax; xValue++)
            {
                if (yValue < yMax - 1 && xValue < xMax - 1 && tileArray[xValue, yValue] == area && (tileArray[xValue + 1, yValue] == area || (tileArray[xValue + 1, yValue] == 301) || (tileArray[xValue + 1, yValue] == 201))) // Wenn dicker Grasboden auf einem Feld und dem darüber ist
                {
                    if ((tileArray[xValue, yValue + 1] == area || (tileArray[xValue, yValue + 1] == 301) || (tileArray[xValue, yValue + 1] == 201)) && (tileArray[xValue + 1, yValue + 1] == area || (tileArray[xValue + 1, yValue + 1] == 301) || (tileArray[xValue + 1, yValue + 1] == 201)))
                    {
                        if (extrasArray[xValue, yValue] == 0 && extrasArray[xValue, yValue + 1] == 0 && extrasArray[xValue + 1, yValue] == 0 && extrasArray[xValue + 1, yValue + 1] == 0)
                        {
                            if (Random.Range(0, chanceModifier) == 0)
                            {
                                int treeType = Random.Range(1, 3);
                                switch (treeType)
                                { 
                                    case 1:
                                        extrasArray[xValue, yValue] = 201;
                                        extrasArray[xValue + 1, yValue] = 201;
                                        extrasArray[xValue, yValue + 1] = 201;
                                        extrasArray[xValue + 1, yValue + 1] = 201;
                                        tokenSlotReferenceArray[xValue, yValue].transform.Find("ExtraFeature").GetComponent<SpriteRenderer>().sprite = bitStone0Sprite00;
                                        tokenSlotReferenceArray[xValue, yValue].transform.Find("ExtraFeature").gameObject.SetActive(true);
                                        tokenSlotReferenceArray[xValue, yValue + 1].transform.Find("ExtraFeature").GetComponent<SpriteRenderer>().sprite = bigStone0Sprite01;
                                        tokenSlotReferenceArray[xValue, yValue + 1].transform.Find("ExtraFeature").gameObject.SetActive(true);

                                        tokenSlotReferenceArray[xValue + 1, yValue].transform.Find("ExtraFeature").GetComponent<SpriteRenderer>().sprite = bigStone0Sprite10;
                                        tokenSlotReferenceArray[xValue + 1, yValue].transform.Find("ExtraFeature").gameObject.SetActive(true);
                                        tokenSlotReferenceArray[xValue + 1, yValue + 1].transform.Find("ExtraFeature").GetComponent<SpriteRenderer>().sprite = bigStone0Sprite11;
                                        tokenSlotReferenceArray[xValue + 1, yValue + 1].transform.Find("ExtraFeature").gameObject.SetActive(true);
                                        break;
                                    case 2:
                                        extrasArray[xValue, yValue] = 202;
                                        extrasArray[xValue + 1, yValue] = 202;
                                        extrasArray[xValue, yValue + 1] = 202;
                                        extrasArray[xValue + 1, yValue + 1] = 202;
                                        tokenSlotReferenceArray[xValue, yValue].transform.Find("ExtraFeature").GetComponent<SpriteRenderer>().sprite = bitStone1Sprite00;
                                        tokenSlotReferenceArray[xValue, yValue].transform.Find("ExtraFeature").gameObject.SetActive(true);
                                        tokenSlotReferenceArray[xValue, yValue + 1].transform.Find("ExtraFeature").GetComponent<SpriteRenderer>().sprite = bigStone1Sprite01;
                                        tokenSlotReferenceArray[xValue, yValue + 1].transform.Find("ExtraFeature").gameObject.SetActive(true);

                                        tokenSlotReferenceArray[xValue + 1, yValue].transform.Find("ExtraFeature").GetComponent<SpriteRenderer>().sprite = bigStone1Sprite10;
                                        tokenSlotReferenceArray[xValue + 1, yValue].transform.Find("ExtraFeature").gameObject.SetActive(true);
                                        tokenSlotReferenceArray[xValue + 1, yValue + 1].transform.Find("ExtraFeature").GetComponent<SpriteRenderer>().sprite = bigStone1Sprite11;
                                        tokenSlotReferenceArray[xValue + 1, yValue + 1].transform.Find("ExtraFeature").gameObject.SetActive(true);
                                        break;
                                    default:
                                        break;
                                }

                                newCount -= 1;

                                goto ThisLoopEnds;
                            }

                        }
                    }
                }
            }
        }
    ThisLoopEnds:
        AddBigStones(newCount, chanceModifier, area);
    }

    void AddCrystals(int crystalCount, int chanceModifier = 1000, int area = 400)
    {
        if (crystalCount == 0) return;
        int newCrystalCount = crystalCount;

        for (int yValue = yMax - 1; yValue >= 0; yValue--)
        {
            for (int xValue = 0; xValue < xMax; xValue++)
            {
                if (yValue < yMax - 1 && tileArray[xValue, yValue] == area && (tileArray[xValue, yValue + 1] == area || (tileArray[xValue, yValue + 1] == 301) || (tileArray[xValue, yValue + 1] == 201))) // Wenn dicker Grasboden auf einem Feld und dem darüber ist
                {
                    if (extrasArray[xValue, yValue] == 0 && extrasArray[xValue, yValue + 1] == 0)
                    {
                        if (Random.Range(0, chanceModifier) == 0)
                        {
                            int treeType = Random.Range(1, 3);
                            switch (treeType)
                            {
                                case 1:
                                    extrasArray[xValue, yValue] = 301;
                                    extrasArray[xValue, yValue + 1] = 301;
                                    tokenSlotReferenceArray[xValue, yValue].transform.Find("ExtraFeature").GetComponent<SpriteRenderer>().sprite = crystal0Sprite0;
                                    tokenSlotReferenceArray[xValue, yValue].transform.Find("ExtraFeature").gameObject.SetActive(true);
                                    tokenSlotReferenceArray[xValue, yValue + 1].transform.Find("ExtraFeature").GetComponent<SpriteRenderer>().sprite = crystal0Sprite1;
                                    tokenSlotReferenceArray[xValue, yValue + 1].transform.Find("ExtraFeature").gameObject.SetActive(true);
                                    break;
                                case 2:
                                    extrasArray[xValue, yValue] = 302;
                                    extrasArray[xValue, yValue + 1] = 302;
                                    tokenSlotReferenceArray[xValue, yValue].transform.Find("ExtraFeature").GetComponent<SpriteRenderer>().sprite = crystal1Sprite0;
                                    tokenSlotReferenceArray[xValue, yValue].transform.Find("ExtraFeature").gameObject.SetActive(true);
                                    tokenSlotReferenceArray[xValue, yValue + 1].transform.Find("ExtraFeature").GetComponent<SpriteRenderer>().sprite = crystal1Sprite1;
                                    tokenSlotReferenceArray[xValue, yValue + 1].transform.Find("ExtraFeature").gameObject.SetActive(true);
                                    break;
                                default:
                                    break;
                            }

                            newCrystalCount -= 1;

                            goto ThisLoopEnds;
                        }

                    }
                }
            }
        }
    ThisLoopEnds:
        AddCrystals(newCrystalCount, chanceModifier, area);
    }
}