using UnityEngine;
using UnityEngine.PostProcessing;
using PostProcessingMin = UnityEngine.PostProcessing.MinAttribute;

namespace UnityEditor.PostProcessing
{

    [CustomPropertyDrawer(typeof(PostProcessingMin))]
    sealed class MinDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //MinAttribute attribute = (MinAttribute)base.attribute;
            PostProcessingMin attribute = (PostProcessingMin)base.attribute;

            if (property.propertyType == SerializedPropertyType.Integer)
            {
                int v = EditorGUI.IntField(position, label, property.intValue);
                property.intValue = (int)Mathf.Max(v, attribute.min);
            }
            else if (property.propertyType == SerializedPropertyType.Float)
            {
                float v = EditorGUI.FloatField(position, label, property.floatValue);
                property.floatValue = Mathf.Max(v, attribute.min);
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use Min with float or int.");
            }
        }
    }
}
//You'll get that same error on any line that references MinAttribute without disambiguation. To fix it, you have the same two options:

//1. Fully qualify all MinAttribute references:

//csharp
//MinAttribute attribute = (UnityEngine.PostProcessing.MinAttribute)base.attribute;


//2.Add a using alias at the top and use that:

//csharp
//using PostProcessingMin = UnityEngine.PostProcessing.MinAttribute; 

//// ...

//MinAttribute attribute = (PostProcessingMin)base.attribute;


//So your full corrected code would be:

//csharp
//using UnityEngine;
//using UnityEngine.PostProcessing; 
//using PostProcessingMin = UnityEngine.PostProcessing.MinAttribute; 

//namespace UnityEditor.PostProcessing
//{
//    [CustomPropertyDrawer(typeof(PostProcessingMin))]
//    sealed class MinDrawer : PropertyDrawer
//    {
//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        {
//            PostProcessingMin attribute = (PostProcessingMin)base.attribute;

//            // Rest of code...
//        }
//    }
//}


//This should resolve all the ambiguous MinAttribute references and fix the errors. Let me know if you have any other questions!