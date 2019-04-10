namespace DParser
{
    public class ASTNode
    {
        private string _type = null;
        private string _value = null;
        private ASTNode _left = null;
        private ASTNode _right = null;
        private ASTNode _parent = null;
        public ASTNode(string type,string value,ASTNode left,ASTNode right,ASTNode parent)
        {
            _type = type;
            _value = value;
            _left = left;
            _right = right;
            _parent = parent;
        }

        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public ASTNode Left
        {
            get
            {
                return _left;
            }
            set
            {
                _left = value;
            }
        }

        public ASTNode Right
        {
            get
            {
                return _right;
            }
            set
            {
                _right = value;
            }
        }

        public ASTNode Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }
    }
}
