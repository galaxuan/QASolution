using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace LuceneImportTool
{
    public class Active
    {
        public void ExcuteDataTable()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            DateTime startT = DateTime.Now;

            DataTable query = new BL().GetProductTable();

            string indexPath = ConfigurationManager.AppSettings["LuceneIndexPath"];
            Analyzer analyzer = analyzer = new PanGuAnalyzer(); //盘古Analyzer    // analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29); //标准

            if (!System.IO.Directory.Exists(indexPath))
            {
                System.IO.Directory.CreateDirectory(indexPath);
            }
            else
            {
                System.IO.Directory.Delete(indexPath, true);
                System.IO.Directory.CreateDirectory(indexPath);
            }
            Lucene.Net.Store.Directory ramDir = FSDirectory.Open(new DirectoryInfo(indexPath));
            IndexWriter writer = new IndexWriter(ramDir, analyzer, true, IndexWriter.MaxFieldLength.LIMITED);
            writer.SetMaxBufferedDocs(301); //设置最大缓冲文件
            writer.MergeFactor = 100;
            //writer.SetRAMBufferSizeMB(1000);
            //writer.MaxMergeDocs = 100;

            Document document; //文件
            for (int i = 0; i < query.Rows.Count; i++)
            {
                string str = String.Format("ID:{0}  |  question:{1}  |  answer:{2}", query.Rows[i]["ID"], query.Rows[i]["question"], query.Rows[i]["answer"]);

                Console.WriteLine(str);
                document = GetDocument(query.Rows[i]);
                Console.WriteLine("插入第{0}条数据:{1}", i, query.Rows[i]["question"]);
                writer.AddDocument(document);
            }

            writer.Optimize(); //优化
            writer.Dispose();
            watch.Stop();
            analyzer.Close();
            TimeSpan s = DateTime.Now - startT;
            Console.WriteLine("完成，共插入{0}行数据,共耗时{1}秒", query.Rows.Count, s.TotalSeconds);
        }

        private Document GetDocument(DataRow dr)
        {
            #region 说明

            //Field.Store.YES; 存储到索引文件
            //Field.Store.NO;  不存储
            //Field.Store.COMPRESS //压缩后存储到索引文件 把ICSharpCode.SharpZipLib.dll引入项目

            //Field.Index.NO;                         //根本不索引，所以不会被检索到
            //Field.Index.ANALYZED;                   //分词后索引
            //Field.Index.NOT_ANALYZED;               //不分词直接索引，例如URL、系统路径等，用于精确检索。
            //Field.Index.NOT_ANALYZED_NO_NORMS;      //类似Index.NOT_ANALYZED，但不存储NORM TERMS，节约内存但不支持Boost，非常常用 。
            //Field.Index.ANALYZED_NO_NORMS;          //类似Index.ANALYZED，但不存储NORM TERMS，节约内存但不支持Boost。

            #endregion 说明

            Document document = new Document();
            document.Add(new Field("ID", dr["ID"].ToString(), Field.Store.YES, Field.Index.ANALYZED_NO_NORMS));
            document.Add(new Field("question", dr["question"].ToString(), Field.Store.YES, Field.Index.ANALYZED_NO_NORMS));
            document.Add(new Field("answer", dr["answer"].ToString(), Field.Store.YES, Field.Index.ANALYZED_NO_NORMS));

            return document;
        }
    }
}