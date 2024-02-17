using System;
using System.Numerics;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Threading;
using Silk.NET.OpenGL;
using Color = System.Drawing.Color;

namespace AvaloniaUI.Silk.OpenGL;

public class RenderControl : OpenGlControlBase
{
    private static readonly Vector3[] Vertices =
    [
        new Vector3(0.5f,  0.5f, 0.0f),
        new Vector3(0.5f, -0.5f, 0.0f),
        new Vector3(-0.5f, -0.5f, 0.0f),
        new Vector3(-0.5f,  0.5f, 0.0f)
    ];

    private static readonly uint[] Indices =
    [
        0, 1, 3,
        1, 2, 3
    ];

    private Transform _transform = new Transform();
    private Camera _camera = new Camera();
    
    private GL? _gl;
    private BufferObject<Vector3>? _vbo;
    private BufferObject<uint>? _ebo;
    private VertexArrayObject<Vector3, uint>? _vao;
    private Shader? _shader;
    
    protected override void OnOpenGlInit(GlInterface gl)
    {
        base.OnOpenGlInit(gl);

        _gl = GL.GetApi(gl.GetProcAddress);
        
        _vbo = new BufferObject<Vector3>(_gl, Vertices, BufferTargetARB.ArrayBuffer);
        _ebo = new BufferObject<uint>(_gl, Indices, BufferTargetARB.ElementArrayBuffer);
        _vao = new VertexArrayObject<Vector3, uint>(_gl, _vbo, _ebo);
        
        _vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 3 * sizeof(float), 0);

        _shader = new Shader(_gl, "Resources/shader.vert", "Resources/shader.frag");
    }

    protected override unsafe void OnOpenGlRender(GlInterface gl, int fb)
    {
        _gl!.ClearColor(Color.Azure);
        _gl.Clear((uint) ClearBufferMask.ColorBufferBit);
        _gl.Viewport((int) Bounds.X, (int) Bounds.Y, (uint) Bounds.Width, (uint) Bounds.Height);
        
        _transform.Angle = MathF.Tan(DateTime.Now.Millisecond / 1000f * MathF.PI);
        
        _vao!.Bind();
        _shader!.Use();
        _shader.SetUniform("uModel", _transform.GetModelMatrix());
        _shader.SetUniform("uView", _camera.GetViewMatrix((float) Bounds.Width, (float) Bounds.Height));
        
        _gl.DrawElements(PrimitiveType.Triangles, (uint) Indices.Length, DrawElementsType.UnsignedInt, null);
        Dispatcher.UIThread.Post(RequestNextFrameRendering, DispatcherPriority.Background);
    }

    protected override void OnOpenGlDeinit(GlInterface gl)
    {
        _vbo?.Dispose();
        _ebo?.Dispose();
        _vao?.Dispose();
        _shader?.Dispose();
        
        base.OnOpenGlDeinit(gl);
    }
}