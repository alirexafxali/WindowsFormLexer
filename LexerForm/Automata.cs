using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerForm
{
    class Automata
    {
        public State currentState { get; set; }
        public string tokenAhead { get; set; }
        public Dictionary<string, State> allStates { get; set; }
        public Dictionary<string, string> symbolTable { get; set; }
        public List<string> keyWords { get; set; }
        private string Action { get; set; }
        string buffer { get; set; }
        public Automata(State startState)
        {
            currentState = startState;
            buffer = "";
            tokenAhead = "";
            allStates = new Dictionary<string, State>();
            symbolTable = new Dictionary<string, string>();
            allStates.Add(startState.stateName, startState);
            keyWords = new List<string> {"auto","break","case","char","const","continue","default","do","double","else","enum",
                "extern","float","for","goto","if","int","long","register","return","short","signed","sizeof","static","struct"
                ,"switch","typedef","union","unsigned","void","volatil","while"};

        }
        public Tuple<string, string, string> Next(char input)
        {
            string result = "";
            string isNewKeyWord = "";
            string ungetch = "";
            if (input != '$')
            {
                foreach (var item in allStates[currentState.stateName].allTranslations)
                {

                    if (item.isValid(input))
                    {
                        result = "";
                        isNewKeyWord = "";
                        ungetch = "";
                        currentState = item.nextState;
                        Action = item.Action;
                        switch (Action)
                        {
                            case "Nothing":
                                {
                                    result = "Move";
                                    tokenAhead += input;
                                    break;
                                }
                            case "Identifier":
                                {
                                    if (keyWords.Contains(tokenAhead))
                                    {

                                        ungetch = "ungetch";
                                        result = $"<{tokenAhead} ,  KEYWORD>";

                                    }
                                    else
                                    {
                                        if (symbolTable.ContainsKey(tokenAhead))
                                        {
                                            isNewKeyWord = "false";
                                            ungetch = "ungetch";
                                            result = $"<{tokenAhead} ,  {Action}>";
                                        }
                                        else
                                        {
                                            symbolTable.Add(tokenAhead, "Identifier");
                                            isNewKeyWord = "true";
                                        }
                                    }
                                    tokenAhead = "";
                                    break;
                                }
                            case "WS":
                                result = "Move";
                                ungetch = "";
                                break;
                            case "Add":
                            case "INC":
                            case "ADD_Assign":
                            case "SUB":
                            case "DEC":
                            case "SUB_Assign":
                            case "MUL":
                            case "MUL_assign":
                            case "DIV":
                            case "DIV_Assign":
                            case "Assign":
                            case "Equal":
                            case "Not":
                            case "NotEqual":
                            case "LT":
                            case "GT":
                            case "AND":
                            case "OR":
                            case "INT":
                            case "REAL":
                            case "SCI":
                                {
                                    ungetch = "ungetch";
                                    result = $"<{tokenAhead} ,  {Action}>";
                                    tokenAhead = "";
                                    break;
                                }
                            case "LE":
                            case "GE":
                                {
                                    ungetch = "ungetch";
                                    result = $"<{tokenAhead} ,  {Action}>";
                                    tokenAhead = "";
                                    break;
                                }
                            case "OneLineComment":
                                {
                                    result = $"<{tokenAhead} ,  {Action}>";
                                    tokenAhead = "";
                                    break;
                                }
                            case "PUN":
                                {
                                    ungetch = "";
                                    result = $"<{input} ,  {Action}>";
                                    tokenAhead = "";
                                    break;
                                }
                            case "MultilineComment":
                            case "STR":
                                {
                                    ungetch = "";
                                    tokenAhead += input;
                                    result = $"<{tokenAhead} ,  {Action}>";
                                    tokenAhead = "";
                                    break;
                                }
                            default:
                                {
                                    result = "ERROR";
                                    tokenAhead = "";
                                    break;
                                }
                        }
                    }
                }
            }
            if (result == "")
                return new Tuple<string, string, string>($"<{tokenAhead} , ERROR>", isNewKeyWord, ungetch);
            if (result == "Move")
                return new Tuple<string, string, string>("", "", "");
            return new Tuple<string, string, string>(result, isNewKeyWord, ungetch);
        }
        public void addState(State insertState)
        {
            allStates.Add(insertState.stateName, insertState);
        }
        public void addTransaction(State sourceState, State destinationState, string value, string Action)
        {
            Translation translation = new Translation(value);
            translation.currentState = allStates[sourceState.stateName];
            translation.nextState = allStates[destinationState.stateName];
            translation.Action = Action;
            allStates[sourceState.stateName].allTranslations.Add(translation);
        }
    }
}



