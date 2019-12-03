using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Index;
using Lucene.Net.Util;

namespace Demo
{
    /// <summary>
    /// 词条列表
    /// 可根据field和termMatch遍历匹配的词条
    /// 初始时定位到第一个匹配的词条,之后调用Next()定位的下一个匹配的词条
    /// </summary>
    class MyFilterTermsEnum : TermsEnum
    {
        private readonly IEnumerable<FilterItem> _filterItems;
        private readonly TermsEnum _actualTermsEnum;
        private BytesRef _currentTerm;

        /// <summary>
        /// 对词条列表(Terms)过滤,生成TermsEnum
        /// </summary>
        /// <param name="terms">要过滤的词条列表</param>
        /// <param name="filterItems">过滤条件</param>
        public MyFilterTermsEnum(Terms terms, IEnumerable<FilterItem> filterItems)
        { 
            _filterItems = filterItems;
            if (terms != null)
            {
                _actualTermsEnum = terms.GetIterator(null);
            }

            // 定位到第一个词条
            //  Next();
        }

        /// <summary>
        /// 跳转到下一词条,如果已跳转到末尾则返回null
        /// </summary>
        /// <returns></returns>
        public override BytesRef Next()
        {
            if (_actualTermsEnum == null)
            {
                return null;
            }

            while (_actualTermsEnum.Next() != null)
            {
                if (MatchTerm(_actualTermsEnum.Term))
                {
                    _currentTerm = _actualTermsEnum.Term;
                    return _currentTerm;
                }
            }

            return null;
        }
        /// <summary>
        /// 当前词条
        /// </summary>
        public override BytesRef Term => _currentTerm;
        /// <summary>
        /// 获取当前词条对应的文档列表
        /// </summary>
        /// <param name="liveDocs"></param>
        /// <param name="reuse"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public override DocsEnum Docs(IBits liveDocs, DocsEnum reuse, DocsFlags flags)
        {
            return _actualTermsEnum.Docs(liveDocs, reuse, flags);
        }

        /// <summary>
        /// 词条匹配
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        private bool MatchTerm(BytesRef term)
        {
            var value = term.Utf8ToString(); //以utf8转成字符串

            return _filterItems.All(m =>
                {
                    string itemValue = m.Value == null ? "" : m.Value.ToString();
                    switch (m.Oper.ToLower())
                    {
                        case "=":
                            return string.Equals(value, itemValue, StringComparison.OrdinalIgnoreCase);
                        case "==":
                            return string.Equals(value, itemValue);
                        case "like":
                            return value.IndexOf(itemValue, StringComparison.OrdinalIgnoreCase) > -1;
                        case ">":
                            return String.CompareOrdinal(value, itemValue) > 0;
                        case ">=":
                            return String.CompareOrdinal(value, itemValue) >= 0;
                        case "<":
                            return String.CompareOrdinal(value, itemValue) < 0;
                        case "<=":
                            return String.CompareOrdinal(value, itemValue) <= 0;
                        default:
                            return true;
                    }
                });
        }

        #region not implement

        public override IComparer<BytesRef> Comparer => throw new NotImplementedException();

        public override long Ord => throw new NotImplementedException();

        public override int DocFreq => throw new NotImplementedException();

        public override long TotalTermFreq => throw new NotImplementedException();

        public override DocsAndPositionsEnum DocsAndPositions(IBits liveDocs, DocsAndPositionsEnum reuse, DocsAndPositionsFlags flags)
        {
            throw new NotImplementedException();
        }

        public override SeekStatus SeekCeil(BytesRef text)
        {
            throw new NotImplementedException();
        }

        public override void SeekExact(long ord)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}