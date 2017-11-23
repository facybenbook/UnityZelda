using System.Collections;
using UnityEditor;
using UnityEngine;

public class CreateInventoryItemList {
    [MenuItem("Assets/Create/InventoryItemList")]
	public static InventoryItemList Create () {
        InventoryItemList asset = ScriptableObject.CreateInstance<InventoryItemList>();

        AssetDatabase.CreateAsset(asset, "Assets/InventoryItemList.asset");
        AssetDatabase.SaveAssets();
        return asset;
	}
	
}
