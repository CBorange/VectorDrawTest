using VectordrawTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using VectordrawTest.Model.Manager;
using VectordrawTest.Model.CuttingAlgorithm;

namespace VectordrawTest
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Create Model Instance
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 mainForm = new Form1();
            BeamManager beamManager = new BeamManager();
            BeamCutter collisionCalculator = new BeamCutter();
            BeamBuilder beamBuilder = new BeamBuilder();
            CollisionLineCalculator colLineCalculator = new CollisionLineCalculator();
            mainForm.InitializeAllModels(VectorDrawConfigure.Instance, beamManager, collisionCalculator, beamBuilder, colLineCalculator);

            Application.Run(mainForm);
        }
    }
}
