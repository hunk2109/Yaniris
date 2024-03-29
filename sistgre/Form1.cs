﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Drawing.Printing;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Drawing.Drawing2D;
using Microsoft.VisualBasic;

namespace sistgre
{
    public partial class Form1 : Form
    {


        SQLiteConnection cn = new SQLiteConnection();
        cnxsql cns = new cnxsql();
        SQLiteCommand cmd = new SQLiteCommand();
        SQLiteDataReader dr;
        SQLiteParameter picture;
        string codicred;
        SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");

        public Form1()
        {
            InitializeComponent();
        }
        private void Actprddgv()       {


            try
            {

                for (int i = 0; i <= dgvcot.Rows.Count - 1; i++)
                {
                    SQLiteCommand cmd2 = new SQLiteCommand("update inventario set canti_disp = (canti_disp - @canti) where id_cod = @idinv", conn);
                    cmd2.Parameters.AddWithValue("@canti", dgvcot.Rows[i].Cells[3].Value);
                    cmd2.Parameters.AddWithValue("@idinv", dgvcot.Rows[i].Cells[0].Value);

                    conn.Open();
                    cmd2.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch
            {

            }


        }
        private void combo()
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
            {
                SQLiteCommand sqlCmd = new SQLiteCommand("SELECT * FROM Suplidor", conn);
                conn.Open();
                SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    cmbsup.Items.Add(sqlReader["nombre"].ToString());
                }

                sqlReader.Close();

            }
        }

        private void carga()
        {
            dgvsup.DataSource = cns.cosnsultaconresultado("select * from Suplidor");
            dgvinv.DataSource = cns.cosnsultaconresultado("select id_cod , produc,tipo_prod,precio , precio_c ,(precio-precio_c)as Beneficio ,canti_disp ,itbis as Itebis,suplidor_id_supli from inventario");
            dgvcli.DataSource = cns.cosnsultaconresultado("select * from cliente");
            dgvfact.DataSource = cns.cosnsultaconresultado("select ven_id_fac as Codigo, produc as Producto,precio as Precio, cant as Cantidad, (precio * cant) as Total  from ventas   join inventario on id_cod = inventario_id_cod ");
            dgvcp.DataSource = cns.cosnsultaconresultado("select id_cp as ID, monto_o as Monto, fecha as Fecha,mont_pag as Pagado ,nombre as Nombre, comp as Compañia, (monto_o - mont_pag) as Restante from cp join Suplidor on id_supli = id_supli_cp  where Restante > 0 ");
            dgvdatcredi.DataSource = cns.cosnsultaconresultado("select   id_p as ID,nombre,apell, cedula, fecha,monto_o as Original,monto_p as Pagado,(monto_o-monto_p) as Restante from Cliente inner join pagos on id_client = client_id_pag where Restante > 0");
            dgvdencar.DataSource = cns.cosnsultaconresultado("select id_client as ID,nombre as Nombre,apell as Apellido,Cedula,direcc as Direccion,tel as Telefono from cliente");
            double sum = 0;
            for (int i = 0; i < dgvfact.Rows.Count; ++i)
            {
                sum += Convert.ToDouble(dgvfact.Rows[i].Cells[4].Value);
            }
            label44.Text = sum.ToString();
            dgvegre.DataSource = cns.cosnsultaconresultado("select * from engre");

        }

        private void cotprod()
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
            {
                SQLiteCommand sqlCmd = new SQLiteCommand("SELECT * FROM inventario where produc like '%" + txtbuspord.Text + "%'and canti_disp > 0", conn);
                conn.Open();
                SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();

                lvcoprod.Columns.Clear(); // Clear previously added columns
                lvcoprod.Items.Clear(); // Clear previously populated items
                lvcoprod.View = View.Details;

                lvcoprod.Columns.Add("Codigo");
                lvcoprod.Columns.Add("Producto");
                lvcoprod.Columns.Add("Categoria");
                lvcoprod.Columns.Add("Precio");
                lvcoprod.Columns.Add("Disponible");


                while (sqlReader.Read())
                {
                    ListViewItem lv = new ListViewItem(sqlReader[0].ToString());
                    lv.SubItems.Add(sqlReader[1].ToString());
                    lv.SubItems.Add(sqlReader[2].ToString());
                    lv.SubItems.Add(sqlReader[3].ToString());
                    lv.SubItems.Add(sqlReader[4].ToString());

                    lvcoprod.Items.Add(lv);
                }
            }


        }
        private void prod()
        {
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                {
                    SQLiteCommand sqlCmd = new SQLiteCommand("SELECT * FROM inventario where produc like '%" + txtbuspord.Text + "%'and canti_disp > 0", conn);
                    conn.Open();
                    SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();

                    lvprod.Columns.Clear(); // Clear previously added columns
                    lvprod.Items.Clear(); // Clear previously populated items
                    lvprod.View = View.Details;

                    lvprod.Columns.Add("Codigo");
                    lvprod.Columns.Add("Producto");
                    lvprod.Columns.Add("Categoria");
                    lvprod.Columns.Add("Precio");
                    lvprod.Columns.Add("Disponible");


                    while (sqlReader.Read())
                    {
                        ListViewItem lv = new ListViewItem(sqlReader[0].ToString());
                        lv.SubItems.Add(sqlReader[1].ToString());
                        lv.SubItems.Add(sqlReader[2].ToString());
                        lv.SubItems.Add(sqlReader[3].ToString());
                        lv.SubItems.Add(sqlReader[4].ToString());

                        lvprod.Items.Add(lv);
                    }
                }
            }
        }


        private void carcmb()
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
            {
                SQLiteCommand sqlCmd = new SQLiteCommand("SELECT * FROM Suplidor where nombre = '" + cmbsup.Text + "'", conn);
                conn.Open();
                SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();

                while (sqlReader.Read())
                {

                    txtinsupnom.Text = sqlReader["nombre"].ToString();
                    txttelsupinv.Text = sqlReader["Numero"].ToString();
                    txtinvsupcomp.Text = sqlReader["Comp"].ToString();
                    txtinvsupdi.Text = sqlReader["direccion"].ToString();
                }
                sqlReader.Close();





            }
            int codigo;

            using (SQLiteCommand dataCommand1 = new SQLiteCommand("select id_supli from Suplidor where nombre ='" + cmbsup.Text + "'", conn))
            {
                codigo = Convert.ToInt32(dataCommand1.ExecuteScalar());

            }

            conn.Close();
        }

        private void cargtot()
        {
            rbnom.Checked = true;
            carga();
            prod();
            carhora();
            carimg();
            livetime();
            carclicot();
            cotprod();
            SaveStockInfoToAnotherFile();

        }

        private void livetime()
        {

            dtpv.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");



        }
        private void Form1_Load(object sender, EventArgs e)
        {

            cargtot();
            combo();
            tabControl1.TabPages.Remove(tabPage1);
            cbdesc.Text = "0";
            lbpfi.Text = "0";
            textBox6.Text = "0";
            textBox5.Text = "No";
        }

        public void carhora()
        {

        }
        private void btnnuesup_Click(object sender, EventArgs e)
        {
            cns.consultasinreaultado("insert into Suplidor(Nombre,numero,Comp,direccion)values('" + txtnomb.Text + "','" + txttelsup.Text + "','" + txtcompsup.Text + "','" + txtdirecsup.Text + "')");
            dgvsup.DataSource = cns.cosnsultaconresultado("select * from Suplidor");

        }

        private void cmbsup_SelectedIndexChanged(object sender, EventArgs e)
        {
            carcmb();

        }




        private void button7_Click(object sender, EventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");

            int codigo;
            if (rbcred.Checked == false && rbefec.Checked == false)
            {
                MessageBox.Show("Seleccione el tipo de entrada ");
            }

            else
            {
                if (rbcred.Checked == true)
                {
                    if (string.IsNullOrEmpty(cmbsup.Text))
                    {

                        MessageBox.Show("Selecione un Suplidor");

                    }


                    else
                    {

                        /* using (SQLiteCommand dataCommand1 = new SQLiteCommand("select id_supli from Suplidor where nombre ='" + cmbsup.Text + "'", conn))
                         {
                             conn.Open();
                             codigo = Convert.ToInt32(dataCommand1.ExecuteScalar());

                         }
                         cns.consultasinreaultado("insert into inventario(produc,tipo_prod,precio,canti_disp,Suplidor_id_supli)values('" + txtnombprod.Text + "','" + txttipprod.Text + "','" + txtpre.Text + "','" + txtinvcant.Text + "','" + codigo + "')");
                         cns.consultasinreaultado("insert into cp(monto,fecha,precio,id_supli_cp)values('" + txtinvcant.Text + "','" + dtpv.Text + "','" + txtpre.Text + "','" + codigo + "')");
                         carga();
                         conn.Close();
                         cargtot();*/


                        string nomb = txtnombprod.Text;
                        string tip = txttipprod.Text;
                        string cant = txtinvcant.Text;
                        string pre = txtpre.Text;
                        string cod = txtcodprod.Text;




                        try
                        {
                            string total;
                            double p, c, pf;
                            p = Convert.ToDouble(pre);
                            c = Convert.ToDouble(cant);
                            pf = p * c;
                            total = pf.ToString();




                            string[] row = { cod, nomb, tip, cant, pre, total };
                            dgvinlist.Rows.Add(row);
                            pictureBox1.Image.Save(@"C:/bdd/img/" + txtnombprod.Text + ".jpg");

                            double sum = 0;
                            for (int i = 0; i < dgvinlist.Rows.Count; ++i)
                            {
                                sum += Convert.ToDouble(dgvinlist.Rows[i].Cells[5].Value);
                            }
                            label47.Text = sum.ToString();

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }


                else if (rbefec.Checked == true)
                {
                    SQLiteDataAdapter ad;
                    DataTable dt = new DataTable();
                    SQLiteCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "select id_Cod from inventario where id_Cod = '" + txtcodprod.Text + "'";
                    ad = new SQLiteDataAdapter(cmd);

                    DataSet ds = new DataSet();
                    ad.Fill(dt);
                    ds.Tables.Add(dt);
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Este Codigo Existe");
                    }

                    else
                    {
                        if (string.IsNullOrEmpty(txtnombprod.Text) && (string.IsNullOrEmpty(txtpre.Text) && (string.IsNullOrEmpty(txtprecomp.Text))))
                        {
                            MessageBox.Show("Llene todos los campos");
                        }
                        else
                        {
                            if (chbint.Checked == true)
                            {
                                using (SQLiteCommand dataCommand1 = new SQLiteCommand("select id_supli from Suplidor where nombre ='" + cmbsup.Text + "'", conn))
                                {
                                    conn.Open();
                                    codigo = Convert.ToInt32(dataCommand1.ExecuteScalar());

                                }
                                cns.consultasinreaultado("insert into inventario(produc,tipo_prod,precio,precio_c,canti_disp,itbis,Suplidor_id_supli)values('" + txtnombprod.Text + "','" + txttipprod.Text + "','" + txtpre.Text + "','" + txtprecomp.Text + "','" + txtinvcant.Text + "','1','" + codigo + "')");
                                carga();
                                conn.Close();
                                cargtot();
                                try
                                {
                                    pictureBox1.Image.Save(@"C:/bdd/img/" + txtnombprod.Text + ".jpg");
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }

                                txtcodprod.Clear();
                                txtnombprod.Clear();
                                txttipprod.Clear();
                                txtprodcant.Clear();
                                txtpre.Clear();
                                txtinvcant.Clear();

                            }
                            else
                            {
                                using (SQLiteCommand dataCommand1 = new SQLiteCommand("select id_supli from Suplidor where nombre ='" + cmbsup.Text + "'", conn))
                                {
                                    conn.Open();
                                    codigo = Convert.ToInt32(dataCommand1.ExecuteScalar());

                                }
                                cns.consultasinreaultado("insert into inventario(produc,tipo_prod,precio,precio_c,canti_disp,itbis,Suplidor_id_supli)values('" + txtnombprod.Text + "','" + txttipprod.Text + "','" + txtpre.Text + "','" + txtprecomp.Text + "','" + txtinvcant.Text + "','0','" + codigo + "')");
                                carga();
                                conn.Close();
                                cargtot();
                                try
                                {
                                    pictureBox1.Image.Save(@"C:/bdd/img/" + txtnombprod.Text + ".jpg");
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }

                                txtcodprod.Clear();
                                txtnombprod.Clear();
                                txttipprod.Clear();
                                txtprodcant.Clear();
                                txtpre.Clear();
                                txtinvcant.Clear();
                            }
                        }
                    }
                }

            }
        }
        private void lxbprod_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillda();
        }

        private void fillda()
        {

            try
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem = lvprod.SelectedItems[0];






                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                {

                    SQLiteCommand sqlCmd = new SQLiteCommand("SELECT * FROM inventario where id_Cod ='" + listViewItem.Text + "' ", conn);

                    conn.Open();
                    SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();

                    while (sqlReader.Read())
                    {

                        txtprev.Text = sqlReader["precio"].ToString();
                        string prod = sqlReader["produc"].ToString();
                        Image image = Image.FromFile(@"C:\bdd\img\" + prod + ".jpg");
                        this.pictureBox2.Image = image;

                        txtprodcant.Text = "1";





                    }

                    double p, c, pf;
                    p = Convert.ToDouble(txtprev.Text);
                    c = Convert.ToDouble(txtprodcant.Text);
                    pf = p * c;
                    txtprfin.Text = pf.ToString();

                    sqlReader.Close();

                }

            }
            catch (Exception ex)
            {


            }

        }

        private void cotfillprod()
        {
            try
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem = lvcoprod.SelectedItems[0];






                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                {

                    SQLiteCommand sqlCmd = new SQLiteCommand("SELECT * FROM inventario where id_Cod ='" + listViewItem.Text + "' ", conn);

                    conn.Open();
                    SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();

                    while (sqlReader.Read())
                    {

                        txtprecot.Text = sqlReader["precio"].ToString();
                        string prod = sqlReader["produc"].ToString();

                        txtcantcot.Text = "1";

                        Image image = Image.FromFile(@"C:\bdd\img\" + prod + ".jpg");
                        this.pictureBox4.Image = image;
                        pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBox4.BorderStyle = BorderStyle.Fixed3D;



                    }

                    double p, c, pf;
                    p = Convert.ToDouble(txtprecot.Text);
                    c = Convert.ToDouble(txtcantcot.Text);
                    pf = p * c;
                    txtprfcot.Text = pf.ToString();

                    sqlReader.Close();

                }

            }
            catch (Exception ex)
            {


            }
        }

        private void txtprodcant_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double p, c, pf;
                p = Convert.ToDouble(txtprev.Text);
                c = Convert.ToDouble(txtprodcant.Text);
                pf = p * c;
                txtprfin.Text = pf.ToString();
            }
            catch (Exception ex)
            {

            }
        }

        private void updatelv()
        {
            if (rbnom.Checked == true)
            {
                try
                {
                    SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                    {
                        SQLiteCommand sqlCmd = new SQLiteCommand("SELECT * FROM inventario where produc like '%" + txtbuspord.Text + "%'and canti_disp > 0", conn);
                        conn.Open();
                        SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();

                        lvprod.Columns.Clear(); // Clear previously added columns
                        lvprod.Items.Clear(); // Clear previously populated items
                        lvprod.View = View.Details;

                        lvprod.Columns.Add("Codigo");
                        lvprod.Columns.Add("Producto");
                        lvprod.Columns.Add("Categoria");
                        lvprod.Columns.Add("Precio");
                        lvprod.Columns.Add("Disponible");


                        while (sqlReader.Read())
                        {
                            ListViewItem lv = new ListViewItem(sqlReader[0].ToString());
                            lv.SubItems.Add(sqlReader[1].ToString());
                            lv.SubItems.Add(sqlReader[2].ToString());
                            lv.SubItems.Add(sqlReader[3].ToString());
                            lv.SubItems.Add(sqlReader[4].ToString());
                            lvprod.Items.Add(lv);


                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else if (rbcod.Checked == true)
            {
                try
                {
                    SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                    {
                        SQLiteCommand sqlCmd = new SQLiteCommand("SELECT * FROM inventario where id_cod like '" + txtbuspord.Text + "%'and canti_disp > 0", conn);
                        conn.Open();
                        SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();

                        lvprod.Columns.Clear(); // Clear previously added columns
                        lvprod.Items.Clear(); // Clear previously populated items
                        lvprod.View = View.Details;

                        lvprod.Columns.Add("Codigo");
                        lvprod.Columns.Add("Producto");
                        lvprod.Columns.Add("Categoria");
                        lvprod.Columns.Add("Precio");
                        lvprod.Columns.Add("Disponible");


                        while (sqlReader.Read())
                        {
                            ListViewItem lv = new ListViewItem(sqlReader[0].ToString());
                            lv.SubItems.Add(sqlReader[1].ToString());
                            lv.SubItems.Add(sqlReader[2].ToString());
                            lv.SubItems.Add(sqlReader[3].ToString());
                            lv.SubItems.Add(sqlReader[4].ToString());
                            lvprod.Items.Add(lv);


                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else
            {
                MessageBox.Show("Seleccione una Opcion de buesqueda");
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            updatelv();
            busccv();

        }
        private void busccv()
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                {
                    SQLiteCommand sqlCmd = new SQLiteCommand("SELECT * FROM Cliente where nombre like '%" + txtbuspord.Text + "%'", conn);
                    conn.Open();
                    SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();

                    lvclient.Columns.Clear(); // Clear previously added columns
                    lvclient.Items.Clear(); // Clear previously populated items
                    lvclient.View = View.Details;

                    lvclient.Columns.Add("Codigo");
                    lvclient.Columns.Add("Nombre");
                    lvclient.Columns.Add("Apellido");
                    lvclient.Columns.Add("Cedula");
                    lvclient.Columns.Add("Direccion");
                    lvclient.Columns.Add("Telefono");


                    while (sqlReader.Read())
                    {
                        ListViewItem lv = new ListViewItem(sqlReader[0].ToString());
                        lv.SubItems.Add(sqlReader[1].ToString());
                        lv.SubItems.Add(sqlReader[2].ToString());
                        lv.SubItems.Add(sqlReader[3].ToString());
                        lv.SubItems.Add(sqlReader[4].ToString());
                        lv.SubItems.Add(sqlReader[5].ToString());
                        lvclient.Items.Add(lv);


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lvprod_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillda();

        }

        private void button1_Click(object sender, EventArgs e)
        {


            try
            {
                if (string.IsNullOrEmpty(txtidstore.Text))
                {
                    DateTime date = DateTime.Now;
                    var shortDate = date.ToString("dd/MM/yyyy");
                    cns.consultasinreaultado("INSERT INTO factura (id_fact,fecha,fec_c) values('" + txtnfact.Text + "','" + dtpcot.Text + "','" + shortDate + "')");

                    ListViewItem listViewItem1 = new ListViewItem();
                    ListViewItem lv2 = new ListViewItem();
                    listViewItem1 = lvprod.SelectedItems[0];
                    string codigo, codvent;


                    SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                    {
                        conn.Open();
                        using (SQLiteCommand dataCommand1 = new SQLiteCommand("select produc from inventario where id_Cod ='" + listViewItem1.Text + "'", conn))
                        {
                            codigo = Convert.ToString(dataCommand1.ExecuteScalar());

                        }

                        using (SQLiteCommand dataCommand2 = new SQLiteCommand("SELECT id_fact FROM factura WHERE fecha IN(SELECT max(id_fac) FROM factura);'", conn))
                        {
                            codvent = Convert.ToString(dataCommand2.ExecuteScalar());
                            txtidstore.Text = codvent;

                        }

                        conn.Close();


                    }


    ;



                    try
                    {
                        double p, c, pf;
                        p = Convert.ToDouble(txtprev.Text);
                        c = Convert.ToDouble(txtprodcant.Text);
                        pf = p * c;
                        txtprfin.Text = pf.ToString();




                        string firstColum = listViewItem1.Text;
                        string secondColum = codigo;
                        string tr3 = txtprev.Text;
                        string tr4 = txtprodcant.Text;
                        string tr5 = pf.ToString();
                        string tr1 = codvent.ToString();

                        string[] row = { tr1, firstColum, secondColum, tr3, tr4, tr5 };
                        dgvventa.Rows.Add(row);
                    }
                    catch (Exception ex)
                    {

                    }
                    //if (string.IsNullOrEmpty(txtidstore.Text))
                    //{
                    //    txtidstore.Text = dtpv.Text;
                    //    cns.consultasinreaultado("insert into Ventas (cant,inventario_id_cod,ven_id_fac)values('" + txtprodcant.Text + "','" + listViewItem.Text + "','" + txtidstore.Text + "')");
                    cns.consultasinreaultado("update inventario set canti_disp = (canti_disp - '" + txtprodcant.Text + "') where id_cod = '" + listViewItem1.Text + "'");
                    updatelv();

                    //    cns.consultasinreaultado("INSERT INTO factura (id_fact,fecha) values('" + txtidstore.Text + "','" + dtpv.Text + "')");
                    //    dgvventa.DataSource = cns.cosnsultaconresultado("select id_cod as Codigo, produc as Producto,precio as Precio, cant as Cantidad, (precio * cant) as Total from ventas   join inventario on id_cod = inventario_id_cod     where ven_id_fac = '" + txtidstore.Text + "'");
                    //   updatelv();


                    //}
                    //else
                    //{

                    //    cns.consultasinreaultado("insert into ventas (cant,inventario_id_cod,ven_id_fac)values('" + txtprodcant.Text + "','" + listViewItem.Text + "','" + txtidstore.Text + "')");
                    //    cns.consultasinreaultado("update inventario set canti_disp = (canti_disp - '" + txtprodcant.Text + "') where id_cod = '" + listViewItem.Text + "'");
                    //    dgvventa.DataSource = cns.cosnsultaconresultado("select id_cod as Codigo, produc as Producto,precio as Precio, cant as Cantidad, (precio * cant) as Total from ventas   join inventario on id_cod = inventario_id_cod     where ven_id_fac = '" + txtidstore.Text + "'");

                    carga();


                    //}

                    double sum = 0;
                    for (int i = 0; i < dgvventa.Rows.Count; ++i)
                    {
                        sum += Convert.ToDouble(dgvventa.Rows[i].Cells[5].Value);
                    }
                    txttp.Text = sum.ToString();

                    txtprev.Clear();
                    txtprodcant.Clear();
                    txtprfin.Clear();

                }

                else
                {
                    ListViewItem listViewItem1 = new ListViewItem();
                    ListViewItem lv2 = new ListViewItem();
                    listViewItem1 = lvprod.SelectedItems[0];
                    string codigo, codvent;
                    SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                    {
                        conn.Open();
                        using (SQLiteCommand dataCommand1 = new SQLiteCommand("select produc from inventario where id_Cod ='" + listViewItem1.Text + "'", conn))
                        {
                            codigo = Convert.ToString(dataCommand1.ExecuteScalar());

                        }

                        using (SQLiteCommand dataCommand2 = new SQLiteCommand("SELECT id_fact FROM factura WHERE fecha IN(SELECT max(fecha) FROM factura);'", conn))
                        {
                            codvent = Convert.ToString(dataCommand2.ExecuteScalar());

                        }

                        conn.Close();


                    }


    ;



                    try
                    {
                        double p, c, pf;
                        p = Convert.ToDouble(txtprev.Text);
                        c = Convert.ToDouble(txtprodcant.Text);
                        pf = p * c;
                        txtprfin.Text = pf.ToString();




                        string firstColum = listViewItem1.Text;
                        string secondColum = codigo;
                        string tr3 = txtprev.Text;
                        string tr4 = txtprodcant.Text;
                        string tr5 = pf.ToString();
                        string tr1 = codvent.ToString();

                        string[] row = { tr1, firstColum, secondColum, tr3, tr4, tr5 };
                        dgvventa.Rows.Add(row);
                    }
                    catch (Exception ex)
                    {

                    }
                    //if (string.IsNullOrEmpty(txtidstore.Text))
                    //{
                    //    txtidstore.Text = dtpv.Text;
                    //    cns.consultasinreaultado("insert into Ventas (cant,inventario_id_cod,ven_id_fac)values('" + txtprodcant.Text + "','" + listViewItem.Text + "','" + txtidstore.Text + "')");
                    //    cns.consultasinreaultado("update inventario set canti_disp = (canti_disp - '" + txtprodcant.Text + "') where id_cod = '" + listViewItem.Text + "'");
                    //    cns.consultasinreaultado("INSERT INTO factura (id_fact,fecha) values('" + txtidstore.Text + "','" + dtpv.Text + "')");
                    //    dgvventa.DataSource = cns.cosnsultaconresultado("select id_cod as Codigo, produc as Producto,precio as Precio, cant as Cantidad, (precio * cant) as Total from ventas   join inventario on id_cod = inventario_id_cod     where ven_id_fac = '" + txtidstore.Text + "'");

                    carga();

                    //}
                    //else
                    //{

                    //    cns.consultasinreaultado("insert into ventas (cant,inventario_id_cod,ven_id_fac)values('" + txtprodcant.Text + "','" + listViewItem.Text + "','" + txtidstore.Text + "')");
                    cns.consultasinreaultado("update inventario set canti_disp = (canti_disp - '" + txtprodcant.Text + "') where id_cod = '" + listViewItem1.Text + "'");
                    updatelv();
                    //    dgvventa.DataSource = cns.cosnsultaconresultado("select id_cod as Codigo, produc as Producto,precio as Precio, cant as Cantidad, (precio * cant) as Total from ventas   join inventario on id_cod = inventario_id_cod     where ven_id_fac = '" + txtidstore.Text + "'");
                    //    updatelv();


                    //}

                    double sum = 0;
                    for (int i = 0; i < dgvventa.Rows.Count; ++i)
                    {
                        sum += Convert.ToDouble(dgvventa.Rows[i].Cells[5].Value);
                    }
                    txttp.Text = sum.ToString();

                    txtprev.Clear();
                    txtprodcant.Clear();
                    txtprfin.Clear();
                }




            }
            catch (Exception ex)
            {

            }
        }

        private void txtidstore_TextChanged(object sender, EventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {



            var format = new StringFormat() { Alignment = StringAlignment.Far };
            var rect = new RectangleF(0, 20, 20, 20);
            Font ft = new Font("Arial", 7, FontStyle.Bold);
            Font ft2 = new Font("Arial", 8, FontStyle.Bold);
            int ancho = 290;
            int y = 20;




            e.Graphics.DrawString("ESC d", ft2, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));

        }


        private void button3_Click(object sender, EventArgs e)
        {
            button7.Visible = true;
            button8.Visible = true;
            button2.Visible = false;
            button3.Visible = false;



        }

        private void button4_Click(object sender, EventArgs e)
        {
            cns.consultasinreaultado("insert into Cliente(nombre,apell,Cedula,direcc,tel)values('" + txtnomc.Text + "','" + txtapellc.Text + "','" + txtcedcli.Text + "','" + txtdirecc.Text + "','" + txttelc.Text + "')");
            dgvcli.DataSource = cns.cosnsultaconresultado("select * from cliente");
            cargtot();

        }

        private void txtprev_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvventa.SelectedRows.Count > 0)
                {
                    dgvventa.Rows.RemoveAt(this.dgvventa.SelectedRows[0].Index);
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void txttp_TextChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            string StrQuery;
            try
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                {
                    using (SQLiteCommand comm = new SQLiteCommand())
                    {
                        comm.Connection = conn;
                        conn.Open();
                        for (int i = 0; i < dgvventa.Rows.Count; i++)
                        {
                            StrQuery = "INSERT INTO Ventas(cant,inventario_id_cod,ven_id_fac,tipo_vent) VALUES ('"
                                + dgvventa.Rows[i].Cells[4].Value.ToString() + "', '"
                                + dgvventa.Rows[i].Cells[1].Value.ToString() + "','"
                                + dgvventa.Rows[i].Cells[0].Value.ToString() + "','1')";
                            comm.CommandText = StrQuery;
                            comm.ExecuteNonQuery();
                            carga();



                        }
                        conn.Close();
                    }
                }
            }
            catch
            {

            }
            printDocument1 = new PrintDocument();
            PrinterSettings ps = new PrinterSettings();
            printDocument1.PrinterSettings = ps;
            //printDocument1.PrinterSettings.PrinterName = "Thermal Printe";
            printDocument1.PrintPage += printDocument1_PrintPage;
            printDocument1.Print();


            dgvventa.Rows.Clear();
            dgvventa.Refresh();

            button7.Visible = false;
            button8.Visible = false;
            button3.Visible = true;
            button2.Visible = true;
            txtidstore.Clear();
        }



        private void txtborrinv_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtcodprod.Text))
            {
                MessageBox.Show("Seleccione un Producto");

            }

            else
            {
                if (MessageBox.Show("Seguro desea Borrar?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cns.consultasinreaultado("delete from inventario where id_Cod = '" + txtcodprod.Text + "'");
                    cargtot();
                    txtcodprod.Clear();
                    txtnombprod.Clear();
                    txttipprod.Clear();
                    txtprodcant.Clear();
                    txtpre.Clear();

                }
                else
                {
                    // user clicked no
                }

            }


        }

        public void dgvinv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                DataGridViewRow act = dgvinv.Rows[e.RowIndex];
                txtnombprod.Text = act.Cells["produc"].Value.ToString();
                txtcodprod.Text = act.Cells["id_Cod"].Value.ToString();
                txttipprod.Text = act.Cells["tipo_prod"].Value.ToString();
                txtpre.Text = act.Cells["precio"].Value.ToString();
                txtinvcant.Text = act.Cells["canti_disp"].Value.ToString();
                txtprecomp.Text = act.Cells["precio_c"].Value.ToString();
                lblben.Text = act.Cells["Beneficio"].Value.ToString();
                Image image = Image.FromFile(@"C:\bdd\img\" + txtnombprod.Text + ".jpg");
                this.pictureBox1.Image = image;
            }
            catch (Exception ex)
            {

            }





        }







        private void open()
        {
            OpenFileDialog f = new OpenFileDialog();
            f.InitialDirectory = "C:/Users/Admin/Downloads";
            f.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            f.FilterIndex = 2;
            if (f.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(f.FileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.BorderStyle = BorderStyle.Fixed3D;

            }
        }



        private void carimg()
        {
            cn.ConnectionString = "Data Source=C:\\bdd\\factura.s3db; Version=3;";
            cmd.Connection = cn;
            picture = new SQLiteParameter("@picture", SqlDbType.Image);


        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            open();

        }

        private void btnmodinv_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtcodprod.Text))
            {
                MessageBox.Show("Seleccione un Producto");

            }

            else
            {
                if (MessageBox.Show("Seguro desea Modificar?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {

                        cns.consultasinreaultado("Update inventario set produc = '" + txtnombprod.Text + "', tipo_prod = '" + txttipprod.Text + "', precio ='" + txtpre.Text + "',precio_c ='"+txtprecomp.Text+"', canti_disp = '" + txtinvcant.Text + "' where id_cod = '" + txtcodprod.Text + "'");
                        pictureBox1.Image.Save(@"C:/bdd/img/" + txtnombprod.Text + ".jpg");
                        carga();
                        txtcodprod.Clear();
                        txtnombprod.Clear();
                        txttipprod.Clear();
                        txtprodcant.Clear();
                        txtpre.Clear();
                        txtprecomp.Clear();
                        txtinvcant.Clear();
                    }
                    catch (Exception ex)
                    {
                        cargtot();
                        txtcodprod.Clear();
                        txtnombprod.Clear();
                        txttipprod.Clear();
                        txtprodcant.Clear();
                        txtpre.Clear();
                        txtinvcant.Clear();
                    }


                }
            }
        }

        private void dgvventa_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {

            txtcleivent.Visible = true;
            if (string.IsNullOrEmpty(txtcleivent.Text))
            {
                lvprod.Visible = false;
                lvclient.Visible = true;
                fillclien();
            }

            else
            {
                button7.Visible = false;
                button8.Visible = false;
            }

        }

        private void fillclien()
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                {
                    SQLiteCommand sqlCmd = new SQLiteCommand("SELECT * FROM Cliente where nombre like '%" + txtbuspord.Text + "%'", conn);
                    conn.Open();
                    SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();

                    lvclient.Columns.Clear(); // Clear previously added columns
                    lvclient.Items.Clear(); // Clear previously populated items
                    lvclient.View = View.Details;

                    lvclient.Columns.Add("Codigo");
                    lvclient.Columns.Add("Nombre");
                    lvclient.Columns.Add("Apellido");
                    lvclient.Columns.Add("Cedula");
                    lvclient.Columns.Add("Direccion");
                    lvclient.Columns.Add("Telefono");


                    while (sqlReader.Read())
                    {
                        ListViewItem lv = new ListViewItem(sqlReader[0].ToString());
                        lv.SubItems.Add(sqlReader[1].ToString());
                        lv.SubItems.Add(sqlReader[2].ToString());
                        lv.SubItems.Add(sqlReader[3].ToString());
                        lv.SubItems.Add(sqlReader[4].ToString());
                        lv.SubItems.Add(sqlReader[5].ToString());
                        lvclient.Items.Add(lv);


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            button7.Visible = false;
            button8.Visible = false;
            button3.Visible = true;
            button2.Visible = true;
        }

        public void fillcli()
        {

            try
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem = lvclient.SelectedItems[0];






                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                {

                    SQLiteCommand sqlCmd = new SQLiteCommand("SELECT * FROM Cliente where id_client ='" + listViewItem.Text + "' ", conn);

                    conn.Open();
                    SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        txtcleivent.Text = (sqlReader["Nombre"].ToString() + " " + (sqlReader["Apell"].ToString()));

                        codicred = sqlReader["id_client"].ToString();








                    }



                    sqlReader.Close();

                    button2.Visible = false;
                    button3.Visible = false;
                }

            }
            catch (Exception ex)
            {


            }
        }

        private void lvclient_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillcli();
        }

        private void txtcleivent_TextChanged(object sender, EventArgs e)
        {



            button7.Visible = false;
            button8.Visible = false;
            button9.Visible = true;


        }

        private void button9_Click(object sender, EventArgs e)
        {

            string StrQuery;
            try
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                {






                    using (SQLiteCommand comm = new SQLiteCommand())
                    {
                        comm.Connection = conn;
                        conn.Open();
                        for (int i = 0; i < dgvventa.Rows.Count - 1; i++)
                        {
                            StrQuery = "INSERT INTO Ventas(cant,inventario_id_cod,Cliente_id_client,ven_id_fac,tipo_vent) VALUES ('"
                                + dgvventa.Rows[i].Cells[4].Value.ToString() + "', '"
                                + dgvventa.Rows[i].Cells[1].Value.ToString() + "','"
                                + codicred + "','"
                                + dgvventa.Rows[i].Cells[0].Value.ToString() + "','2')";
                            comm.CommandText = StrQuery;
                            comm.ExecuteNonQuery();
                            carga();



                        }
                    }
                }
            }
            catch
            {

            }
            printDocument2 = new PrintDocument();
            PrinterSettings ps = new PrinterSettings();
            printDocument1.PrinterSettings = ps;
            //printDocument2.PrinterSettings.PrinterName = "Microsoft Print to PDF";
            printDocument2.PrintPage += PrintDocument2_PrintPage;
            printDocument2.Print();
            cns.consultasinreaultado("insert into pagos (id_p,monto_o,monto_p,fecha,client_id_pag)values('" + txtidstore.Text + "','" + txttp.Text + "','0','" + dtpv.Text + "','" + codicred + "')");

            dgvventa.Rows.Clear();
            dgvventa.Refresh();
            lvclient.Visible = false;
            lvprod.Visible = true;
            txtcleivent.Clear();
            txtcleivent.Visible = false;
            txtidstore.Clear();


            button9.Visible = false;
            button3.Visible = true;
            button2.Visible = true;

        }

        private void txtbusfac_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioButton2.Checked == true)
                {
                    dgvfact.DataSource = cns.cosnsultaconresultado("select ven_id_fac as Codigo, produc as Producto,precio as Precio, cant as Cantidad, (precio * cant) as Total,fecha, nombre as Nombre, apell as Apellido from ventas   join inventario on id_cod = inventario_id_cod  join Cliente on Cliente_id_client = id_client   INNER JOIN factura on id_fact = ven_id_fac where nombre like '%" + txtbusfac.Text + "%'  ");

                }

                else if (radioButton1.Checked == true)
                {
                    dgvfact.DataSource = cns.cosnsultaconresultado("select ven_id_fac as Codigo, produc as Producto,precio as Precio, cant as Cantidad, (precio * cant) as Total,fecha from ventas   join inventario on id_cod = inventario_id_cod   INNER JOIN factura on id_fact = ven_id_fac where fecha like '%" + txtbusfac.Text + "%'  ");

                }
            }

            catch (Exception ex)
            {

            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            carga();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            livetime();
        }

        private void dgvcp_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow act = dgvcp.Rows[e.RowIndex];
            txtidcp.Text = act.Cells["ID"].Value.ToString();
            txtnomcp.Text = act.Cells["Nombre"].Value.ToString();
            txtmontcp.Text = act.Cells["Monto"].Value.ToString();
            txtmpagcp.Text = act.Cells["Pagado"].Value.ToString();

        }

        private void btnpagcp_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtidcp.Text)) { }

            else
            {

                cns.consultasinreaultado("insert into pagos(monto_p,fecha,supli_id_pag)values('" + txtmotpacp.Text + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + txtidcp.Text + "')");

            }
        }

        private void txtcodprod_TextChanged(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {



        }



        private void fillclicot()
        {
            try
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem = lvclicot.SelectedItems[0];






                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                {

                    SQLiteCommand sqlCmd = new SQLiteCommand("SELECT * FROM Cliente where id_client ='" + listViewItem.Text + "' ", conn);

                    conn.Open();
                    SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        txtidcred.Text = sqlReader["id_client"].ToString();
                        txtcocli.Text = (sqlReader["nombre"].ToString() + " " + sqlReader["apell"].ToString());
                        txtcedcot.Text = sqlReader["cedula"].ToString();
                        txtdireccot.Text = sqlReader["direcc"].ToString();
                        txtcttel.Text = sqlReader["tel"].ToString();









                    }



                    sqlReader.Close();

                    button2.Visible = false;
                    button3.Visible = false;
                }

            }
            catch (Exception ex)
            {


            }

        }


        private void carclicot()
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                {
                    SQLiteCommand sqlCmd = new SQLiteCommand("SELECT * FROM Cliente where nombre like '%" + txtbuspord.Text + "%'", conn);
                    conn.Open();
                    SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();

                    lvclicot.Columns.Clear(); // Clear previously added columns
                    lvclicot.Items.Clear(); // Clear previously populated items
                    lvclicot.View = View.Details;

                    lvclicot.Columns.Add("Codigo");
                    lvclicot.Columns.Add("Nombre");
                    lvclicot.Columns.Add("Apellido");
                    lvclicot.Columns.Add("Direccion");
                    lvclicot.Columns.Add("Telefono");


                    while (sqlReader.Read())
                    {
                        ListViewItem lv = new ListViewItem(sqlReader[0].ToString());
                        lv.SubItems.Add(sqlReader[1].ToString());
                        lv.SubItems.Add(sqlReader[2].ToString());
                        lv.SubItems.Add(sqlReader[3].ToString());
                        lv.SubItems.Add(sqlReader[4].ToString());
                        lvclicot.Items.Add(lv);


                    }
                }

            }



            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            button7.Visible = false;
            button8.Visible = false;
            button3.Visible = true;
            button2.Visible = true;
        }

        private void lvclicot_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillclicot();
        }

        private void txtcocli_TextChanged(object sender, EventArgs e)
        {
            lvclicot.Visible = false;
            lvcoprod.Visible = true;
        }

        private void lvcoprod_SelectedIndexChanged(object sender, EventArgs e)
        {
            cotfillprod();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {

                ListViewItem listViewItem1 = new ListViewItem();
                ListViewItem lv2 = new ListViewItem();
                listViewItem1 = lvcoprod.SelectedItems[0];
                string codigo;
                int codvent;


                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                {
                    conn.Open();
                    using (SQLiteCommand dataCommand1 = new SQLiteCommand("select produc from inventario where id_Cod ='" + listViewItem1.Text + "'", conn))
                    {
                        codigo = Convert.ToString(dataCommand1.ExecuteScalar());

                    }

                    if (string.IsNullOrEmpty(txtnfact.Text))
                    {
                        using (SQLiteCommand dataCommand2 = new SQLiteCommand("SELECT id_fact FROM factura WHERE id_fact IN(SELECT max(id_fact) FROM factura);;'", conn))
                        {
                            codvent = Convert.ToInt32(dataCommand2.ExecuteScalar());
                            txtnfact.Text = (codvent + 1).ToString();


                        }
                    }
                    else
                    {

                    }

                    conn.Close();


                }



                try
                {
                    double p, c, pf;
                    p = Convert.ToDouble(txtprecot.Text);
                    c = Convert.ToDouble(txtcantcot.Text);
                    pf = p * c;

                    txtprfcot.Text = pf.ToString();




                    string firstColum = listViewItem1.Text;
                    string secondColum = codigo;
                    string tr3 = txtprecot.Text;
                    string tr4 = txtcantcot.Text;
                    string tr5 = pf.ToString();



                    string[] row = { firstColum, secondColum, tr3, tr4, tr5 };
                    dgvcot.Rows.Add(row);


                    double sum = 0;
                    for (int i = 0; i < dgvcot.Rows.Count; ++i)
                    {
                        sum += Convert.ToDouble(dgvcot.Rows[i].Cells[4].Value);
                    }
                    lbpfi.Text = sum.ToString();
                    //cns.consultasinreaultado("update inventario set canti_disp = (canti_disp - '" + txtcantcot.Text + "') where id_cod = '" + listViewItem1.Text + "'");
                    ACTPROD();
                }


                catch (Exception ex)
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Seleccione un producto");
            }
        }
        private void ACTPROD()
        {

            try
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                {
                    SQLiteCommand sqlCmd = new SQLiteCommand("SELECT * FROM inventario where  canti_disp > 0", conn);
                    conn.Open();
                    SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();

                    lvcoprod.Columns.Clear(); // Clear previously added columns
                    lvcoprod.Items.Clear(); // Clear previously populated items
                    lvcoprod.View = View.Details;

                    lvcoprod.Columns.Add("Codigo");
                    lvcoprod.Columns.Add("Producto");
                    lvcoprod.Columns.Add("Categoria");
                    lvcoprod.Columns.Add("Precio");
                    lvcoprod.Columns.Add("Disponible");


                    while (sqlReader.Read())
                    {
                        ListViewItem lv = new ListViewItem(sqlReader[0].ToString());
                        lv.SubItems.Add(sqlReader[1].ToString());
                        lv.SubItems.Add(sqlReader[2].ToString());
                        lv.SubItems.Add(sqlReader[3].ToString());
                        lv.SubItems.Add(sqlReader[4].ToString());
                        lvcoprod.Items.Add(lv);


                    }
                }
            }
            catch
            {

            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            double preb,prf;
            preb = Convert.ToDouble(lbpfi.Text);
            prf = preb + (preb * 0.18);
            groupBox21.Visible = true;
            txttpag.Text = prf.ToString();
            txtpagado.Text = "0";



        }

        private void cut2()
        {

        }
        private void generar()
        {
            DataTable dt = new DataTable();
            for (int i = 1; i < dgvcot.Columns.Count + 1; i++)
            {
                DataColumn column = new DataColumn(dgvcot.Columns[i - 1].HeaderText);
                dt.Columns.Add(column);
            }
            int columnCount = dgvcot.Columns.Count;
            foreach (DataGridViewRow dr in dgvcot.Rows)
            {
                DataRow dataRow = dt.NewRow();
                for (int i = 0; i < columnCount; i++)
                {
                    //returns checkboxes and dropdowns as string with .value..... nearly got it
                    dataRow[i] = dr.Cells[i].Value;
                }
                dt.Rows.Add(dataRow);
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);


            XmlTextWriter xmlSave = new XmlTextWriter(@"C:\bdd\ctzn/DGVXML.xml", Encoding.UTF8);
            CrystalReport1 objRpt = new CrystalReport1();
            ds.WriteXml(xmlSave);
            xmlSave.Close();


            cotiz f = new cotiz();
            CrystalReport1 cr = new CrystalReport1();
            CrystalReport3 cr2 = new CrystalReport3();

            TextObject text = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txtclicr"];
            //TextObject text1 = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txtcrced"];
            TextObject text2 = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["txtpagcr"];
            TextObject text3 = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["txtdevcr"];
            TextObject text4 = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["txtcrtt"];
            TextObject text10 = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["txtdescr"];
            TextObject text11 = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["txtit"];
            TextObject text12 = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["txtsubt"];





            TextObject text5 = (TextObject)cr2.ReportDefinition.Sections["Section2"].ReportObjects["txtclicr"];
            //TextObject text1 = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txtcrced"];
            TextObject text6 = (TextObject)cr2.ReportDefinition.Sections["Section4"].ReportObjects["txtpagcr"];
            TextObject text7 = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txtclicr"];
            TextObject text8 = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txtcrced"];            
            vent();

            text.Text = txtcocli.Text;
            //text1.Text = txtcedcot.Text;
            text2.Text = txtpagado.Text;
            text3.Text = txtdevuelta.Text;
            text10.Text = cbdesc.Text;
            text7.Text = txtcocli.Text;
            text8.Text = txtcedcot.Text;

            //text11.Text = lbpfi.Text;
            double des, tot, resul;
            double preb, prf,itb;
            preb = Convert.ToDouble(lbpfi.Text);
            prf = (preb+(preb * 0.18) - preb *0.18);
            text12.Text = prf.ToString();
            tot = Convert.ToDouble(txttpag.Text);
            des = Convert.ToDouble(cbdesc.Text);
            resul = tot - tot * (des / 100);
            text4.Text = resul.ToString();
            itb = preb * 0.18;
            text11.Text = itb.ToString();
            f.crystalReportViewer1.ReportSource = cr;
            cr.PrintToPrinter(2, false, 0, 0);



            //cut();
            cr.Close();
            cr.Dispose();
            factdevt();
            dgvcot.Rows.Clear();
            txtidcred.Clear();
            txtcocli.Clear();
            txtcedcli.Clear();
            txtcedcot.Clear();
            txtdireccot.Clear();
            txtcttel.Clear();
            txtprecot.Clear();
            txtcantcot.Clear();
            txtprfcot.Clear();
            lbpfi.Text = "";

        }

        private void copia()
        {
            cotiz f = new cotiz();
            CrystalReport3 cr = new CrystalReport3();

            TextObject text = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txtclicr"];
            //TextObject text1 = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txtcrced"];
            TextObject text2 = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["txtpagcr"];
            TextObject text3 = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["txtdevcr"];
            TextObject text4 = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["txtcrtt"];


            text.Text = txtcocli.Text;
            //text1.Text = txtcedcot.Text;
            text2.Text = txtpagado.Text;
            text3.Text = txtdevuelta.Text;
            text4.Text = lbpfi.Text;
            f.crystalReportViewer1.ReportSource = cr;
            cr.PrintToPrinter(1, false, 0, 0);
            cr.Close();
            cr.Dispose();
        }
       
        private void vent()
        {
            string StrQuery;

            SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
            {


                if (string.IsNullOrEmpty(txtidcred.Text))
                {
                    using (SQLiteCommand comm = new SQLiteCommand())
                    {
                        string cod = Convert.ToString(txtidcred.Text);
                        comm.Connection = conn;

                        for (int i = 0; i < dgvcot.Rows.Count - 1; i++)
                        {
                            conn.Open();
                            StrQuery = "INSERT INTO Ventas(cant,inventario_id_cod,Cliente_id_client,ven_id_fac,tipo_vent) VALUES ('"
                                + dgvcot.Rows[i].Cells[3].Value.ToString() + "', '"
                                + dgvcot.Rows[i].Cells[0].Value.ToString() + "','"
                                + cod.ToString() + "','"
                                + txtnfact.Text + "','1')";
                            comm.CommandText = StrQuery;
                            comm.ExecuteNonQuery();
                            carga();
                            conn.Close();





                        }
                    }
                }

                else
                {
                    using (SQLiteCommand comm = new SQLiteCommand())
                    {
                        string cod = Convert.ToString(txtidcred.Text);
                        comm.Connection = conn;

                        for (int i = 0; i < dgvcot.Rows.Count - 1; i++)
                        {
                            conn.Open();
                            StrQuery = "INSERT INTO Ventas(cant,inventario_id_cod,Cliente_id_client,ven_id_fac,tipo_vent) VALUES ('"
                                    + dgvcot.Rows[i].Cells[3].Value.ToString() + "', '"
                                    + dgvcot.Rows[i].Cells[0].Value.ToString() + "','"
                                    + cod.ToString() + "','"
                                    + txtnfact.Text + "','2')";
                            comm.CommandText = StrQuery;
                            comm.ExecuteNonQuery();
                            carga();
                            conn.Close();
                            cns.consultasinreaultado("insert into pagos (monto_o,monto_p,fecha,client_id_pag)values('" + lbpfi.Text + "','0','" + dtpcot.Text + "','" + txtidcred.Text + "')");




                        }
                    }

                }



                //using (SQLiteCommand comm = new SQLiteCommand())
                //{
                //    string cod = Convert.ToString(txtidcred.Text);
                //    comm.Connection = conn;

                //    for (int i = 0; i < dgvcot.Rows.Count -1; i++)
                //    {
                //    conn.Open();
                //    StrQuery = "INSERT INTO Ventas(cant,inventario_id_cod,Cliente_id_client,ven_id_fac,tipo_vent) VALUES ('"
                //            + dgvcot.Rows[i].Cells[3].Value.ToString() + "', '"
                //            + dgvcot.Rows[i].Cells[0].Value.ToString() + "','"
                //            + cod.ToString() + "','"
                //            + txtnfact.Text + "','2')";
                //        comm.CommandText = StrQuery;
                //        comm.ExecuteNonQuery();
                //        carga();
                //        conn.Close();
                //        cns.consultasinreaultado("insert into pagos (monto_o,monto_p,fecha,client_id_pag)values('" + lbpfi.Text + "','0','" + dtpcot.Text + "','" + txtidcred.Text + "')");




                //    }
                //}







            }
        }
        private void PictureBox2_Click(object sender, EventArgs e)
        {
            PictureBox pb = pictureBox2 as PictureBox;
            imgt f2 = new imgt(pb.Image);
            f2.Show();
        }

        private void Dgvcli_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow act = dgvcli.Rows[e.RowIndex];
            txtidcli.Text = act.Cells["id_client"].Value.ToString();
            txtnomc.Text = act.Cells["nombre"].Value.ToString();
            txtapellc.Text = act.Cells["apell"].Value.ToString();
            txtcedcli.Text = act.Cells["Cedula"].Value.ToString();
            txtdirecc.Text = act.Cells["direcc"].Value.ToString();
            txttelc.Text = act.Cells["tel"].Value.ToString();
        }

        private void GroupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void Button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtidcli.Text))
            {
                MessageBox.Show("Seleccione un Producto");

            }

            else
            {
                if (MessageBox.Show("Seguro desea Borrar?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cns.consultasinreaultado("delete from cliente where id_Client = '" + txtidcli.Text + "'");
                    cargtot();
                    txtidcli.Clear();
                    txtnomc.Clear();
                    txtapellc.Clear();
                    txtcedcli.Clear();
                    txtdirecc.Clear();
                    txttelc.Clear();

                }
                else
                {
                    // user clicked no
                }

            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtcodprod.Text))
            {
                MessageBox.Show("Seleccione un Cliente");

            }

            else
            {
                if (MessageBox.Show("Seguro desea Modificar?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {

                        cns.consultasinreaultado("Update cliente set nombre = '" + txtnomc.Text + "', apell = '" + txtapellc.Text + "', cedula ='" + txtcedcli.Text + "', direcc = '" + txtdirecc.Text + "', tel ='" + txttelc.Text + "' where id_client = '" + txtidcli.Text + "'");

                        carga();
                        txtidcli.Clear();
                        txtnomc.Clear();
                        txtapellc.Clear();
                        txtcedcli.Clear();
                        txtdirecc.Clear();
                        txttelc.Clear();
                    }

                    catch (Exception ex)
                    {

                    }
                }

                else
                {
                    // user clicked no
                }
            }



        }

        public void SaveStockInfoToAnotherFile()
        {
            string sourcePath = @"C:\bdd";
            string destinationPath = @"C:\bdd\backup";
            string sourceFileName = "factura.s3db";
            string destinationFileName = DateTime.Now.ToString("yyyyMMddhhmmss") + ".S3db"; // Don't mind this. I did this because I needed to name the copied files with respect to time.
            string sourceFile = System.IO.Path.Combine(sourcePath, sourceFileName);
            string destinationFile = System.IO.Path.Combine(destinationPath, destinationFileName);

            if (!System.IO.Directory.Exists(destinationPath))
            {
                System.IO.Directory.CreateDirectory(destinationPath);
            }
            System.IO.File.Copy(sourceFile, destinationFile, true);
        }

        private void PrintDocument2_PrintPage(object sender, PrintPageEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
            {

                SQLiteCommand sqlCmd = new SQLiteCommand("select id_cod as Codigo, produc as Producto, precio as Precio, cant as Cantidad,Cliente_id_client as Cliente, (precio * cant) as Total from ventas   join inventario on id_cod = inventario_id_cod  join factura on id_fact = ven_id_fac   where  ven_id_fac ='" + txtidstore.Text + "'  ", conn);
                string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                conn.Open();
                SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();

                var format = new StringFormat() { Alignment = StringAlignment.Far };
                var rect = new RectangleF(0, 20, 20, 20);
                Font ft = new Font("Arial", 5, FontStyle.Bold);
                Font ft2 = new Font("Arial", 6, FontStyle.Bold);
                int ancho = 203;
                int y = 20;
                e.Graphics.DrawString("                      VARIEDADES NATHALIE", ft2, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));
                e.Graphics.DrawString("                      Fecha: " + date + "", ft2, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));
                e.Graphics.DrawString("                      AV.DR.MORILLO #29 ", ft2, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));
                e.Graphics.DrawString("               Tel 829-781-4474          RNC. 036001734", ft2, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));
                e.Graphics.DrawString("                       VENTA AL CONTADO", ft2, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));
                e.Graphics.DrawString("                                         ", ft, Brushes.Black, new RectangleF(0, y += 20, ancho, 20));
                e.Graphics.DrawString("                    Numero de Factura: " + txtidstore.Text, ft, Brushes.Black, new RectangleF(0, y += 20, ancho, 20));
                e.Graphics.DrawString("                   Cliente: " + txtcleivent.Text, ft, Brushes.Black, new RectangleF(0, y += 20, ancho, 20));
                e.Graphics.DrawString("-------------------------------------------------------------------------------------", ft, Brushes.Black, new RectangleF(0, y += 20, ancho, 20));
                e.Graphics.DrawString("DESCRIPCION         PRECIO         Cantidad       Importe", ft, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));




                while (sqlReader.Read())
                {





                    e.Graphics.DrawString(sqlReader["Producto"].ToString(), ft, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));
                    e.Graphics.DrawString("                                   " + sqlReader["Precio"].ToString(), ft, Brushes.Black, new RectangleF(0, y += 0, ancho, 20));
                    e.Graphics.DrawString("                                                            " + sqlReader["Cantidad"].ToString(), ft, Brushes.Black, new RectangleF(0, y += 0, ancho, 20));
                    e.Graphics.DrawString("                                                                                 " + sqlReader["Total"].ToString(), ft, Brushes.Black, new RectangleF(0, y += 0, ancho, 20));




                }
                e.Graphics.DrawString("-------------------------------------------------------------------------------------", ft, Brushes.Black, new RectangleF(0, y += 20, ancho, 20));

                e.Graphics.DrawString("Total:" + txttp.Text, ft, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));
                e.Graphics.DrawString("                 ", ft, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));
                e.Graphics.DrawString("                 ", ft, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));
                e.Graphics.DrawString("HYC                  ", ft, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));







            }
        }

        private void TextBox1_TextChanged_1(object sender, EventArgs e)
        {

            busclicot();
            busprocot();

        }
        private void busclicot()
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                {
                    SQLiteCommand sqlCmd = new SQLiteCommand("SELECT * FROM Cliente where nombre like '%" + txtbusclicot.Text + "%'", conn);
                    conn.Open();
                    SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();

                    lvclicot.Columns.Clear(); // Clear previously added columns
                    lvclicot.Items.Clear(); // Clear previously populated items
                    lvclicot.View = View.Details;

                    lvclicot.Columns.Add("Codigo");
                    lvclicot.Columns.Add("Nombre");
                    lvclicot.Columns.Add("Apellido");
                    lvclicot.Columns.Add("Cedula");
                    lvclicot.Columns.Add("Direccion");
                    lvclicot.Columns.Add("Telefono");


                    while (sqlReader.Read())
                    {
                        ListViewItem lv = new ListViewItem(sqlReader[0].ToString());
                        lv.SubItems.Add(sqlReader[1].ToString());
                        lv.SubItems.Add(sqlReader[2].ToString());
                        lv.SubItems.Add(sqlReader[3].ToString());
                        lv.SubItems.Add(sqlReader[4].ToString());
                        lv.SubItems.Add(sqlReader[5].ToString());
                        lvclicot.Items.Add(lv);


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void busprocot()
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                {
                    SQLiteCommand sqlCmd = new SQLiteCommand("SELECT * FROM inventario where produc like '%" + txtbusclicot.Text + "%'and canti_disp > 0", conn);
                    conn.Open();
                    SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();

                    lvcoprod.Columns.Clear(); // Clear previously added columns
                    lvcoprod.Items.Clear(); // Clear previously populated items
                    lvcoprod.View = View.Details;

                    lvcoprod.Columns.Add("Codigo");
                    lvcoprod.Columns.Add("Producto");
                    lvcoprod.Columns.Add("Categoria");
                    lvcoprod.Columns.Add("Precio");
                    lvcoprod.Columns.Add("Disponible");


                    while (sqlReader.Read())
                    {
                        ListViewItem lv = new ListViewItem(sqlReader[0].ToString());
                        lv.SubItems.Add(sqlReader[1].ToString());
                        lv.SubItems.Add(sqlReader[2].ToString());
                        lv.SubItems.Add(sqlReader[3].ToString());
                        lv.SubItems.Add(sqlReader[4].ToString());
                        lvcoprod.Items.Add(lv);


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TextBox1_TextChanged_2(object sender, EventArgs e)
        {
            dgvinv.DataSource = cns.cosnsultaconresultado("SELECT * FROM inventario where produc like '%" + txtbuscinv.Text + "%'");
        }

        private void GroupBox10_Enter(object sender, EventArgs e)
        {

        }

        private void Dgvdatcredi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow act = dgvdatcredi.Rows[e.RowIndex];
            txtidpag.Text = act.Cells["ID"].Value.ToString();
            txtnombpag.Text = act.Cells["nombre"].Value.ToString();
            txtdopag.Text = act.Cells["Original"].Value.ToString();
            txtfecpag.Text = act.Cells["fecha"].Value.ToString();





        }

        private void Button11_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtidpag.Text))
            {
                MessageBox.Show("Seleccione una deuda");
            }
            cns.consultasinreaultado("update pagos set monto_p =(monto_p+'" + txtrealipag.Text + "') where id_p = '" + txtidpag.Text + "'");
            cns.consultasinreaultado("INSERT INTO factura (fecha,fec_c,ttdv) values('" + dtpcot.Text + "','" +dtpcot.Text + "','" + txtrealipag.Text + "')");

            printDocument3 = new PrintDocument();
            PrinterSettings ps = new PrinterSettings();
            printDocument3.PrinterSettings = ps;

            //printDocument2.PrinterSettings.PrinterName = "Microsoft Print to PDF";
            printDocument3.PrintPage += PrintDocument3_PrintPage;
            printDocument3.Print();
            cargtot();
            txtidpag.Clear();
            txtnombpag.Clear();
            txtdopag.Clear();
            txtfecpag.Clear();
            txtrealipag.Clear();

        }

        private void PrintDocument3_PrintPage(object sender, PrintPageEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
            {

                SQLiteCommand sqlCmd = new SQLiteCommand(" select  id_p as ID, nombre,apell, cedula, fecha,monto_o as Original,monto_p as Pagado,(monto_o - monto_p) as Restante from Cliente inner join pagos on id_client = client_id_pag where id_p='" + txtidpag.Text + "'", conn);
                string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                conn.Open();
                SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();

                var format = new StringFormat() { Alignment = StringAlignment.Far };
                var rect = new RectangleF(0, 20, 20, 20);
                Font ft = new Font("Arial", 5, FontStyle.Bold);
                Font ft2 = new Font("Arial", 6, FontStyle.Bold);
                int ancho = 203;
                int y = 20;
                e.Graphics.DrawString("                    EZ-Print", ft2, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));
                e.Graphics.DrawString("                    Fecha: " + date + "", ft2, Brushes.Black, new RectangleF(0, y += 30, ancho, 20)); e.Graphics.DrawString("                    Pago de Deuda", ft2, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));

                e.Graphics.DrawString("                                         ", ft, Brushes.Black, new RectangleF(0, y += 20, ancho, 20));
                e.Graphics.DrawString("                                         ", ft, Brushes.Black, new RectangleF(0, y += 20, ancho, 20));
                e.Graphics.DrawString("                                Numero de Factura: " + txtidpag.Text, ft, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));
                e.Graphics.DrawString("Cliente:  " + txtnombpag.Text, ft, Brushes.Black, new RectangleF(0, y += 20, ancho, 20));
                e.Graphics.DrawString("-------------------------------------------------------------------------------------------------", ft, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));
                e.Graphics.DrawString("Deuda Original                     Monto Pagado", ft, Brushes.Black, new RectangleF(0, y += 40, ancho, 20));
                e.Graphics.DrawString("        " + txtdopag.Text + "                             " + txtrealipag.Text + "", ft, Brushes.Black, new RectangleF(0, y += 40, ancho, 20));

                e.Graphics.DrawString("-------------------------------------------------------------------------------------------------", ft, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));



                while (sqlReader.Read())
                {






                    e.Graphics.DrawString("Monto Restante: " + sqlReader["Restante"].ToString(), ft, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));




                }
                e.Graphics.DrawString("-------------------------------------------------------------------------------------------------", ft, Brushes.Black, new RectangleF(0, y += 20, ancho, 20));







            }

        }

        private void PictureBox4_Click(object sender, EventArgs e)
        {
            PictureBox pb = pictureBox4 as PictureBox;
            imgt f2 = new imgt(pb.Image);
            f2.Show();
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            Reportes f = new Reportes();
            f.Show();
        }

        private void TextBox1_TextChanged_3(object sender, EventArgs e)
        {
            dgvdatcredi.DataSource = cns.cosnsultaconresultado("select   id_p as ID,nombre,apell, cedula, fecha,monto_o as Original,monto_p as Pagado,(monto_o-monto_p) as Restante from Cliente inner join pagos on id_client = client_id_pag where Restante > 0 and  nombre like '%" + txtbusd.Text + "%'");
        }

        private void Button15_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvcot.SelectedRows.Count > 0)
                {
                    dgvcot.Rows.RemoveAt(this.dgvcot.SelectedRows[0].Index);
                    sumtot();
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void Button16_Click(object sender, EventArgs e)
        {
            lvclicot.Visible = false;
            lvcoprod.Visible = true;
            button16.Visible = false;
            button17.Visible = true;

        }

        private void Button17_Click(object sender, EventArgs e)
        {
            lvclicot.Visible = true;
            lvcoprod.Visible = false;
            button16.Visible = true;
            button17.Visible = false;

        }

        private void Rbcred_CheckedChanged(object sender, EventArgs e)
        {
            groupBox19.Visible = true;
        }

        private void Rbefec_CheckedChanged(object sender, EventArgs e)
        {
            groupBox19.Visible = false;

        }

        private void Button18_Click(object sender, EventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");

            int codigo;
            using (SQLiteCommand dataCommand1 = new SQLiteCommand("select id_supli from Suplidor where nombre ='" + cmbsup.Text + "'", conn))
            {
                conn.Open();
                codigo = Convert.ToInt32(dataCommand1.ExecuteScalar());
                conn.Close();

            }
            string StrQuery1;
            string StrQuery2;
            try
            {
                {
                    using (SQLiteCommand comm = new SQLiteCommand())
                    {
                        comm.Connection = conn;
                        conn.Open();
                        for (int i = 0; i < dgvinlist.Rows.Count; i++)
                        {
                            StrQuery1 = "insert into inventario(id_cod,produc,tipo_prod,precio,canti_disp,Suplidor_id_supli) VALUES ('"
                                + dgvinlist.Rows[i].Cells[0].Value.ToString() + "', '"
                                + dgvinlist.Rows[i].Cells[1].Value.ToString() + "', '"
                                + dgvinlist.Rows[i].Cells[2].Value.ToString() + "','"
                                + dgvinlist.Rows[i].Cells[3].Value.ToString() + "','"
                                + dgvinlist.Rows[i].Cells[4].Value.ToString() + "','" + codigo + "')";
                            comm.CommandText = StrQuery1;
                            comm.ExecuteNonQuery();
                            cns.consultasinreaultado("insert into cp(monto_o,fecha,mont_pag,id_supli_cp)values('" + label47.Text + "','" + DateTime.Now + "','0','" + codigo + "')");
                            carga();
                            dgvinlist.Rows.Clear();



                        }
                    }
                }
            }
            catch
            {

            }
        }

        private void Button20_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtidcp.Text))
            {
                MessageBox.Show("Seleccione una deuda");
            }

            else
            {
                cns.consultasinreaultado("update cp set mont_pag = (mont_pag +'" + txtmotpacp.Text + "') where id_cp = '" + txtidcp.Text + "'");
                printDocument4 = new PrintDocument();
                PrinterSettings ps = new PrinterSettings();
                printDocument3.PrinterSettings = ps;
                //printDocument4.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                printDocument4.PrintPage += PrintDocument4_PrintPage;
                printDocument4.Print();
                cargtot();
                txtidcp.Clear();
                txtnomcp.Clear();
                txtnomcp.Clear();
                txtmpagcp.Clear();
                txtmotpacp.Clear();
                txtmontcp.Clear();

            }
        }

        private void TextBox1_TextChanged_4(object sender, EventArgs e)
        {
            dgvcp.DataSource = cns.cosnsultaconresultado("select id_cp as ID, monto_o as Monto, fecha as Fecha,mont_pag as Pagado ,nombre as Nombre, comp as Compañia, (monto_o - mont_pag) as Restante from cp join Suplidor on id_supli = id_supli_cp  where Restante > 0 and nombre like '%" + textBox1.Text + "%' ");

        }

        private void PrintDocument4_PrintPage(object sender, PrintPageEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");

            SQLiteCommand sqlCmd = new SQLiteCommand("select id_cp as ID, monto_o as Monto, fecha as Fecha,mont_pag as Pagado ,nombre as Nombre, comp as Compañia, (monto_o - mont_pag) as Restante from cp join Suplidor on id_supli = id_supli_cp  where Restante > 0 and ID ='" + txtidcp.Text + "'", conn);
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            conn.Open();
            SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();

            var format = new StringFormat() { Alignment = StringAlignment.Far };
            var rect = new RectangleF(0, 20, 20, 20);
            Font ft = new Font("Arial", 5, FontStyle.Bold);
            Font ft2 = new Font("Arial", 6, FontStyle.Bold);
            int ancho = 203;
            int y = 20;
            e.Graphics.DrawString("                    VARIEDADES NATHALIE", ft2, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));
            e.Graphics.DrawString("                    Fecha: " + date + "", ft2, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));
            e.Graphics.DrawString("                    AV.DR.MORILLO #29 ", ft2, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));
            e.Graphics.DrawString("                    Tel 829-781-4474          RNC. 036001734", ft2, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));
            e.Graphics.DrawString("                    Pago de Deuda", ft2, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));

            e.Graphics.DrawString("                                         ", ft, Brushes.Black, new RectangleF(0, y += 20, ancho, 20));
            e.Graphics.DrawString("                                         ", ft, Brushes.Black, new RectangleF(0, y += 20, ancho, 20));
            e.Graphics.DrawString("Suplidor:  " + txtnomcp.Text, ft, Brushes.Black, new RectangleF(0, y += 20, ancho, 20));
            e.Graphics.DrawString("-------------------------------------------------------------------------------------------------", ft, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));
            e.Graphics.DrawString("Deuda Original        Pagado a la Fecha         Pagado", ft, Brushes.Black, new RectangleF(0, y += 40, ancho, 20));
            e.Graphics.DrawString("        " + txtmontcp.Text + "                   " + txtmpagcp.Text + "                                 " + txtmotpacp.Text + "", ft, Brushes.Black, new RectangleF(0, y += 40, ancho, 20));


            e.Graphics.DrawString("-------------------------------------------------------------------------------------------------", ft, Brushes.Black, new RectangleF(0, y += 30, ancho, 20));



            while (sqlReader.Read())
            {






                e.Graphics.DrawString("Monto Restante: " + sqlReader["Restante"].ToString(), ft, Brushes.Black, new RectangleF(0, y += 40, ancho, 20));




            }
            e.Graphics.DrawString("-------------------------------------------------------------------------------------------------", ft, Brushes.Black, new RectangleF(0, y += 20, ancho, 20));


            conn.Close();





        }

        private void GroupBox12_Enter(object sender, EventArgs e)
        {

        }

        private void Txtbuspord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (string.IsNullOrEmpty(txtidstore.Text))
                    {
                        DateTime date = DateTime.Now;
                        var shortDate = date.ToString("dd/MM/yyyy");
                        cns.consultasinreaultado("INSERT INTO factura (fecha,fec_c) values('" + dtpv.Text + "','" + shortDate + "')");

                        ListViewItem listViewItem1 = new ListViewItem();
                        ListViewItem lv2 = new ListViewItem();
                        listViewItem1 = lvprod.SelectedItems[0];
                        string codigo, codvent;


                        SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                        {
                            conn.Open();
                            using (SQLiteCommand dataCommand1 = new SQLiteCommand("select produc from inventario where id_Cod ='" + txtbuspord.Text + "'", conn))
                            {
                                codigo = Convert.ToString(dataCommand1.ExecuteScalar());

                            }

                            using (SQLiteCommand dataCommand2 = new SQLiteCommand("SELECT id_fact FROM factura WHERE fecha IN(SELECT max(fecha) FROM factura);'", conn))
                            {
                                codvent = Convert.ToString(dataCommand2.ExecuteScalar());
                                txtidstore.Text = codvent;

                            }

                            conn.Close();


                        }


        ;



                        try
                        {
                            double p, c, pf;
                            p = Convert.ToDouble(txtprev.Text);
                            c = Convert.ToDouble(txtprodcant.Text);
                            pf = p * c;
                            txtprfin.Text = pf.ToString();




                            string firstColum = listViewItem1.Text;
                            string secondColum = codigo;
                            string tr3 = txtprev.Text;
                            string tr4 = txtprodcant.Text;
                            string tr5 = pf.ToString();
                            string tr1 = codvent.ToString();

                            string[] row = { tr1, firstColum, secondColum, tr3, tr4, tr5 };
                            dgvventa.Rows.Add(row);
                        }
                        catch (Exception ex)
                        {

                        }
                        //if (string.IsNullOrEmpty(txtidstore.Text))
                        //{
                        //    txtidstore.Text = dtpv.Text;
                        //    cns.consultasinreaultado("insert into Ventas (cant,inventario_id_cod,ven_id_fac)values('" + txtprodcant.Text + "','" + listViewItem.Text + "','" + txtidstore.Text + "')");
                        cns.consultasinreaultado("update inventario set canti_disp = (canti_disp - '" + txtprodcant.Text + "') where id_cod = '" + listViewItem1.Text + "'");
                        updatelv();

                        //    cns.consultasinreaultado("INSERT INTO factura (id_fact,fecha) values('" + txtidstore.Text + "','" + dtpv.Text + "')");
                        //    dgvventa.DataSource = cns.cosnsultaconresultado("select id_cod as Codigo, produc as Producto,precio as Precio, cant as Cantidad, (precio * cant) as Total from ventas   join inventario on id_cod = inventario_id_cod     where ven_id_fac = '" + txtidstore.Text + "'");
                        //   updatelv();


                        //}
                        //else
                        //{

                        //    cns.consultasinreaultado("insert into ventas (cant,inventario_id_cod,ven_id_fac)values('" + txtprodcant.Text + "','" + listViewItem.Text + "','" + txtidstore.Text + "')");
                        //    cns.consultasinreaultado("update inventario set canti_disp = (canti_disp - '" + txtprodcant.Text + "') where id_cod = '" + listViewItem.Text + "'");
                        //    dgvventa.DataSource = cns.cosnsultaconresultado("select id_cod as Codigo, produc as Producto,precio as Precio, cant as Cantidad, (precio * cant) as Total from ventas   join inventario on id_cod = inventario_id_cod     where ven_id_fac = '" + txtidstore.Text + "'");

                        carga();


                        //}

                        double sum = 0;
                        for (int i = 0; i < dgvventa.Rows.Count; ++i)
                        {
                            sum += Convert.ToDouble(dgvventa.Rows[i].Cells[5].Value);
                        }
                        txttp.Text = sum.ToString();

                        txtprev.Clear();
                        txtprodcant.Clear();
                        txtprfin.Clear();

                    }

                    else
                    {
                        ListViewItem listViewItem1 = new ListViewItem();
                        ListViewItem lv2 = new ListViewItem();
                        listViewItem1 = lvprod.SelectedItems[0];
                        string codigo, codvent;
                        SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                        {
                            conn.Open();
                            using (SQLiteCommand dataCommand1 = new SQLiteCommand("select produc from inventario where id_Cod ='" + txtbuspord.Text + "'", conn))
                            {
                                codigo = Convert.ToString(dataCommand1.ExecuteScalar());

                            }

                            using (SQLiteCommand dataCommand2 = new SQLiteCommand("SELECT id_fact FROM factura WHERE fecha IN(SELECT max(fecha) FROM factura);'", conn))
                            {
                                codvent = Convert.ToString(dataCommand2.ExecuteScalar());

                            }

                            conn.Close();


                        }


        ;



                        try
                        {
                            double p, c, pf;
                            p = Convert.ToDouble(txtprev.Text);
                            c = Convert.ToDouble(txtprodcant.Text);
                            pf = p * c;
                            txtprfin.Text = pf.ToString();




                            string firstColum = listViewItem1.Text;
                            string secondColum = codigo;
                            string tr3 = txtprev.Text;
                            string tr4 = txtprodcant.Text;
                            string tr5 = pf.ToString();
                            string tr1 = codvent.ToString();

                            string[] row = { tr1, firstColum, secondColum, tr3, tr4, tr5 };
                            dgvventa.Rows.Add(row);
                        }
                        catch (Exception ex)
                        {

                        }
                        //if (string.IsNullOrEmpty(txtidstore.Text))
                        //{
                        //    txtidstore.Text = dtpv.Text;
                        //    cns.consultasinreaultado("insert into Ventas (cant,inventario_id_cod,ven_id_fac)values('" + txtprodcant.Text + "','" + listViewItem.Text + "','" + txtidstore.Text + "')");
                        //    cns.consultasinreaultado("update inventario set canti_disp = (canti_disp - '" + txtprodcant.Text + "') where id_cod = '" + listViewItem.Text + "'");
                        //    cns.consultasinreaultado("INSERT INTO factura (id_fact,fecha) values('" + txtidstore.Text + "','" + dtpv.Text + "')");
                        //    dgvventa.DataSource = cns.cosnsultaconresultado("select id_cod as Codigo, produc as Producto,precio as Precio, cant as Cantidad, (precio * cant) as Total from ventas   join inventario on id_cod = inventario_id_cod     where ven_id_fac = '" + txtidstore.Text + "'");

                        carga();

                        //}
                        //else
                        //{

                        //    cns.consultasinreaultado("insert into ventas (cant,inventario_id_cod,ven_id_fac)values('" + txtprodcant.Text + "','" + listViewItem.Text + "','" + txtidstore.Text + "')");
                        cns.consultasinreaultado("update inventario set canti_disp = (canti_disp - '" + txtprodcant.Text + "') where id_cod = '" + listViewItem1.Text + "'");
                        updatelv();
                        //    dgvventa.DataSource = cns.cosnsultaconresultado("select id_cod as Codigo, produc as Producto,precio as Precio, cant as Cantidad, (precio * cant) as Total from ventas   join inventario on id_cod = inventario_id_cod     where ven_id_fac = '" + txtidstore.Text + "'");
                        //    updatelv();


                        //}

                        double sum = 0;
                        for (int i = 0; i < dgvventa.Rows.Count; ++i)
                        {
                            sum += Convert.ToDouble(dgvventa.Rows[i].Cells[5].Value);
                        }
                        txttp.Text = sum.ToString();

                        txtprev.Clear();
                        txtprodcant.Clear();
                        txtprfin.Clear();
                    }




                }
                catch (Exception ex)
                {

                }
            }
        }

        private void Txtpag_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double t, p, d;
                t = Convert.ToDouble(txttp.Text);
                p = Convert.ToDouble(txtpag.Text);
                d = (t - p) * -1;
                txtdev.Text = d.ToString();
            }
            catch (Exception ex)
            {

            }
        }

        private void Btnborrsup_Click(object sender, EventArgs e)
        {

        }

        private void Txtnc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtnnp.Focus();
            }
        }

        private void Txtnnp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtnp.Focus();
            }
        }

        private void Txtnp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtncant.Focus();
            }
        }

        private void Button21_Click(object sender, EventArgs e)
        {
            groupBox20.Visible = true;
        }

        private void Txtncant_KeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(txtidstore.Text))
            {
                DateTime date = DateTime.Now;
                var shortDate = date.ToString("dd/MM/yyyy");
                cns.consultasinreaultado("INSERT INTO factura (fecha,fec_c) values('" + dtpv.Text + "','" + shortDate + "')");


                string codigo, codvent;


                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                {
                    conn.Open();


                    using (SQLiteCommand dataCommand2 = new SQLiteCommand("SELECT id_fact FROM factura WHERE fecha IN(SELECT max(fecha) FROM factura);'", conn))
                    {
                        codvent = Convert.ToString(dataCommand2.ExecuteScalar());
                        txtidstore.Text = codvent;

                    }

                    conn.Close();
                    if (e.KeyCode == Keys.Enter)
                    {
                        try
                        {
                            double p, c, pf;
                            p = Convert.ToDouble(txtnp.Text);
                            c = Convert.ToDouble(txtncant.Text);
                            pf = p * c;
                            txtprfin.Text = pf.ToString();

                            string firstColum = txtnc.Text;
                            string secondColum = txtnnp.Text;
                            string tr3 = txtnp.Text;
                            string tr4 = txtncant.Text;
                            string tr5 = pf.ToString();
                            string tr1 = codvent.ToString();

                            string[] row = { tr1, firstColum, secondColum, tr3, tr4, tr5 };
                            dgvventa.Rows.Add(row);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            else
            {
                string codigo, codvent;


                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
                {
                    conn.Open();


                    using (SQLiteCommand dataCommand2 = new SQLiteCommand("SELECT id_fact FROM factura WHERE fecha IN(SELECT max(fecha) FROM factura);'", conn))
                    {
                        codvent = Convert.ToString(dataCommand2.ExecuteScalar());
                        txtidstore.Text = codvent;

                    }

                    conn.Close();
                    if (e.KeyCode == Keys.Enter)
                    {
                        try
                        {
                            double p, c, pf;
                            p = Convert.ToDouble(txtnp.Text);
                            c = Convert.ToDouble(txtncant.Text);
                            pf = p * c;
                            txtprfin.Text = pf.ToString();

                            string firstColum = txtnc.Text;
                            string secondColum = txtnnp.Text;
                            string tr3 = txtnp.Text;
                            string tr4 = txtncant.Text;
                            string tr5 = pf.ToString();
                            string tr1 = codvent.ToString();

                            string[] row = { tr1, firstColum, secondColum, tr3, tr4, tr5 };
                            dgvventa.Rows.Add(row);
                        }
                        catch (Exception ex)
                        {

                        }


                    }

                }
            }

        }

        private void Txtpagado_TextChanged(object sender, EventArgs e)
        {
            try
            {


                double des, tot, resul, resul2, pag;
                tot = Convert.ToDouble(lbpfi.Text);
                des = Convert.ToDouble(cbdesc.Text);
                pag = Convert.ToDouble(txtpagado.Text);
                if (des == 0)

                {
                    resul = pag - (tot +(tot*0.18));
                    txtdevuelta.Text = resul.ToString();


                }

                else
                {
                    resul2 = tot - tot * (des / 100);
                    resul = pag - (tot - tot * (des / 100));
                    txtdevuelta.Text = resul.ToString();
                    txxttcd.Text = resul2.ToString();

                }
            }
            catch
            {

            }
        }
        private void cut()
        {
            printDocument1 = new PrintDocument();
            PrinterSettings ps = new PrinterSettings();
            printDocument1.PrinterSettings = ps;
            //printDocument2.PrinterSettings.PrinterName = "Microsoft Print to PDF";
            printDocument1.PrintPage += printDocument1_PrintPage;
            printDocument1.Print();
        }
        private void Btnpag_Click(object sender, EventArgs e)
        {
            Actprddgv();
            generar();
            ACTPROD();
            groupBox21.Visible = false;
            txttpag.Clear();
            txtpagado.Clear();
            txtdevuelta.Clear();
            txtnfact.Clear();
            cbdesc.Text = "0";
            txxttcd.Clear();
        }

        private void Button22_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            for (int i = 1; i < dgvcot.Columns.Count + 1; i++)
            {
                DataColumn column = new DataColumn(dgvcot.Columns[i - 1].HeaderText);
                dt.Columns.Add(column);
            }
            int columnCount = dgvcot.Columns.Count;
            foreach (DataGridViewRow dr in dgvcot.Rows)
            {
                DataRow dataRow = dt.NewRow();
                for (int i = 0; i < columnCount; i++)
                {
                    //returns checkboxes and dropdowns as string with .value..... nearly got it
                    dataRow[i] = dr.Cells[i].Value;
                }
                dt.Rows.Add(dataRow);
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);


            XmlTextWriter xmlSave = new XmlTextWriter(@"C:\bdd\ctzn/DGVXML.xml", Encoding.UTF8);
            ds.WriteXml(xmlSave);
            xmlSave.Close();


            cotiz f = new cotiz();
            CrystalReport5 cr = new CrystalReport5();
            TextObject text = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txtclicr"];
            //TextObject text1 = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txtcrced"];
            //TextObject text2 = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["txtpagcr"];
            TextObject text3 = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txttelcr"];
            TextObject text4 = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["txtcrtt"];



            text.Text = txtcocli.Text;
            //text1.Text = txtcedcot.Text;
            //text2.Text = txtpagado.Text;
            text3.Text = txtcttel.Text;
            text4.Text = lbpfi.Text;
            f.crystalReportViewer1.ReportSource = cr;
            f.Show();
            dgvcot.Rows.Clear();
            txtidcred.Clear();
            txtcocli.Clear();
            txtcedcli.Clear();
            txtcedcot.Clear();
            txtdireccot.Clear();
            txtcttel.Clear();
            txtprecot.Clear();
            txtcantcot.Clear();
            txtprfcot.Clear();
            lbpfi.Text = "";

        }
        public static class Prompt
        {
            public static string ShowDialog(string text, string caption)
            {
                Form prompt = new Form()
                {
                    Width = 500,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };
                Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
                TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
                textBox.PasswordChar = '*';
                Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
            }
        }
        private void Cbdesc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int val;
                try
                {


                    double des, tot, resul, resul2, pag;
                    tot = Convert.ToDouble(lbpfi.Text);
                    des = Convert.ToDouble(cbdesc.Text);
                    pag = Convert.ToDouble(txtpagado.Text);
                    if (des == 0)

                    {
                        resul = pag - (tot/*+tot*0.18*/);
                        txtdevuelta.Text = resul.ToString();


                    }

                    else
                    {
                        resul2 = ((tot + /*(tot * 0.18))*/ -( tot * (des / 100))));
                        resul = pag - ((tot/*+(tot*0.18)*/) - tot * (des / 100));
                        txtdevuelta.Text = resul.ToString();
                        txxttcd.Text = resul2.ToString();

                    }
                }
                catch
                {

                }


                val = Convert.ToInt32(cbdesc.Text);



                if (val >= 10)
                {
                    string pass = "12345678";
                    MessageBox.Show("Necesitas Permsisos de admistrador, consulta a Vanesa");

                    string promptValue = Prompt.ShowDialog("Administrador", "");
                    if (promptValue == pass)
                    {


                    }

                    else
                    {
                        MessageBox.Show("Contraseña Incorrecta!, Vulva a intentarlo");
                        cbdesc.Text = "0";
                    }



                }

                else
                {

                }
            }
            catch (Exception ex)
            {

            }



        }

        private void Button23_Click(object sender, EventArgs e)
        {
            groupBox21.Visible = false;
        }

        private void Txtcttel_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button24_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            for (int i = 1; i < dgvcot.Columns.Count + 1; i++)
            {
                DataColumn column = new DataColumn(dgvcot.Columns[i - 1].HeaderText);
                dt.Columns.Add(column);
            }
            int columnCount = dgvcot.Columns.Count;
            foreach (DataGridViewRow dr in dgvcot.Rows)
            {
                DataRow dataRow = dt.NewRow();
                for (int i = 0; i < columnCount; i++)
                {
                    //returns checkboxes and dropdowns as string with .value..... nearly got it
                    dataRow[i] = dr.Cells[i].Value;
                }
                dt.Rows.Add(dataRow);
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);


            XmlTextWriter xmlSave = new XmlTextWriter(@"C:\bdd\ctzn/DGVXML.xml", Encoding.UTF8);
            ds.WriteXml(xmlSave);
            xmlSave.Close();


            cotiz f = new cotiz();
            CrystalReport5 cr = new CrystalReport5();
            TextObject text = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txtclicr"];
            //TextObject text1 = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txtcrced"];
            //TextObject text2 = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["txtpagcr"];
            TextObject text3 = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txttelcr"];
            TextObject text4 = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["txtcrtt"];



            text.Text = txtcocli.Text;
            //text1.Text = txtcedcot.Text;
            //text2.Text = txtpagado.Text;
            text3.Text = txtcttel.Text;
            text4.Text = lbpfi.Text;
            f.crystalReportViewer1.ReportSource = cr;
            f.Show();
            dgvcot.Rows.Clear();
            txtidcred.Clear();
            txtcocli.Clear();
            txtcedcli.Clear();
            txtcedcot.Clear();
            txtdireccot.Clear();
            txtcttel.Clear();
            txtprecot.Clear();
            txtcantcot.Clear();
            txtprfcot.Clear();
            lbpfi.Text = "";
        }

        private void Button22_Click_1(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            for (int i = 1; i < dgvcot.Columns.Count + 1; i++)
            {
                DataColumn column = new DataColumn(dgvcot.Columns[i - 1].HeaderText);
                dt.Columns.Add(column);
            }
            int columnCount = dgvcot.Columns.Count;
            foreach (DataGridViewRow dr in dgvcot.Rows)
            {
                DataRow dataRow = dt.NewRow();
                for (int i = 0; i < columnCount; i++)
                {
                    //returns checkboxes and dropdowns as string with .value..... nearly got it
                    dataRow[i] = dr.Cells[i].Value;
                }
                dt.Rows.Add(dataRow);
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);


            XmlTextWriter xmlSave = new XmlTextWriter(@"C:\bdd\ctzn/DGVXML.xml", Encoding.UTF8);
            ds.WriteXml(xmlSave);
            xmlSave.Close();


            cotiz f = new cotiz();
            CrystalReport5 cr = new CrystalReport5();
            TextObject text = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txtclicr"];
            //TextObject text1 = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txtcrced"];
            //TextObject text2 = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["txtpagcr"];
            TextObject text3 = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txttelcr"];
            TextObject text4 = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["txtcrtt"];
            TextObject text6 = (TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["txtdirec"];


            text6.Text = txtdireccot.Text;
            text.Text = txtcocli.Text;
            //text1.Text = txtcedcot.Text;
            //text2.Text = txtpagado.Text;
            text3.Text = txtcttel.Text;
            text4.Text = lbpfi.Text;
            f.crystalReportViewer1.ReportSource = cr;
            f.Show();
            dgvcot.Rows.Clear();
            txtidcred.Clear();
            txtcocli.Clear();
            txtcedcli.Clear();
            txtcedcot.Clear();
            txtdireccot.Clear();
            txtcttel.Clear();
            txtprecot.Clear();
            txtcantcot.Clear();
            txtprfcot.Clear();
            lbpfi.Text = "";
        }

        private void Dgvcot_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void Dgvcot_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dgvcot.Rows)
                {
                    if (row.Cells[2].Value == null)
                    {
                        break;
                    }

                    double a = Convert.ToDouble(row.Cells[2].Value.ToString());
                    double b = Convert.ToDouble(row.Cells[3].Value.ToString());

                    row.Cells[4].Value = (a * b).ToString();
                    sumtot();
                }
            }
            catch
            {


            }
        }

        private void sumtot()
        {
            double sum = 0;
            for (int i = 0; i < dgvcot.Rows.Count; ++i)
            {
                sum += Convert.ToDouble(dgvcot.Rows[i].Cells[4].Value);
            }
            lbpfi.Text = sum.ToString();
        }


        private void factdevt()
        {
            DateTime date = DateTime.Now;
            var shortDate = date.ToString("dd/MM/yyyy");
            if (string.IsNullOrEmpty(txxttcd.Text))
            {
                cns.consultasinreaultado("INSERT INTO factura (id_fact,fecha,fec_c,ttdv) values('" + txtnfact.Text + "','" + dtpcot.Text + "','" + shortDate + "','" + txttpag.Text + "')");
            }

            else
            {
                cns.consultasinreaultado("INSERT INTO factura (id_fact,fecha,fec_c,ttdv) values('" + txtnfact.Text + "','" + dtpcot.Text + "','" + shortDate + "','" + txxttcd.Text + "')");
            }
        }

        private void Dgvdencar_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            DataGridViewRow act = dgvdencar.Rows[e.RowIndex];
            if (string.IsNullOrEmpty(txtidprodencar.Text))
            {
                txtidprodencar.Text = act.Cells["ID"].Value.ToString();
            }
            else
            {
                txtidcliencar.Text = act.Cells["Id_Cod"].Value.ToString();
            }
        }

        private void Txtidprodencar_TextChanged(object sender, EventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
            {

                SQLiteCommand sqlCmd = new SQLiteCommand("SELECT * FROM Cliente where id_client ='" + txtidprodencar.Text + "' ", conn);

                conn.Open();
                SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();

                while (sqlReader.Read())
                {

                    txtnombencar.Text = sqlReader["nombre"].ToString();

                    txttelenc.Text = sqlReader["tel"].ToString();









                }



                sqlReader.Close();
                dgvdencar.DataSource = cns.cosnsultaconresultado("select id_cod,produc as Producto,precio as Precio,canti_disp as Disponible from inventario");
            }
        }

        private void Txtidcliencar_TextChanged(object sender, EventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
            {

                SQLiteCommand sqlCmd = new SQLiteCommand("SELECT * FROM inventario where id_cod ='" + txtidcliencar.Text + "' ", conn);

                conn.Open();
                SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();

                while (sqlReader.Read())
                {

                    txtpreenc.Text = sqlReader["precio"].ToString();











                }
                txtcantenc.Text = "1";




            }
        }

        private void Txtcantenc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double x, y, resulV;
                x = Convert.ToDouble(txtpreenc.Text);
                y = Convert.ToDouble(txtcantenc.Text);
                resulV = x * y;
                txttprdencar.Text = resulV.ToString();
            }
            catch
            {

            }

        }

        private void Txtcantcot_TextChanged(object sender, EventArgs e)
        {

        }

        private void Btnagreencarg_Click(object sender, EventArgs e)
        {
            string codigo;
            SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
            {
                conn.Open();
                using (SQLiteCommand dataCommand1 = new SQLiteCommand("select produc from inventario where id_Cod ='" + txtidcliencar.Text + "'", conn))
                {
                    codigo = Convert.ToString(dataCommand1.ExecuteScalar());

                }
                conn.Close();

                string firstColum = txtidcliencar.Text;
                string secondColum = codigo;
                string tr3 = txtpreenc.Text;
                string tr4 = txtcantenc.Text;
                string tr5 = textBox5.Text;
                string tr6 = textBox6.Text;








                string[] row = { firstColum, secondColum, tr3, tr4, tr5, tr6 };
                dgvencar.Rows.Add(row);




                if (string.IsNullOrEmpty(txtnecna.Text))
                {
                    int codvent;
                    using (SQLiteCommand dataCommand2 = new SQLiteCommand("SELECT id_encar FROM encar WHERE id_encar IN(SELECT max(id_encar) FROM encar);;'", conn))
                    {
                        conn.Open();
                        codvent = Convert.ToInt32(dataCommand2.ExecuteScalar());
                        txtnecna.Text = (codvent + 1).ToString();
                        conn.Close();





                    }
                }

                else
                {

                }
            }
        }
        private void Dgvencar_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dgvencar.Rows)
                {
                    if (row.Cells[2].Value == null)
                    {
                        break;
                    }

                    double a = Convert.ToDouble(row.Cells[2].Value.ToString());
                    double b = Convert.ToDouble(row.Cells[3].Value.ToString());
                    double c = Convert.ToDouble(row.Cells[5].Value.ToString());

                    row.Cells[6].Value = ((a * b) + c).ToString();

                }
            }
            catch
            {


            }
        }

        private void Dgvencar_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dgvencar.Rows)
                {
                    if (row.Cells[2].Value == null)
                    {
                        break;
                    }

                    double a = Convert.ToDouble(row.Cells[2].Value.ToString());
                    double b = Convert.ToDouble(row.Cells[3].Value.ToString());
                    double c = Convert.ToDouble(row.Cells[5].Value.ToString());

                    row.Cells[6].Value = ((a * b) + c).ToString();

                }
            }
            catch
            {


            }

        }
        private void geca()
        {
            string StrQuery;

            SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
            {
                try
                {






                    using (SQLiteCommand comm = new SQLiteCommand())
                    {
                        comm.Connection = conn;

                        for (int i = 0; i < dgvencar.Rows.Count - 1; i++)
                        {
                            conn.Open();
                            StrQuery = "INSERT INTO encar(numeencar,id_cli,item,cat,mod,pre) VALUES ('"
                                + txtnecna.Text + "', '"
                                + txtidprodencar.Text + "', '"
                                + dgvencar.Rows[i].Cells[0].Value.ToString() + "', '"
                                + dgvencar.Rows[i].Cells[3].Value.ToString() + "','"
                                + dgvencar.Rows[i].Cells[4].Value.ToString() + "','"
                                + dgvencar.Rows[i].Cells[5].Value.ToString() + "')";
                            comm.CommandText = StrQuery;
                            comm.ExecuteNonQuery();

                            conn.Close();
                            carga();
                            dgvencar.Rows.Clear();



                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }


        }




        private void Button24_Click_1(object sender, EventArgs e)
        {
            geca();

        }

        private void Button24_Click_2(object sender, EventArgs e)
        {
            geca();
        }

        private void bac()

        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");

            if (string.IsNullOrEmpty(txtbarcode.Text))
            {

            }
            else
            {
                SQLiteDataAdapter ad;
                DataTable dt = new DataTable();
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select id_Cod from inventario where id_Cod = '" + txtbarcode.Text + "'";
                ad = new SQLiteDataAdapter(cmd);

                DataSet ds = new DataSet();
                ad.Fill(dt);
                ds.Tables.Add(dt);
                if (dt.Rows.Count <= 0)
                {
                    MessageBox.Show("Este Codigo No Existe");
                    txtbarcode.Clear();
                    txtbarcode.Focus();
                }

                else
                {



                    string codigo, produc, precio;
                    {
                        conn.Open();
                        using (SQLiteCommand dataCommand1 = new SQLiteCommand("select id_Cod from inventario where id_Cod ='" + txtbarcode.Text + "'", conn))
                        {
                            codigo = Convert.ToString(dataCommand1.ExecuteScalar());

                        }
                        using (SQLiteCommand dataCommand2 = new SQLiteCommand("select produc from inventario where id_Cod ='" + txtbarcode.Text + "'", conn))
                        {
                            produc = Convert.ToString(dataCommand2.ExecuteScalar());

                        }
                        using (SQLiteCommand dataCommand3 = new SQLiteCommand("select precio from inventario where id_Cod ='" + txtbarcode.Text + "'", conn))
                        {
                            precio = Convert.ToString(dataCommand3.ExecuteScalar());

                        }

                        conn.Close();

                        string firstColum = codigo;
                        string secondColum = produc;
                        string tr3 = precio;
                        string tr4 = "1";
                        string tr5 = precio;









                        string[] row = { firstColum, secondColum, tr3, tr4, tr5 };
                        dgvcot.Rows.Add(row);
                        double sum = 0;
                        for (int i = 0; i < dgvcot.Rows.Count; ++i)
                        {
                            sum += Convert.ToDouble(dgvcot.Rows[i].Cells[4].Value);
                        }
                        lbpfi.Text = sum.ToString();
                        txtbarcode.Clear();
                        txtbarcode.Focus();
                    }
                }

            }
        }

        private void Txtbarcode_TextChanged(object sender, EventArgs e)
        {

           
        }

        private void Txtbarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bac();
            }
        }

        private void Txtbarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void Button25_Click(object sender, EventArgs e)
        {
           
            Actprddgv();
            ACTPROD();
            
        }

        private void Btnagreen_Click(object sender, EventArgs e)
        {
            cns.consultasinreaultado("insert into engre(egre,consec,fecha)values('"+txtegre.Text+"','"+txtconcep.Text+"','"+dtpegre.Text+"')");
            cns.consultasinreaultado("insert into factura(fecha,fec_c,ttdv)values('"+dtpegre.Text+ "','" + dtpegre.Text + "','-"+txtegre.Text+"')");
            dgvegre.DataSource = cns.cosnsultaconresultado("select * from engre");
        }

        private void txtpre_TextChanged(object sender, EventArgs e)
        {
            if (txtpre.Text.Trim() == string.Empty && txtprecomp.Text.Trim() == string.Empty)
            {

            }
            else
            {
                try
                {
                    double prec, prev, ben;
                    prec = Convert.ToDouble(txtprecomp.Text.Trim());
                    prev = Convert.ToDouble(txtpre.Text.Trim());
                    ben = (prev - prec);
                    lblben.Text = ben.ToString();
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void lblben_Click(object sender, EventArgs e)
        {

        }

        private void lblben_TextChanged(object sender, EventArgs e)
        {
            double ben;
            ben = Convert.ToDouble(lblben.Text.Trim());
            if(ben >= 0)
            {
                lblben.ForeColor = Color.Green;
            }
            else
            {
                lblben.ForeColor = Color.Red;

            }
        }

        private void btnlimoinv_Click(object sender, EventArgs e)
        {
            txtcodprod.Clear();
            txtnombprod.Clear();
            txttipprod.Clear();
            txtprodcant.Clear();
            txtpre.Clear();
            txtprecomp.Clear();
            txtinvcant.Clear();
        }

        private void label33_Click(object sender, EventArgs e)
        {

        }
    }
}




    




        

    
    



    

