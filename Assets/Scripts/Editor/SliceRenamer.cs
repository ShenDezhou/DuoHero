#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class SliceRenamer : EditorWindow
{
    public Texture2D texture;
    public string prefix = "";
    public string suffix = "";
    public int startSlice = 0;
    public int endSlice = 0;
    public int startNumber = 0;
    public int increment = 1;

    private Vector2 scroll;
    private string status = "";
    private string path;
    private TextureImporter textureImporter;

    public int valueForIndex(int index)
    {
        int value = 0;
        // Increment.
        if (increment == 0)
        { value = startNumber + index; } // Simply increment
        else
        { value = startNumber + (index / increment); } // Increment every Nth only
        return value;
    }

    // Window.
    [MenuItem("Window/Slice Renamer")]
    public static void ShowWindow()
    {
        // Show window.
        EditorWindow.GetWindow(typeof(SliceRenamer), false, "Slice Renamer");
    }

    // GUI.
    void OnGUI()
    {
        // Texture asset.
        string textureName = (texture == null) ? "No Texture selected" : texture.name;
        EditorGUILayout.LabelField(textureName, EditorStyles.boldLabel);
        texture = EditorGUILayout.ObjectField("Texture", texture, typeof(Texture2D), true) as Texture2D;

        // Fields.
        prefix = EditorGUILayout.TextField("Prefix", prefix);
        suffix = EditorGUILayout.TextField("Suffix", suffix);
        startSlice = EditorGUILayout.IntField("Start Slice", startSlice);
        endSlice = EditorGUILayout.IntField("End Slice", endSlice);
        startNumber = EditorGUILayout.IntField("Start Number", startNumber);
        increment = EditorGUILayout.IntField("Increment", increment);

        // Buttons.
        if (GUILayout.Button("Preview"))
        { Apply(true); }
        if (GUILayout.Button("Rename Slices"))
        { Apply(false); }

        // Status.
        scroll = EditorGUILayout.BeginScrollView(scroll);
        EditorGUILayout.LabelField(status, EditorStyles.helpBox);
        EditorGUILayout.EndScrollView();
    }

    // Apply settings.
    void Apply(bool preview)
    {
        // Error.
        if (texture == null)
        {
            status = "Drag a texture into the slot.";
            return;
        }

        // Locate asset, get meta.
        path = AssetDatabase.GetAssetPath(texture);
        textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        SpriteMetaData[] sliceMetaData = textureImporter.spritesheet;


        // Error.
        if (sliceMetaData == null || sliceMetaData.Length == 0)
        {
            status = "Seems no slices defined in texture.";
            return;
        }

        // Naming loop.
        int index = 0;
        status = "";
        for (int i = startSlice; i <= endSlice; i++)
        {
            string eachName = prefix + valueForIndex(index).ToString() + suffix;

            // Assemble name.
            string verb = (preview) ? "Rename" : "Renamed";
            if (index > 0) status += "\n";
            status += verb + " `" + sliceMetaData[i].name + "` to `" + eachName + "`.";

            // Assign.
            if (preview == false)
            { sliceMetaData[i].name = eachName; }

            index++;
        }

        // Branding.
        status += "\n\nGameSuntzu.com";

        // Apply.
        if (preview == false)
        {
            // Save settings.
            textureImporter.spritesheet = sliceMetaData;
            EditorUtility.SetDirty(textureImporter);
            textureImporter.SaveAndReimport();

            // Reimport/refresh asset.
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
    }
}
#endif