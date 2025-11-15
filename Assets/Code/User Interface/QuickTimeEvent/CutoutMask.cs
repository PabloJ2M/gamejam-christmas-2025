using UnityEngine.Rendering;

namespace UnityEngine.UI
{
    public class CutoutMask : Image
    {
        public override Material materialForRendering
        {
            get {
                Material material = new Material(base.materialForRendering);
                material.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
                return material;
            }
        }
    }
}