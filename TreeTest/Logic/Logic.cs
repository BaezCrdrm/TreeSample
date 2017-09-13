using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TreeTest.Model;

namespace TreeTest.Logic
{
    public class Logic
    {
        public Node FirstNode { get; set; }
        public Node Objective { get; set; }        
        private List<Node> _nodes;
        private List<Node> _opened;
        private List<Node> _visited;        
        private int _cost;

        #region Propierties

        public List<Node> Visited
        {
            get { return _visited; }
            set { _visited = value; }
        }
        
        public List<Node> Opened
        {
            get { return _opened; }
            set { _opened = value; }
        }
        
        public List<Node> Nodes
        {
            get { return _nodes; }
            set { _nodes = value; }
        }

        public int Cost { get { return _cost; } }
        #endregion

        public Logic()
        {
            InitValues();
        }

        private void InitValues()
        {
            this._opened = new List<Node>();
            this._visited = new List<Node>();
            this._cost = 0;
        }

        public void GetLifo()
        {
            InitValues();
            _visited.Add(FirstNode);
            OpenChildren(FirstNode);
            Node temp;

            do
            {
                if (_opened.Count > 0)
                {
                    temp = _opened.Last<Node>();
                    if (!VerificaNodo(_visited, temp))
                    {
                        // Visita nodo nuevo y remueve el existente de la tabla de visitados
                        _opened.Remove(temp);
                        _visited.Add(temp);

                        _cost++;
                        if (temp.ID == Objective.ID)
                            break;
                        else
                        {
                            OpenChildren(this._nodes.FirstOrDefault<Node>(p => p.ID == _visited.Last<Node>().ID));
                        }
                    }
                }
            } while (_visited.Last<Node>().ID != this.Objective.ID || _cost > Nodes.Count);
        }
             
        private void OpenChildren(Node node)
        {
            List<Node> tempList = new List<Node>();
            // Abre los nodos hijo en caso de que este
            // no se encuentre en la lista de visitados ni en la de abiertos.
            foreach (Node n in node.Children)
            {
                Node temp = this._nodes.FirstOrDefault<Node>(p => p.ID == n.ID);
                if (!VerificaNodo(_opened, temp) && !VerificaNodo(_visited, temp))
                    tempList.Add(temp);
            }

            if(tempList.Count > 0)
            {
                tempList.OrderByDescending(p => p.ID);

                foreach (Node n in tempList)
                {
                    _opened.Add(n);
                }
            }
            tempList = null;
        }

        public void LoadDataFromJsonFile(string filePath)
        {            
            try
            {
                JArray array = JArray.Parse(File.ReadAllText(@filePath));
                LoadData(array);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al cargar los datos");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                _nodes = null;
            }
        }

        private void LoadData(JArray array)
        {
            _nodes = new List<Node>();
            Node node;
            Node tempNode;
            foreach (JObject obj in array.Children<JObject>())
            {
                node = new Node
                {
                    ID = obj.Property("ID").Value.ToString(),
                    Alias = obj.Property("Alias").Value.ToString()
                };

                foreach (JObject children in obj.Property("Children").Value)
                {
                    tempNode = new Node
                    {
                        ID = children.Property("ID").Value.ToString(),
                        //Alias = obj.Property("Alias").Value.ToString()
                    };
                    node.Children.Add(tempNode);
                }

                _nodes.Add(node);
            }
        }

        // Verifica si el nodo seleccionado se encuentra en una lista de nodos
        public Node VerificaNodo(List<Node> nodes, string seleccion)
        {
            // Regresa el nodo en caso de que se encuentre en la lista de nodos
            Node node = nodes.FirstOrDefault<Node>(
                                    p => p.ID.ToUpper() == seleccion.ToUpper());
            return node;
        }

        private bool VerificaNodo(List<Node> nodes, Node node)
        {
            // Regresa verdadero si el nodo Sí se encuentra en la lista de nodos
            if (nodes.FirstOrDefault<Node>(p => p == node) == null)
                return false;
            else return true;
        }
    }
}
