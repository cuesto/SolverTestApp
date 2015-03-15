using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SolverFoundation.Services;

namespace SolverTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var solver = SolverContext.GetContext();
            var model = solver.CreateModel();
            double pArroz = 20;
            double pAceite = 280;
            double pHabichuela = 25;
            double presupuesto = 1500;

            var decisionX = new Decision(Domain.IntegerNonnegative, "Arroz");
            var decisionY = new Decision(Domain.IntegerNonnegative, "Aceite");
            var decisionZ = new Decision(Domain.IntegerNonnegative, "Habichuela");
            model.AddDecision(decisionX);
            model.AddDecision(decisionY);
            model.AddDecision(decisionZ);
            var tGoal = (pArroz*decisionX) + (pAceite*decisionY) + (pHabichuela*decisionZ);

            model.AddGoal("Meta", GoalKind.Maximize, tGoal);
            model.AddConstraint("cantminX", decisionX >= 20);
            model.AddConstraint("cantmaxX", decisionX <= 40);
            model.AddConstraint("cantminY", decisionY >= 1);
            model.AddConstraint("cantmaxY", decisionY <= 3);
            model.AddConstraint("cantminZ", decisionZ >= 1);
            model.AddConstraint("cantmaxZ", decisionZ <= 3);
            model.AddConstraint("Total", tGoal >= 1);
            model.AddConstraint("Solucion", tGoal <= presupuesto);
            solver.CheckModel();

            var solution = solver.Solve();
            var qa = solution.Quality;
            var gls = solution.Goals;
            double x = decisionX.GetDouble();
            double y = decisionY.GetDouble();
            double z = decisionZ.GetDouble();
            double total = (x*pArroz) + (y*pAceite) + (z*pHabichuela);
            Console.WriteLine("Cantidades Máximas: ");
            Console.WriteLine("Arroz: " + x + " - Aceite: " + y + " - Habichuela: " + z);
            Console.WriteLine("Solución: " + total);
            Report r = solution.GetReport();

            using (StreamWriter sw = new StreamWriter("ejemplo1.txt"))
            {
                solver.SaveModel(FileFormat.FreeMPS, sw);
            }
        }
    }
}
