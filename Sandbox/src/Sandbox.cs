using System;
using Gamengine.core;
using Gamengine.core.render;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Sandbox {
	public class Program : Engine {
		
		private int _vertexArrayObject;
		private VertexBuffer _vertexBuffer;
		private Shader _shader;

		private float[] _vertices = {
			-0.5f, -0.5f, 0.0f, //Bottom-left vertex
			0.5f, -0.5f, 0.0f, //Bottom-right vertex
			0.0f,  0.5f, 0.0f  //Top vertex
		};
		
		static void Main(string[] args) {
			Program engine = new(args);
			engine.Run();
		}

		public Program(string[] args) : base(args) {
			GetInputSystem().RegisterInput(Keys.R, window => window.Title = new Random().Next().ToString() );
			GetInputSystem().RegisterInput(Keys.Escape, window => window.Destroy() );
			GetInputSystem().RegisterInput(Keys.K, window => Console.WriteLine("delay!"), 400);
		}
		
		protected override void OnLoad() {
			GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
			Context.SwapBuffers();

			_vertexBuffer = new VertexBuffer( _vertices, _vertices.Length * sizeof(float) );
			
			
			_vertexArrayObject = GL.GenVertexArray();
			// bind Vertex Array Object
			GL.BindVertexArray(_vertexArrayObject);
			
			// then set our vertex attributes pointers
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

			_shader = new Shader("src/shader/triangle.shader");

			base.OnLoad();
		}

		protected override void OnUnload() {
			_shader.Dispose();
			_vertexBuffer.UnBind();
			base.OnUnload();
		}

		protected override void OnResize(ResizeEventArgs e) {
			GL.Viewport(0, 0, this.Size.X, this.Size.Y);
			base.OnResize(e);
		}

		protected override void OnRenderFrame(FrameEventArgs evt) {
			GL.Clear(ClearBufferMask.ColorBufferBit);

			_shader.Use();
			_vertexBuffer.Bind();
			
			GL.BindVertexArray(_vertexArrayObject);
			GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length);
			
			Context.SwapBuffers();
			base.OnRenderFrame(evt);
		}
	}
}