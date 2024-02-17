using System.Numerics;

namespace AvaloniaUI.Silk.OpenGL;

public class Transform
{
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 Scale { get; set; } = Vector2.One * 100;
    public float Angle { get; set; }

    public Matrix4x4 GetModelMatrix()
    {
        return Matrix4x4.CreateRotationZ(Angle) *
               Matrix4x4.CreateScale(new Vector3(Scale, 0.0f)) *
               Matrix4x4.CreateTranslation(new Vector3(Position, 0.0f));   
    }
}