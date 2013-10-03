using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class ItemDatabaseManager : EditorWindow
{

    string scriptPath = Directory.GetCurrentDirectory () + "/Assets/Scripts/ProjectScripts/";
    string className = "ItemIDs";
    string fileExtension = ".cs";

    [MenuItem("FarmWithFriends/Item Database Manager")]
    static void Init ()
    {
        ItemDatabaseManager window = (ItemDatabaseManager)EditorWindow.CreateInstance (typeof(ItemDatabaseManager));
        window.Show ();
    }

    void OnGUI ()
    {
        if (GUILayout.Button ("Import Items to DB")) {
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
    int tomatoID = 6;
    int tomatoSeedsID = 7;
    int beanID = 8;
    int beanSeedsID = 9;
    int wildflowerID = 10;
    int wildflowerSeedsID = 11;

    /*
     * Writes item ids parsed out from a file of Items.
     */
    void WriteItemIDs ()
    {
        string fileName = className + fileExtension;
        string header = "using UnityEngine;\nusing System.Collections;\n\npublic class " + className + " : ScriptableObject\n{";
        string itemDeclaration = "\n    public static readonly int ";
        string footer = "\n}";

        // TODO: Generate this body from a parsed file, not hard coded.
        string body = itemDeclaration + "RADISH = " + radishID.ToString () + ";" +
            itemDeclaration + "RADISH_SEEDS = " + radishSeedsID.ToString () + ";" +
            itemDeclaration + "ONION = " + onionID.ToString () + ";" +
            itemDeclaration + "ONION_SEEDS = " + onionSeedsID.ToString () + ";" +
            itemDeclaration + "POTATO = " + pototoID.ToString () + ";" +
            itemDeclaration + "POTATO_SEEDS = " + potatoSeedsID.ToString () + ";" +
            itemDeclaration + "TOMATO = " + tomatoID.ToString () + ";" +
            itemDeclaration + "TOMATO_SEEDS = " + tomatoSeedsID.ToString () + ";" +
            itemDeclaration + "BEAN = " + beanID.ToString () + ";" +
            itemDeclaration + "BEAN_SEEDS = " + beanSeedsID.ToString () + ";" +
            itemDeclaration + "WILDFLOWER = " + wildflowerID.ToString () + ";" +
            itemDeclaration + "WILDFLOWER_SEEDS = " + wildflowerSeedsID.ToString () + ";";

        string output = header + body + footer;

        System.IO.File.WriteAllText (scriptPath + fileName, output);
        Debug.Log ("Printed out file" + scriptPath + fileName);
    }

    /*
     * Parse out Items from a file and Add them to a database.
     */
    void ImportItems ()
    {
        ItemDatabase itemDatabase = (ItemDatabase)GameObject.Find ("ItemDatabase").GetComponent<ItemDatabase> ();

        // In theory, all of this info should go into an xml or text file that we parse.
        Item radish = (Item)ScriptableObject.CreateInstance (typeof(Item));
        radish.name = "Radish";
        radish.itemName = "Radish";
        radish.maxCount = 200;
        radish.price = 16;
        radish.sellPrice = 8;
        radish.isEquippable = false;
        radish.id = radishID;
        itemDatabase.AddItem (radish);

        Item radishSeeds = (Item)ScriptableObject.CreateInstance (typeof(Item));
        radishSeeds.name = "Radish Seeds";
        radishSeeds.itemName = "Radish Seeds";
        radishSeeds.description = "Time to grow: 3 nights.\nWater needed: Daily\nRadishes sell for 8 gold.";
        radishSeeds.maxCount = 200;
        radishSeeds.price = 5;
        radishSeeds.sellPrice = 5;
        radishSeeds.isEquippable = true;
        radishSeeds.id = radishSeedsID;
        radishSeeds.plantPrefab = (GameObject)Resources.Load ("Radish");
        itemDatabase.AddItem (radishSeeds);

        Item onion = (Item)ScriptableObject.CreateInstance (typeof(Item));
        onion.name = "Onion";
        onion.itemName = "Onion";
        onion.maxCount = 200;
        onion.price = 16;
        onion.sellPrice = 8;
        onion.isEquippable = false;
        onion.id = onionID;
        itemDatabase.AddItem (onion);

        Item onionSeeds = (Item)ScriptableObject.CreateInstance (typeof(Item));
        onionSeeds.name = "Onion Seeds";
        onionSeeds.itemName = "Onion Seeds";
        onionSeeds.description = "Time to grow: 3 nights.\nOnions Sell for 8 gold, but never need water.";
        onionSeeds.maxCount = 200;
        onionSeeds.price = 5;
        onionSeeds.sellPrice = 5;
        onionSeeds.isEquippable = true;
        onionSeeds.id = onionSeedsID;
        onionSeeds.plantPrefab = (GameObject)Resources.Load ("Onion");
        itemDatabase.AddItem (onionSeeds);

        Item potato = (Item)ScriptableObject.CreateInstance (typeof(Item));
        potato.name = "Potato";
        potato.itemName = "Potato";
        potato.maxCount = 200;
        potato.price = 60;
        potato.sellPrice = 50;
        potato.isEquippable = false;
        potato.id = pototoID;
        itemDatabase.AddItem (potato);

        Item potatoSeeds = (Item)ScriptableObject.CreateInstance (typeof(Item));
        potatoSeeds.name = "Potato Seeds";
        potatoSeeds.itemName = "Potato Seeds";
        potatoSeeds.description = "Time to grow: 6 nights.\nWater needed: Daily\nPotatos sell for 50 gold.";
        potatoSeeds.maxCount = 200;
        potatoSeeds.price = 20;
        potatoSeeds.sellPrice = 20;
        potatoSeeds.isEquippable = true;
        potatoSeeds.id = potatoSeedsID;
        potatoSeeds.plantPrefab = (GameObject)Resources.Load ("Potato");
        itemDatabase.AddItem (potatoSeeds);

        Item tomato = (Item)ScriptableObject.CreateInstance (typeof(Item));
        tomato.name = "Tomato";
        tomato.itemName = "Tomato";
        tomato.maxCount = 200;
        tomato.price = 70;
        tomato.sellPrice = 50;
        tomato.isEquippable = false;
        tomato.id = tomatoID;
        itemDatabase.AddItem (tomato);

        Item tomatoSeeds = (Item)ScriptableObject.CreateInstance (typeof(Item));
        tomatoSeeds.name = "Tomato Seeds";
        tomatoSeeds.itemName = "Tomato Seeds";
        tomatoSeeds.description = "Time to grow: 8 nights but repickable every 4.\nWater needed: Daily\nTomatos sell for 50 gold.";
        tomatoSeeds.maxCount = 200;
        tomatoSeeds.price = 100;
        tomatoSeeds.sellPrice = 100;
        tomatoSeeds.isEquippable = true;
        tomatoSeeds.id = tomatoSeedsID;
        tomatoSeeds.plantPrefab = (GameObject)Resources.Load ("Tomato");
        itemDatabase.AddItem (tomatoSeeds);

        Item bean = (Item)ScriptableObject.CreateInstance (typeof(Item));
        bean.name = "Bean";
        bean.itemName = "Bean";
        bean.maxCount = 200;
        bean.price = 10;
        bean.sellPrice = 5;
        bean.isEquippable = false;
        bean.id = beanID;
        itemDatabase.AddItem (bean);

        Item beanSeeds = (Item)ScriptableObject.CreateInstance (typeof(Item));
        beanSeeds.name = "Bean Seeds";
        beanSeeds.itemName = "Bean Seeds";
        beanSeeds.description = "Time to grow: 3 nights but repickable every night.\nWater needed: Every 3 Days\nBeans sell for 5 gold.";
        beanSeeds.maxCount = 200;
        beanSeeds.price = 20;
        beanSeeds.sellPrice = 20;
        beanSeeds.isEquippable = true;
        beanSeeds.id = beanSeedsID;
        beanSeeds.plantPrefab = (GameObject)Resources.Load ("Bean");
        itemDatabase.AddItem (beanSeeds);

        Item wildflower = (Item)ScriptableObject.CreateInstance (typeof(Item));
        wildflower.name = "Wildflower";
        wildflower.itemName = "Wildflower";
        wildflower.maxCount = 200;
        wildflower.price = 20;
        wildflower.sellPrice = 20;
        wildflower.isEquippable = false;
        wildflower.id = wildflowerID;
        itemDatabase.AddItem (wildflower);

        Item wildflowerSeeds = (Item)ScriptableObject.CreateInstance (typeof(Item));
        wildflowerSeeds.name = "Wildflower Seeds";
        wildflowerSeeds.itemName = "Wildflower Seeds";
        wildflowerSeeds.description = "STUB WILDFLOWER SEED TEXT";
        wildflowerSeeds.maxCount = 200;
        wildflowerSeeds.price = 20;
        wildflowerSeeds.sellPrice = 20;
        wildflowerSeeds.isEquippable = true;
        wildflowerSeeds.id = wildflowerSeedsID;
        wildflowerSeeds.plantPrefab = (GameObject)Resources.Load ("Wildflower");
        itemDatabase.AddItem (wildflowerSeeds);
        // Write out the parsed IDs to a file so that we can access them in script
        WriteItemIDs ();
    }
}
