using Gamengine.core.render;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Gamengine.core  {
	public class Engine : GameWindow {
		
		private float[] vertices = {
			-0.5f, -0.5f, 0.0f, //Bottom-left vertex
			0.5f, -0.5f, 0.0f, //Bottom-right vertex
			0.0f, 0.5f, 0.0f //Top vertex
		};
		private int _vertexBufferObject;
		private int _vertexArrayObject;
		private InputSystem _inputSystem = new();
		private Shader Shader;

		public InputSystem GetInputSystem() {
			return _inputSystem;
		}
		
		public Engine() : base( GameWindowSettings.Default, NativeWindowSettings.Default ) {
		}

		protected override void OnUpdateFrame(FrameEventArgs evt) {
			
			_inputSystem.CheckInput(this, KeyboardState);

			base.OnUpdateFrame(evt);
		}

		protected override void OnLoad() {
			GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
			Context.SwapBuffers();
			
			_vertexBufferObject = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
			
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

			_vertexArrayObject = GL.GenVertexArray();
			// ..:: Initialization code (done once (unless your object frequently changes)) :: ..
			// 1. bind Vertex Array Object
			GL.BindVertexArray(_vertexArrayObject);
			// 2. copy our vertices array in a buffer for OpenGL to use
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
			// 3. then set our vertex attributes pointers
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);
			
			Shader = new Shader(
				 "C:/Users/Flavia/RiderProjects/Gamengine/Gamegine/src/shader/shader.vert",
				 "C:/Users/Flavia/RiderProjects/Gamengine/Gamegine/src/shader/shader.frag"
				);
			
			base.OnLoad();
		}

		protected override void OnUnload() {
			Shader.Dispose();
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.DeleteBuffer(_vertexBufferObject);
			base.OnUnload();
		}

		protected override void OnRenderFrame(FrameEventArgs args) {
			GL.Clear(ClearBufferMask.ColorBufferBit);

			Shader.Use();
			GL.BindVertexArray(_vertexArrayObject);
			GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
			
			Context.SwapBuffers();
			
			base.OnRenderFrame(args);
		}

		protected override void OnResize(ResizeEventArgs e) {
			GL.Viewport(0, 0, this.Size.X, this.Size.Y);
			
			base.OnResize(e);
		}

		public void Destroy() {
			DestroyWindow();
		}
	}
}