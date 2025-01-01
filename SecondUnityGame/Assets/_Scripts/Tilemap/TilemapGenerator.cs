using UnityEngine;

public class TilemapGenerator : MonoBehaviour
{
    #region Generation-params
    int xMax = 80;                   // Breite der Map in Anzahl der Tiles
    int yMax = 45;                   // H�he der Map " " " "

    int waterHeight = 3;             // Dicke des Wasserstreifens

    int sandHeight = 5;              // Dicke des Sandstreifens
    int sandOffset = 8;              // Offset des Sandstreifens

    int lightGrassHeight = 8;        // Dicke des d�nnen Grasstreifens
    int lightGrassOffset = 18;       // Offset des d�nnen Grasstreifens

    int darkGrassHeight = 14;        // Dicke des dicken Grasstreifens
    int darkGrassOffset = 31;        // Offset des dicken Grasstreifens
    #endregion

    int[,] tileArray;       // Bedeutung des Arrays: 0 = Wasser, 1 = Sand, 2 = d�nnes Gras, 3 = dickes Gras

    [SerializeField] Sprite waterTile, grassLightTile, grassDarkTile, sandTile;
    [SerializeField] GameObject tokenSlot;
    [SerializeField] Vector3 generationVector;

    private void Start()
    {
        tileArray = new int[xMax, yMax];

        tokenSlot = transform.parent.Find("TokenSlot_prefabs").Find("TokenSlot").gameObject;

        GenerateRoughTilemap();
        CorrectBorders();
    }

    public void PaintTile( Vector3 position, int type)
    {
        switch (type)
        {
            case 0:     // Wasser
                GameObject tSlot0 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot0.GetComponent<SpriteRenderer>().sprite = waterTile;
                break;
            case 1:     // Sand
                GameObject tSlot1 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot1.GetComponent<SpriteRenderer>().sprite = sandTile;
                break;
            case 2:     // D�nnes Gras
                GameObject tSlot2 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot2.GetComponent<SpriteRenderer>().sprite = grassLightTile;
                break;
            case 3:     // Dickes Gras
                GameObject tSlot3 = Instantiate(tokenSlot, position, Quaternion.identity, transform.parent.Find("Grid"));
                tSlot3.GetComponent<SpriteRenderer>().sprite = grassDarkTile;
                break;
            default:
                break;
        }

    }

    void GenerateRoughTilemap()
    {
        // Erstelle Wasser mit einer H�he von "waterHeight"
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = 0; yValue < waterHeight; yValue++)
            {
                Vector3 position = new Vector3(xValue, yValue, 0);
                PaintTile(position, 0); // Wasser
                tileArray[xValue, yValue] = 0;
            }
        }

        // Erstelle Sand mit einer H�he von "sandHeight"
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = sandOffset; yValue < sandOffset + sandHeight; yValue++)
            {
                Vector3 position = new Vector3(xValue, yValue, 0);
                PaintTile(position, 1); // Sand
                tileArray[xValue, yValue] = 1;
            }
        }

        // Erstelle d�nnes Gras mit einer H�he von "lightGrassHeight"
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = lightGrassOffset; yValue < lightGrassOffset + lightGrassHeight; yValue++)
            {
                Vector3 position = new Vector3(xValue, yValue, 0);
                PaintTile(position, 2); // D�nnes Gras
                tileArray[xValue, yValue] = 2;
            }
        }

        // Erstelle dickes Gras mit einer H�he von "darkGrassHeight"
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            for (int yValue = darkGrassOffset; yValue < darkGrassOffset + darkGrassHeight; yValue++)
            {
                Vector3 position = new Vector3(xValue, yValue, 0);
                PaintTile( position, 3); // Dickes Gras
                tileArray[xValue, yValue] = 3;
            }
        }

        // Grober �bergang zwischen Wasser und Sand
        int borderValue = Random.Range(waterHeight + 1, sandOffset + 1); // Zuf�lliger Wert f�r den �bergang zwischen den harten Grenzen
        int borderValueBefore = borderValue;                            // Vergleichswert um zu sehen ob der zuf�llige Wert gr��er oder kleiner wurde.
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            // Wenn der zuf�llige Wert nicht an den Grenzen anst��t und sich in der vorherigen Runde nicht ge�ndert hat, darf er um eins gr��er oder kleiner werden oder gleich bleiben.
            if (borderValue > waterHeight + 1 && borderValue < sandOffset && borderValue - borderValueBefore == 0) { borderValueBefore = borderValue; borderValue += Random.Range(-1, 2); }

            // Wenn der zuf�llige Wert nicht an der unteren Grenze anst��t und er in vorherigen Runde nicht gr��er wurde, darf er kleiner werden oder gleich bleiben
            else if (borderValue > waterHeight + 1 && borderValue - borderValueBefore != 1) { borderValueBefore = borderValue; borderValue += Random.Range(-1, 1); }

            // Wenn der zuf�llige Wert nicht an der oberen Grenze anst��t und er in vorherigen Runde nicht kleiner wurde, darf er gr��er werden oder gleich bleiben
            else if (borderValue < sandOffset && borderValue - borderValueBefore != -1) { borderValueBefore = borderValue; borderValue += Random.Range(0, 2); }

            // Wenn keines der Kriterien erf�llt ist, bleibt er gleich
            else borderValueBefore = borderValue;

            // Alle Felder unterhalb des Zufallswertes werden Wasser
            for (int yValue = waterHeight; yValue < borderValue - 1; yValue++)
            {
                Vector3 position = new Vector3(xValue, yValue, 0);
                PaintTile(position, 0); // Wasser
                tileArray[xValue, yValue] = 0;
            }

            // Alle �berhalb des Zufallswertes werden Sand
            for (int yValue = borderValue + 1; yValue < sandOffset; yValue++)
            {
                Vector3 position = new Vector3(xValue, yValue, 0);
                PaintTile(position, 1); // Sand
                tileArray[xValue, yValue] = 1;
            }
        }

        // Grober �bergang zwischen Sand und d�nnem Gras
        borderValue = Random.Range(sandOffset+sandHeight + 1, lightGrassOffset + 1); // Zuf�lliger Wert f�r den �bergang zwischen den harten Grenzen
        borderValueBefore = borderValue;                                             // Vergleichswert um zu sehen ob der zuf�llige Wert gr��er oder kleiner wurde.
        for (int xValue = 0; xValue < xMax; xValue++)   
        {
            // Wenn der zuf�llige Wert nicht an den Grenzen anst��t und sich in der vorherigen Runde nicht ge�ndert hat, darf er um eins gr��er oder kleiner werden oder gleich bleiben.
            if (borderValue > sandOffset + sandHeight + 1 && borderValue < lightGrassOffset && borderValue - borderValueBefore == 0) { borderValueBefore = borderValue; borderValue += Random.Range(-1, 2); }

            // Wenn der zuf�llige Wert nicht an der unteren Grenze anst��t und er in vorherigen Runde nicht gr��er wurde, darf er kleiner werden oder gleich bleiben
            else if (borderValue > sandOffset + sandHeight + 1 && borderValue - borderValueBefore != 1) { borderValueBefore = borderValue; borderValue += Random.Range(-1, 1); }

            // Wenn der zuf�llige Wert nicht an der oberen Grenze anst��t und er in vorherigen Runde nicht kleiner wurde, darf er gr��er werden oder gleich bleiben
            else if (borderValue < lightGrassOffset && borderValue - borderValueBefore != -1) { borderValueBefore = borderValue; borderValue += Random.Range(0, 2); }

            // Wenn keines der Kriterien erf�llt ist, bleibt er gleich
            else borderValueBefore = borderValue;

            // Alle Felder unterhalb des Zufallswertes werden Sand
            for (int yValue = sandOffset + sandHeight; yValue < borderValue - 1; yValue++)
            {
                Vector3 position = new Vector3(xValue, yValue, 0);
                PaintTile(position, 1); // Sand
                tileArray[xValue, yValue] = 1;
            }

            // Alle �berhalb des Zufallswertes werden D�nnes Gras
            for (int yValue = borderValue + 1; yValue < lightGrassOffset; yValue++)
            {
                Vector3 position = new Vector3(xValue, yValue, 0);
                PaintTile(position, 2); // D�nnes Gras
                tileArray[xValue, yValue] = 2;
            }
        }

        // Grober �bergang zwischen d�nnem und dickem Gras
        borderValue = Random.Range(lightGrassOffset + lightGrassHeight + 1, darkGrassOffset + 1);       // Zuf�lliger Wert f�r den �bergang zwischen den harten Grenzen
        borderValueBefore = borderValue;                                                                // Vergleichswert um zu sehen ob der zuf�llige Wert gr��er oder kleiner wurde.
        for (int xValue = 0; xValue < xMax; xValue++)
        {
            // Wenn der zuf�llige Wert nicht an den Grenzen anst��t und sich in der vorherigen Runde nicht ge�ndert hat, darf er um eins gr��er oder kleiner werden oder gleich bleiben.
            if (borderValue > lightGrassOffset + lightGrassHeight + 1 && borderValue < darkGrassOffset && borderValue - borderValueBefore == 0) { borderValueBefore = borderValue; borderValue += Random.Range(-1, 2); }

            // Wenn der zuf�llige Wert nicht an der unteren Grenze anst��t und er in vorherigen Runde nicht gr��er wurde, darf er kleiner werden oder gleich bleiben
            else if (borderValue > lightGrassOffset + lightGrassHeight + 1 && borderValue - borderValueBefore != 1) { borderValueBefore = borderValue; borderValue += Random.Range(-1, 1); }

            // Wenn der zuf�llige Wert nicht an der oberen Grenze anst��t und er in vorherigen Runde nicht kleiner wurde, darf er gr��er werden oder gleich bleiben
            else if (borderValue < darkGrassOffset && borderValue - borderValueBefore != -1) { borderValueBefore = borderValue; borderValue += Random.Range(0, 2); }

            // Wenn keines der Kriterien erf�llt ist, bleibt er gleich
            else borderValueBefore = borderValue;

            // Alle Felder unterhalb des Zufallswertes werden d�nnes Gras
            for (int yValue = lightGrassOffset + lightGrassHeight; yValue < borderValue - 1; yValue++)
            {
                Vector3 position = new Vector3(xValue, yValue, 0);
                PaintTile(position, 2); // D�nnes Gras
                tileArray[xValue, yValue] = 2;
            }

            // Alle �berhalb des Zufallswertes werden dickes Gras
            for (int yValue = borderValue + 1; yValue < darkGrassOffset; yValue++)
            {
                Vector3 position = new Vector3(xValue, yValue, 0);
                PaintTile(position, 3); // Dickes Gras
                tileArray[xValue, yValue] = 3;
            }
        }
    }

    void CorrectBorders()
    {
        
    }
}
