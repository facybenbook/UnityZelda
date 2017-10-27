using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : ScriptableObject {

    List<LinkedMap> maps;
    
    void Awake()
    {
        if (maps == null)
            maps = new List<LinkedMap>();
    }

    /// <summary>
    /// Adds a map specified as GameObject
    /// </summary>
    /// <param name="go"></param>
    public void AddMap(GameObject go)
    {
        if (go.GetComponent<Tiled2Unity.TiledMap>())
        {
            maps.Add(new LinkedMap(go));
        }
        else
            Debug.Log("AddMap: Invalid Map Object passed, addition aborted");
    }

    public void AddLink(LinkedMap source, WarpController sourceWarp, LinkedMap destination, WarpController destinationWarp, bool reciprocal)
    {
        source.mapLinks.Add(new WarpLink(sourceWarp, destinationWarp, destination));
        if (reciprocal == true)
            destination.mapLinks.Add(new WarpLink(destinationWarp, sourceWarp, destination));
    }

    public void RemoveMap(LinkedMap map)
    {
        foreach (WarpLink warp in map.mapLinks)
        {
            map.DestroyLink(warp);
        }
        maps.Remove(map);
    }

    //data structure that will be used to access map linking
    public class LinkedMap
    {
        public GameObject map;
        public List<WarpController> warps;
        public List<WarpLink> mapLinks;

        public LinkedMap(GameObject go)
        {
            map = go;

            //fetch all warps on the map
            Transform objectsContainer = go.transform.Find("GameObjects");
            warps = new List<WarpController>();
            foreach (Transform obj in objectsContainer)
            {
                if (obj.GetComponent<WarpController>())
                    warps.Add(obj.GetComponent<WarpController>());
            }
            if (warps.Count == 0)
                Debug.LogWarning("AddMap: Map Object " + go.name + " doesn't contain any warp");
            //---------------------------
            mapLinks = new List<WarpLink>();
        }
        /// <summary>
        /// Destroys the reciprocal links from and to the map at this point
        /// </summary>
        /// <param name="linkToDestroy"></param>
        public void DestroyLink(WarpLink linkToDestroy)
        {
            //reciprocally destroy the corresponding link if there is one
            linkToDestroy.linkedMap.mapLinks.Remove(
                linkToDestroy.linkedMap.mapLinks.Find(
                    x => x.destinationWarp == linkToDestroy.sourceWarp));
            mapLinks.Remove(linkToDestroy);
        }
    }

    public class WarpLink
    {
        public WarpController sourceWarp;
        public LinkedMap linkedMap;
        public WarpController destinationWarp;

        public WarpLink(WarpController source, WarpController destination, LinkedMap linkedMap)
        {
            sourceWarp = source;
            this.linkedMap = linkedMap;
            destinationWarp = destination;
        }
    }
}
