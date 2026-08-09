using UnityEditor;
using UnityEngine;

public class BlueShadeStudioNameDialog : EditorWindow
{
    private string input = "Custom";
    public static string Result = null;

    public static string ShowDialog(string windowTitle, string dialogMessage, string defaultText = "Custom")
    {
        Result = null;
        var window = GetWindow<BlueShadeStudioNameDialog>(true);
        window.titleContent = new GUIContent(windowTitle);
        window.minSize = new Vector2(300, 96);
        window.maxSize = new Vector2(600, 96);
        window.input = defaultText;
        window.message = dialogMessage;
        window.ShowPopup();
        window.FocusWindowIfItsOpen<BlueShadeStudioNameDialog>();
        return Result;
    }

    private string message = "Enter name:";

    void OnFocus()
    {
        EditorGUIUtility.labelWidth = 80;
    }

    void OnLostFocus()
    {
        EditorGUIUtility.labelWidth = 0;
    }

    void OnGUI()
    {
        EditorGUILayout.Space(12);
        EditorGUILayout.LabelField(message, EditorStyles.wordWrappedLabel);
        EditorGUILayout.Space(4);

        input = EditorGUILayout.TextField("Preset Name:", input);

        EditorGUILayout.Space(8);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save", GUILayout.Height(24)))
        {
            Result = input;
            Close();
        }
        if (GUILayout.Button("Cancel", GUILayout.Height(24)))
        {
            Result = null;
            Close();
        }
        EditorGUILayout.EndHorizontal();

        if (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter)
        {
            Result = input;
            Close();
        }
    }
}
