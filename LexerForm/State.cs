using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerForm
{
    class State
    {
        public string stateName { get; set; }
        public List<Translation> allTranslations { get; set; }

        public State(string stateName)
        {
            this.stateName = stateName;
            allTranslations = new List<Translation>();
        }
    }
}
