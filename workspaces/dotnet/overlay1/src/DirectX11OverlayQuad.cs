using System;
using System.Runtime.InteropServices;

namespace OMP.LSWTSS;

partial class Overlay1
{
    DirectX11OverlayQuad? _directX11OverlayQuad;

    sealed class DirectX11OverlayQuad : IDisposable
    {
        readonly SharpDX.Direct3D11.Device1 _device1;

        readonly SharpDX.Direct3D11.DeviceContext _deviceContext;

        readonly SharpDX.Direct3D11.Texture2DDescription _textureDesc;

        readonly SharpDX.Direct3D11.Texture2D _texture;

        readonly DirectX11Quad _quad;

        readonly object _lock;

        bool _isDisposed;

        SharpDX.Direct3D11.Texture2D? _sharedTexture;

        volatile bool _isSharedTextureDirty;

        public readonly SharpDX.DXGI.SwapChain SwapChain;

        public int TextureWidth => _textureDesc.Width;

        public int TextureHeight => _textureDesc.Height;

        public DirectX11OverlayQuad(
            SharpDX.DXGI.SwapChain swapChain
        )
        {
            SwapChain = swapChain;

            _device1 = swapChain.GetDevice<SharpDX.Direct3D11.Device1>();

            _deviceContext = _device1.ImmediateContext;

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
                Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                SampleDescription = new SharpDX.DXGI.SampleDescription
                {
                    Count = 1,
                    Quality = 0,
                },
                Usage = SharpDX.Direct3D11.ResourceUsage.Default,
                BindFlags = SharpDX.Direct3D11.BindFlags.ShaderResource,
                CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
            };

            _texture = new SharpDX.Direct3D11.Texture2D(_device1, _textureDesc);

            _quad = new DirectX11Quad(_device1, _texture, _textureDesc, backBuffer, backBufferViewport);

            _lock = new object();
        }

        public void UpdateTexture(nint sharedTextureNativeHandle)
        {
            lock (_lock)
            {
                if (_isDisposed)
                {
                    throw new InvalidOperationException();
                }

                if (_sharedTexture?.NativePointer != sharedTextureNativeHandle)
                {
                    _sharedTexture?.Dispose();
                    _sharedTexture = _device1.OpenSharedResource1<SharpDX.Direct3D11.Texture2D>(sharedTextureNativeHandle);
                }

                _isSharedTextureDirty = true;
            }
        }

        public void Draw()
        {

            if (_isDisposed)
            {
                throw new InvalidOperationException();
            }

            if (_isSharedTextureDirty)
            {
                lock (_lock)
                {
                    if (_sharedTexture != null)
                    {
                        try
                        {
                            var sharedTextureDesc = _sharedTexture.Description;

                            if (
                                sharedTextureDesc.Width == _textureDesc.Width
                                &&
                                sharedTextureDesc.Height == _textureDesc.Height
                                &&
                                sharedTextureDesc.Format == _textureDesc.Format
                            )
                            {
                                _deviceContext.CopyResource(_sharedTexture, _texture);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        finally
                        {
                            _isSharedTextureDirty = false;
                        }
                    }
                }
            }

            _quad.Draw();
        }

        public void Dispose()
        {
            lock (_lock)
            {
                if (!_isDisposed)
                {
                    _quad.Dispose();
                    _texture.Dispose();
                    _deviceContext.Dispose();
                    _device1.Dispose();
                    _sharedTexture?.Dispose();

                    _isDisposed = true;
                }
            }
        }
    }
}