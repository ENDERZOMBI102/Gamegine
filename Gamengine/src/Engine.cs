using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Gamengine  {
	public class Engine : GameWindow {
		public Engine() : base( GameWindowSettings.Default, NativeWindowSettings.Default ) {
			
		}

		protected override void OnUpdateFrame(FrameEventArgs evt) {
			
			KeyboardState input = KeyboardState;

			if ( input.IsKeyDown(Keys.Escape) )
			{
				this.DestroyWindow();
			}
			
			base.OnUpdateFrame(evt);
		}
	}
}