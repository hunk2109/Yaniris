using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace sistgre
{
    public partial class Reportes : Form
    {
        
        public Reportes()
        {
            InitializeComponent();
        }
        cnxsql cns = new cnxsql();


        private void Button1_Click(object sender, EventArgs e)
        {
            if (rbefec.Checked == true)
            {

                DataSet ds = new DataSet();

                DataTable dt = cns.cosnsultaconresultado("select ven_id_fac as Codigo, produc as Producto,precio as Precio, cant as Cantidad, (precio * cant) as Total,fecha from ventas   join inventario on id_cod = inventario_id_cod   INNER JOIN factura on id_fact = ven_id_fac where  fec_c >= '" + dateTimePicker1.Text + "' and  fec_c <='" + dateTimePicker2.Text + "' and tipo_vent = 1");

                ds.Tables.Add(dt);
                double sum = 0;
                ds.WriteXml(@"C:\bdd\reporte.xml");
                dgvrepor.DataSource = dt;
                for (int i = 0; i < dgvrepor.Rows.Count; ++i)
                {
                    sum += Convert.ToDouble(dgvrepor.Rows[i].Cells[4].Value);
                }
                lbto.Text = sum.ToString();
            }

            else if(rbcred.Checked == true)
            {
                DataSet ds = new DataSet();

                DataTable dt = cns.cosnsultaconresultado("select ven_id_fac as Codigo, produc as Producto,precio as Precio, cant as Cantidad, (precio * cant) as Total,Nombre, apell,fecha from ventas   join inventario on id_cod = inventario_id_cod   INNER JOIN factura on id_fact = ven_id_fac left join Cliente on id_client =Cliente_id_client where  fec_c >= '" + dateTimePicker1.Text + "' and  fec_c <='" + dateTimePicker2.Text + "' and tipo_vent = 2");

                ds.Tables.Add(dt);
                double sum = 0;
                ds.WriteXml(@"C:\bdd\reporte.xml");
                dgvrepor.DataSource = dt;
                for (int i = 0; i < dgvrepor.Rows.Count; ++i)
                {
                    sum += Convert.ToDouble(dgvrepor.Rows[i].Cells[4].Value);
                }
                lbto.Text = sum.ToString();
            }

            else
            {
                MessageBox.Show("Seleccione un tipo de entrada");
            }

           


        }

        private void Reportes_Load(object sender, EventArgs e)
        {
            
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            
            Reportesf f = new Reportesf();
            CrystalReport2 cr = new CrystalReport2();
            if (rbefec.Checked == true)
            {
                string enc;
                enc = "Reporte de entradas en efectivo hasta la Fecha";
                TextObject text1 = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["Text9"];
                text1.Text = enc;
            }
            else if (rbcred.Checked)
            {
                string enc;
                enc = "Reporte de entradas a Credito hasta la Fecha";
                TextObject text1 = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["Text9"];
                text1.Text = enc;
            }
            
            TextObject text = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["Text8"];
            text.Text = lbto.Text;

            f.crystalReportViewer1.ReportSource = cr;
            f.Show();
        }
    }
}
