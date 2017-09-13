using System.Collections.Generic;

namespace TreeTest.Model
{
    public class Node
    {
        public string ID { get; set; }
        public string Alias { get; set; }
        private List<Node> _children;

        public List<Node> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        public Node()
        {
            _children = new List<Node>();
        }

    }
}
