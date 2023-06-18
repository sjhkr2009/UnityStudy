using UnityEditor;
using UnityEditor.AddressableAssets.Settings;

public class AddressableBuilder
{
    [MenuItem("MyMenu/Build Addressables")]
    public static void BuildAddressables() {
        AddressableAssetSettings.CleanPlayerContent();
        AddressableAssetSettings.BuildPlayerContent();
    }
}