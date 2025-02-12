namespace Environment
{
    // This one is for Unity 2021, the one for 6 is elsewhere
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    /// <summary>
    /// This render feature creates depth normals and colors after rendering opaques.
    /// </summary>
    public class PixelOutlineSetupFeature : ScriptableRendererFeature
    {
        public class EmptyPass : ScriptableRenderPass
        {
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) { }
        }

        EmptyPass emptyPass;

        public override void Create()
        {
            this.emptyPass = new EmptyPass
            {
                renderPassEvent = RenderPassEvent.AfterRenderingOpaques
            };
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            this.emptyPass.ConfigureInput(ScriptableRenderPassInput.Depth | ScriptableRenderPassInput.Normal | ScriptableRenderPassInput.Color);
            renderer.EnqueuePass(this.emptyPass);
        }
    }
}