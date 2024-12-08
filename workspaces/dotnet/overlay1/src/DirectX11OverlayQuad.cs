using System;
using System.Runtime.InteropServices;

namespace OMP.LSWTSS;

partial class Overlay1
{
    DirectX11OverlayQuad? _directX11OverlayQuad;

    sealed class DirectX11OverlayQuad : IDisposable
    {
        readonly SharpDX.Direct3D11.Device _device;

        readonly SharpDX.Direct3D11.DeviceContext _deviceContext;

        readonly SharpDX.Direct3D11.Texture2DDescription _textureDesc;

        readonly SharpDX.Direct3D11.Texture2D _texture;

        readonly int _textureBytesLength;

        readonly nint _textureBytesPtr;

        readonly DirectX11Quad _quad;

        readonly object _lock;

        bool _areTextureBytesDirty;

        bool _isDisposed;

        public readonly SharpDX.DXGI.SwapChain SwapChain;

        public int TextureWidth => _textureDesc.Width;

        public int TextureHeight => _textureDesc.Height;

        public DirectX11OverlayQuad(
            SharpDX.DXGI.SwapChain swapChain
        )
        {
            SwapChain = swapChain;

            _device = swapChain.GetDevice<SharpDX.Direct3D11.Device>();

            _deviceContext = _device.ImmediateContext;

            var backBuffer = swapChain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0);

            var backBufferDesc = backBuffer.Description;

            var backBufferViewport = new SharpDX.Mathematics.Interop.RawViewportF
            {
                X = 0,
                Y = 0,
                Width = backBufferDesc.Width,
                Height = backBufferDesc.Height,
                MinDepth = 0,
                MaxDepth = 1,
            };

            _textureDesc = new SharpDX.Direct3D11.Texture2DDescription
            {
                Width = backBufferDesc.Width,
                Height = backBufferDesc.Height,
                MipLevels = 1,
                ArraySize = 1,
                Format = backBufferDesc.Format switch
                {
                    SharpDX.DXGI.Format.R8G8B8A8_UNorm_SRgb => SharpDX.DXGI.Format.B8G8R8A8_UNorm_SRgb,
                    SharpDX.DXGI.Format.B8G8R8A8_UNorm_SRgb => SharpDX.DXGI.Format.B8G8R8A8_UNorm_SRgb,
                    SharpDX.DXGI.Format.B8G8R8X8_UNorm_SRgb => SharpDX.DXGI.Format.B8G8R8A8_UNorm_SRgb,
                    _ => SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                },
                SampleDescription = new SharpDX.DXGI.SampleDescription
                {
                    Count = 1,
                    Quality = 0,
                },
                Usage = SharpDX.Direct3D11.ResourceUsage.Default,
                BindFlags = SharpDX.Direct3D11.BindFlags.ShaderResource,
                CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.Write,
                OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
            };

            _texture = new SharpDX.Direct3D11.Texture2D(_device, _textureDesc);

            _textureBytesLength = _textureDesc.Width * _textureDesc.Height * 4;

            _textureBytesPtr = Marshal.AllocHGlobal(_textureBytesLength);

            _quad = new DirectX11Quad(_device, _texture, _textureDesc, backBuffer, backBufferViewport);

            _lock = new object();
        }

        public void UpdateTexture(nint textureBytesPtr)
        {
            lock (_lock)
            {
                if (_isDisposed)
                {
                    throw new InvalidOperationException();
                }

                unsafe
                {
                    Buffer.MemoryCopy(
                        (void*)textureBytesPtr,
                        (void*)_textureBytesPtr,
                        _textureBytesLength,
                        _textureBytesLength
                    );
                }

                _areTextureBytesDirty = true;
            }
        }

        public void Draw()
        {
            lock (_lock)
            {
                if (_isDisposed)
                {
                    throw new InvalidOperationException();
                }

                if (_areTextureBytesDirty)
                {
                    _deviceContext.UpdateSubresource(
                        dstResourceRef: _texture,
                        dstSubresource: 0,
                        dstBoxRef: new SharpDX.Direct3D11.ResourceRegion
                        {
                            Left = 0,
                            Top = 0,
                            Front = 0,
                            Right = _textureDesc.Width,
                            Bottom = _textureDesc.Height,
                            Back = 1,
                        },
                        srcDataRef: _textureBytesPtr,
                        srcRowPitch: _textureDesc.Width * 4,
                        srcDepthPitch: 0
                    );

                    _areTextureBytesDirty = false;
                }

                _quad.Draw();
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                if (!_isDisposed)
                {
                    _quad.Dispose();
                    Marshal.FreeHGlobal(_textureBytesPtr);
                    _texture.Dispose();
                    _deviceContext.Dispose();
                    _device.Dispose();

                    _isDisposed = true;
                }
            }
        }
    }
}