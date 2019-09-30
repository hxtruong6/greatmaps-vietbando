namespace GMap.NET.WindowsPresentation
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Windows;
   using System.Windows.Media;
   using System.Windows.Media.Effects;
   using System.Windows.Shapes;
   public class GMapCircle : GMapMarker, IShapable
   {
      private double radius;

      public GMapCircle(PointLatLng center, double radius = 5.0f)
      {
         Points = new List<PointLatLng>();
         Points.Add(center);
         Points.Add(center); // Treat like a path
         this.radius = radius;
      }

      public override void Clear()
      {
         base.Clear();
         Points.Clear();
      }

      public List<PointLatLng> Points { get; set; }

      /// <summary>
      /// creates circle from center point, for performance set addBlurEffect to false
      /// </summary>
      /// <param name="pl"></param>
      /// <returns></returns>
      public virtual Path CreatePath(List<Point> localPath, bool addBlurEffect = false)
      {
         // Create a StreamGeometry to use to specify myPath.
         StreamGeometry geometry = new StreamGeometry();

         using (StreamGeometryContext ctx = geometry.Open())
         {
            var c = localPath[0];
            ctx.BeginFigure(new Point(c.X + radius, c.Y - radius), isFilled: true, isClosed: true);
            ctx.ArcTo(
                new Point(c.X + radius * Math.Cos(0.01), c.Y + radius * Math.Sin(0.01)), // need a small angle but large enough that the ellipse is positioned accurately
                new Size(radius, radius), // docs say it should be 10,10 but in practice it appears that this should be half the desired width/height...
                0, true, SweepDirection.Counterclockwise, true, true);
         }

         // Freeze the geometry (make it unmodifiable)
         // for additional performance benefits.
         geometry.Freeze();

         // Create a path to draw a geometry with.
         Path myPath = new Path();
         {
            // Specify the shape of the Path using the StreamGeometry.
            myPath.Data = geometry;
            myPath.Stroke = Brushes.Green;
            myPath.Fill = Brushes.GreenYellow;
            myPath.StrokeThickness = 2;
            //myPath.Opacity = 1.0;
            myPath.IsHitTestVisible = false;
         }
         return myPath;
      }
   }
}
