using UnityEngine;

[ExecuteInEditMode]
public class CameraEffect : MonoBehaviour
{
    public Material Mat;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(Mat == null)
        {
            Graphics.Blit(source, destination);
            return;
        }

        Graphics.Blit(source, destination, Mat);
    }
}
