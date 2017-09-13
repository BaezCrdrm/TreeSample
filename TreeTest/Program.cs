using System;
using System.Collections.Generic;
using TreeTest.Model;
using TreeLogic = TreeTest.Logic.Logic;

namespace TreeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TreeLogic lg;
            string filePath = "";
            
            do
            {
                // Inicio de programa
                lg = new TreeLogic();

                // Muestra menú del programa
                Menu_PrintMainMenu();
                ConsoleKeyInfo key = Console.ReadKey();

                //Lectura del menú.
                Console.Clear();
                if (key.Key == ConsoleKey.D1 || key.Key == ConsoleKey.NumPad1)
                {
                    Console.WriteLine("\n\tCarga nodos desde archivo JSON");
                    Console.WriteLine("\nIngresa la ruta del archivo JSON de datos");
                    // C:\Users\Samuel\Desktop\tree-data-test.json
                    filePath = Console.ReadLine();
                }

                if (key.Key == ConsoleKey.D2 || key.Key == ConsoleKey.NumPad2)
                {
                    filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    filePath += "\\tree-data-test.json";
                }

                LoadDataFromJsonFile(lg, filePath);

                if (lg.Nodes != null && lg.Nodes.Count > 0)
                {
                    // Selección de usuario de nodo de inicio
                    SeleccionNodoInicioFinal(lg);

                    // Continuar dentro de este segmento
                    lg.GetLifo();
                    Console.Clear();
                    Console.WriteLine("\tResultado");
                    Console.WriteLine(GetStringRes(lg.Visited));
                    Console.WriteLine("Costo: " + lg.Cost.ToString());
                }
                else
                    Console.WriteLine("\nEror: No se encontraron datos para continuar...");

                Console.WriteLine("\n\nPresiona cualquier tecla para continuar...");
                Console.ReadKey();
            } while (true);
        }

        private static string GetStringRes(List<Node> Nodes)
        {
            string visited = "";
            foreach (Node node in Nodes)
            {
                visited += String.Format(" {0} ", node.ID);
            }
            return visited;
        }

        private static void Menu_PrintMainMenu()
        {
            Console.Clear();
            Console.WriteLine("\tBienvenido");
            Console.WriteLine("Selecciona una opcion");
            Console.WriteLine("1) Carga nodos desde archivo JSON");
            Console.WriteLine("2) Carga nodos desde archivo JSON predeterminado (Desktop)");
        }

        private static void SeleccionNodoInicioFinal(TreeLogic lg)
        {
            Console.Clear();
            do
            {
                Menu_SelectNode(lg.Nodes, lg.FirstNode);
                string sn = Console.ReadLine();

                if (lg.FirstNode == null)
                {
                    lg.FirstNode = lg.VerificaNodo(lg.Nodes, sn);
                    if (lg.FirstNode == null)
                        Console.WriteLine("Selecciona un nodo válido");
                    else
                        Console.WriteLine(String.Format("Nodo '{0}' seleccionado como nodo de inicio",
                            sn.ToUpper()));
                } else
                {
                    lg.Objective = lg.VerificaNodo(lg.Nodes, sn);
                    if (lg.Objective == null)
                        Console.WriteLine("Selecciona un nodo válido");
                    else
                        Console.WriteLine(String.Format("Nodo '{0}' seleccionado como nodo final (Objetivo)",
                            sn.ToUpper()));
                }
                Console.WriteLine("\nPresione Enter para continuar...");
                Console.ReadLine();
            } while (lg.FirstNode == null || lg.Objective == null);
        }

        private static void Menu_SelectNode(List<Node> nodes, Node exception)
        {
            string msg = "\tSeleccione ";
            if (exception == null)
                msg += "un nodo de inicio.\n";
            else
                msg += "el final (Objetivo).\n";
            Console.WriteLine(msg);
            foreach (Node n in nodes)
            {
                Console.Write(String.Format(" {0} ", n.ID));
            }
            Console.WriteLine();
        }

        private static void LoadDataFromJsonFile(TreeLogic lg, string filePath)
        {
            do
            {                
                lg.LoadDataFromJsonFile(filePath);

                if (lg.Nodes == null)
                    Console.WriteLine("No se pudieron cargar los datos. Intenta de nuevo.");
                else
                {
                    Console.WriteLine("Datos cargados correctamente");
                    Console.WriteLine("Cantidad de elementos nodos cargados: " + lg.Nodes.Count.ToString());
                }
            } while (lg.Nodes == null);
        }
    }
}
