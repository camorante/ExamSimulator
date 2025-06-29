using ExamGenerator.Models;
using ExamSimulator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExamSimulator.Controls
{
    /// <summary>
    /// Lógica de interacción para OpinionList.xaml
    /// </summary>
    public partial class OpinionList : UserControl
    {
        public static readonly DependencyProperty question =
            DependencyProperty.Register("Question", typeof(Question),
            typeof(OpinionList),
                new FrameworkPropertyMetadata(
                    default(Question),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnUriChanged)
                )
            );

        private static void OnUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((OpinionList)d).SetOpinions();
        }

        public OpinionList()
        {
            InitializeComponent();
        }

        public Question Question
        {
            get { return (Question)GetValue(question); }
            set
            {
                SetValue(question, value);
                
            }
        }

        public void SetOpinions()
        {
            if (this.Question != null && this.Question.Opinions != null)
            {
                this.stkOpinionList.Children.Clear();
                foreach (var opinion in this.Question.Opinions)
                {
                    var opinionItem = new OpinionItem();
                    opinionItem.SetOpinion(opinion);
                    this.stkOpinionList.Children.Add(opinionItem);
                }
            }
            else
            {
                this.stkOpinionList.Children.Clear();
            }
        }
    }
}
