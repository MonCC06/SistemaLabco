using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using ET;
namespace SistemaLabco
{
    public partial class FrmInicio : Form
    {
        public FrmInicio()
        {
            InitializeComponent();
        }


        //
        int EstadoGuarda = 1;
        int IDProducto = 0;

        #region Metodo Producto
        private void FormatoPR()
        {
            DGVProducto.Columns[0].Width = 90;
            DGVProducto.Columns[0].HeaderText = "ID Producto";
            DGVProducto.Columns[1].Width = 240;
            DGVProducto.Columns[1].HeaderText = "Descripcion";
            DGVProducto.Columns[2].Width = 150;
            DGVProducto.Columns[2].HeaderText = "Precio";
            DGVProducto.Columns[3].Width = 80;
            DGVProducto.Columns[3].HeaderText = "Stock_Actual";
        }


        private void ListadoPR(string cTexto)
        {
            try
            {
                DGVProducto.DataSource = BLProducto.ListadoPR(cTexto);
                this.FormatoPR();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + ex.StackTrace);
            }
            finally
            {

            }
        }

        private void BotonesProducto(bool LEstado)
        {
            this.BTGuardarProducto.Enabled = LEstado;
            this.BTEliminararProducto.Enabled = LEstado;
            this.BTModificarProducto.Enabled = LEstado;
            this.BTBusarProducto.Enabled = LEstado;
            this.BTCancelarProducto.Enabled = LEstado;
        }


       private void SeleccionaProducto()
        {
            if (string.IsNullOrEmpty(Convert.ToString(DGVProducto.CurrentRow.Cells["IDProducto"].Value)))
            {

                MessageBox.Show("No hay datos que mostrar", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            else
            {
                //Convertir id de producto de celda de string a entero
                this.IDProducto = Convert.ToInt32(DGVProducto.CurrentRow.Cells["IDProducto"].Value);
                TBDescripcionServicio.Text = Convert.ToString(DGVProducto.CurrentRow.Cells["Descripcion"].Value);
                TBPrecioProducto.Text = Convert.ToString(DGVProducto.CurrentRow.Cells["Precio"].Value);
                TBStockProducto.Text = Convert.ToString(DGVProducto.CurrentRow.Cells["Stock_Actual"].Value);




            }


        }
        

        private void ActualizarProducto()
        {

            GuardarProducto();
            this.ListadoPR("%");
        }


        private void ModificarProducto()
        {

            EstadoGuarda = 2;

            if (DGVProducto.CurrentRow != null)
            {

                this.IDProducto = Convert.ToInt32(DGVProducto.CurrentRow.Cells["IDProducto"].Value);


                TBDescripcionProducto.Text = Convert.ToString(DGVProducto.CurrentRow.Cells["Descripcion"].Value);
                TBPrecioProducto.Text = Convert.ToString(DGVProducto.CurrentRow.Cells["Precio"].Value);
                TBStockProducto.Text = Convert.ToString(DGVProducto.CurrentRow.Cells["Stock_Actual"].Value);
                TBDescripcionServicio.Focus();
            
            }

            else
            {

                MessageBox.Show("Seleccione un producto para modificar", "Aviso del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }




      private void GuardarProducto()
        {
           
            if (TBDescripcionProducto.Text == String.Empty)
            {


                MessageBox.Show("Descripcion del producto requerida(*)", "Aviso del sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            if (TBPrecioProducto.Text == String.Empty)
            {


                MessageBox.Show("Precio del producto requerido(*)", "Aviso del sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            if (TBStockProducto.Text == String.Empty)
            {


                MessageBox.Show("Stock del producto requerido(*)", "Aviso del sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



            else
            {

                ETProducto eTProducto = new ETProducto();

                String Rpta = "";
                eTProducto.IDProducto = this.IDProducto;


                eTProducto.Descripcion = TBDescripcionProducto.Text.Trim();
                eTProducto.Precio = float.Parse(TBPrecioProducto.Text.Trim()); // Convierte el precio
                eTProducto.StockActual = int.Parse(TBStockProducto.Text.Trim()); // Convierte el stock




                try
                {

                    Rpta = BLProducto.GuardarPR(EstadoGuarda, eTProducto);

                    if (Rpta == "OK")
                    {

                        MessageBox.Show("Los datos se han registrado", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        EstadoGuarda = 1;
                        this.BotonesProducto(true);

                        TBDescripcionProducto.Text = "";
                        TBPrecioProducto.Text = "";
                        TBStockProducto.Text = "";
                        this.IDProducto = 0;
                    }


                    else
                    {
                        MessageBox.Show("No se logró registrar el dato: " + Rpta, "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);


                    }

                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }


            }





        #endregion

        private void BTGuardarProducto_Click(object sender, EventArgs e)
        {
            if (EstadoGuarda == 2)
            {

                ActualizarProducto();


            }

            else if (EstadoGuarda == 1)
            {

                GuardarProducto();
            }

            else
            {
                MessageBox.Show("El estado de guardado no está definido.", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }

        private void BTBusarProducto_Click(object sender, EventArgs e)
        {
            this.ListadoPR(TBBuscarProducto.Text.Trim());
        }

        private void BTCancelarProducto_Click(object sender, EventArgs e)
        {
            this.BotonesProducto(true);
            TBDescripcionProducto.Text = "";
            TBPrecioProducto.Text = "";
            TBStockProducto.Text = "";
        }

        private void BTModificarProducto_Click(object sender, EventArgs e)
        {
            ModificarProducto();
        }

        private void BTEliminararProducto_Click(object sender, EventArgs e)
        {
if (String.IsNullOrEmpty(Convert.ToString(DGVProducto.CurrentRow.Cells["IDProducto"].Value)))
            {
                MessageBox.Show("No hay datos que mostar", "Aviso del Sistema", MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
            }
            else
            {
                DialogResult opcion;
                //preguntar si se quiere realizar procedimiento y a opcion se le asignael valor de la respuesta
                opcion = MessageBox.Show("¿Está seguro de eliminar el registro seleccionado?", "Aviso del Sistema", MessageBoxButtons.YesNoCancel,
                   MessageBoxIcon.Question);
                if (opcion == DialogResult.Yes)
                {
                    String Rpta = "";

                    this.IDProducto = Convert.ToInt32(DGVProducto.CurrentRow.Cells["IDProducto"].Value);

                    Rpta = BLProducto.EliminaPR(this.IDProducto);


                    if (Rpta.Equals("OJ"))
                    {

                        this.IDProducto = 0;
                        MessageBox.Show("Registro Eliminado", "Aviso del Sistema", MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Exclamation);
                    }
                }
        }
    }


  
}

}

