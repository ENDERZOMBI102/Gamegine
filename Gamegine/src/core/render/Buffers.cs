using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace Gamengine.core.render {

	public class BufferLayout {
		private List<BufferElement> _elements;
		private uint _stride;
		public BufferLayout(List<BufferElement> elements) {
			_elements = elements;
			InitOffsetAndStride();
		}
		
		public List<BufferElement> GetElements() {
			return _elements;
		}

		private void InitOffsetAndStride() {
			uint offset = 0;
			_stride = 0;
			foreach (var element in _elements ) {
				element.Offset = offset;
				offset += element.Size;
				_stride += element.Size;
			}
			
		}
		
	}

	public class BufferElement {
		public string Name;
		public ShaderDataType Type;
		public uint Offset;
		public uint Size;

		BufferElement(string name, ShaderDataType type) {
			Name = name;
			Type = type;
			Size = GetSizeForType(type);
			Offset = 0;
		}

		private static uint GetSizeForType(ShaderDataType type) {
			switch (type) {
				case ShaderDataType.Float: return 4;
				case ShaderDataType.Float2: return 4 * 2;
				case ShaderDataType.Float3: return 4 * 3;
				case ShaderDataType.Float4: return 4 * 4;
				case ShaderDataType.Mat3: return 4 * 3 * 3;
				case ShaderDataType.Mat4: return 4 * 4 * 4;
				case ShaderDataType.Int: return 4;
				case ShaderDataType.Int2: return 4 * 2;
				case ShaderDataType.Int3: return 4 * 3;
				case ShaderDataType.Int4: return 4 * 4;
				case ShaderDataType.Bool: return 1;
			}
			// THIS SHOULD NEVER HAPPEN
			return 0;
		}
		
	}

	public enum ShaderDataType {
		None, Float, Float2, Float3, Float4, Mat3, Mat4, Int, Int2, Int3, Int4, Bool
	}

	public class VertexBuffer {

		private readonly int _handle;
		
		public VertexBuffer(float[] vertices, int size) {
			_handle = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _handle);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
		}
		
		public void Bind() {
			GL.BindBuffer(BufferTarget.ArrayBuffer, _handle);
		}
		
		public void UnBind() {
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}

		~VertexBuffer() {
			GL.DeleteBuffer(_handle);
		}

	}
	
	public class IndexBuffer {
		
		private readonly int _handle;
		
		public IndexBuffer(int[] indices) {
			_handle = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, _handle);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(float), indices, BufferUsageHint.StaticDraw);
		}
		
		public void Bind() {
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, _handle);
		}
		
		public void UnBind() {
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
		}
		
		~IndexBuffer() {
			GL.DeleteBuffer(_handle);
		}
		
	}
}