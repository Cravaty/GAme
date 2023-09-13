using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Set Dynamically")]
    public int x;
    public int y;
    public int tileNum;

    private BoxCollider bColl;

    void Awake()
    {
        bColl = GetComponent<BoxCollider>(); // a
    }


    public void SetTile(int eX, int eY, int eTileNum = -1)
    { //a
        x = eX;
        y = eY;
        transform.localPosition = new Vector3(x, y, 0);
        gameObject.name = x.ToString("D3") + "x" + y.ToString("D3"); // b
        if (eTileNum == -1)
        {
            eTileNum = TileCamera.GET_MAP(x, y); // c
        }
        else
        {
            TileCamera.SET_MAP(x, y, eTileNum); // �������� ������, ���� ����������
        }
        tileNum = eTileNum;
        GetComponent<SpriteRenderer>().sprite = TileCamera.SPRITES[tileNum]; // d
        SetCollider();
    }

    // ��������� ��������� ��� ���� ������
    void SetCollider()
    {
        // �������� ���������� � ���������� �� Collider DelverCollisions.txt
        bColl.enabled = true;
        char c = TileCamera.COLLISIONS[tileNum]; // c
        switch (c)
        {
            case 'S': // ��� ������
                bColl.center = Vector3.zero;
                bColl.size = Vector3.one;
                break;
            case 'W': // ������� ��������
                bColl.center = new Vector3(0, 0.25f, 0);
                bColl.size = new Vector3(1, 0.5f, 1);
                break;
            case 'A': // ����� ��������
                bColl.center = new Vector3(-0.25f, 0, 0);
                bColl.size = new Vector3(0.5f, 1, 1);
                break;
            case 'D': // ������ ��������
                bColl.center = new Vector3(0.25f, 0, 0);
                bColl.size = new Vector3(0.5f, 1, 1);
                break;
            default: // ��� ���������: |, � ��. // �
                bColl.enabled = false;
                break;

        }
    }
}
