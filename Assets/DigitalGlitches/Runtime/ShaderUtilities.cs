// Copyright 2014-2019 Elringus (Artyom Sovetnikov). All Rights Reserved.

using UnityEngine;

namespace DigitalGlitches
{
    /// <summary>
    /// Available types of a <see cref="UnityEngine.Shader"/> property.
    /// </summary>
    public enum ShaderPropertyType
    {
        Color,
        Vector,
        Float,
        Texture
    }

    /// <summary>
    /// Collection of static utilities to work with <see cref="Shader"/> and associated objects.
    /// </summary>
    public static class ShaderUtilities
    {
        /// <summary>
        /// Copies material property value from one material to another when the property is defined in both materials.
        /// </summary>
        public static void TransferMaterialProperty (int propertyId, ShaderPropertyType propertyType, Material fromMaterial, Material toMaterial)
        {
            if (!fromMaterial || !toMaterial) return;
            if (!fromMaterial.HasProperty(propertyId) || !toMaterial.HasProperty(propertyId)) return;

            switch (propertyType)
            {
                case ShaderPropertyType.Color:
                    toMaterial.SetColor(propertyId, fromMaterial.GetColor(propertyId));
                    break;
                case ShaderPropertyType.Vector:
                    toMaterial.SetVector(propertyId, fromMaterial.GetVector(propertyId));
                    break;
                case ShaderPropertyType.Float:
                    toMaterial.SetFloat(propertyId, fromMaterial.GetFloat(propertyId));
                    break;
                case ShaderPropertyType.Texture:
                    toMaterial.SetTexture(propertyId, fromMaterial.GetTexture(propertyId));
                    break;
            }
        }

        /// <summary>
        /// Copies material property value from one material to another when the property is defined in both materials.
        /// </summary>
        public static void TransferMaterialProperty (string propertyName, ShaderPropertyType propertyType, Material fromMaterial, Material toMaterial)
        {
            var propertyId = Shader.PropertyToID(propertyName);
            TransferMaterialProperty(propertyId, propertyType, fromMaterial, toMaterial);
        }

        /// <summary>
        /// Checks whether provided value object is of a type.
        /// </summary>
        public static bool CheckPropertyValueType (object value, ShaderPropertyType type)
        {
            if (value == null) return false;

            switch (type)
            {
                case ShaderPropertyType.Color:
                    return value is Color;
                case ShaderPropertyType.Vector:
                    return value is Vector4;
                case ShaderPropertyType.Float:
                    return value is float;
                case ShaderPropertyType.Texture:
                    return value is Texture;
                default: return false;
            }
        }

        /// <summary>
        /// Attempts to set provided property value object to the material.
        /// Will silently fail if the property is of a wrong type or doesn't exist.
        /// </summary>
        public static void SetProperty (this Material material, ShaderPropertyType type, string name, object value)
        {
            if (!material || !material.HasProperty(name) || !CheckPropertyValueType(value, type)) return;

            switch (type)
            {
                case ShaderPropertyType.Color:
                    material.SetColor(name, (Color)value);
                    break;
                case ShaderPropertyType.Vector:
                    material.SetVector(name, (Vector4)value);
                    break;
                case ShaderPropertyType.Float:
                    material.SetFloat(name, (float)value);
                    break;
                case ShaderPropertyType.Texture:
                    material.SetTexture(name, (Texture)value);
                    break;
            }
        }

        /// <summary>
        /// Attempts to get a value of the property with the provided name from the material.
        /// </summary>
        public static object GetProperty (this Material material, ShaderPropertyType type, string name)
        {
            if (!material || !material.HasProperty(name)) return null;

            switch (type)
            {
                case ShaderPropertyType.Color:
                    return material.GetColor(name);
                case ShaderPropertyType.Vector:
                    return material.GetVector(name);
                case ShaderPropertyType.Float:
                    return material.GetFloat(name);
                case ShaderPropertyType.Texture:
                    return material.GetTexture(name);
                default: return null;
            }
        }
    }
}
