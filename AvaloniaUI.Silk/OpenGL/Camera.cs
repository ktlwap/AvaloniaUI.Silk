using System.Numerics;

namespace AvaloniaUI.Silk.OpenGL;

public class Camera
{
    public Matrix4x4 GetViewMatrix(float width, float height)
    {
        return Matrix4x4.CreateOrthographic(width, height, 0f, 10f);
    }
}