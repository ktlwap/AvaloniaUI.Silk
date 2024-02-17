using System;
using Silk.NET.OpenGL;

namespace AvaloniaUI.Silk.OpenGL;

public class BufferObject<TDataType> : IDisposable
    where TDataType : unmanaged
{
    private readonly GL _gl;
    private readonly uint _handle;
    private readonly BufferTargetARB _bufferType;

    public unsafe BufferObject(GL gl, Span<TDataType> data, BufferTargetARB bufferType)
    {
        _gl = gl;
        _bufferType = bufferType;

        _handle = _gl.GenBuffer();
        Bind();
        fixed (void* pData = data)
        {
            _gl.BufferData(bufferType, (nuint) (data.Length * sizeof(TDataType)), pData, BufferUsageARB.StaticDraw);
        }
    }

    public void Bind()
    {
        _gl.BindBuffer(_bufferType, _handle);
    }

    public void Dispose()
    {
        _gl.DeleteBuffer(_handle);
    }
}