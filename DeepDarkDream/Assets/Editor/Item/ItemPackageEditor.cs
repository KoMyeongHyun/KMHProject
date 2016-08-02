using UnityEngine;
using UnityEditor;
using System.Collections;
[CustomEditor(typeof(ItemPackage))]
public class ItemPackageEditor : Editor
{
    protected ItemPackage src
    {
        get
        {
            return (ItemPackage)target;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("AddChild"))
        {
            src.childTransform = new Transform[src.transform.childCount];

            for (int i = 0; i < src.transform.childCount; ++i)
            {
                src.childTransform[i] = src.transform.GetChild(i);
            }
        }
    }
}
