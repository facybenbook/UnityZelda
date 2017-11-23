using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryItemEditor : EditorWindow
{

    public InventoryItemList inventoryItemList;
    private int viewIndex = 1;

    [MenuItem("Window/Inventory Item Editor %#e")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(InventoryItemEditor));
    }

    void OnEnable()
    {
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            inventoryItemList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(InventoryItemList)) as InventoryItemList;
        }

    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Inventory Item Editor", EditorStyles.boldLabel);
        if (inventoryItemList != null)
        {
            if (GUILayout.Button("Show Item List"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = inventoryItemList;
            }
        }
        if (GUILayout.Button("Open Item List"))
        {
            OpenItemList();
        }
        if (GUILayout.Button("New Item List"))
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = inventoryItemList;
        }
        GUILayout.EndHorizontal();

        if (inventoryItemList == null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button("Create New Item List", GUILayout.ExpandWidth(false)))
            {
                CreateNewItemList();
            }
            if (GUILayout.Button("Open Existing Item List", GUILayout.ExpandWidth(false)))
            {
                OpenItemList();
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        if (inventoryItemList != null)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex > 1)
                    viewIndex--;
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex < inventoryItemList.Count())
                {
                    viewIndex++;
                }
            }

            GUILayout.Space(60);

            if (GUILayout.Button("Add Item", GUILayout.ExpandWidth(false)))
            {
                AddItem();
            }
            if (GUILayout.Button("Delete Item", GUILayout.ExpandWidth(false)))
            {
                DeleteItem(viewIndex - 1);
            }

            GUILayout.EndHorizontal();
            if (inventoryItemList == null)
                Debug.Log("wtf");
            if (inventoryItemList.Count() > 0)
            {
                GUILayout.BeginHorizontal();
                viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Item", viewIndex, GUILayout.ExpandWidth(false)), 1, inventoryItemList.Count());
                //Mathf.Clamp (viewIndex, 1, inventoryItemList.itemList.Count);
                EditorGUILayout.LabelField("of " + inventoryItemList.Count().ToString() + " items", "", GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                inventoryItemList.GetItem(viewIndex - 1).name = EditorGUILayout.TextField("ItemName", inventoryItemList.GetItem(viewIndex - 1).name);
                inventoryItemList.GetItem(viewIndex - 1).icon = EditorGUILayout.ObjectField("ItemIcon", inventoryItemList.GetItem(viewIndex - 1).icon, typeof(Sprite), false) as Sprite;
                inventoryItemList.GetItem(viewIndex - 1).itemType = (ItemType)EditorGUILayout.EnumPopup("ItemType", inventoryItemList.GetItem(viewIndex - 1).itemType);

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                if (inventoryItemList.GetItem(viewIndex - 1).itemType == ItemType.Weapon)
                {
                    inventoryItemList.GetItem(viewIndex - 1).usesAmmo = (bool)EditorGUILayout.Toggle("UsesAmmo", inventoryItemList.GetItem(viewIndex - 1).usesAmmo, GUILayout.ExpandWidth(false));
                }
                else if (inventoryItemList.GetItem(viewIndex - 1).itemType == ItemType.Consummable)
                {
                    int selectedIndex = inventoryItemList.GetItem(viewIndex - 1).afterConsumed;
                    inventoryItemList.GetItem(viewIndex - 1).afterConsumed = EditorGUILayout.Popup("AfterConsumed:", selectedIndex, inventoryItemList.GetNameList());
                }
                else if (inventoryItemList.GetItem(viewIndex - 1).itemType == ItemType.Quest)
                {

                }
                else if (inventoryItemList.GetItem(viewIndex - 1).itemType == ItemType.Other)
                {
                    //do something
                }
                GUILayout.EndHorizontal();

            }
            else
            {
                GUILayout.Label("This Inventory List is Empty.");
            }
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(inventoryItemList);
        }
    }

    void CreateNewItemList()
    {
        // There is no overwrite protection here!
        // There is No "Are you sure you want to overwrite your existing object?" if it exists.
        // This should probably get a string from the user to create a new name and pass it ...
        viewIndex = 1;
        inventoryItemList = CreateInventoryItemList.Create();
        if (inventoryItemList)
        {
            inventoryItemList.Initialize();
            string relPath = AssetDatabase.GetAssetPath(inventoryItemList);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }

    void OpenItemList()
    {
        string absPath = EditorUtility.OpenFilePanel("Select Inventory Item List", "", "");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            inventoryItemList = AssetDatabase.LoadAssetAtPath(relPath, typeof(InventoryItemList)) as InventoryItemList;
            if (inventoryItemList)
            {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }

    void AddItem()
    {
        InventoryItem newItem = new InventoryItem
        {
            name = "new item"
        };
        inventoryItemList.AddItem(newItem);
        viewIndex = inventoryItemList.Count();
    }

    void DeleteItem(int index)
    {
        inventoryItemList.RemoveItem(index);
    }
}