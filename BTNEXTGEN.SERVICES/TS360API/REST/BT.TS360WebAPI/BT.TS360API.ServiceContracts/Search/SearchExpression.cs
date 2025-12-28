using System;
using System.Collections.Generic;
using BT.TS360Constants;

namespace BT.TS360API.ServiceContracts.Search
{
    public class SearchExpression
    {
        //protected int _count;
        private string _terms;
        private string _displayName;

        public string Operator { get; set; }
        public string ComparisionOperator { get; set; }

        public string Scope { get; set; }

        //public int Count { get { return _count; } }

        public string Terms
        {
            get { return _terms; }
            set
            {
                _terms = value;
                //FormatKeyword();
            }
        }

        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                _displayName = value;
                //FormatKeyword();
            }
        }

        public SearchExpression()
            : this(string.Empty, string.Empty)
        {
        }

        public SearchExpression(string scope, string terms)
            : this(scope, terms, BooleanOperatorConstants.And)
        {
        }

        public SearchExpression(string scope, string terms, string op)
        {
            this.Scope = scope;
            this.Terms = terms;
            this.Operator = op;
            //_count = 1;
        }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(this.Terms) && !string.IsNullOrEmpty(this.Scope) && this.Scope != "-1";
            }
        }

        //public void FormatKeyword()
        //{
        //    //isbn with dashed #5534
        //    if (!string.IsNullOrEmpty(_displayName) && _displayName.Equals("Keywords") && CommonHelper.IsISBN(_terms))
        //    {
        //        _terms = _terms.Replace("-", string.Empty);//be carefull of infinite loop
        //    }
        //}
        public bool RemoveSearchExpression(HashSet<string> removeSet)
        {
            if (removeSet == null || removeSet.Count == 0) return false;

            if (this is SearchExpressionGroup)
            {
                var group = (SearchExpressionGroup)this;
                for (int index = group.SearchExpressions.Count - 1; index >= 0; index--)
                {
                    var searchExpression = group.SearchExpressions[index];
                    if (searchExpression.RemoveSearchExpression(removeSet))
                    {
                        group.SearchExpressions.RemoveAt(index);
                        //group._count--;
                    }
                }
                return group.SearchExpressions.Count == 0;
            }
            return removeSet.Contains(Scope);
        }
        public bool ContainSearchExpression(string text)
        {
            return FindSearchExpression(text) != null;
        }
        public SearchExpression FindSearchExpression(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            if (this is SearchExpressionGroup)
            {
                var group = (SearchExpressionGroup)this;
                SearchExpression found = null;
                foreach (var exp in group.SearchExpressions)
                {
                    found = exp.FindSearchExpression(text);
                    if (found != null) break;
                }
                return found;
            }
            if (string.Compare(Scope, text, StringComparison.OrdinalIgnoreCase) == 0)
                return this;
            return null;
        }
        public bool ContainScopeTerms(string text, IList<string> textTerms)
        {
            return FindScopeTerms(text, textTerms) != null;
        }
        public SearchExpression FindScopeTerms(string text, IList<string> textTerms)
        {
            if (string.IsNullOrEmpty(text)) return null;

            if (this is SearchExpressionGroup)
            {
                var group = (SearchExpressionGroup)this;
                SearchExpression found = null;
                foreach (var exp in group.SearchExpressions)
                {
                    found = exp.FindSearchExpression(text);
                    if (found != null) break;
                }
                return found;
            }
            if (string.Compare(Scope, text, StringComparison.OrdinalIgnoreCase) == 0)
            {
                var term = this.Terms.ToLower();
                foreach (var textTerm in textTerms)
                {
                    if (term.Contains(textTerm))
                        return this;
                }
            }
            return null;
        }
        public SearchExpression Remove(SearchExpression expArg)
        {
            if (string.IsNullOrEmpty(expArg.Operator)
                || string.IsNullOrEmpty(expArg.Scope)
                || string.IsNullOrEmpty(expArg.Terms)) return null;

            if (this is SearchExpressionGroup)
            {
                var group = (SearchExpressionGroup)this;
                SearchExpression found = null;
                foreach (var exp in group.SearchExpressions)
                {
                    found = exp.Remove(expArg);
                    if (found != null)
                    {
                        group.SearchExpressions.Remove(found);
                        break;
                    }
                }
                return null;
            }
            if (string.Compare(Operator, expArg.Operator, StringComparison.OrdinalIgnoreCase) == 0
                && string.Compare(Scope, expArg.Scope, StringComparison.OrdinalIgnoreCase) == 0
                && string.Compare(Terms, expArg.Terms, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return this;
            }
            return null;
        }
    }

    public class RangeExpression : SearchExpression
    {
        public int Min { get; set; }
        public int Max { get; set; }
    }
    //public class BoundaryExpression : SearchExpression
    //{
    //    //public string Mode { get; set; }
    //}
    public class SearchExpressionGroup : SearchExpression
    {
        public string OperatorGroup { get; set; }
        public List<SearchExpression> SearchExpressions { get; set; }
        /// <summary>
        /// Default: BooleanOperatorConstants.And
        /// </summary>
        public SearchExpressionGroup()
            : this(BooleanOperatorConstants.And)
        {
        }

        public SearchExpressionGroup(string op)
        {
            this.OperatorGroup = string.IsNullOrEmpty(op) ? BooleanOperatorConstants.And : op;
            this.SearchExpressions = new List<SearchExpression>();
        }

        public void AddSearchExpress(SearchExpression exp)
        {
            if (exp == null) return;

            if (exp is SearchExpressionGroup)
            {
                var group = (SearchExpressionGroup)exp;
                if (string.Compare(group.OperatorGroup, OperatorGroup, StringComparison.OrdinalIgnoreCase) == 0 &&
                    string.Compare(group.OperatorGroup, BooleanOperatorConstants.Not, StringComparison.OrdinalIgnoreCase) != 0)
                    SearchExpressions.AddRange(group.SearchExpressions);
                else
                {
                    SearchExpressions.Add(group);
                }
                //this._count += exp.Count;
            }
            else
            {
                if (string.Compare(exp.Operator, OperatorGroup, StringComparison.OrdinalIgnoreCase) == 0 ||
                    string.Compare(exp.Operator, BooleanOperatorConstants.Not, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    SearchExpressions.Add(exp);
                }
                else
                {
                    var temp = this.OperatorGroup;
                    this.OperatorGroup = exp.Operator;
                    var group = new SearchExpressionGroup(temp);
                    group.SearchExpressions.AddRange(this.SearchExpressions);
                    //group._count = this._count;

                    this.SearchExpressions.Clear();
                    this.SearchExpressions.Add(group);
                    this.SearchExpressions.Add(exp);
                }
                //this._count++;
            }
        }

        public void AddSearchExpress2(SearchExpression exp)
        {
            if (exp == null) return;

            if (exp is SearchExpressionGroup)
            {
                var group = (SearchExpressionGroup)exp;
                if (string.Compare(group.OperatorGroup, OperatorGroup, StringComparison.OrdinalIgnoreCase) == 0 &&
                    string.Compare(group.OperatorGroup, BooleanOperatorConstants.Not, StringComparison.OrdinalIgnoreCase) != 0)
                    SearchExpressions.AddRange(group.SearchExpressions);
                else
                {
                    SearchExpressions.Add(group);
                }
            }
            else
            {
                SearchExpressions.Add(exp);
            }
        }

        public void AddSearchExpress(IList<SearchExpression> exps)
        {
            foreach (var searchExpression in exps)
            {
                AddSearchExpress(searchExpression);
            }
        }
    }
}
