using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MainMenuVillage : MonoBehaviour
{
    public static MainMenuVillage instance {get; private set;}
    [Header("Villagers")]
    public int minVillagers;
    public GameObject villagerPrefab;
    public List<Vector3> villagerPos = new List<Vector3>();

    [Header("Gate")]
    public GameObject gatePrefab;
    public Vector2 gatePos;

    [Header("Buildings")]
    public Vector2 offset;
    public Sprite[] buildings;
    public GameObject buildingPrefab;
    public List<Vector3> buildingPos = new List<Vector3>();
    public List<int> buildingId = new List<int>();

    [Header("Roads")]
    public GameObject roadPrefab;
    public List<Vector2> roadPos = new List<Vector2>();

    public bool built = false;

    void Awake()
    {
        instance = this;
    }

    public void BuildVillage()
    {
        if(built) return;

        //Villagers
        foreach(var villagerPos in villagerPos)
        {
            Vector2 finalPos = new Vector2(villagerPos.x + offset.x, villagerPos.y + offset.y);
            Instantiate(villagerPrefab, finalPos, Quaternion.identity);
        }

        //Roads
        foreach(var roadPos in roadPos)
        {
            Vector2 finalPos = new Vector2(roadPos.x + offset.x, roadPos.y + offset.y);
            Instantiate(roadPrefab, finalPos, Quaternion.identity);
        }

        if(buildingPos.Count > buildingId.Count || buildingPos.Count < buildingId.Count)
        {
            Debug.Log("The lists for buildings are not alligned in indexes!");
        }

        //Buildings
        for(int i = 0; i < buildingPos.Count; i++)
        {
            Sprite buildingSprite = buildings[buildingId[i]];
            Vector2 finalPos = new Vector2(buildingPos[i].x + offset.x, buildingPos[i].y + offset.y);
            GameObject go = Instantiate(buildingPrefab, finalPos, Quaternion.identity);
            if(go.TryGetComponent(out SpriteRenderer goRend))
            {
                goRend.sprite = buildingSprite;
            }
        }

        //Gate
        Vector2 finalGatePos = new Vector2(gatePos.x + offset.x, gatePos.y + offset.y);
        Instantiate(gatePrefab, finalGatePos, Quaternion.identity);

        //Road under always
        Vector2 finalUnderPos = new Vector2(finalGatePos.x, finalGatePos.y - 1);
        Instantiate(roadPrefab, finalUnderPos, Quaternion.identity);

        built = true;
    }
}