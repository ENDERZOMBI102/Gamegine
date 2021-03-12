using System;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace Gamengine.core.render {
	
	public class Shader : IDisposable {
		
		int Handle;
		public int VertexShader;
		public int FragmentShader;
		private bool _disposedValue;


		public Shader(string vertexPath, string fragmentPath) {
			// load shader code
			string vertexShaderSource;
			using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8)) {
				VertexShaderSource = reader.ReadToEnd();
			}

			string fragmentShaderSource;
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
			if (infoLogVert != String.Empty)
				Console.WriteLine(infoLogVert);

			GL.CompileShader(FragmentShader);

			string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

			if (infoLogFrag != String.Empty)
				Console.WriteLine(infoLogFrag);
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
			if (!_disposedValue) {
				GL.DeleteProgram(Handle);

				_disposedValue = true;
			}
		}

		public int GetAttribLocation(string name) {
			return GL.GetAttribLocation(Handle, name);
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~Shader() {
			GL.DeleteProgram(Handle);
		}


		public static Shader ShaderFromFile(string path) {
			path = path.Replace('\\', '/');
			
			string fragmentShaderSource;
			string vertexShaderSource;
			string commonShader;
			string shaderName = path.Substring(
				path.LastIndexOf('/'),
				path.LastIndexOf('.') - path.LastIndexOf('/')
			);
			// load shader code
			using (StreamReader reader = new StreamReader(path, Encoding.UTF8)) {
				commonShader = reader.ReadToEnd();
			}

			fragmentShaderSource = commonShader.Substring( commonShader.IndexOf("// FRAGMENT SHADER") );




		}
	}
}