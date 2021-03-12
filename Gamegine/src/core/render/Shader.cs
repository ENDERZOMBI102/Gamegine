using System;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace Gamengine.core.render {
	
	public class Shader : IDisposable {
		int Handle;
		public int VertexShader;
		public int FragmentShader;
		private bool disposedValue = false;


		public Shader(string vertexPath, string fragmentPath) {
			// load shader code
			string VertexShaderSource;

			using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8)) {
				VertexShaderSource = reader.ReadToEnd();
			}

			string FragmentShaderSource;

			using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8)) {
				FragmentShaderSource = reader.ReadToEnd();
			}
			// generate shaders
			VertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(VertexShader, VertexShaderSource);

			FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(FragmentShader, FragmentShaderSource);
			// compile and check for errors
			GL.CompileShader(VertexShader);

			string infoLogVert = GL.GetShaderInfoLog(VertexShader);
			if (infoLogVert != System.String.Empty)
				System.Console.WriteLine(infoLogVert);

			GL.CompileShader(FragmentShader);

			string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

			if (infoLogFrag != System.String.Empty)
				System.Console.WriteLine(infoLogFrag);
			// create the program
			Handle = GL.CreateProgram();

			GL.AttachShader(Handle, VertexShader);
			GL.AttachShader(Handle, FragmentShader);

			GL.LinkProgram(Handle);
			// cleanup
			GL.DetachShader(Handle, VertexShader);
			GL.DetachShader(Handle, FragmentShader);
			GL.DeleteShader(FragmentShader);
			GL.DeleteShader(VertexShader);
		}
		
		public void Use() {
			GL.UseProgram(Handle);
		}

		protected virtual void Dispose(bool disposing) {
			if (!disposedValue) {
				GL.DeleteProgram(Handle);

				disposedValue = true;
			}
		}

		~Shader() {
			GL.DeleteProgram(Handle);
		}


		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
	}
}