// Copyright 2014-2019 Elringus (Artyom Sovetnikov). All Rights Reserved.

using UnityEngine;

namespace DigitalGlitches
{
    public abstract class GlitchEffect : MonoBehaviour
    {
        [Header("Textures")]
        [Tooltip("The texture to use for glitch pattern.\nIf non specified the default one will be used.\n\nApplied on scene start.")]
        public Texture GlitchTexture;
        [Tooltip("The texture to show on top of the final image.\nIf non specified the default one will be used.\n\nApplied on scene start.")]
        public Texture OverlayTexture;
        [Tooltip("Whether to show the overlay texture.")]
        public bool ShowOverlay = false;

        [Header("Intensity")]
        [Range(.0f, 10f), Tooltip("General intensity of the effect.")]
        public float Intensity = 1.0f;
        [Tooltip("Whether to randomize glitch triggering.")]
        public bool RandomGlitchFrequency = true;
        [Range(.001f, 1f), Tooltip("Frequency of glitches.\n\nIgnored if RandomGlitchFrequency is enabled.")]
        public float GlitchFrequency = .5f;

        [Header("UV shifting")]
        [Tooltip("Whether to apply shifting to the UV coordinates of the final image.")]
        public bool PerformUVShifting = true;
        [Range(.001f, 10f), Tooltip("Controls amount of the UV shifting.\n\nHave no effect when PerformUVShifting is disabled.")]
        public float ShiftAmount = 1f;
        [Tooltip("Whether to apply shifting to the whole body of the element.\n\nHave no effect when PerformUVShifting is disabled.")]
        public bool PerformBodyShifting = true;

        [Header("Colors")]
        [Tooltip("Whether to apply color shifting to the image.")]
        public bool PerformColorShifting = true;
        [Tooltip("Glitches will be tinted with the specified color.\n\nHave no effect when PerformColorShifting is disabled.")]
        public Color TintColor = new Color(.2f, .2f, 0f, 0f);
        [Tooltip("Whether to perform an advanced color blending (color burn and divide) when shifting colors.\n\nHave no effect when PerformColorShifting is disabled.")]
        public bool BurnColors = true;
        [Tooltip("Whether to perform an advanced color blending (color dodge and difference) when shifting colors.\n\nHave no effect when PerformColorShifting is disabled.")]
        public bool DodgeColors = false;

        protected static readonly int GlitchTexturePropertyId = Shader.PropertyToID("_DIGITAL_GLITCHES_GlitchTex");
        protected static readonly int GlitchTextureSTPropertyId = Shader.PropertyToID("_DIGITAL_GLITCHES_GlitchTex_ST");
        protected static readonly int OverlayTexturePropertyId = Shader.PropertyToID("_DIGITAL_GLITCHES_OverlayTex");
        protected static readonly int ShowOverlayPropertyId = Shader.PropertyToID("_DIGITAL_GLITCHES_ShowOverlay");
        protected static readonly int IntensityPropertyId = Shader.PropertyToID("_DIGITAL_GLITCHES_Intensity");
        protected static readonly int ColorTintPropertyId = Shader.PropertyToID("_DIGITAL_GLITCHES_ColorTint");
        protected static readonly int BurnColorsPropertyId = Shader.PropertyToID("_DIGITAL_GLITCHES_BurnColors");
        protected static readonly int DodgeColorsPropertyId = Shader.PropertyToID("_DIGITAL_GLITCHES_DodgeColors");
        protected static readonly int PerformUVShiftingPropertyId = Shader.PropertyToID("_DIGITAL_GLITCHES_PerformUVShifting");
        protected static readonly int PerformColorShiftingPropertyId = Shader.PropertyToID("_DIGITAL_GLITCHES_PerformColorShifting");
        protected static readonly int PerformBodyShiftingPropertyId = Shader.PropertyToID("_DIGITAL_GLITCHES_PerformBodyShifting");
        protected static readonly int FilterRadiusPropertyId = Shader.PropertyToID("_DIGITAL_GLITCHES_FilterRadius");
        protected static readonly int FlipUpPropertyId = Shader.PropertyToID("_DIGITAL_GLITCHES_FlipUp");
        protected static readonly int FlipDownPropertyId = Shader.PropertyToID("_DIGITAL_GLITCHES_FlipDown");
        protected static readonly int DisplacePropertyId = Shader.PropertyToID("_DIGITAL_GLITCHES_Displace");

        protected abstract string GlitchShaderResourcePath { get; }
        protected virtual string GlitchTextureResourcePath => "DigitalGlitches/GlitchTexture";
        protected virtual string OverlayTextureResourcePath => "DigitalGlitches/GlitchTexture";
        protected virtual Shader GlitchShader => cachedShader ? cachedShader : (cachedShader = CreateGlitchShader());
        protected virtual Material GlitchMaterial => cachedMaterial ? cachedMaterial :(cachedMaterial = CreateGlitchMaterial());

        private float glitchUp, glitchDown, flicker, glitchUpTime = .05f, glitchDownTime = .05f, flickerTime = .5f;

        private Shader cachedShader;
        private Material cachedMaterial;

        protected virtual void OnEnable ()
        {
            flickerTime = RandomGlitchFrequency ? Random.value : 1f - GlitchFrequency;
            glitchUpTime = RandomGlitchFrequency ? Random.value : .1f - GlitchFrequency / 10f;
            glitchDownTime = RandomGlitchFrequency ? Random.value : .1f - GlitchFrequency / 10f;
        }

        protected virtual void OnDisable ()
        {
            if (cachedMaterial)
            {
                if (Application.isPlaying) Destroy(cachedMaterial);
                else DestroyImmediate(cachedMaterial);

                cachedMaterial = null;
            }
        }

        protected virtual Shader CreateGlitchShader ()
        {
            return Resources.Load(GlitchShaderResourcePath) as Shader;
        }

        protected virtual Material CreateGlitchMaterial ()
        {
            var material = new Material(GlitchShader);
            material.SetTexture(GlitchTexturePropertyId, GlitchTexture ? GlitchTexture : Resources.Load(GlitchTextureResourcePath) as Texture);
            material.SetVector(GlitchTextureSTPropertyId, GetCurrentTextureScale());
            material.SetTexture(OverlayTexturePropertyId, OverlayTexture ? OverlayTexture : Resources.Load(OverlayTextureResourcePath) as Texture);
            material.hideFlags = HideFlags.HideAndDontSave;
            return material;
        }

        protected virtual Vector2 GetCurrentTextureScale ()
        {
            return new Vector2(Screen.width / (GlitchTexture ? (float)GlitchTexture.width : 512f),
                Screen.height / (GlitchTexture ? (float)GlitchTexture.height : 512f));
        }

        protected virtual void UpdateGlitchMaterial ()
        {
            GlitchMaterial.SetFloat(ShowOverlayPropertyId, ShowOverlay ? 1 : 0);
            GlitchMaterial.SetFloat(IntensityPropertyId, Intensity);
            GlitchMaterial.SetColor(ColorTintPropertyId, TintColor);
            GlitchMaterial.SetFloat(BurnColorsPropertyId, BurnColors ? 1 : 0);
            GlitchMaterial.SetFloat(DodgeColorsPropertyId, DodgeColors ? 1 : 0);
            GlitchMaterial.SetFloat(PerformUVShiftingPropertyId, PerformUVShifting ? 1 : 0);
            GlitchMaterial.SetFloat(PerformColorShiftingPropertyId, PerformColorShifting ? 1 : 0);
            GlitchMaterial.SetFloat(PerformBodyShiftingPropertyId, PerformBodyShifting ? 1 : 0);

            if (Intensity == 0) GlitchMaterial.SetFloat(FilterRadiusPropertyId, 0);

            glitchUp += Time.deltaTime * Intensity;
            glitchDown += Time.deltaTime * Intensity;
            flicker += Time.deltaTime * Intensity;

            if (flicker > flickerTime)
            {
                GlitchMaterial.SetFloat(FilterRadiusPropertyId, Random.Range(-3f, 3f) * Intensity * ShiftAmount);
                var currentScale = GetCurrentTextureScale();
                GlitchMaterial.SetVector(GlitchTextureSTPropertyId, new Vector4(currentScale.x, currentScale.y, Random.Range(-3f, 3f), Random.Range(-3f, 3f)));
                flicker = 0;
                flickerTime = RandomGlitchFrequency ? Random.value : 1f - GlitchFrequency;
            }

            if (glitchUp > glitchUpTime)
            {
                if (Random.Range(0f, 1f) < .1f * Intensity) GlitchMaterial.SetFloat(FlipUpPropertyId, Random.Range(0f, 1f) * Intensity);
                else GlitchMaterial.SetFloat(FlipUpPropertyId, 0);

                glitchUp = 0;
                glitchUpTime = RandomGlitchFrequency ? Random.value / 10f : .1f - GlitchFrequency / 10f;
            }

            if (glitchDown > glitchDownTime)
            {
                if (Random.Range(0f, 1f) < .1f * Intensity) GlitchMaterial.SetFloat(FlipDownPropertyId, 1f - Random.Range(0f, 1f) * Intensity);
                else GlitchMaterial.SetFloat(FlipDownPropertyId, 1f);

                glitchDown = 0;
                glitchDownTime = RandomGlitchFrequency ? Random.value / 10f : .1f - GlitchFrequency / 10f;
            }

            if (Random.Range(0f, 1f) < .1f * Intensity * (RandomGlitchFrequency ? 1 : GlitchFrequency))
                GlitchMaterial.SetFloat(DisplacePropertyId, Random.value * Intensity);
            else GlitchMaterial.SetFloat(DisplacePropertyId, 0);
        }
    }
}
