using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using DAL;
using ET;



namespace SistemaLabco
{
    public partial class FrmInicio : Form
    {
        public FrmInicio()
        {
            InitializeComponent();
        }


        #region Variables globales
        int EstadoGuarda = 0;
        #endregion 

        #region Variables de Marca
        int IdMarca = 0;

        #endregion




        #region Metodo Marca

        private void FormatoMa()
        {
            dataGridViewMarca.Columns[0].Width = 100;
            dataGridViewMarca.Columns[0].HeaderText = "ID Marca";
            dataGridViewMarca.Columns[1].Width = 100;
            dataGridViewMarca.Columns[1].HeaderText = "Marca";

        }

        private void ListadoMarca(string cTexto)
        {

            try
            {
                dataGridViewMarca.DataSource = BLMarca.ListadoMA(cTexto);
                this.FormatoMa();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }



        }



        private void ActualizarMarca()
        {
            
            GuardarMarca();
            this.ListadoMarca("%");
        }



        private void SeleccionaItemMarca()
        {
            //Validasmos que el DATAGEIP tenga datos para que no nos de error

            if (string.IsNullOrEmpty(Convert.ToString(dataGridViewMarca.CurrentRow.Cells["IDMarca"].Value)))
            {
                MessageBox.Show("No hay datos que mostrar", "Aviso del sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                this.IdMarca = Convert.ToInt32(dataGridViewMarca.CurrentRow.Cells["IDMarca"].Value);
                TxTNombreMarca.Text = Convert.ToString(dataGridViewMarca.CurrentRow.Cells["Nombre"].Value);

            }

        }




        private void ModificarMarca()
        {
            EstadoGuarda = 2; // Indica que se trata de una actualización

            // Verificar si hay una fila seleccionada en el DataGridView
            if (dataGridViewMarca.CurrentRow != null)
            {
                // Obtener el IDCliente de la fila seleccionada
                this.IdMarca = Convert.ToInt32(dataGridViewMarca.CurrentRow.Cells["IDMarca"].Value);

                // Poblar los campos con los datos actuales del cliente
                TxTNombreMarca.Text = Convert.ToString(dataGridViewMarca.CurrentRow.Cells["Nombre"].Value);


                // Establecer el enfoque en el primer campo editable
                TxTNombreMarca.Focus();
            }
            else
            {
                MessageBox.Show("Seleccione un cliente para modificar.", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void GuardarMarca()
        {

            if (TxTNombreMarca.Text == String.Empty)
            {
                MessageBox.Show("Falta ingresar datos requeridos(*)", "Aviso del sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {

                ETMarca etmarca = new ETMarca();
                string Rpta = "";
                etmarca.IDMarca = this.IdMarca;
                etmarca.Nombre = TxTNombreMarca.Text.Trim();
                Rpta = BLMarca.GuardarMA(EstadoGuarda, etmarca);


                if (Rpta == "OK")
                {
                    this.ListadoMarca("%");
                    MessageBox.Show("Los datos se han registrado", "Aviso del sistema", MessageBoxButtons.OK,
                   MessageBoxIcon.Information);
                    EstadoGuarda = 0; // si no guardo nada
                    TxTNombreMarca.Text = "";
                    TxTNombreMarca.ReadOnly = false;
                    this.IdMarca = 0;

                }
                else
                {
                    MessageBox.Show(Rpta, "Aviso del sistema", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);

                }

            }



        }

        #endregion


        #region Metodo Marca
        private void buttonGuardarMarca_Click(object sender, EventArgs e)
        {
            EstadoGuarda = 1;
            GuardarMarca();

        }

        private void buttonBuscarMarca_Click(object sender, EventArgs e)
        {
            this.ListadoMarca(TxTBuscarMarca.Text.Trim());

        }

        private void buttonCancelarMarca_Click(object sender, EventArgs e)
        {
            EstadoGuarda = 0;//Sin ninguna accion
            this.IdMarca = 0;
            TxTNombreMarca.Text = "";
            TxTNombreMarca.ReadOnly = true;

        }

        private void buttonEliminarMarca_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(dataGridViewMarca.CurrentRow.Cells["IDMarca"].Value)))
            {
                MessageBox.Show("No hay datos que mostrar", "Aviso del sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                DialogResult opcion;
                opcion = MessageBox.Show("Esta seguro de eliminar el registro seleccionado ?", "Aviso del sistema", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (opcion == DialogResult.Yes)
                {
                    string Rpta = "";
                    this.IdMarca = Convert.ToInt32(dataGridViewMarca.CurrentRow.Cells["IDMarca"].Value);
                    Rpta = BLMarca.EliminarMA(this.IdMarca);

                    if (Rpta.Equals("OK"))
                    {
                        this.ListadoMarca("%");//LLAMAMOS EL METODO PARA ACTUALIZAR LA LISTA
                        this.IdMarca = 0;
                        MessageBox.Show("Registro eliminado", "Aviso del sistema", MessageBoxButtons.OK,
                   MessageBoxIcon.Exclamation);
                    }

                }


            }

        }

    

        #endregion

        private void buttonGuardarMarca_Click_1(object sender, EventArgs e)
        {


            if (EstadoGuarda == 2)
            {
                // Actualización del cliente
                ActualizarMarca();
            }
            else if (EstadoGuarda == 1)
            {
                // Inserción de nuevo cliente
                GuardarMarca();
            }
            else
            {
                MessageBox.Show("El estado de guardado no está definido.", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }



        }

        private void buttonCancelarMarca_Click_1(object sender, EventArgs e)
        {
            
            TxTNombreMarca.Text = "";
            TxTNombreMarca.ReadOnly = false;
        }

        private void buttonBuscarMarca_Click_1(object sender, EventArgs e)
        {
            this.ListadoMarca(TxTBuscarMarca.Text.Trim());
        }

        private void buttonEliminarMarca_Click_1(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(Convert.ToString(dataGridViewMarca.CurrentRow.Cells["IDMarca"].Value)))
            {
                MessageBox.Show("No hay datos que mostrar", "Aviso del sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                DialogResult opcion;
                opcion = MessageBox.Show("Esta seguro de eliminar el registro seleccionado ?", "Aviso del sistema", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (opcion == DialogResult.Yes)
                {
                    string Rpta = "";
                    this.IdMarca = Convert.ToInt32(dataGridViewMarca.CurrentRow.Cells["IDMarca"].Value);
                    Rpta = BLMarca.EliminarMA(this.IdMarca);

                    if (Rpta.Equals("OK"))
                    {
                        this.ListadoMarca("%");//LLAMAMOS EL METODO PARA ACTUALIZAR LA LISTA
                        this.IdMarca = 0;
                        MessageBox.Show("Registro eliminado", "Aviso del sistema", MessageBoxButtons.OK,
                   MessageBoxIcon.Exclamation);
                    }

                }


            }

        }

        private void buttonModificarMarca_Click(object sender, EventArgs e)
        {
            ModificarMarca();
        }
    }
    }
