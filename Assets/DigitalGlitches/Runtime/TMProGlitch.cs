// Copyright 2014-2019 Elringus (Artyom Sovetnikov). All Rights Reserved.

using UnityEngine;

namespace DigitalGlitches
{
    #if DIGITAL_GLITCHES_TMPRO // Can define the whole script as Unity won't properly serialize it this way.
    [RequireComponent(typeof(TMPro.TMP_Text)), AddComponentMenu("Effects/Digital Glitches/TextMesh Pro Glitch"), ExecuteInEditMode]
    #endif
    public class TMProGlitch : GlitchEffect
    {
        protected override string GlitchShaderResourcePath => "DigitalGlitches/TMProGlitch"; 
        #if DIGITAL_GLITCHES_TMPRO
        protected Material DefaultMaterial => tmpText.font.material;

        private TMPro.TMP_Text tmpText;

        private void Awake ()
        {
            tmpText = GetComponent<TMPro.TMP_Text>();
        }

        protected override void OnEnable ()
        {
            base.OnEnable();

            tmpText.fontMaterial = GlitchMaterial;
        }

        protected override void OnDisable ()
        {
            base.OnEnable();

            tmpText.fontMaterial = DefaultMaterial;
        }

        protected override Material CreateGlitchMaterial ()
        {
            var material = base.CreateGlitchMaterial();

            // Copy atlas texture and other props from the previous material.
            TransferTMProProperties(DefaultMaterial, material);

            return material;
        }

        private void Update ()
        {
            UpdateGlitchMaterial();
        }

        private static void TransferTMProProperties (Material fromMaterial, Material toMaterial)
        {
            // Not using Material.CopyPropertiesFromMaterial(), cause we shouldn't copy stencil props and some other stuff.

            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_MainTex, ShaderPropertyType.Texture, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_FaceTex, ShaderPropertyType.Texture, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_FaceColor, ShaderPropertyType.Color, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_FaceDilate, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_Shininess, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_UnderlayColor, ShaderPropertyType.Color, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_UnderlayOffsetX, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_UnderlayOffsetY, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_UnderlayDilate, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_UnderlaySoftness, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_WeightNormal, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_WeightBold, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_OutlineTex, ShaderPropertyType.Texture, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_OutlineWidth, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_OutlineSoftness, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_OutlineColor, ShaderPropertyType.Color, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_Padding, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_GradientScale, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_ScaleX, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_ScaleY, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_PerspectiveFilter, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_TextureWidth, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_TextureHeight, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_BevelAmount, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_GlowColor, ShaderPropertyType.Color, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_GlowOffset, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_GlowPower, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_GlowOuter, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_LightAngle, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_ShaderFlags, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_ScaleRatio_A, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_ScaleRatio_B, ShaderPropertyType.Float, fromMaterial, toMaterial);
            ShaderUtilities.TransferMaterialProperty(TMPro.ShaderUtilities.ID_ScaleRatio_C, ShaderPropertyType.Float, fromMaterial, toMaterial);

            if (fromMaterial.IsKeywordEnabled(TMPro.ShaderUtilities.Keyword_Bevel)) toMaterial.EnableKeyword(TMPro.ShaderUtilities.Keyword_Bevel);
            if (fromMaterial.IsKeywordEnabled(TMPro.ShaderUtilities.Keyword_Glow)) toMaterial.EnableKeyword(TMPro.ShaderUtilities.Keyword_Glow);
            if (fromMaterial.IsKeywordEnabled(TMPro.ShaderUtilities.Keyword_Underlay)) toMaterial.EnableKeyword(TMPro.ShaderUtilities.Keyword_Underlay);
            if (fromMaterial.IsKeywordEnabled(TMPro.ShaderUtilities.Keyword_Ratios)) toMaterial.EnableKeyword(TMPro.ShaderUtilities.Keyword_Ratios);
            if (fromMaterial.IsKeywordEnabled(TMPro.ShaderUtilities.Keyword_Outline)) toMaterial.EnableKeyword(TMPro.ShaderUtilities.Keyword_Outline);
        }
        #endif
    }
}
