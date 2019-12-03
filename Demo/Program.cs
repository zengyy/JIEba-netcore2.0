using JiebaNet.Segmenter;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System;
using System.Linq;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //CutWord();
            Query();
            Console.WriteLine("finish!");
        }

        public static void CutWord()
        {
            var segmenter = new JiebaSegmenter();
            var segments = segmenter.Cut("我来到北京清华大学", cutAll: true);

            Console.WriteLine(string.Join(",", segments));
        }

        public static void Query()
        {
            string word = "办公室";
            string[] words = new JiebaSegmenter().CutForSearch(word).ToArray();
            word = string.Join(" OR ", words);
            //1. 创建analyzer
            var analyzer = new jieba.NET.JieBaAnalyzer(TokenizerMode.Default);

            //2. 创建query
            // var query =  MultiFieldQueryParser.Parse(LuceneVersion.LUCENE_48, word, new string[] { "单位名称" }, new[] { Occur.SHOULD }, analyzer);
            var query = new MatchAllDocsQuery();

            //3.搜索
            using (var reader = DirectoryReader.Open(FSDirectory.Open(@"E:\Index\unit")))
            {
                var indexSearcher = new IndexSearcher(reader);
                var topDocs = indexSearcher.Search(query, new MyFilter(new[] {
                    new FilterItem() { Field = "areaCode", Oper = "=", Value = "GX" },
                    //new FilterItem() { Field = "单位代码", Oper = "=", Value = "101001" },
                    new FilterItem() { Field = "单位名称", Oper = "like", Value = "研究" }
                }), 100);
                Console.WriteLine("搜索结果:" + topDocs.TotalHits);
            }
        }
    }
}