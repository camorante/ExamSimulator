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
    /// Lógica de interacción para OpinionItem.xaml
    /// </summary>
    public partial class OpinionItem : UserControl
    {
        public OpinionItem()
        {
            InitializeComponent();
        }

        public void SetOpinion(Opinion opinion)
        {
            if (opinion != null)
            {
                this.txtUsername.Text = opinion.Username;
                this.txtScore.Text = opinion.Score;
                this.txtCreated.Text = opinion.CreatedOn;
                this.txtOpinion.Text = opinion.OpinionText;
            }
            else
            {
                this.txtUsername.Text = "";
                this.txtScore.Text = "";
                this.txtCreated.Text = "";
                this.txtOpinion.Text = "";
            }
        }
    }
}
