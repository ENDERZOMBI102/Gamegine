using System;
using Gamengine.core;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Sandbox {
	public class Program {
		static void Main(string[] args) {
			Engine engine = new Engine();
			engine.GetInputSystem().RegisterInput(Keys.R, OnKeyR);
			engine.GetInputSystem().RegisterInput(Keys.Escape, window => engine.Destroy() );
			
			engine.GetInputSystem().RegisterInput(Keys.K, window => Console.WriteLine("delay!"), 400);
			engine.Run();
		}
		static void OnKeyR(NativeWindow window) {
			window.Title = new Random().Next().ToString();
		}
	}
}