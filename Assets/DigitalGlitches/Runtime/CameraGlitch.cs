// Copyright 2014-2019 Elringus (Artyom Sovetnikov). All Rights Reserved.

using UnityEngine;
using UnityEngine.Rendering;

namespace DigitalGlitches
{
    [RequireComponent(typeof(Camera)), AddComponentMenu("Effects/Digital Glitches/Camera Glitch"), ExecuteInEditMode]
    public class CameraGlitch : GlitchEffect
    {
        protected override string GlitchShaderResourcePath => "DigitalGlitches/CameraGlitch";

        private static readonly int inverseUVPropertyId = Shader.PropertyToID("_InverseUV");

        private void OnRenderImage (RenderTexture source, RenderTexture destination)
        {
            GlitchMaterial.SetFloat(inverseUVPropertyId, 0);

            Graphics.Blit(source, destination, GlitchMaterial);
        }

        private void Update () => UpdateGlitchMaterial();

        #if DIGITAL_GLITCHES_SRP
        protected override void OnEnable ()
        {
            base.OnEnable();
            
            RenderPipelineManager.endCameraRendering += HandleSRPCameraRender;
        }

        protected override void OnDisable ()
        {
            base.OnDisable();

            RenderPipelineManager.endCameraRendering -= HandleSRPCameraRender;
        }

        private void HandleSRPCameraRender (ScriptableRenderContext context, Camera camera)
        {
            GlitchMaterial.SetFloat(inverseUVPropertyId, 1);

            var renderTex = RenderTexture.GetTemporary(camera.pixelWidth, camera.pixelHeight);
            var buffer = CommandBufferPool.Get("DigitalGlitchesCamera");

            buffer.Blit(BuiltinRenderTextureType.CameraTarget, renderTex);
            buffer.Blit(renderTex, BuiltinRenderTextureType.CameraTarget, GlitchMaterial);
            context.ExecuteCommandBuffer(buffer);
            context.Submit();

            buffer.Clear();
            CommandBufferPool.Release(buffer);
            RenderTexture.ReleaseTemporary(renderTex);
        }
        #endif
    }
}
