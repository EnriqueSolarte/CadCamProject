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

            centerPoint = gettingArcParameters(initialPoint, finalPoint, radius);

           
        }

        public Arc(double initial_coord1, double initial_coord2, double final_coord1, double final_coord2, double center_coord1, double center_coord2, ArcDirection _arcDirection)
        {

            arcDirection = _arcDirection;
            radiusDefinition = RadiusDefinition.byCoordinateCenter;
            initialPoint = new CoordinatePoint(initial_coord1, initial_coord2);
            finalPoint = new CoordinatePoint(final_coord1, final_coord2);
            centerPoint = new CoordinatePoint(center_coord1, center_coord2);

            radius = gettingArcParameters(initialPoint, finalPoint, centerPoint);

          
        }

        private double gettingArcParameters(CoordinatePoint initialPoint, CoordinatePoint finalPoint, CoordinatePoint centerPoint)
        {
            double _radius = 0.000;
            return _radius;
        }

        private CoordinatePoint gettingArcParameters(CoordinatePoint initialPoint, CoordinatePoint finalPoint, double radius)
        {
            CoordinatePoint cPoint = new CoordinatePoint();
            return cPoint;
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
            point = new Point(coord2, -coord1);
        }
    

        public CoordinatePoint(double _coord1, double _coord2)
        {
            coord1 = _coord1;
            coord2 = _coord2;
            point = new Point(coord2, -coord1);
        }

        public Point ToPoint()
        {
            point = new Point(coord2, -coord1);
            return point;
        }
        
    }
    #endregion

    
    public class Drawing
    {
        public Point ProfileStartPoint { get; }
        public Point StockStartPoint { get; }
        public Point A_ChuckStartPoint { get; }
        public Point B_ChuckStartPoint { get; }
        public double scale { get; }
        public double scaleOrigin { get;}

        public LineSegment line { get; }
        public ArcSegment Arc { get;}
        public Type type { get; }
        public List<Drawing> listProfileDrawing { get; }
        public List<LineSegment> listExtStockDrawing { get; }
        public List<LineSegment> listIntStockDrawing { get; }
        public List<LineSegment> list_A_ChuckDrawing { get; }
        public List<LineSegment> list_B_ChuckDrawing { get; }

        private double chuckWidth { get; }
        private double chuckHight { get; }
        public string scaleLabel { get; }

        private double drawExternalDiameter { get; }
        private double drawInternalDiameter { get; }
        private double drawLenthStock{ get; }
        private double drawPositionChuck { get; }
       

        public Drawing(BlackStock stock)
        {
            Point middlePoint = new Point(485, 330);
            double _chuckWidth = 40;
            double lenthZ = stock.initialPosition - stock.finalPosition;

            scale = GetScale(lenthZ, stock.externalDiameter+_chuckWidth,650);
            scaleLabel = "1:" + (Math.Pow(scale,-1)*10).ToString("0.000") + "mm";


            chuckWidth = 40* scale;
            chuckHight = 0.5*chuckWidth;
            drawLenthStock = scale * lenthZ;
            drawInternalDiameter = scale * stock.internalDiameter;
            drawExternalDiameter = scale * stock.externalDiameter;
            drawPositionChuck = scale * (stock.initialPosition - stock.splindleLimit);
            
            scaleOrigin = scale;

            ProfileStartPoint = new Point(middlePoint.X + drawLenthStock / 2 - scale * stock.initialPosition, middlePoint.Y);
            StockStartPoint = new Point(middlePoint.X + drawLenthStock / 2, middlePoint.Y);
            B_ChuckStartPoint = new Point((middlePoint.X + drawLenthStock / 2) - drawPositionChuck, middlePoint.Y + drawExternalDiameter / 2);
            A_ChuckStartPoint = new Point((middlePoint.X + drawLenthStock / 2) - drawPositionChuck, middlePoint.Y - drawExternalDiameter / 2);


            listProfileDrawing = new List<Drawing>();
           
            listExtStockDrawing = new List<LineSegment>();
            listIntStockDrawing = new List<LineSegment>();
            SetListStockDrawing();

            list_A_ChuckDrawing = new List<LineSegment>();
            list_B_ChuckDrawing = new List<LineSegment>();
            SetListChickDrawing();
        }

        private void SetListChickDrawing()
        {
            #region Chuck A
            {
                LineSegment line = new LineSegment();
                line.Point = new Point(A_ChuckStartPoint.X, A_ChuckStartPoint.Y- chuckHight);
                list_A_ChuckDrawing.Add(line);
            }
            {
                LineSegment line = new LineSegment();
                line.Point = new Point(A_ChuckStartPoint.X - chuckWidth, A_ChuckStartPoint.Y - chuckHight);
                list_A_ChuckDrawing.Add(line);
            }
            {
                LineSegment line = new LineSegment();
                line.Point = new Point(A_ChuckStartPoint.X-chuckWidth, A_ChuckStartPoint.Y);
                list_A_ChuckDrawing.Add(line);
            }
      
            #endregion

            #region Chuck B
            {
                LineSegment line = new LineSegment();
                line.Point = new Point(B_ChuckStartPoint.X, B_ChuckStartPoint.Y + chuckHight);
                list_B_ChuckDrawing.Add(line);
            }
            {
                LineSegment line = new LineSegment();
                line.Point = new Point(B_ChuckStartPoint.X - chuckWidth, B_ChuckStartPoint.Y + chuckHight);
                list_B_ChuckDrawing.Add(line);
            }
            {
                LineSegment line = new LineSegment();
                line.Point = new Point(B_ChuckStartPoint.X - chuckWidth, B_ChuckStartPoint.Y);
                list_B_ChuckDrawing.Add(line);
            }
            #endregion
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
            double cX;
            double cY;
            if (wPlane == WorkingPlane.ZX)
            {
                 cX = 1;
                 cY = 0.5;
            }else
            {
                 cX = 1;
                 cY = 1;
            }

            listProfileDrawing.Clear();
            foreach (Geometry _geometry in geometry)
            {
               
                if (_geometry.transition.enableTransition)
                {
                    #region With transition

                    int nextIndex=0;
                    if (_geometry.id + 1 < geometry.Count)
                    {
                        nextIndex = _geometry.id + 1;
                    }

                    if (nextIndex != 0)
                    {
                        Drawing draw = DrawingWithOutTransition(cX, cY, _geometry);
                        listProfileDrawing.Add(draw);
                    }
                    else
                    {
                        Drawing draw = new Drawing(_geometry.typeGeometry);
                        if (_geometry.typeGeometry.Name == TypeGeometry.Line.ToString())
                        {
                            Vector vbase = new Vector(_geometry.initialPosition.coord2, _geometry.initialPosition.coord1);
                            Vector vector_curr = new Vector(_geometry.finalPosition.coord2, _geometry.finalPosition.coord1);
                            Vector vector_next = new Vector(geometry[nextIndex].finalPosition.coord2, geometry[nextIndex].finalPosition.coord1);



                        }
                        else
                        {

                           
                        }
                    }
                       
                   
                    #endregion
                    


                }
                else
                {
                    Drawing draw = DrawingWithOutTransition(cX, cY, _geometry);
                    listProfileDrawing.Add(draw);
                }

            }

        }

        private Drawing DrawingWithOutTransition(double cX, double cY, Geometry _geometry)
        {
            #region Without transition
            Drawing draw = new Drawing(_geometry.typeGeometry);
            if (_geometry.typeGeometry.Name == TypeGeometry.Line.ToString())
            {

                draw.line.Point = new Point(cX * scale * _geometry.line.finalPoint.ToPoint().X + ProfileStartPoint.X,
                                             cY * scale * _geometry.line.finalPoint.ToPoint().Y + ProfileStartPoint.Y);
            }
            else
            {

                draw.Arc.Point = new Point(cX * scale * _geometry.arc.finalPoint.ToPoint().X + ProfileStartPoint.X,
                                             cY * scale * _geometry.arc.finalPoint.ToPoint().Y + ProfileStartPoint.Y);
                draw.Arc.Size = new Size(_geometry.arc.GetSize().Height * scale, _geometry.arc.GetSize().Width * scale);
                draw.Arc.SweepDirection = _geometry.arc.GetSweepDirection();
            }
            #endregion
            return draw;
        }

        public Drawing(Type _type)
        {
            line = new LineSegment();
            Arc = new ArcSegment();
            type = _type;
        }

        public Drawing()
        {
            line = new LineSegment();
            Arc = new ArcSegment();
        }
    }


}
