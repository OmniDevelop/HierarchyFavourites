#if UNITY_EDITOR

namespace OmniDevelop.EditorTools
{
	using UnityEditor;

	using UnityEngine;

	[InitializeOnLoad]
	public static class HierarchyFavourites
	{
		private static readonly Texture2D _icon = Resources.Load<Texture2D>("Star");

		static HierarchyFavourites()
		{
			EditorApplication.hierarchyWindowItemOnGUI += HandleGUI;
		}

		private static void HandleGUI(int instanceID, Rect selectionRect)
		{
			if (!EditorPrefs.HasKey(GlobalObjectId.GetGlobalObjectIdSlow(instanceID).ToString()))
			{
				return;
			}

			if (EditorUtility.InstanceIDToObject(instanceID) is not GameObject obj)
			{
				return;
			}

			float width = GUI.skin.label.CalcSize(new GUIContent(obj.name)).x;

			Rect iconRect = new()
			{
				x = selectionRect.x + width + 32,
				y = selectionRect.y,
				width = selectionRect.height,
				height = selectionRect.height
			};

			GUI.DrawTexture(iconRect, _icon);
		}

		[MenuItem("GameObject/Add to favourites &d", false, -1)]
		private static void AddToFavourites()
		{
			int[] instanceIds = Selection.instanceIDs;

			for (int i = 0; i < instanceIds.Length; i++)
			{
				string current = GlobalObjectId.GetGlobalObjectIdSlow(instanceIds[i]).ToString();

				if (EditorPrefs.HasKey(current))
				{
					EditorPrefs.DeleteKey(current);

					continue;
				}

				EditorPrefs.SetString(current, string.Empty);
			}
		}
	}
}

#endif