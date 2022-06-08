using Microsoft.UI.Xaml.Media.Media3D;
using System;

namespace Handler3D;

// Note: Most of this file is based on THREE.js implementation ported to C#

public struct Vector3 : IEquatable<Vector3>
{
    public double X { get; set; } = 0;
    public double Y { get; set; } = 0;
    public double Z { get; set; } = 0;
    public Vector3()
    {

    }
    public Vector3(double X, double Y, double Z)
    {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
    }
    public double Length => Math.Sqrt(X * X + Y * Y + Z * Z);
    public static Vector3 Zero => new Vector3(0, 0, 0);
    public static Vector3 One => new Vector3(1, 1, 1);

    public bool Equals(Vector3 other)
        => X == other.X && Y == other.Y && Z == other.Z;
    
    public static Vector3 operator +(Vector3 A, Vector3 B) => new (A.X + B.X, A.Y + B.Y, A.Z + B.Z);
    public static Vector3 operator *(Vector3 A, double B) => new(A.X * B, A.Y * B, A.Z * B);

    public static bool operator ==(Vector3 A, Vector3 B) => A.Equals(B);
    public static bool operator !=(Vector3 A, Vector3 B) => !A.Equals(B);

    public Vector3 Clamp(double low, double high) => new(Math.Clamp(X, low, high), Math.Clamp(Y, low, high), Math.Clamp(Z, low, high));

    public override bool Equals(object? obj)
        => obj is Vector3 vector && Equals(vector);
}
public struct Quaternion
{
    public double X { get; set; } = 0;
    public double Y { get; set; } = 0;
    public double Z { get; set; } = 0;
    public double W { get; set; } = 0;
    public Quaternion()
    {

    }
    public Quaternion(double X, double Y, double Z, double W)
    {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
        this.W = W;
    }
    public Matrix4x4 ToMatrix()
        => Matrix4x4.Compose(Position: Vector3.Zero, Quaternion: this, Scale: Vector3.One);
    public Euler ToEuler()
        => ToMatrix().Euler;
}
public struct Euler
{
    public Euler()
    {

    }
    public Euler(double X, double Y, double Z)
    {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
    }
    public double X { get; set; } = default;
    public double Y { get; set; } = default;
    public double Z { get; set; } = default;
    public double Yaw { get => Y; set => Y = value; }
    public double Pitch { get => X; set => X = value; }
    public double Roll { get => Z; set => Z = value; }
    public Quaternion ToQuaternion()
    {
        double c1 = Math.Cos(X / 2);
        double c2 = Math.Cos(Y / 2);
        double c3 = Math.Cos(Z / 2);

        double s1 = Math.Sin(X / 2);
        double s2 = Math.Sin(Y / 2);
        double s3 = Math.Sin(Z / 2);
        return new Quaternion(
            X: s1 * c2 * c3 + c1 * s2 * s3,
            Y: c1 * s2 * c3 - s1 * c2 * s3,
            Z: c1 * c2 * s3 + s1 * s2 * c3,
            W: c1 * c2 * c3 - s1 * s2 * s3
        );
    }
    public DegreeEuler ToDegree() => new(
        X / Math.PI * 180,
        Y / Math.PI * 180,
        Z / Math.PI * 180
    );
}
public struct DegreeEuler
{
    public DegreeEuler()
    {

    }
    public DegreeEuler(double X, double Y, double Z)
    {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
    }
    public double X { get; set; } = default;
    public double Y { get; set; } = default;
    public double Z { get; set; } = default;
    public double Yaw { get => Y; set => Y = value; }
    public double Pitch { get => X; set => X = value; }
    public double Roll { get => Z; set => Z = value; }
    public Euler ToRadian() => new(
        X * Math.PI / 180,
        Y * Math.PI / 180,
        Z * Math.PI / 180
    );
    public Quaternion ToQuaternion()
        => ToRadian().ToQuaternion();
    public static implicit operator Euler(DegreeEuler DegreeEuler) => DegreeEuler.ToRadian();
    public static implicit operator DegreeEuler(Euler DegreeEuler) => DegreeEuler.ToDegree();
}
public struct Matrix4x4
{
    readonly double[] Matrix;
    public Matrix4x4()
    {
        Matrix = new double[]
        {
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1
        };
    }
    public Matrix4x4(double[] Matrix)
    {
        if (Matrix.Length != 16) throw new InvalidOperationException();
        this.Matrix = Matrix;
    }
    public Matrix4x4(
        double M11,
        double M12,
        double M13,
        double M14,
        double M21,
        double M22,
        double M23,
        double M24,
        double M31,
        double M32,
        double M33,
        double M34,
        double M41,
        double M42,
        double M43,
        double M44)
    {
        Matrix = new double[16];
        this[1, 1] = M11;
        this[1, 2] = M12;
        this[1, 3] = M13;
        this[1, 4] = M14;
        this[2, 1] = M21;
        this[2, 2] = M22;
        this[2, 3] = M23;
        this[2, 4] = M24;
        this[3, 1] = M31;
        this[3, 2] = M32;
        this[3, 3] = M33;
        this[3, 4] = M34;
        this[4, 1] = M41;
        this[4, 2] = M42;
        this[4, 3] = M43;
        this[4, 4] = M44;
    }

    public static Matrix4x4 Empty => new Matrix4x4();
    public double this[int loc]
    {
        get => Matrix[loc];
        set => Matrix[loc] = value;
    }
    public double this[int c, int r]
    {
        get => Matrix[(c - 1) * 4 + (r - 1)];
        set => Matrix[(c - 1) * 4 + (r - 1)] = value;
    }
    public Matrix4x4 Clone()
    {
        var newarr = new double[16];
        Array.Copy(Matrix, newarr, 16);
        return new Matrix4x4(newarr);
    }
    public void CopyTo(Matrix4x4 another)
    {
        Array.Copy(Matrix, another.Matrix, 16);
    }
    public static Matrix4x4 Compose(Vector3 Position, Quaternion Quaternion, Vector3 Scale)
    {
        double x = Quaternion.X, y = Quaternion.Y, z = Quaternion.Z, w = Quaternion.W;
        double x2 = x + x, y2 = y + y, z2 = z + z;
        double xx2 = x * x2, xy2 = x * y2, xz2 = x * z2;
        double yy2 = y * y2, yz2 = y * z2, zz2 = z * z2;
        double wx2 = w * x2, wy2 = w * y2, wz2 = w * z2;
        var mat = new Matrix4x4(
            (1 - (yy2 + zz2)) * Scale.X,
            (xy2 + wz2) * Scale.X,
            (xz2 - wy2) * Scale.X,
            0,
            (xy2 - wz2) * Scale.Y,
            (1 - (xx2 + zz2)) * Scale.Y,
            (yz2 + wx2) * Scale.Y,
            0,
            (xz2 + wy2) * Scale.Z,
            (yz2 - wx2) * Scale.Z,
            (1 - (xx2 + yy2)) * Scale.Z,
            0,
            Position.X,
            Position.Y,
            Position.Z,
            1
        );
        return mat;
    }
    public static Matrix4x4 operator *(Matrix4x4 A, Matrix4x4 B)
    {
        double
            a11 = A[0], a12 = A[4], a13 = A[8], a14 = A[12],
            a21 = A[1], a22 = A[5], a23 = A[9], a24 = A[13],
            a31 = A[2], a32 = A[6], a33 = A[10], a34 = A[14],
            a41 = A[3], a42 = A[7], a43 = A[11], a44 = A[15],
            b11 = B[0], b12 = B[4], b13 = B[8], b14 = B[12],
            b21 = B[1], b22 = B[5], b23 = B[9], b24 = B[13],
            b31 = B[2], b32 = B[6], b33 = B[10], b34 = B[14],
            b41 = B[3], b42 = B[7], b43 = B[11], b44 = B[15];
        Matrix4x4 output = new();
        output[0] = a11 * b11 + a12 * b21 + a13 * b31 + a14 * b41;
        output[4] = a11 * b12 + a12 * b22 + a13 * b32 + a14 * b42;
        output[8] = a11 * b13 + a12 * b23 + a13 * b33 + a14 * b43;
        output[12] = a11 * b14 + a12 * b24 + a13 * b34 + a14 * b44;
        output[1] = a21 * b11 + a22 * b21 + a23 * b31 + a24 * b41;
        output[5] = a21 * b12 + a22 * b22 + a23 * b32 + a24 * b42;
        output[9] = a21 * b13 + a22 * b23 + a23 * b33 + a24 * b43;
        output[13] = a21 * b14 + a22 * b24 + a23 * b34 + a24 * b44;
        output[2] = a31 * b11 + a32 * b21 + a33 * b31 + a34 * b41;
        output[6] = a31 * b12 + a32 * b22 + a33 * b32 + a34 * b42;
        output[10] = a31 * b13 + a32 * b23 + a33 * b33 + a34 * b43;
        output[14] = a31 * b14 + a32 * b24 + a33 * b34 + a34 * b44;
        output[3] = a41 * b11 + a42 * b21 + a43 * b31 + a44 * b41;
        output[7] = a41 * b12 + a42 * b22 + a43 * b32 + a44 * b42;
        output[11] = a41 * b13 + a42 * b23 + a43 * b33 + a44 * b43;
        output[15] = a41 * b14 + a42 * b24 + a43 * b34 + a44 * b44;
        return output;
    }
    public static Matrix4x4 Compose(Vector3 Position, Euler Euler, Vector3 Scale)
        => Compose(Position: Position, Quaternion: Euler.ToQuaternion(), Scale: Scale);
    public Matrix4x4 Invert()
    {
        double
            n11 = this[0], n21 = this[1], n31 = this[2], n41 = this[3],
            n12 = this[4], n22 = this[5], n32 = this[6], n42 = this[7],
            n13 = this[8], n23 = this[9], n33 = this[10], n43 = this[11],
            n14 = this[12], n24 = this[13], n34 = this[14], n44 = this[15],
            t11 = n23 * n34 * n42 - n24 * n33 * n42 + n24 * n32 * n43 - n22 * n34 * n43 - n23 * n32 * n44 + n22 * n33 * n44,
            t12 = n14 * n33 * n42 - n13 * n34 * n42 - n14 * n32 * n43 + n12 * n34 * n43 + n13 * n32 * n44 - n12 * n33 * n44,
            t13 = n13 * n24 * n42 - n14 * n23 * n42 + n14 * n22 * n43 - n12 * n24 * n43 - n13 * n22 * n44 + n12 * n23 * n44,
            t14 = n14 * n23 * n32 - n13 * n24 * n32 - n14 * n22 * n33 + n12 * n24 * n33 + n13 * n22 * n34 - n12 * n23 * n34,

            det = n11 * t11 + n21 * t12 + n31 * t13 + n41 * t14;

        if (det == 0) return Empty;
        double detInv = 1 / det;
        return new Matrix4x4(
            t11 * detInv,
            (n24 * n33 * n41 - n23 * n34 * n41 - n24 * n31 * n43 + n21 * n34 * n43 + n23 * n31 * n44 - n21 * n33 * n44) * detInv,
            (n22 * n34 * n41 - n24 * n32 * n41 + n24 * n31 * n42 - n21 * n34 * n42 - n22 * n31 * n44 + n21 * n32 * n44) * detInv,
            (n23 * n32 * n41 - n22 * n33 * n41 - n23 * n31 * n42 + n21 * n33 * n42 + n22 * n31 * n43 - n21 * n32 * n43) * detInv,
            t12 * detInv,
            (n13 * n34 * n41 - n14 * n33 * n41 + n14 * n31 * n43 - n11 * n34 * n43 - n13 * n31 * n44 + n11 * n33 * n44) * detInv,
            (n14 * n32 * n41 - n12 * n34 * n41 - n14 * n31 * n42 + n11 * n34 * n42 + n12 * n31 * n44 - n11 * n32 * n44) * detInv,
            (n12 * n33 * n41 - n13 * n32 * n41 + n13 * n31 * n42 - n11 * n33 * n42 - n12 * n31 * n43 + n11 * n32 * n43) * detInv,
            t13 * detInv,
            (n14 * n23 * n41 - n13 * n24 * n41 - n14 * n21 * n43 + n11 * n24 * n43 + n13 * n21 * n44 - n11 * n23 * n44) * detInv,
            (n12 * n24 * n41 - n14 * n22 * n41 + n14 * n21 * n42 - n11 * n24 * n42 - n12 * n21 * n44 + n11 * n22 * n44) * detInv,
            (n13 * n22 * n41 - n12 * n23 * n41 - n13 * n21 * n42 + n11 * n23 * n42 + n12 * n21 * n43 - n11 * n22 * n43) * detInv,
            t14 * detInv,
            (n13 * n24 * n31 - n14 * n23 * n31 + n14 * n21 * n33 - n11 * n24 * n33 - n13 * n21 * n34 + n11 * n23 * n34) * detInv,
            (n14 * n22 * n31 - n12 * n24 * n31 - n14 * n21 * n32 + n11 * n24 * n32 + n12 * n21 * n34 - n11 * n22 * n34) * detInv,
            (n12 * n23 * n31 - n13 * n22 * n31 + n13 * n21 * n32 - n11 * n23 * n32 - n12 * n21 * n33 + n11 * n22 * n33) * detInv
        );
    }
    public double Determinant()
    {
        double
            n11 = this[0], n12 = this[4], n13 = this[8], n14 = this[12],
            n21 = this[1], n22 = this[5], n23 = this[9], n24 = this[13],
            n31 = this[2], n32 = this[6], n33 = this[10], n34 = this[14],
            n41 = this[3], n42 = this[7], n43 = this[11], n44 = this[15];

        return (
            n41 * (
                +n14 * n23 * n32
                 - n13 * n24 * n32
                 - n14 * n22 * n33
                 + n12 * n24 * n33
                 + n13 * n22 * n34
                 - n12 * n23 * n34
            ) +
            n42 * (
                +n11 * n23 * n34
                 - n11 * n24 * n33
                 + n14 * n21 * n33
                 - n13 * n21 * n34
                 + n13 * n24 * n31
                 - n14 * n23 * n31
            ) +
            n43 * (
                +n11 * n24 * n32
                 - n11 * n22 * n34
                 - n14 * n21 * n32
                 + n12 * n21 * n34
                 + n14 * n22 * n31
                 - n12 * n24 * n31
            ) +
            n44 * (
                -n13 * n22 * n31
                 - n11 * n23 * n32
                 + n11 * n22 * n33
                 + n13 * n21 * n32
                 - n12 * n21 * n33
                 + n12 * n23 * n31
            )

        );

    }
    public void Decompose(out Vector3 Position, out Quaternion Quaternion, out Vector3 Scale)
    {
        Scale = this.Scale;
        
        Position = this.Position;

        var MatrixClone = Clone();

        double invSX = 1 / Scale.X, invSY = 1 / Scale.Y, invSZ = 1 / Scale.Z;

        MatrixClone[0] *= invSX;
        MatrixClone[1] *= invSX;
        MatrixClone[2] *= invSX;

        MatrixClone[4] *= invSY;
        MatrixClone[5] *= invSY;
        MatrixClone[6] *= invSY;

        MatrixClone[8] *= invSZ;
        MatrixClone[9] *= invSZ;
        MatrixClone[10] *= invSZ;
        Quaternion = MatrixClone.Quaternion;
    }
    public (Vector3 Position, Quaternion Quaternion, Vector3 Scale) Decompose()
    {
        Decompose(out Vector3 Position, out Quaternion Quaternion, out Vector3 Scale);
        return (Position, Quaternion, Scale);
    }
    public Quaternion Quaternion
    {
        get
        {
            double 
                m11 = this[0],
				m12 = this[4],
				m13 = this[8],
				m21 = this[1],
				m22 = this[5],
				m23 = this[9],
				m31 = this[2],
				m32 = this[6],
				m33 = this[10],
				trace = m11 + m22 + m33;

            if (trace > 0)
            {
                double s = 0.5 / Math.Sqrt(trace + 1.0);
                return new Quaternion(W: 0.25 / s, X: (m32 - m23) * s, Y: (m13 - m31) * s, Z: (m21 - m12) * s);
            }
            else if (m11 > m22 && m11 > m33)
            {
                double s = 2.0 * Math.Sqrt(1.0 + m11 - m22 - m33);
                return new Quaternion(W: (m32 - m23) / s, X: 0.25 * s, Y: (m12 + m21) / s, Z: (m13 + m31) / s);
            }
            else if (m22 > m33)
            {
                double s = 2.0 * Math.Sqrt(1.0 + m22 - m11 - m33);
                return new Quaternion(W: (m13 - m31) / s, X: (m12 + m21) / s, Y: 0.25 * s, Z: (m23 + m32) / s);
            }
            else
            {
                double s = 2.0 * Math.Sqrt(1.0 + m33 - m11 - m22);
                return new Quaternion(W: (m21 - m12) / s, X: (m13 + m31) / s, Y: (m23 + m32) / s, Z: 0.25 * s);
            }
        }
        //set => Matrix4x4.Compose(Position, value, Scale);
    }
    public Euler Euler
    {
        get
        {
            double
                m11 = this[0], m12 = this[4], m13 = this[8],
                               m22 = this[5], m23 = this[9],    // m21 = this[1], 
                               m32 = this[6], m33 = this[10];   // m31 = this[2], 
            double X, Y, Z;
            Y = Math.Asin(Math.Clamp(m13, -1, 1));

            if (Math.Abs(m13) < 0.9999999)
            {
                X = Math.Atan2(-m23, m33);
                Z = Math.Atan2(-m12, m11);
            }
            else
            {
                X = Math.Atan2(m32, m22);
                Z = 0;
            }
            return new Euler(X, Y, Z);
        }
        set
        {
            Compose(Position: Position, value, Scale: Scale).CopyTo(this);
        }
    }
    public DegreeEuler DegreeEuler
    {
        get => Euler.ToDegree();
        set => Euler = value.ToRadian();
    }
    public Vector3 Position
    {
        get => new(this[4, 1], this[4, 2], this[4, 3]);
        set
        {
            this[4, 1] = value.X;
            this[4, 2] = value.Y;
            this[4, 3] = value.Z;
        }
    }
    public Vector3 Scale
    {
        get
        {
            var scale = new Vector3
            {
                X = new Vector3(this[0], this[1], this[2]).Length,
                Y = new Vector3(this[4], this[5], this[6]).Length,
                Z = new Vector3(this[8], this[9], this[10]).Length
            };

            double det = Determinant();
            if (det < 0)
                scale.X = -scale.X;
            return scale;
        }
    }
    public Matrix4x4 RelativeTo(Matrix4x4 Parent) => Invert() * Parent;
}
public struct Transform
{
    public Transform()
    {
        Scale = new Vector3(1, 1, 1);
        Position = new Vector3(0, 0, 0);
        Euler = new Euler(0, 0, 0);
    }
    public Transform(Matrix4x4 Matrix)
    {
        _Matrix = Matrix;
    }
    public Vector3 Position
    {
        get => _Matrix.Position;
        set => _Matrix.Position = value;
    }
    public Quaternion Quaternion
    {
        get
        {
            Quaternion Quaternion = new();
            var trace = _Matrix[1, 1] + _Matrix[2, 2] + _Matrix[3, 3];
            if (trace > 0)
            {

                float s = 0.5f / (float)Math.Sqrt(trace + 1.0);

                Quaternion.W = 0.25f / s;
                Quaternion.X = (_Matrix[3, 2] - _Matrix[2, 3]) * s;
                Quaternion.Y = (_Matrix[1, 3] - _Matrix[3, 1]) * s;
                Quaternion.Z = (_Matrix[2, 1] - _Matrix[1, 2]) * s;

            }
            else if (_Matrix[1, 1] > _Matrix[2, 2] && _Matrix[1, 1] > _Matrix[3, 3])
            {

                float s = 2.0f * (float)Math.Sqrt(1.0 + _Matrix[1, 1] - _Matrix[2, 2] - _Matrix[3, 3]);

                Quaternion.W = (_Matrix[3, 2] - _Matrix[2, 3]) / s;
                Quaternion.X = 0.25f * s;
                Quaternion.Y = (_Matrix[1, 2] + _Matrix[2, 1]) / s;
                Quaternion.Z = (_Matrix[1, 3] + _Matrix[3, 1]) / s;

            }
            else if (_Matrix[2, 2] > _Matrix[3, 3])
            {

                float s = 2.0f * (float)Math.Sqrt(1.0f + _Matrix[2, 2] - _Matrix[1, 1] - _Matrix[3, 3]);

                Quaternion.W = (_Matrix[1, 3] - _Matrix[3, 1]) / s;
                Quaternion.X = (_Matrix[1, 2] + _Matrix[2, 1]) / s;
                Quaternion.Y = 0.25f * s;
                Quaternion.Z = (_Matrix[2, 3] + _Matrix[3, 2]) / s;

            }
            else
            {
                float s = 2.0f * (float)Math.Sqrt(1.0 + _Matrix[3, 3] - _Matrix[1, 1] - _Matrix[2, 2]);

                Quaternion.W = (_Matrix[2, 1] - _Matrix[1, 2]) / s;
                Quaternion.X = (_Matrix[1, 3] + _Matrix[3, 1]) / s;
                Quaternion.Y = (_Matrix[2, 3] + _Matrix[3, 2]) / s;
                Quaternion.Z = 0.25f * s;
            }
            return Quaternion;
        }
        set => Matrix4x4.Compose(Position, value, Scale);
    }
    public Euler Euler
    {
        get
        {
            double
                m11 = _Matrix[0], m12 = _Matrix[4], m13 = _Matrix[8],
                m21 = _Matrix[1], m22 = _Matrix[5], m23 = _Matrix[9],
                m31 = _Matrix[2], m32 = _Matrix[6], m33 = _Matrix[10];
            double X, Y, Z;
            Y = Math.Asin(Math.Clamp(m13, -1, 1));

            if (Math.Abs(m13) < 0.9999999)
            {
                X = Math.Atan2(-m23, m33);
                Z = Math.Atan2(-m12, m11);
            }
            else
            {
                X = Math.Atan2(m32, m22);
                Z = 0;
            }
            return new Euler(X, Y, Z);
        }
        set => Quaternion = value.ToQuaternion();
    }
    public Vector3 Scale
    {
        get => new(_Matrix[1, 1], _Matrix[2, 1], _Matrix[3, 1]);
        set
        {
            _Matrix[1, 1] = value.X;
            _Matrix[2, 1] = value.Y;
            _Matrix[3, 1] = value.Z;
        }
    }
    Matrix4x4 _Matrix = new();
    public Matrix4x4 Matrix
    {
        get => _Matrix; // MatrixExtension.Compose(Position, Quaternion, Scale)
        set => _Matrix = value;
    }
    public Transform Invert()
    {
        return new Transform(Matrix.Invert());
    }
    public static Transform operator *(Transform a, Transform b)
        => new Transform(a._Matrix * b._Matrix);

}
public static class MatrixExtension
{
    public static Matrix3D ToMatrix3D(this Matrix4x4 Matrix)
        => new(
            Matrix[1,1],
            Matrix[1,2],
            Matrix[1,3],
            Matrix[1,4],
            Matrix[2,1],
            Matrix[2,2],
            Matrix[2,3],
            Matrix[2,4],
            Matrix[3,1],
            Matrix[3,2],
            Matrix[3,3],
            Matrix[3,4],
            Matrix[4,1],
            Matrix[4,2],
            Matrix[4,3],
            Matrix[4,4]
        );
    public static Matrix4x4 ToMatri4x4(this Matrix3D Matrix)
        => new(
            Matrix.M11,
            Matrix.M12,
            Matrix.M13,
            Matrix.M14,
            Matrix.M21,
            Matrix.M22,
            Matrix.M23,
            Matrix.M24,
            Matrix.M31,
            Matrix.M32,
            Matrix.M33,
            Matrix.M34,
            Matrix.OffsetX,
            Matrix.OffsetY,
            Matrix.OffsetZ,
            Matrix.M44
        );

}
