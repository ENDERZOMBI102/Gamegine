using System;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace Gamengine.core.render {
	
	public class Shader : IDisposable {
		
		private int _handle;
		private readonly string _path;
		private bool _disposedValue;
		private bool _reloading = false;
		private int _vertexShader;
		private int _fragmentShader;

		public Shader(string shaderPath) {
			_path = shaderPath;
			// load, compile and execute shaders
			LoadShaders();
		}
		
		public void Use() {
			GL.UseProgram(_handle);
		}

		public void Reload() {
			_reloading = true;
			LoadShaders();
			_reloading = false;
		}

		public int GetAttribLocation(string name) {
			return GL.GetAttribLocation(_handle, name);
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
			_vertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(_vertexShader, sources[1]);
			
			_fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(_fragmentShader, sources[0]);
			
			// compile shaders
			GL.CompileShader(_vertexShader);
			GL.CompileShader(_fragmentShader);
			
			string infoLogVert = GL.GetShaderInfoLog(_vertexShader); // error check
			if (infoLogVert != String.Empty) {
				Console.WriteLine(infoLogVert);
				if (_reloading) return;
			}

			string infoLogFrag = GL.GetShaderInfoLog(_fragmentShader); // error check
			if (infoLogFrag != String.Empty) {
				Console.WriteLine(infoLogFrag);
				if (_reloading) return;
			}
			
			// create the program
			// but only if we're not reloading
			if (! _reloading ) _handle = GL.CreateProgram();

			GL.AttachShader(_handle, _vertexShader);
			GL.AttachShader(_handle, _fragmentShader);

			GL.LinkProgram(_handle);
			
			// cleanup
			GL.DetachShader(_handle, _vertexShader);
			GL.DetachShader(_handle, _fragmentShader);
			GL.DeleteShader(_fragmentShader);
			GL.DeleteShader(_vertexShader);
		}
		
		~Shader() {
			GL.DeleteProgram(_handle);
		}
		
		protected void Dispose(bool disposing) {
			if (!_disposedValue) {
				GL.DeleteProgram(_handle);

				_disposedValue = true;
			}
		}
		
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}