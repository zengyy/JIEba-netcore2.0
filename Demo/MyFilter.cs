using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Util;
using System.Collections.Generic;
using System.Linq;

namespace Demo
{
    class MyFilter : Filter
    {
        private readonly IEnumerable<FilterItem> _filterItems;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterItems">过滤条件</param>
        public MyFilter(IEnumerable<FilterItem> filterItems)
        {
            _filterItems = filterItems;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="acceptDocs"></param>
        /// <returns></returns>
        public override DocIdSet GetDocIdSet(AtomicReaderContext context, IBits acceptDocs)
        {
            var indexReader = context.Reader;
            var bitSet = new OpenBitSet(indexReader.MaxDoc);
            bitSet.Set(0, indexReader.MaxDoc);

            // 带过滤条件的词条列表
            var filterTermsEnum = _filterItems.GroupBy(m => m.Field).Select((m) => new MyFilterTermsEnum(context.AtomicReader.GetTerms(m.Key), m));

            foreach (var termsEnum in filterTermsEnum)
            {
                var tempBitSet = new OpenBitSet();
                // 遍历词条
                while (termsEnum.Next() != null)
                {
                    // 遍历词条对应的文档
                    var docsEnum = termsEnum.Docs(acceptDocs, null, DocsFlags.NONE);
                    while (docsEnum.NextDoc() != DocIdSetIterator.NO_MORE_DOCS)
                    {
                        tempBitSet.Set(docsEnum.DocID);
                    }
                }

                //2.2.2 将每个field匹配的文档合并到一起,And的关系
                bitSet.And(tempBitSet);
            }

            return bitSet;
        }
    }
}