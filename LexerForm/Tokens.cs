using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace LexerForm
{
    public partial class Tokens : Form
    {
        static char[] endLineChar = { '\r' };
        char input = ' ';
        string endLine = new string(endLineChar);
        static State startState = new State("StartState");
        Automata automata = new Automata(startState);

        public Tokens()
        {
            InitializeComponent();
        }

        private void Tokens_Load(object sender, EventArgs e)
        {
            #region Fields
            string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string punctuations = "{}[]();,.";
            string numbers = "1234567890";
            string delimiters = " " + "   ";
            char[] quotation = { '\"' };
            string fileEndChar = "$";
            string otherChars = "@#%?";
            string operators = "+\\*-=";
            string numbersOtherChars = alphabet + punctuations + delimiters + fileEndChar + otherChars + operators + endLine;

            StringBuilder sb = new StringBuilder(alphabet + punctuations + numbers + delimiters + fileEndChar + otherChars + endLine + new string(quotation));
            StringBuilder sbCmt = new StringBuilder(alphabet + punctuations + numbers + delimiters + fileEndChar + otherChars + new string(quotation));
            StringBuilder sb2 = new StringBuilder(alphabet + punctuations + numbers + delimiters + fileEndChar + otherChars + endLine);

            StringBuilder sb3 = new StringBuilder(delimiters + punctuations + fileEndChar + otherChars + operators + endLine);
            StringBuilder sb4 = new StringBuilder(alphabet + punctuations + delimiters + fileEndChar + otherChars + new string(quotation));


            #endregion

            #region States



            State incState = new State("IncState");
            State decState = new State("DecState");

            State addState = new State("AddState");
            State subState = new State("SubState");
            State mulState = new State("MulState");
            State divState = new State("DivState");

            State idState1 = new State("IdentifierState1");

            State assignState = new State("AssignState");
            State addAssignState = new State("AddAssignState");
            State subAssignState = new State("SubAssignState");
            State mulAssignState = new State("MulAssignState");
            State divAssignState = new State("DivAssignState");

            State eqState = new State("EqState");
            State neState = new State("NeState");
            State ltState = new State("LtState");
            State gtState = new State("GtState");
            State leState = new State("LeState");
            State geState = new State("GeState");

            State notState = new State("NotState");
            State andState = new State("AndState");
            State orState = new State("OrState");

            State intState = new State("IntState");
            State realState1 = new State("RealState1");
            State realState2 = new State("RealState2");
            State sciState1 = new State("SCIState1");
            State sciState2 = new State("SCIState2");
            State sciState3 = new State("SCIState3");


            State punctuationState = new State("PunctuationState");

            State commentState1 = new State("CommentState1");
            State commentState2 = new State("CommentState2");
            State commentState2Ending = new State("CommentState2Ending");


            State strState = new State("StrState");

            #endregion

            #region AddAutomataStates

            automata.addState(incState);
            automata.addState(decState);

            automata.addState(addState);
            automata.addState(subState);
            automata.addState(mulState);
            automata.addState(divState);
            
            automata.addState(idState1);
            
            automata.addState(assignState);
            automata.addState(addAssignState);
            automata.addState(subAssignState);
            automata.addState(mulAssignState);
            automata.addState(divAssignState);
            
            automata.addState(eqState);
            automata.addState(neState);
            automata.addState(ltState);
            automata.addState(gtState);
            automata.addState(leState);
            automata.addState(geState);
            
            automata.addState(notState);
            automata.addState(andState);
            automata.addState(orState);
            
            automata.addState(intState);
            automata.addState(realState1);
            automata.addState(realState2);
            automata.addState(sciState1);
            automata.addState(sciState2);
            automata.addState(sciState3);
            
            automata.addState(punctuationState);
            automata.addState(commentState1);
            automata.addState(commentState2);
            automata.addState(commentState2Ending);
            
            automata.addState(strState);

            #endregion

            #region AddTransactions

            automata.addTransaction(startState, startState, endLine + delimiters, "WS");
            
            automata.addTransaction(startState, addState, "+", "Nothing");
            automata.addTransaction(addState, startState, sb4.ToString() + endLine, "ADD");
            automata.addTransaction(addState, incState, "+", "Nothing");
            automata.addTransaction(incState, startState, sb.ToString(), "INC");
            automata.addTransaction(addState, addAssignState, "=", "Nothing");
            automata.addTransaction(addAssignState, startState, sb.ToString(), "ADD_Assign");
            
            automata.addTransaction(startState, subState, "-", "Nothing");
            automata.addTransaction(subState, startState, sb4.ToString() + endLine, "SUB");
            automata.addTransaction(subState, decState, "-", "Nothing");
            automata.addTransaction(decState, startState, sb.ToString(), "DEC");
            automata.addTransaction(subState, subAssignState, "=", "Nothing");
            automata.addTransaction(subAssignState, startState, sb.ToString(), "SUB_Assign");
            
            automata.addTransaction(startState, mulState, "*", "Nothing");
            automata.addTransaction(mulState, startState, sb.ToString(), "MUL");
            automata.addTransaction(mulState, mulAssignState, "=", "Nothing");
            automata.addTransaction(mulAssignState, startState, sb.ToString(), "MUL_Assign");
            
            automata.addTransaction(startState, divState, "/", "Nothing");
            automata.addTransaction(divState, startState, sb.ToString(), "DIV");
            automata.addTransaction(divState, divAssignState, "=", "Nothing");
            automata.addTransaction(divAssignState, startState, sb.ToString(), "DIV_Assign");
            
            automata.addTransaction(startState, assignState, "=", "Nothing");
            automata.addTransaction(assignState, startState, sb.ToString() + operators, "Assign");
            automata.addTransaction(assignState, eqState, "=", "Nothing");
            automata.addTransaction(eqState, startState, sb.ToString(), "Equal");
            
            automata.addTransaction(startState, notState, "!", "Nothing");
            automata.addTransaction(notState, startState, sb.ToString(), "Not");
            automata.addTransaction(notState, neState, "=", "Nothing");
            automata.addTransaction(neState, startState, sb.ToString(), "NotEqual");
            
            automata.addTransaction(startState, ltState, "<", "Nothing");
            automata.addTransaction(ltState, startState, "=", "LE");
            automata.addTransaction(ltState, startState, sb.ToString(), "LT");
            
            automata.addTransaction(startState, gtState, ">", "Nothing");
            automata.addTransaction(gtState, startState, "=", "GE");
            automata.addTransaction(gtState, startState, sb.ToString(), "GT");
            
            automata.addTransaction(startState, startState, "&", "AND");
            
            automata.addTransaction(startState, startState, "|", "OR");
            
            automata.addTransaction(startState, intState, numbers.Substring(0, numbers.Length - 1), "Nothing");
            automata.addTransaction(addState, intState, numbers.Substring(0, numbers.Length - 1), "Nothing");
            automata.addTransaction(subState, intState, numbers.Substring(0, numbers.Length - 1), "Nothing");
            automata.addTransaction(intState, intState, numbers, "Nothing");
            automata.addTransaction(intState, startState, numbersOtherChars, "INT");
            automata.addTransaction(intState, realState1, ".", "Nothing");
            automata.addTransaction(realState1, realState2, numbers, "Nothing");
            automata.addTransaction(realState2, realState2, numbers, "Nothing");
            automata.addTransaction(realState2, startState, numbersOtherChars, "REAL");
            automata.addTransaction(realState2, sciState1, "eE", "Nothing");
            automata.addTransaction(intState, sciState1, "eE", "Nothing");
            automata.addTransaction(sciState1, sciState2, "+-", "Nothing");
            automata.addTransaction(sciState1, sciState3, numbers, "Nothing");
            automata.addTransaction(sciState2, sciState3, numbers, "Nothing");
            automata.addTransaction(sciState3, sciState3, numbers, "Nothing");
            automata.addTransaction(sciState3, startState, numbersOtherChars, "SCI");
            
            automata.addTransaction(divState, commentState1, "/", "Nothing");
            automata.addTransaction(commentState1, commentState1, sbCmt.ToString(), "Nothing");
            automata.addTransaction(commentState1, startState, endLine, "OneLineComment");
            
            automata.addTransaction(divState, commentState2, "*", "Nothing");
            automata.addTransaction(commentState2, commentState2, sb.ToString() + "+\\-", "Nothing");
            automata.addTransaction(commentState2, commentState2Ending, "*", "Nothing");
            automata.addTransaction(commentState2Ending, commentState2, sb.ToString() + "+\\-", "Nothing");
            automata.addTransaction(commentState2Ending, startState, "/", "MultilineComment");
            
            automata.addTransaction(startState, startState, punctuations, "PUN");
            
            automata.addTransaction(startState, strState, new string(quotation), "Nothing");
            automata.addTransaction(strState, strState, sb2.ToString(), "Nothing");
            automata.addTransaction(strState, startState, new string(quotation), "STR");
           
            automata.addTransaction(startState, idState1, alphabet + "_", "Nothing");
            automata.addTransaction(idState1, idState1, alphabet + "_" + numbers, "Nothing");
            automata.addTransaction(idState1, startState, sb3.ToString(), "Identifier");

            #endregion


        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            StringBuilder LineTokens = new StringBuilder();
            string path = txtPath.Text;
            bool getch = true;
            if (!String.IsNullOrEmpty(path))
            {
                StreamReader streamReader = new StreamReader(path);
                while (streamReader.Peek() > -1)
                {
                    string line = streamReader.ReadLine() + endLine;

                    for (int i = 0; i < line.Length; i++)
                    {
                        input = line[i];
                        if (input != '$')
                        {
                            Tuple<string, string, string> tuple = automata.Next(input);
                            if (tuple.Item1 == "" && tuple.Item2 == "" && tuple.Item3 == "")
                            {
                                continue;
                            }
                            LineTokens.AppendLine(tuple.Item1);
                            LineTokens.AppendLine(tuple.Item2);
                            LineTokens.AppendLine(tuple.Item3);
                            if (tuple.Item3 == "ungetch")
                            {
                                getch = false;
                                i--;
                            }
                            else if (tuple.Item3 == "")
                                getch = true;
                        }
                    }
                }
            }
            else
                MessageBox.Show("Path is Empty!");
            LineTokens.AppendLine(" ");
            LineTokens.AppendLine("------Symbol Table-----");
            LineTokens.AppendLine(" ");
            foreach (var item in automata.symbolTable)
                LineTokens.AppendLine($"{item.Key} , {item.Value}");
            LineTokens.AppendLine("End Of File!");
            txtTokens.Text = LineTokens.ToString();
        }
    }
}
