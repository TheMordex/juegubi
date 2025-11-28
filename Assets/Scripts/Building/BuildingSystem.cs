using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
  public static BuildingSystem current;
  
  public GridLayout gridLayout;
  private Grid grid;
  [SerializeField] private Tilemap MainTilemap;
  [SerializeField] private TileBase whiteTile;
  
  public GameObject prefab1;
  public GameObject prefab2;
  
  private PlaceableObject objectToPlace;

  private void Awake()
  {
    current = this;
    grid = gridLayout.gameObject.GetComponent<Grid>();
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.N))
    {
      InitializeWithObject(prefab1);
    }
    if (Input.GetKeyDown(KeyCode.M))
    {
      InitializeWithObject(prefab2);
    }

    if (!objectToPlace)
    {
      return;
    }
    
    if(Input.GetKeyDown(KeyCode.R))
    {
      objectToPlace.Rotate();
    }
    else if (Input.GetKeyDown(KeyCode.Space))
    {
      if (CanBePlaced(objectToPlace))
      {
        objectToPlace.Place();
        Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        TakenArea(start, objectToPlace.Size);
      }
      else
      {
        Destroy(objectToPlace.gameObject);
      }
    }
    else if(Input.GetKeyDown(KeyCode.Space))
    {
        Destroy(objectToPlace.gameObject);
    }
  }
  public static Vector3 GetMouseWorldPosition()
  {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    if (Physics.Raycast(ray, out RaycastHit hit))
    {
      return hit.point;
    }
    else
    {
      return Vector3.zero;
    }
  }

  public Vector3 SnapCoordinateToGrid(Vector3 position)
  {
    Vector3Int cellPos = gridLayout.WorldToCell(position);
    position = grid.GetCellCenterWorld(cellPos);
    return position;
  }

  private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
  {
    TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
    int counter = 0;

    foreach (var v in area.allPositionsWithin)
    {
      Vector3Int pos = new Vector3Int(v.x, v.y, 0);
      array[counter] = tilemap.GetTile(pos);
      counter++;
    }
    return array;
  }
  
  public void InitializeWithObject(GameObject prefab)
  {
    Vector3 position = SnapCoordinateToGrid(Vector3Int.zero);
    GameObject obj = Instantiate(prefab, position, Quaternion.identity);
    objectToPlace = obj.GetComponent<PlaceableObject>();
    obj.AddComponent<ObjectDrag>();
  }

  private bool CanBePlaced(PlaceableObject placeableObject)
  {
    BoundsInt area = new BoundsInt();
    area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
    area.size = placeableObject.Size;

    TileBase[] baseArray = GetTilesBlock(area, MainTilemap);

    foreach (var b in baseArray)
    {
      if (b == whiteTile)
      {
        return false;
      }
    }
    return true;
  }

  public void TakenArea(Vector3Int start, Vector3Int size)
  {
    MainTilemap.BoxFill(start, whiteTile, start.x, start.y, start.x + size.x, start.y + size.y);
  }
}
