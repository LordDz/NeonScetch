// Copyright 2014-2019 Elringus (Artyom Sovetnikov). All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;

namespace DigitalGlitches
{
    [RequireComponent(typeof(MaskableGraphic)), AddComponentMenu("Effects/Digital Glitches/UI Glitch"), ExecuteInEditMode]
    public class UIGlitch : GlitchEffect
    {
        protected override string GlitchShaderResourcePath => "DigitalGlitches/UIGlitch";

        private MaskableGraphic maskableGraphic;
        private Material initialMaterial;

        private void Awake ()
        {
            maskableGraphic = GetComponent<MaskableGraphic>();
            initialMaterial = maskableGraphic.material;
        }

        protected override void OnEnable ()
        {
            base.OnEnable();

            maskableGraphic.material = GlitchMaterial;
        }

        protected override void OnDisable ()
        {
            base.OnEnable();

            maskableGraphic.material = initialMaterial;
        }

        private void Update ()
        {
            UpdateGlitchMaterial();
        }
    }
}
