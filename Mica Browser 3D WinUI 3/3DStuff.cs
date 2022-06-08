using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Media3D;
using System;
using System.Collections.ObjectModel;

namespace Handler3D;

// Note: Most of this file is based on THREE.js implementation ported to C#

public class Object3D
{
    public Transform LocalTransform { get; set; } = new Transform();
    public Transform WorldTransform
    {
        get => Parent == null ? LocalTransform : new Transform(Parent.WorldTransform.Matrix * LocalTransform.Matrix);
    }
    public Object3D? Parent { get; protected set; } = null;
    public ObservableCollection<Object3D> Children { get; } = new ObservableCollection<Object3D>();
    public void Attach(Object3D Object3D)
    {
        var transform = WorldTransform.Invert();
        if (Object3D.Parent != null)
        {
            transform *= Object3D.Parent.WorldTransform;
        }
        Object3D.LocalTransform = transform;
        Children.Add(Object3D);
    }
}

public interface IUI3D<T> where T : UIElement
{
    T LinkedElement { get; }
}
public class Object3DUI<T> : Object3D, IUI3D<T> where T : UIElement
{
    public T LinkedElement { get; }
    public Object3DUI(T LinkedElement)
    {
        this.LinkedElement = LinkedElement;
        LinkedElement.Projection = new PlaneProjection();
    }
}

public static class UWPExtension
{
    public static void SetCompositeTransform3D(this Transform Transform, CompositeTransform3D compositeTransform)
        => Transform.Matrix.SetCompositeTransform3D(compositeTransform);
    
    public static void SetCompositeTransform3D(this Matrix4x4 Matrix, CompositeTransform3D compositeTransform)
    {
        var (Pos, Quad, Scale) = Matrix.Decompose();
        var Euler = Quad.ToEuler();
        
        compositeTransform.TranslateX = Pos.X;
        compositeTransform.TranslateY = Pos.Y;
        compositeTransform.TranslateZ = Pos.Z;
        compositeTransform.ScaleX = Scale.X;
        compositeTransform.ScaleY = Scale.Y;
        compositeTransform.ScaleZ = Scale.Z;
        compositeTransform.RotationX = Euler.X / Math.PI * 180;
        compositeTransform.RotationY = Euler.Y / Math.PI * 180;
        compositeTransform.RotationZ = Euler.Z / Math.PI * 180;
    }
    public static void SetPlaneProjection(this Transform Transform, PlaneProjection PlaneProjection)
        => Transform.Matrix.SetPlaneProjection(PlaneProjection);
    public static void SetPlaneProjection(this Matrix4x4 Matrix, PlaneProjection PlaneProjection)
    {
        var (Pos, Quad, _) = Matrix.Decompose();
        var Euler = Quad.ToEuler();
        PlaneProjection.GlobalOffsetX = Pos.X;
        PlaneProjection.GlobalOffsetY = Pos.Y;
        PlaneProjection.GlobalOffsetZ = Pos.Z;
        PlaneProjection.RotationX = Euler.X / Math.PI * 180;
        PlaneProjection.RotationY = Euler.Y / Math.PI * 180;
        PlaneProjection.RotationZ = Euler.Z / Math.PI * 180;
    }
}
//class Camera : Object3D
//{

//    public Camera()
//    {

//    }
//}
//class PersepctiveCamera : Camera
//{
//    public float FOV { get; set; } = 50;
//    public float AspectRatio { get; set; } = 1920f / 1080f;
//    public float Near { get; set; } = 0.1f;
//    public float Far { get; set; } = 2000;
//    //public float Focus { get; set; } = 10;
//    //public float Zoom { get; set; } = 1;
//    //public float FilmGauge { get; set; } = 35;
//    //public float FilmOffset { get; set; } = 0;
//    public Matrix4x4 ProjectionMatrix
//    {
//        get => Matrix4x4.CreatePerspectiveFieldOfView(FOV, AspectRatio, Near, Far);
//    }
//    public virtual Size Size { get; set; } = new Size(1920, 1080);
//    public PersepctiveCamera()
//    {

//    }
//}
//class PersepctiveCameraUI : PersepctiveCamera
//{
//    Grid LinkedElement { get; }
//    public override Size Size { get => new(LinkedElement.Width, LinkedElement.Height); set => throw new InvalidOperationException(); }
//    public PersepctiveCameraUI(Grid LinkedElement)
//    {
//        this.LinkedElement = LinkedElement;
//    }
//}