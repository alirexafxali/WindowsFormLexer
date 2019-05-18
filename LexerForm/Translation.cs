using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerForm
{
    class Translation
    {
        public string Value { get; set; }
        public State currentState { get; set; }
        public State nextState { get; set; }
        public string Action { get; set; }
        public Translation(string value)
        {
            this.Value = value;
            currentState = null;
            nextState = null;
            Action = "";
        }
        public bool isValid(char inputChar)
        {
            if (Value.ToCharArray().Contains(inputChar))
                return true;
            return false;
        }
    }
}
