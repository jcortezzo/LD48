using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Room currentRoom;  // unserialize this when ready
    [SerializeField] private Room prevRoom;
    [SerializeField] private IList<Room> prevRooms;
    [SerializeField] private float GEN_THRESHHOLD;
    [SerializeField] private float DELETE_THRESHHOLD;

    [SerializeField] private GameObject[] rooms;

    [SerializeField] private GameObject[] defaultRooms;
    private Player player;

    private bool beginGenerate;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        prevRooms = new List<Room>();
        beginGenerate = true;
    }

    // Update is called once per frame
    void Update()
    {
        DeletePrevRoom();
        GenerateNextRoom();
    }

    public void DisableGenerate()
    {
        beginGenerate = false;
    }

    public void BeginGenerate()
    {
        beginGenerate = true;
    }
    /// <summary>
    /// Generates a new room if the player is close enough to the next room.
    /// </summary>
    private void GenerateNextRoom()
    {
        //if (!GlobalManager.instance.HasPlayer()) return;
        if (!beginGenerate) return;
        if (GlobalManager.Instance.gameDirection == Direction.RIGHT && 
            player.transform.position.x >= currentRoom.boundingBox.center.x - GEN_THRESHHOLD && !currentRoom.isInitialRoom)
        {
            Room nextRoom =
                    Instantiate(defaultRooms[Random.Range(0, defaultRooms.Length)],   // was prev from rooms arr
                                new Vector2(currentRoom.boundingBox.max.x + currentRoom.boundingBox.size.x / 2, currentRoom.transform.position.y),
                                Quaternion.identity)
                                .GetComponent<Room>();
            Debug.Log("There are " + prevRooms.Count + " rooms to destroy");
            foreach (Room prev in prevRooms)
            {
                if (prev == null || prev.dontDelete) continue;
                Destroy(prev.gameObject);
            }
            prevRoom = currentRoom;
            prevRooms.Add(prevRoom);
            currentRoom = nextRoom;
        }

        if (GlobalManager.Instance.gameDirection == Direction.LEFT &&
            player.transform.position.x <= currentRoom.boundingBox.center.x + GEN_THRESHHOLD && !currentRoom.isInitialRoom)
        {
            Room nextRoom =
                    Instantiate(defaultRooms[Random.Range(0, defaultRooms.Length)],   // was prev from rooms arr
                                new Vector2(currentRoom.boundingBox.min.x - currentRoom.boundingBox.size.x / 2, currentRoom.transform.position.y),
                                Quaternion.identity)
                                .GetComponent<Room>();
            Debug.Log("There are " + prevRooms.Count + " rooms to destroy");
            foreach (Room prev in prevRooms)
            {
                if (prev == null || prevRoom.dontDelete) continue;
                Destroy(prev.gameObject);
            }
            prevRoom = currentRoom;
            prevRooms.Add(prevRoom);
            currentRoom = nextRoom;
        }
    }

    /// <summary>
    /// Deletes a room if the player is far enough away from it.
    /// The player cannot move backwards so this is okay and efficient.
    /// </summary>
    private void DeletePrevRoom()
    {
        //if (!GlobalManager.instance.HasPlayer()) return;
        if (prevRoom == null || prevRoom.dontDelete) return;

        if ((GlobalManager.Instance.gameDirection == Direction.RIGHT && 
            player.transform.position.x >= currentRoom.boundingBox.center.x + DELETE_THRESHHOLD) ||

            (GlobalManager.Instance.gameDirection == Direction.LEFT &&
            player.transform.position.x <= currentRoom.boundingBox.center.x - DELETE_THRESHHOLD)
            )
        {
            // detatch enemy children before destroying
            // so they can move between screens
            // NOTE: I wish I could do this in OnDestroy() in Room,
            //       but that causes undesirable behavior. It seems
            //       even after detaching in OnDestroy(), the Room
            //       still tries to delete all of its children at
            //       the time that Destroy() was called.
            //foreach (Transform child in prevRoom.transform)
            //{
            //    if (child.gameObject.CompareTag("Enemy") ||
            //        child.gameObject.CompareTag("Holy"))
            //    {
            //        child.SetParent(null);
            //    }
            //}

            // if only 1
            Destroy(prevRoom.gameObject);
            //Debug.Log("There are " + prevRooms.Count + " rooms to destroy");
            //foreach (Room prev in prevRooms)
            //{
            //    Destroy(prev.gameObject);
            //}
        }
    }
}
