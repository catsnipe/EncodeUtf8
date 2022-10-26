using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 新規スクリプト生成時にテキストエンコードを UTF8 に矯正します
/// </summary>
public class OnPostprocessEncodeUtf8 : AssetPostprocessor
{	
    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetsPath)
    {
        foreach(string asset in importedAssets)
        {
            if (Path.GetExtension(asset) != ".cs")
            {
                continue;
            }
            if(File.Exists(asset) == false)
            {
                continue;
            }

            var bs  = File.ReadAllBytes(asset);
            // UTF8 with BOM
            if((bs[0] == 0xEF) && (bs[1] == 0xBB) && (bs[2] == 0xBF))
            {
                continue;
            }

            var enc = EncodeUtf8.GetEncode(bs);

            if(enc != Encoding.UTF8)
            {
                var text = enc.GetString(bs).Replace("\r\n", "\n");

                File.WriteAllText(asset, text, Encoding.UTF8);
                Debug.LogWarning("Convert to UTF-8: " + asset);
            }
        }
    }
}