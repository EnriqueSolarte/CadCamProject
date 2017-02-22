using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace CadCamProject
{
    public class PostProcessor
    {
        private WorkSettings wSettings { get; }
        private Turning turning { get; }
        private SpecialsChart sCh { get;}


        public string gCodeFile { get; internal set; }

        public PostProcessor(WorkSettings _wSettings, Turning _turningOperation)
        {
            wSettings = _wSettings;
            turning = _turningOperation;
        }

        private void Add(string _data)
        {
           
        }

        private void NewBlock()
        {
           
        }

        private void NewLine()
        {
           
        }

        private void LineMove()
        {

        }

        private void ArcMove()
        {

        }

        private void GetGCode()
        {
           
           
        }

        private void StartFile()
        {
           
        }

        private void ToolChanging(Tool tool)
        {
            

        }


        private void EndFile()
        {
            
        }
    }
}
