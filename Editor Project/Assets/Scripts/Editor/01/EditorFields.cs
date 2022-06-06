using UnityEditor;
using UnityEngine;

public class EditorFields : EditorWindow {
    // 메뉴의 우선순위를 priority로 지정할 수 있다. 기본값은 1000이며, 앞뒤 요소와 11 이상 차이가 날 경우 구분선이 생긴다.
    [MenuItem("Custom/02-Fields", false, 2000)]
    static void Open() {
        GetWindow<EditorFields>();
    }

    private int intValue;
    private float floatValue;
    private Color colorValue;
    private Gradient gradientValue = new Gradient(); // 클래스니까 기본값이 있어야 에디터에서 null ref 에러가 안 난다.
    private Vector3 vector3Value;
    private Rect rectValue;
    
    private UnityEngine.Object objValue;
    private string stringValue;
    private string passwordValue;
    private string tagValue;
    
    private ParticleSystemCollisionType enumValue;
    private int selectionValue;
    private string[] stringArr = new string[] {"String 1", "String 2", "String 3"};
    private bool boolValue;

    private void OnGUI() {
        intValue = EditorGUILayout.IntField("int 값", intValue);
        floatValue = EditorGUILayout.FloatField("float 값", floatValue);
        colorValue = EditorGUILayout.ColorField("Color 값", colorValue);
        gradientValue = EditorGUILayout.GradientField("Gradient 값", gradientValue);
        vector3Value = EditorGUILayout.Vector3Field("Vector3 값", vector3Value);
        rectValue = EditorGUILayout.RectField("Rect 값", rectValue);

        EditorGUILayout.Space(20);
        
        // HelpBox로 박스 형태의 문구를 띄울 수 있다.
        EditorGUILayout.HelpBox("오브젝트는 오브젝트의 타입을 입력한다. 타입이 맞지 않는 오브젝트는 놓을 수 없다.", MessageType.None);
        objValue = EditorGUILayout.ObjectField("Object 값", objValue, typeof(Object), true);
        
        EditorGUILayout.Space(20);
        
        EditorGUILayout.HelpBox("문자열은 여러 가지 형태로 입력이 가능하다.", MessageType.Info);
        stringValue = EditorGUILayout.TextField("string 값", stringValue);
        passwordValue = EditorGUILayout.PasswordField("string (숨김)", passwordValue);
        tagValue = EditorGUILayout.TagField("string (태그)", tagValue);
        
        EditorGUILayout.Space(20);
        
        EditorGUILayout.HelpBox("enum값은 Enum 타입을 반환하므로 원하는 enum 타입으로 형변환해야 한다.", MessageType.Warning);
        enumValue = (ParticleSystemCollisionType)EditorGUILayout.EnumFlagsField("Enum 값", enumValue);
        
        EditorGUILayout.Space(20);
        
        EditorGUILayout.HelpBox("bool값은 함수명이 Toggle임에 유의", MessageType.Error);
        boolValue = EditorGUILayout.Toggle("Bool 값", boolValue);
        
        EditorGUILayout.Space(20);
        
        EditorGUILayout.HelpBox("Toolbar는 string 배열에 적힌 이름들로 항목을 나열하고, 몇 번째가 선택되어 있는지 반환한다.", MessageType.Info);
        selectionValue = GUILayout.Toolbar(selectionValue, stringArr);
        EditorGUILayout.HelpBox("반환받은 값으로 선택에 따른 추가적인 처리를 할 수 있다.", MessageType.None);
        selectionValue = GUILayout.SelectionGrid(selectionValue, stringArr, 2); // xCount는 한 가로줄에 몇 개의 요소를 나열할지 결정
        
        EditorGUILayout.Space(20);
        
        EditorGUILayout.HelpBox("Box 함수로 텍스쳐를 그릴 수 있다. " +
                                "GUIContent에 이름을 입력해서 유니티 Built-In 텍스쳐들을 가져올 수 있는데, " +
                                "이름과 종류에 대해서는 IconInfo/UnityEditorIconInfos.md 문서를 참고", MessageType.Info);
        GUILayout.Box(EditorGUIUtility.IconContent("Animation.Record"));
        GUILayout.Box(EditorGUIUtility.IconContent("_Help"));
    }
}
