using System;
using System.Collections.Generic;
using Demo;
using Lucene.Net.Index;
using Lucene.Net.Util;

namespace HuasSoft.SE.Core.Search
{
    /// <summary>
    /// 词条列表
    /// 可根据field和termMatch遍历匹配的词条
    /// 初始时定位到第一个匹配的词条,之后调用Next()定位的下一个匹配的词条
    /// </summary>
    class MyTerms : Terms
    {
        public override IComparer<BytesRef> Comparer => throw new NotImplementedException();

        public override long Count => throw new NotImplementedException();

        public override long SumTotalTermFreq => throw new NotImplementedException();

        public override long SumDocFreq => throw new NotImplementedException();

        public override int DocCount => throw new NotImplementedException();

        public override bool HasFreqs => throw new NotImplementedException();

        public override bool HasOffsets => throw new NotImplementedException();

        public override bool HasPositions => throw new NotImplementedException();

        public override bool HasPayloads => throw new NotImplementedException();

        public override TermsEnum GetIterator(TermsEnum reuse)
        { 
            throw new NotImplementedException();
        }
    }
}