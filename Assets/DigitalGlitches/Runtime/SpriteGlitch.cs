// Copyright 2014-2019 Elringus (Artyom Sovetnikov). All Rights Reserved.

using UnityEngine;

namespace DigitalGlitches
{
    [RequireComponent(typeof(SpriteRenderer)), AddComponentMenu("Effects/Digital Glitches/Sprite Glitch"), ExecuteInEditMode]
    public class SpriteGlitch : GlitchEffect
    {
        protected override string GlitchShaderResourcePath => "DigitalGlitches/SpriteGlitch";

        private SpriteRenderer spriteRenderer;
        private Material initialMaterial;

        private void Awake ()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            initialMaterial = spriteRenderer.sharedMaterial;
        }

        protected override void OnEnable ()
        {
            base.OnEnable();

            spriteRenderer.sharedMaterial = GlitchMaterial;
        }

        protected override void OnDisable ()
        {
            base.OnEnable();

            spriteRenderer.sharedMaterial = initialMaterial;
        }

        private void Update ()
        {
            UpdateGlitchMaterial();
        }
    }
}
