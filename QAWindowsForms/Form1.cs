using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using PanGu;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QAWindowsForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            search();
        }

        public static string GetKeyWordsSplitBySpace(string keywords, PanGuTokenizer ktTokenizer)
        {
            StringBuilder result = new StringBuilder();
            ICollection<WordInfo> words = ktTokenizer.SegmentToWordInfos(keywords);
            foreach (WordInfo word in words)
            {
                if (word == null)
                {
                    continue;
                }
                result.AppendFormat("{0}^{1}.0 ", word.Word, (int)Math.Pow(3, word.Rank));
            }
            return result.ToString().Trim();
        }

        private void txtQuestion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                search();
            }
        }

        private void search()
        {
            //Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29); //标准
            Analyzer analyzer = new PanGuAnalyzer();
            Term term;

            string content = txtQuestion.Text.Trim();
            if (!string.IsNullOrEmpty(txtQuestion.Text.Trim()))
            {
                QueryParser queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "question", analyzer);

                string panguQueryword = GetKeyWordsSplitBySpace(content, new PanGuTokenizer());//对关键字进行分词处理
                Query query = queryParser.Parse(panguQueryword);

                string indexPath = ConfigurationManager.AppSettings["LuceneIndexPath"];
                Lucene.Net.Store.Directory directory = FSDirectory.Open(indexPath);
                IndexSearcher search = new IndexSearcher(directory, true);

                Sort sort = new Sort();
                TopDocs topDocs = search.Search(query, (Filter)null, 100);
                int count = topDocs.TotalHits;
                lbAnswer.Items.Clear();
                for (int i = 0; i < count; i++)
                {
                    Document document = search.Doc(topDocs.ScoreDocs[i].Doc);
                    lbAnswer.Items.Add(String.Format("问题{0}：{1}", i + 1, document.Get("question")));
                    lbAnswer.Items.Add(String.Format("答案：{0}", document.Get("answer")));
                }

                search.Dispose();
                analyzer.Close();
            }
        }
    }
}
