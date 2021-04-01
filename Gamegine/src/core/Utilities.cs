using BulletSharp.Math;
using OpenTK.Mathematics;

namespace Gamengine.core {
	public class Utilities {
		public static Matrix4 MatrixToMatrix4(Matrix matrix) {
			return new Matrix4(
				matrix.M11,
				matrix.M12,
				matrix.M13,
				matrix.M14,
				matrix.M21,
				matrix.M22,
				matrix.M23,
				matrix.M24,
				matrix.M31,
				matrix.M32,
				matrix.M33,
				matrix.M34,
				matrix.M41,
				matrix.M42,
				matrix.M43,
				matrix.M44
			);
		}
	}
}