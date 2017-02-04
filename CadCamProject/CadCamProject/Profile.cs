using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CadCamProject.Pages;


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
        public string Path { get; set; }

        

        //Specific parameters
        public string OperationName { get; set; }
        public int workOffsetIndex { get; set; }
        public Gcode workOffset { get; set; }
        public wPlane workingPlane { get; set; }
        public List<Geometry> geometry { get; set; }


        public Profile()
        {
            TypeImagineOperation = "/Images/Profile.png";
            TypeOperation = Operations.Profile;
            workingPlane = wPlane.XZ;
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

    [Serializable]
    public class Geometry
    {
        public Line line { get; set; }
        public Arc arc { get; set; }
        public TransitionGeometry transition { get; set; }

        public Point initialPosition { get; }
        public Point finalPosition { get; }

        public int id { get; set; }
        public TypeGeometry typeGeometry { get; set; }

        public Geometry(Arc _arc, TransitionGeometry _transition, TypeGeometry _typeGeometry, int _id)
        {
            arc = _arc;
            transition = _transition;
            typeGeometry = _typeGeometry;
            id = _id;
            initialPosition = _arc.initialPoint;
            finalPosition = _arc.finalPoint;    

        }

        public Geometry(Line _line, TransitionGeometry _transition, TypeGeometry _typeGeometry, int _id)
        {
            line = _line;
            transition = _transition;
            typeGeometry = _typeGeometry;
            id = _id;
            initialPosition = _line.initialPoint;
            finalPosition = _line.finalPoint;

        }
    }
    [Serializable]
    public class TransitionGeometry
    {
        public TypeTransitionGeometry typeTransition { get; set; }
        private TransitionParameter labelParameter { get; set; }
        public double parameter { get; set; }
        
    }
    [Serializable]
    public class Arc
    {
        public RadiusDefinition radiusDefinition { get; set; }
        public Point initialPoint { get; set; }
        public Point finalPoint { get; set; }
        public Point centerPoint { get; set; }
        public double radius { get; set; }
        public ArcDirection arcDirection {get; set;}

        public Arc()
        {
            radiusDefinition = RadiusDefinition.byRadius;
            arcDirection = ArcDirection.CW;
            initialPoint = new Point();
            finalPoint = new Point();
            centerPoint = new Point();
            radius = 0.000;
        }

        public Arc(double initial_coord1, double initial_coord2, double final_coord1, double final_coord2, double _radius, ArcDirection _arcDirection)
        {
            arcDirection = _arcDirection;
            radiusDefinition = RadiusDefinition.byRadius;

            initialPoint = new Point(initial_coord1, initial_coord2);
            finalPoint = new Point(final_coord1, final_coord2);
            radius = _radius;

            centerPoint = gettingArcParameters(initialPoint, finalPoint, radius);
        }

        public Arc(double initial_coord1, double initial_coord2, double final_coord1, double final_coord2, double center_coord1, double center_coord2, ArcDirection _arcDirection)
        {

            arcDirection = _arcDirection;
            radiusDefinition = RadiusDefinition.byCoordinateCenter;
            initialPoint = new Point(initial_coord1, initial_coord2);
            finalPoint = new Point(final_coord1, final_coord2);
            centerPoint = new Point(center_coord1, center_coord2);

            radius = gettingArcParameters(initialPoint, finalPoint, centerPoint);

        }

        private double gettingArcParameters(Point initialPoint, Point finalPoint, Point centerPoint)
        {
            double _radius = 0.000;
            return _radius;
        }

        private Point gettingArcParameters(Point initialPoint, Point finalPoint, double radius)
        {
            Point cPoint = new Point();
            return cPoint;
        }
    }
    [Serializable]
    public class Line
    {
        public Point initialPoint { get; set; }
        public Point finalPoint { get; set; }

        public Line()
        {
            initialPoint = new Point();
            finalPoint = new Point(); 
        }

        public Line(double initial_coord1, double initial_coord2, double final_coord1, double final_coord2)
        {
            initialPoint = new Point(initial_coord1, initial_coord2);
            finalPoint = new Point(final_coord1, final_coord2);
        }
    }
    [Serializable]
    public class Point
    {
        public double coord1 { get; set; }
        public double coord2 { get; set; }

        public Point()
        {
            coord1 = 0.00;
            coord2 = 0.00;
        }

        public Point(double _coord1, double _coord2)
        {
            coord1 = _coord1;
            coord2 = _coord2;
        }
    }

}
