namespace GMap.NET.WindowsPresentation
{
   using System.Collections.Generic;
   using System.Windows;
   using System.Windows.Media;
   using System.Windows.Media.Effects;
   using System.Windows.Shapes;
   class GMapCircle : GMapMarker, IShapable
   {
      private double radius;

      public GMapCircle(PointLatLng center, double radius = 1.0)
      {
         Points = new List<PointLatLng>();
         Points.Add(center);
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
      public virtual Path CreatePath(List<Point> localPath, bool addBlurEffect)
      {
         // Create a StreamGeometry to use to specify myPath.
         StreamGeometry geometry = new StreamGeometry();

         using (StreamGeometryContext ctx = geometry.Open())
         {
            // Set the begin point of the shape.
            ctx.BeginFigure(new Point(10, 100), true /* is filled */, true /* is closed */);
            // Create an arc. Draw the arc from the begin point to 200,100 with the specified parameters.
            ctx.ArcTo(localPath[0], new Size(radius, radius), 0 /* rotation angle */, true /* is large arc */,
                      SweepDirection.Counterclockwise, true /* is stroked */, true /* is smooth join */);
         }

         // Freeze the geometry (make it unmodifiable)
         // for additional performance benefits.
         geometry.Freeze();

         // Create a path to draw a geometry with.
         Path myPath = new Path();
         {
            // Specify the shape of the Path using the StreamGeometry.
            myPath.Data = geometry;

            if (addBlurEffect)
            {
               BlurEffect ef = new BlurEffect();
               {
                  ef.KernelType = KernelType.Gaussian;
                  ef.Radius = 3.0;
                  ef.RenderingBias = RenderingBias.Performance;
               }

               myPath.Effect = ef;
            }

            myPath.Stroke = Brushes.BlueViolet;
            myPath.StrokeThickness = 2;
            myPath.StrokeLineJoin = PenLineJoin.Round;
            myPath.StrokeStartLineCap = PenLineCap.Triangle;
            myPath.StrokeEndLineCap = PenLineCap.Square;

            myPath.Fill = Brushes.AliceBlue;

            myPath.Opacity = 1.0;
            myPath.IsHitTestVisible = false;
         }
         return myPath;
      }
   }
}
