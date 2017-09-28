using UnityEngine;
using UnityEditor;

public class SpritePixelsPerUnitChanger : AssetPostprocessor
{
	void OnPreprocessTexture ()
	{
		TextureImporter textureImporter  = (TextureImporter) assetImporter;
		textureImporter.spritePixelsPerUnit = 16;
		textureImporter.filterMode = FilterMode.Point;
		textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
	}
}