using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    int xMax = 80;
    int yMax = 45;
    int[,] tileArray;

    [SerializeField] GameObject waterTile, grassLightTile, grassDarkTile;
    [SerializeField] Vector3 generationVector;

    private void Start()
    {
        tileArray = new int[xMax, yMax];
        waterTile = transform.parent.Find("TokenSlots").Find("TokenSlot_Water").gameObject;
        grassLightTile = transform.parent.Find("TokenSlots").Find("TokenSlot_GrassLight").gameObject;
        grassDarkTile = transform.parent.Find("TokenSlots").Find("TokenSlot_GrassDark").gameObject;

        GenerateTilemap();
    }

    [ContextMenu("Paint")]
    public void PaintTileDebug()
    {
        PaintTile(waterTile, generationVector);
    }



    public void PaintTile(GameObject tileToPaint, Vector3 position)
    {
        GameObject myTile = Instantiate(tileToPaint, position, Quaternion.identity, transform.parent.Find("Grid"));
    }

    void GenerateTilemap()
    {
        for (int i = 0; i < xMax; i++)
        {
            for (int k = 0; k < yMax; k++)
            {
                Vector3 position = new Vector3(i, k, 0);
                if ((k + i) % 2 == 1) PaintTile(grassLightTile, position);
                else PaintTile(waterTile, position);

                tileArray[i, k] = 2;
            }
        }
    }


}
