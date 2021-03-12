using System;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace Gamengine.core.render {
	
	public class Shader : IDisposable {
		
		private int Handle;
		private string _name;
		private string _path;
		private bool _disposedValue;
		private bool _reloading = false;
		public int VertexShader;
		public int FragmentShader;

		public Shader(string shaderPath, string name) {
			_name = name;
			_path = shaderPath;
			// load, compile and execute shaders
			LoadShaders();
		}
		
		public void Use() {
			GL.UseProgram(Handle);
		}

		public void Reload() {
			_reloading = true;
			LoadShaders();
			_reloading = false;
		}

		public int GetAttribLocation(string name) {
			return GL.GetAttribLocation(Handle, name);
		}

		public string GetName() {
			return _name;
		}

		public string GetPath() {
			return _path;
		}
		
		private static string[] LoadShadersCode(string path) {
			path = path.Replace('\\', '/');
			
			string fragmentShaderSource;
			string vertexShaderSource;
			string commonShader;
			// load shader code
			using (StreamReader reader = new StreamReader(path, Encoding.UTF8)) {
				commonShader = reader.ReadToEnd();
			}

			fragmentShaderSource = commonShader.Substring(
				commonShader.IndexOf("#FRAGMENT") + 11,
				commonShader.Length - commonShader.IndexOf("#VERTEX") - 17
			);
			vertexShaderSource = commonShader.Substring(
				commonShader.IndexOf("#VERTEX") + 9
			);
			
			return new[] {
				fragmentShaderSource, vertexShaderSource
			};
		}

		private void LoadShaders() {
			// load shaders
			string[] sources = LoadShadersCode(_path);
			
			// generate shaders
			VertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(VertexShader, sources[1]);
			
			FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(FragmentShader, sources[0]);
			
			// compile shaders
			GL.CompileShader(VertexShader);
			GL.CompileShader(FragmentShader);
			
			string infoLogVert = GL.GetShaderInfoLog(VertexShader); // error check
			if (infoLogVert != String.Empty) {
				Console.WriteLine(infoLogVert);
				if (_reloading) return;
			}

			string infoLogFrag = GL.GetShaderInfoLog(FragmentShader); // error check
			if (infoLogFrag != String.Empty) {
				Console.WriteLine(infoLogFrag);
				if (_reloading) return;
			}
			
			// create the program
			// but only if we're not reloading
			if (! _reloading ) Handle = GL.CreateProgram();

			GL.AttachShader(Handle, VertexShader);
			GL.AttachShader(Handle, FragmentShader);

			GL.LinkProgram(Handle);
			
			// cleanup
			GL.DetachShader(Handle, VertexShader);
			GL.DetachShader(Handle, FragmentShader);
			GL.DeleteShader(FragmentShader);
			GL.DeleteShader(VertexShader);
		}
		
		~Shader() {
			GL.DeleteProgram(Handle);
		}
		
		protected virtual void Dispose(bool disposing) {
			if (!_disposedValue) {
				GL.DeleteProgram(Handle);

				_disposedValue = true;
			}
		}
		
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}