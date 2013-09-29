﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class ItemDatabaseManager : EditorWindow {

    string scriptPath = Directory.GetCurrentDirectory() + "/Assets/Scripts/";
    string className = "ItemIDs";
    string fileExtension = ".cs";

    [MenuItem("FarmWithFriends/Item Database Manager")]
	static void Init ()
    {
        ItemDatabaseManager window = (ItemDatabaseManager)EditorWindow.CreateInstance(typeof(ItemDatabaseManager));
        window.Show ();
    }

    void OnGUI()
    {
        if(GUILayout.Button ("Import Items to DB"))
        {
            ItemDatabase itemDatabase = (ItemDatabase) GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
            ImportItems ();
        }


    }

    // TODO: Unhardcode these IDs by putting them in a file
    int radishID = 0;
    int radishSeedsID = 1;
    int onionID = 2;
    int onionSeedsID = 3;
    int pototoID = 4;
    int potatoSeedsID = 5;

    /*
     * Writes item ids parsed out from a file of Items.
     */
    void WriteItemIDs()
    {
        string fileName = className + fileExtension;
        string header = "using UnityEngine;\nusing System.Collections;\n\npublic class " + className + " : ScriptableObject\n{";
        string itemDeclaration = "\n    public static readonly int ";
        string footer = "\n}";

        // TODO: Generate this body from a parsed file, not hard coded.
        string body = itemDeclaration + "RADISH = " + radishID.ToString() + ";" +
            itemDeclaration + "RADISH_SEEDS = " + radishSeedsID.ToString() + ";" +
            itemDeclaration + "ONION = " + onionID.ToString() + ";" +
            itemDeclaration + "ONION_SEEDS = " + onionSeedsID.ToString() + ";" +
            itemDeclaration + "POTATO = " + pototoID.ToString() + ";" +
            itemDeclaration + "POTATO_SEEDS = " + potatoSeedsID.ToString() + ";";

        string output = header + body + footer;

        System.IO.File.WriteAllText(scriptPath + fileName, output);
        Debug.Log ("Printed out file" + scriptPath + fileName);
    }

    /*
     * Parse out Items from a file and Add them to a database.
     */
    void ImportItems()
    {
        ItemDatabase itemDatabase = (ItemDatabase) GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();

        // In theory, all of this info should go into an xml or text file that we parse.
        Item radish = (Item)ScriptableObject.CreateInstance (typeof(Item));
        radish.name = "Radish";
        radish.itemName = "Radish";
        radish.maxCount = 100;
        radish.price = 30;
        radish.sellPrice = 20;
        radish.id = radishID;
        itemDatabase.AddItem(radish);

        Item radishSeeds = (Item)ScriptableObject.CreateInstance (typeof(Item));
        radishSeeds.name = "Radish Seeds";
        radishSeeds.itemName = "Radish Seeds";
        radishSeeds.description = "Time to grow: Short.\nWater needed: Daily";
        radishSeeds.maxCount = 100;
        radishSeeds.price = 5;
        radishSeeds.sellPrice = 2;
        radishSeeds.id = radishSeedsID;
        radishSeeds.plantPrefab = (GameObject) Resources.Load("Radish");
        itemDatabase.AddItem(radishSeeds);

        Item onion = (Item)ScriptableObject.CreateInstance (typeof(Item));
        onion.name = "Onion";
        onion.itemName = "Onion";
        onion.maxCount = 100;
        onion.price = 30;
        onion.sellPrice = 20;
        onion.id = onionID;
        itemDatabase.AddItem(onion);

        Item onionSeeds = (Item)ScriptableObject.CreateInstance (typeof(Item));
        onionSeeds.name = "Onion Seeds";
        onionSeeds.itemName = "Onion Seeds";
        onionSeeds.description = "Time to grow: Long.\nWater needed: Drought Only";
        onionSeeds.maxCount = 100;
        onionSeeds.price = 5;
        onionSeeds.sellPrice = 2;
        onionSeeds.id = onionSeedsID;
        onionSeeds.plantPrefab = (GameObject) Resources.Load("Onion");
        itemDatabase.AddItem(onionSeeds);

        Item potato = (Item)ScriptableObject.CreateInstance (typeof(Item));
        potato.name = "Potato";
        potato.itemName = "Potato";
        potato.maxCount = 100;
        potato.price = 100;
        potato.sellPrice = 80;
        potato.id = pototoID;
        itemDatabase.AddItem(potato);

        Item potatoSeeds = (Item)ScriptableObject.CreateInstance (typeof(Item));
        potatoSeeds.name = "Potato Seeds";
        potatoSeeds.itemName = "Potato Seeds";
        potatoSeeds.description = "Time to grow: Very Long.\nWater needed: Daily";
        potatoSeeds.maxCount = 100;
        potatoSeeds.price = 5;
        potatoSeeds.sellPrice = 2;
        potatoSeeds.id = potatoSeedsID;
        potatoSeeds.plantPrefab = (GameObject) Resources.Load("Potato");
        itemDatabase.AddItem(potatoSeeds);

        // Write out the parsed IDs to a file so that we can access them in script
        WriteItemIDs();
    }
}