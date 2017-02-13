using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CadCamProject.Pages;
using System.Windows;
using System.Windows.Media;

namespace CadCamProject
{
    [Serializable]
    public class Profile
    {
        //General Parameters
        public string TypeImagineOperation { get; set; }
        public Operations TypeOperation { get; set; }
        public string Parameters { get; set; }
        public int Index { get; set; }
        public string dataContent { get; set; }
        public string version { get; set; }



        //Specific parameters
        public string OperationName { get; set; }
        public int workOffsetIndex { get; set; }
        public Gcode workOffset { get; set; }
        public WorkingPlane workingPlane { get; set; }
        public List<Geometry> geometry { get; set; }
       

        public Profile()
        {
            TypeImagineOperation = "/Images/Profile.png";
            TypeOperation = Operations.Profile;
            workingPlane = WorkingPlane.ZX;
            workOffsetIndex = 0;
            geometry = new List<Geometry>();
            
            
            dataContent = "vamos por buen camino kikin";
        }

        public Profile GetParameters(Main MainPage, int index)
        {
            Profile op = new Profile();

            if (index <= MainPage.listViewOperations.Items.Count)
            {
                var listOperation = MainPage.listViewOperations.Items.GetItemAt(index) as List<Profile>;
                op = listOperation.Last();
            }
            else
            {
                op.Index = MainPage.listViewOperations.Items.Count;
            }


            return op;
        }

        public List<Profile> SetParameters(Profile opParameters, Main MainPage)
        {
            List<Profile> listOperation = new List<Profile>();
            listOperation.Add(opParameters);
            if (opParameters.Index != MainPage.listViewOperations.Items.Count)
            {
                MainPage.listViewOperations.Items.RemoveAt(opParameters.Index);
            }
            return listOperation;
        }

        internal string ShowingParameters(Profile _profile)
        {
            SpecialChart sCh = new SpecialChart();
            string dataOut = sCh.chLF + _profile.OperationName + sCh.chRH +
                             sCh.chLF + "WO " + _profile.workOffsetIndex.ToString() + sCh.blank + _profile.workOffset + sCh.chRH +
                             sCh.chLF + "GT " + _profile.geometry.Count.ToString() + sCh.chRH +
                             sCh.chLF + "PL " + _profile.workingPlane.ToString() + sCh.chRH;                           
            return dataOut;
        }

    }

    #region Class Geometry Transition Arc Line Point
    [Serializable]
    public class Geometry
    {
        public Line line { get; set; }
        public Arc arc { get; set; }
        public TransitionGeometry transition { get; set; }
        

        public CoordinatePoint initialPosition { get; }
        public CoordinatePoint finalPosition { get; }

        public int id { get; set; }
        public Type typeGeometry { get; set; }

        public Geometry(Arc _arc, TransitionGeometry _transition, int _id)
        {
            arc = _arc;
            transition = _transition;
            typeGeometry = _arc.GetType();
            id = _id;
            initialPosition = _arc.initialPoint;
            finalPosition = _arc.finalPoint;          

        }

        public Geometry(Line _line, TransitionGeometry _transition, int _id)
        {
            line = _line;
            transition = _transition;
            typeGeometry =line.GetType();
            id = _id;
            initialPosition = _line.initialPoint;
            finalPosition = _line.finalPoint;

            
        }

        public Geometry()
        {
            line = new Line();
            arc = new Arc();
        }
    }

    [Serializable]
    public class TransitionGeometry
    {
        public TypeTransitionGeometry typeTransition { get; set; }
        
        public double parameter { get; set; }
        public bool enableTransition { get; set; }
    }

    [Serializable]
    public class Arc
    {
        public RadiusDefinition radiusDefinition { get; set; }
        public CoordinatePoint initialPoint { get; set; }
        public CoordinatePoint finalPoint { get; set; }
        public CoordinatePoint centerPoint { get; set; }
        public double radius { get; set; }
        public ArcDirection arcDirection { get; set; }
            
        public Arc()
        {
            
            radiusDefinition = RadiusDefinition.byRadius;
            arcDirection = ArcDirection.CW;
            initialPoint = new CoordinatePoint();
            finalPoint = new CoordinatePoint();
            centerPoint = new CoordinatePoint();
            radius = 0.000;

            
        }

        public Arc(double initial_coord1, double initial_coord2, double final_coord1, double final_coord2, double _radius, ArcDirection _arcDirection)
        {

            arcDirection = _arcDirection;
            radiusDefinition = RadiusDefinition.byRadius;

            initialPoint = new CoordinatePoint(initial_coord1, initial_coord2);
            finalPoint = new CoordinatePoint(final_coord1, final_coord2);
            radius = _radius;
           // centerPoint = new CoordinatePoint();
            SettingCenterPoint();

           
        }

        public Arc(double initial_coord1, double initial_coord2, double final_coord1, double final_coord2, double center_coord1, double center_coord2, ArcDirection _arcDirection)
        {

            arcDirection = _arcDirection;
            radiusDefinition = RadiusDefinition.byCoordinateCenter;
            initialPoint = new CoordinatePoint(initial_coord1, initial_coord2);
            finalPoint = new CoordinatePoint(final_coord1, final_coord2);
            centerPoint = new CoordinatePoint(center_coord1, center_coord2);

           // SettingRadius();

          
        }

        private void SettingRadius()
        {
            radius = Math.Sqrt(Math.Pow(initialPoint.coord1 - centerPoint.coord1, 2) + Math.Pow(finalPoint.coord2 - centerPoint.coord2, 2));
        }

        public Size GetSize()
        {
            Size arcSize = new Size(radius, radius);
            return arcSize;
        }

        public SweepDirection GetSweepDirection()
        {
            if(arcDirection== ArcDirection.CCW)
            {
                return SweepDirection.Counterclockwise;
            }else
            {
                return SweepDirection.Clockwise;
            }
        }

        private void SettingCenterPoint()
        {
            double x1 = initialPoint.coord1;
            double y1 = initialPoint.coord2;
            double x2 = finalPoint.coord1;
            double y2 = finalPoint.coord2;
            double _radius = radius;

            double radsq = _radius * _radius;
            double q = Math.Sqrt(((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)));
            double x3 = (x1 + x2) / 2;


            centerPoint.coord2 = x3 + Math.Sqrt(radsq - ((q / 2) * (q / 2))) * ((y1 - y2) / q);


            q = Math.Sqrt(((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)));

            double y3 = (y1 + y2) / 2;

            centerPoint.coord1 = y3 + Math.Sqrt(radsq - ((q / 2) * (q / 2))) * ((x2 - x1) / q);
        }
        
    }
   
    [Serializable]
    public class Line
    {
        public CoordinatePoint initialPoint { get; set; }
        public CoordinatePoint finalPoint { get; set; }
      

        public Line()
        {
            
            initialPoint = new CoordinatePoint();
            finalPoint = new CoordinatePoint();
           
        }

        public Line(double initial_coord1, double initial_coord2, double final_coord1, double final_coord2)
        {
         
            initialPoint = new CoordinatePoint(initial_coord1, initial_coord2);
            finalPoint = new CoordinatePoint(final_coord1, final_coord2);            
        }

        public Vector ToVector(WorkingPlane wPlane)
        {
            Vector initialVect = initialPoint.GetVector(wPlane) ;
            Vector finalVect = finalPoint.GetVector(wPlane);
            Vector result = Vector.Subtract(finalVect, initialVect);
            return result;
        }
        

        public double[] GetConstantsLine(WorkingPlane wPlane)
        {
            double _m,_b;
            
            _m=(finalPoint.GetPoint(wPlane).Y - initialPoint.GetPoint(wPlane).Y /(finalPoint.GetPoint(wPlane).X -initialPoint.GetPoint(wPlane).X));
            _b = finalPoint.GetPoint(wPlane).Y - _m * finalPoint.GetPoint(wPlane).X;
            double[] constants =
            {
                -_m,
                -_b,
            };
            return constants;
        }
        
    }

    [Serializable]
    public class CoordinatePoint
    {
        public double coord1 { get; set; }
        public double coord2 { get; set; }
        private Point point { get; set; } 

        public CoordinatePoint()
        {
            coord1 = 0.00;
            coord2 = 0.00;
            
        }
    
        public CoordinatePoint GetCoordinatePoint(WorkingPlane wPlane)

        {
            CoordinatePoint _coordinatePoint = new CoordinatePoint();
            if(wPlane== WorkingPlane.XY)
            {
                _coordinatePoint.coord1 = coord2 ;
                _coordinatePoint.coord2 = coord1 ;
            }
            if (wPlane == WorkingPlane.ZX)
            {
                _coordinatePoint.coord1 = coord2;
                _coordinatePoint.coord2 = coord1*0.5;
            }

            return _coordinatePoint;
        }
        public Point GetPoint(WorkingPlane wPlane)
        {
            Point _point = new Point(GetCoordinatePoint(wPlane).coord1, GetCoordinatePoint(wPlane).coord2);
            return _point;
        }
        public Vector GetVector(WorkingPlane wPlane)
        {
            Vector _vector;
           _vector = new Vector(GetPoint(wPlane).X, GetPoint(wPlane).Y);
           return _vector;
        }

        public CoordinatePoint(double _coord1, double _coord2)
        {
            coord1 = _coord1;
            coord2 = _coord2;
        
        }

    }
    #endregion

    
    public class Drawing
    {
        public Point ProfileStartPoint { get; }     
        public double scale { get; }
        public double scaleOrigin { get;}
        public string scaleLabel { get; }

        public LineSegment line { get; }
        public ArcSegment arc { get;}
        public Type type { get; }
        public List<Drawing> listProfileDrawing { get; }
              
        public DrawingStock drawingStock { get; set; }
        public Point middlePoint { get; set; }

        public Drawing(BlackStock stock, double drawingLength, Point _middlePoint)
        {
            middlePoint = _middlePoint;
            drawingStock = new DrawingStock(stock,middlePoint);
            double lenthZ = stock.initialPosition - stock.finalPosition;

            scale = GetScale(lenthZ, stock.externalDiameter + drawingStock._chuckHight*2, drawingLength);
            scaleLabel = "1:" + (Math.Pow(scale,-1)*10).ToString("0.000") + "mm";
          
            
            scaleOrigin = scale*1.5;

            drawingStock.scale = scale;
            drawingStock.SettingDrawingStock();

            ProfileStartPoint = new Point(middlePoint.X + drawingStock.drawLenthStock / 2 - scale * stock.initialPosition, middlePoint.Y);
            listProfileDrawing = new List<Drawing>();
 
        }

        private double GetScale(double lenthZ, double externalDiameter,double ratio)
        {
            double _scale;
            if(lenthZ > externalDiameter)
            {
                _scale = ratio / lenthZ;
            }else
            {
                _scale = ratio*0.9/ externalDiameter;
            }

         
            return _scale;
        }

        public void SetListProfileDrawing(List<Geometry> geometry, WorkingPlane wPlane)
        {
           

            listProfileDrawing.Clear();
            foreach (Geometry _geometry in geometry)
            {
                if (_geometry.transition.enableTransition)
                {
                    #region With transition

                    #region If the geometry is the lastone
                    int nextIndex =0;
                    if (_geometry.id + 1 < geometry.Count)
                    {
                        nextIndex = _geometry.id + 1;
                    }
                    if (nextIndex == 0)
                    {
                        Drawing draw = DrawingWithOutTransition(_geometry, wPlane);
                        listProfileDrawing.Add(draw);
                    }
                    #endregion

                    else
                    {
                        Drawing draw;
                        Drawing drawtransition;

                        #region Transition between lines
                        if (_geometry.typeGeometry.Name == TypeGeometry.Line.ToString() &&
                            geometry[nextIndex].typeGeometry.Name == TypeGeometry.Line.ToString() )
                        {
                            DrawingWithTransitionBetweenLines(geometry, _geometry, nextIndex, out draw, out drawtransition,wPlane);

                            listProfileDrawing.Add(draw);
                            listProfileDrawing.Add(drawtransition);
                        }
                        #endregion

                        #region Transition between Line and Arc
                        if (_geometry.typeGeometry.Name == TypeGeometry.Line.ToString() &&
                            geometry[nextIndex].typeGeometry.Name == TypeGeometry.Arc.ToString())
                        {

                            double[] constantsLine = _geometry.line.GetConstantsLine(wPlane);
                            double epsilon = Math.Sqrt(1+Math.Pow(constantsLine[0],2));
                            int sgn;
                            if (geometry[nextIndex].arc.arcDirection == ArcDirection.CCW)
                            {
                                sgn = 1;
                            }else
                            {
                                sgn = -1;
                            }
                            double[] constantsParallelLine =
                            {
                                constantsLine[0],
                                (constantsLine[1]-sgn*_geometry.transition.parameter*epsilon),
                            };

                            double[] x_result =
                            {
                                // ax^2 + bx +c >>>> x = -b+-srq(b^2 - 4ac) / 2a

                               
                            };
                        }
                        }


                    #endregion

                    #endregion
                }
                else
                {
                    #region Without transition
                    Drawing draw = DrawingWithOutTransition(_geometry,wPlane);
                    listProfileDrawing.Add(draw);
                    #endregion
                }

            }

        }

        private void DrawingWithTransitionBetweenLines(List<Geometry> geometry, Geometry _geometry, int nextIndex, out Drawing draw, out Drawing drawtransition, WorkingPlane _wPlane)
        {
            Vector currentVector = _geometry.line.ToVector(_wPlane);
            Vector nextVector = geometry[nextIndex].line.ToVector(_wPlane);

            double alpha = Vector.AngleBetween(currentVector, nextVector);

            bool rigthDirection = alpha < 0;
            bool leftdirecction = alpha > 0;
            bool parallelDirection = alpha == 0;

            alpha = (180 - Math.Abs(alpha)) / 2;

            double length;
            if (_geometry.transition.typeTransition == TypeTransitionGeometry.Round)
            {
                #region Round
                length = _geometry.transition.parameter / Math.Tan(alpha * Math.PI / 180);

                Vector auxCurrentVector = Vector.Multiply(1 - (length / currentVector.Length), currentVector);
                Vector auxTransitionVector = Vector.Multiply((length / nextVector.Length), nextVector);

                Vector newCurrentVector = Vector.Add(_geometry.line.initialPoint.GetVector(_wPlane), auxCurrentVector);
                Vector transitionVector = Vector.Add(geometry[nextIndex].line.initialPoint.GetVector(_wPlane), auxTransitionVector);

                LineSegment _line = new LineSegment();
                _line.Point = new Point(ProfileStartPoint.X + scale * newCurrentVector.X,
                                        ProfileStartPoint.Y - scale * newCurrentVector.Y );
                draw = new Drawing(_line);

                ArcSegment _round = new ArcSegment();
                _round.Point = new Point(ProfileStartPoint.X + scale * transitionVector.X ,
                                         ProfileStartPoint.Y - scale * transitionVector.Y);
                _round.Size = new Size(scale * _geometry.transition.parameter,scale * _geometry.transition.parameter);
                if (rigthDirection)
                {
                    _round.SweepDirection = SweepDirection.Clockwise;
                }
                else
                {
                    _round.SweepDirection = SweepDirection.Counterclockwise;
                }

                drawtransition = new Drawing(_round);
                #endregion
            }
            else
            {
                #region Chamfer
                length = _geometry.transition.parameter*0.5 / Math.Sin(alpha * Math.PI / 180);
                Vector auxCurrentVector = Vector.Multiply(1 - (length / currentVector.Length), currentVector);
                Vector auxTransitionVector = Vector.Multiply((length / nextVector.Length), nextVector);

                Vector newCurrentVector = Vector.Add(_geometry.line.initialPoint.GetVector(_wPlane), auxCurrentVector);
                Vector transitionVector = Vector.Add(geometry[nextIndex].line.initialPoint.GetVector(_wPlane), auxTransitionVector);

                LineSegment _line = new LineSegment();
                _line.Point = new Point(ProfileStartPoint.X + scale * newCurrentVector.X,
                                        ProfileStartPoint.Y - scale * newCurrentVector.Y);
                draw = new Drawing(_line);

                LineSegment _chamfer = new LineSegment();
                _chamfer.Point = new Point(ProfileStartPoint.X + scale * transitionVector.X ,
                                           ProfileStartPoint.Y - scale * transitionVector.Y);
                drawtransition = new Drawing(_chamfer);
                #endregion
            }


        }

        private Drawing DrawingWithOutTransition(Geometry _geometry, WorkingPlane _wPlane)
        {
            #region Without transition
            Drawing draw = new Drawing(_geometry.typeGeometry);
            if (_geometry.typeGeometry.Name == TypeGeometry.Line.ToString())
            {

                draw.line.Point = new Point(ProfileStartPoint.X + scale * _geometry.line.finalPoint.GetPoint(_wPlane).X,
                                             ProfileStartPoint.Y - scale * _geometry.line.finalPoint.GetPoint(_wPlane).Y);
            }
            else
            {

                draw.arc.Point = new Point(ProfileStartPoint.X + scale * _geometry.arc.finalPoint.GetPoint(_wPlane).X ,
                                           ProfileStartPoint.Y - scale * _geometry.arc.finalPoint.GetPoint(_wPlane).Y);
                draw.arc.Size = new Size(_geometry.arc.GetSize().Height * scale, _geometry.arc.GetSize().Width * scale);
                draw.arc.SweepDirection = _geometry.arc.GetSweepDirection();
            }
            #endregion
            return draw;
        }

        public Drawing(Type _type)
        {
            line = new LineSegment();
            arc = new ArcSegment();
            type = _type;
        }

        public Drawing(ArcSegment _arc)
        {
            
            arc = new ArcSegment(_arc.Point,_arc.Size,_arc.RotationAngle,_arc.IsLargeArc,_arc.SweepDirection,_arc.IsStroked);
            type = new Arc().GetType();
        }

        public Drawing(LineSegment _line)
        {
            line = new LineSegment(_line.Point,_line.IsStroked);
            type = new Line().GetType();
        }
    }

    public class DrawingStock
    {
       
        public Point StockStartPoint { get; set; }
        public Point A_ChuckStartPoint { get; set; }
        public Point B_ChuckStartPoint { get; set; }

        public double scale { get; set; }

        public List<LineSegment> listExtStockDrawing { get; }
        public List<LineSegment> listIntStockDrawing { get; }
        public List<LineSegment> list_A_ChuckDrawing { get; }
        public List<LineSegment> list_B_ChuckDrawing { get; }

        public double chuckWidth { get; set; }
        public double _chuckWidth { get; set; }
        public double chuckHight { get; set; }
        public double _chuckHight { get; set; }

        private double drawExternalDiameter { get; set; }
        private double drawInternalDiameter { get; set; }
        public double drawLenthStock { get; set; }
        private double drawPositionChuck { get; set; }

        BlackStock stock { get; } 
        Point middlePoint { get; }

        public DrawingStock(BlackStock _stock, Point _middlePoint)
        {
            _chuckWidth = 40;
            _chuckHight = _chuckWidth * 0.5;
            middlePoint = _middlePoint;
            stock = new BlackStock();
            stock = _stock;

            listExtStockDrawing = new List<LineSegment>();
            listIntStockDrawing = new List<LineSegment>();


            list_A_ChuckDrawing = new List<LineSegment>();
            list_B_ChuckDrawing = new List<LineSegment>();
     

        }

        public void SettingDrawingStock()
        {
            double lenthZ = stock.initialPosition - stock.finalPosition;
            chuckWidth = _chuckWidth * scale;
            chuckHight = _chuckHight * scale;
            drawLenthStock = scale * lenthZ;
            drawInternalDiameter = scale * stock.internalDiameter;
            drawExternalDiameter = scale * stock.externalDiameter;
            drawPositionChuck = scale * (stock.initialPosition - stock.splindleLimit);

            StockStartPoint = new Point(middlePoint.X + drawLenthStock / 2, middlePoint.Y);
            B_ChuckStartPoint = new Point((middlePoint.X + drawLenthStock / 2) - drawPositionChuck, middlePoint.Y + drawExternalDiameter / 2);
            A_ChuckStartPoint = new Point((middlePoint.X + drawLenthStock / 2) - drawPositionChuck, middlePoint.Y - drawExternalDiameter / 2);

            SetListStockDrawing();
            SetListChickDrawing();
        }

        private void SetListStockDrawing()
        {
            #region External Stock
            {
                LineSegment line = new LineSegment();
                line.Point = new Point(StockStartPoint.X, StockStartPoint.Y + drawExternalDiameter / 2);
                listExtStockDrawing.Add(line);
            }
            {
                LineSegment line = new LineSegment();
                line.Point = new Point(StockStartPoint.X - drawLenthStock, StockStartPoint.Y + drawExternalDiameter / 2);
                listExtStockDrawing.Add(line);
            }
            {
                LineSegment line = new LineSegment();
                line.Point = new Point(StockStartPoint.X - drawLenthStock, StockStartPoint.Y - drawExternalDiameter / 2);
                listExtStockDrawing.Add(line);
            }
            {
                LineSegment line = new LineSegment();
                line.Point = new Point(StockStartPoint.X, StockStartPoint.Y - drawExternalDiameter / 2);
                listExtStockDrawing.Add(line);
            }
            #endregion

            #region Internal Stock
            if (drawInternalDiameter != 0)
            {
                {
                    LineSegment line = new LineSegment();
                    line.Point = new Point(StockStartPoint.X, StockStartPoint.Y + drawInternalDiameter / 2);
                    listIntStockDrawing.Add(line);
                }
                {
                    LineSegment line = new LineSegment();
                    line.Point = new Point(StockStartPoint.X - drawLenthStock, StockStartPoint.Y + drawInternalDiameter / 2);
                    listIntStockDrawing.Add(line);
                }
                {
                    LineSegment line = new LineSegment();
                    line.Point = new Point(StockStartPoint.X - drawLenthStock, StockStartPoint.Y - drawInternalDiameter / 2);
                    listIntStockDrawing.Add(line);
                }
                {
                    LineSegment line = new LineSegment();
                    line.Point = new Point(StockStartPoint.X, StockStartPoint.Y - drawInternalDiameter / 2);
                    listIntStockDrawing.Add(line);
                }
            }
            #endregion
        }

        private void SetListChickDrawing()
        {
            double highMiddleChuck = 0.6;
            #region Chuck A
            {
                LineSegment line = new LineSegment();
                line.Point = new Point(A_ChuckStartPoint.X, A_ChuckStartPoint.Y - chuckHight* highMiddleChuck);
                list_A_ChuckDrawing.Add(line);
            }
            {
                LineSegment line = new LineSegment();
                line.Point = new Point(A_ChuckStartPoint.X - chuckWidth*0.5, A_ChuckStartPoint.Y - chuckHight* highMiddleChuck);
                list_A_ChuckDrawing.Add(line);
            }
            {
                LineSegment line = new LineSegment();
                line.Point = new Point(A_ChuckStartPoint.X - chuckWidth * 0.5, A_ChuckStartPoint.Y - chuckHight);
                list_A_ChuckDrawing.Add(line);
            }
            {
                LineSegment line = new LineSegment();
                line.Point = new Point(A_ChuckStartPoint.X - chuckWidth, A_ChuckStartPoint.Y -  chuckHight);
                list_A_ChuckDrawing.Add(line);
            }
            {
                LineSegment line = new LineSegment();
                line.Point = new Point(A_ChuckStartPoint.X - chuckWidth, A_ChuckStartPoint.Y);
                list_A_ChuckDrawing.Add(line);
            }

            #endregion

            #region Chuck B
            {
                LineSegment line = new LineSegment();
                line.Point = new Point(B_ChuckStartPoint.X, B_ChuckStartPoint.Y + chuckHight* highMiddleChuck);
                list_B_ChuckDrawing.Add(line);
            }
            {
                LineSegment line = new LineSegment();
                line.Point = new Point(B_ChuckStartPoint.X - chuckWidth*0.5, B_ChuckStartPoint.Y + chuckHight* highMiddleChuck);
                list_B_ChuckDrawing.Add(line);
            }
            {
                LineSegment line = new LineSegment();
                line.Point = new Point(B_ChuckStartPoint.X - chuckWidth * 0.5, B_ChuckStartPoint.Y + chuckHight);
                list_B_ChuckDrawing.Add(line);
            }
            {
                LineSegment line = new LineSegment();
                line.Point = new Point(B_ChuckStartPoint.X - chuckWidth , B_ChuckStartPoint.Y + chuckHight);
                list_B_ChuckDrawing.Add(line);
            }
            {
                LineSegment line = new LineSegment();
                line.Point = new Point(B_ChuckStartPoint.X - chuckWidth, B_ChuckStartPoint.Y);
                list_B_ChuckDrawing.Add(line);
            }
            #endregion
        }

    }
}
