using UnityEngine;
using UnityEngine.PostProcessing;

namespace UnityEditor.PostProcessing
{
    public class MinValueAttribute : PropertyAttribute
    {
        public float minValue;

        public MinValueAttribute(float minValue)
        {
            this.minValue = minValue;
        }
    }

    [CustomPropertyDrawer(typeof(MinValueAttribute))]
    sealed class MinValueDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            MinValueAttribute attribute = (MinValueAttribute)base.attribute;

            if (property.propertyType == SerializedPropertyType.Integer)
            {
                int v = EditorGUI.IntField(position, label, property.intValue);
                property.intValue = Mathf.Max(v, (int)attribute.minValue);
            }
            else if (property.propertyType == SerializedPropertyType.Float)
            {
                float v = EditorGUI.FloatField(position, label, property.floatValue);
                property.floatValue = Mathf.Max(v, attribute.minValue);
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use MinValue with float or int.");
            }
        }
    }
}
