using MathPractice.Controller;
using MathPractice.Model;
using MathPractice.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathPractice.Model.Manager;
using MathPractice.Model.CollisionCalculator;

namespace MathPractice
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
            BeamCollisionCalculator collisionCalculator = new BeamCollisionCalculator();
            BeamBuilder beamBuilder = new BeamBuilder();
            mainForm.InitializeAllModels(VectorDrawConfigure.Instance, beamManager, collisionCalculator, beamBuilder);

            MainController mainController = new MainController(mainForm, VectorDrawConfigure.Instance, beamManager, collisionCalculator,
                beamBuilder);
            mainForm.AttachController(mainController);


            Application.Run(mainForm);
        }
    }
}
