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
using ET;

namespace SistemaLabco
{
    public partial class FrmInicio : Form
    {
        public FrmInicio()
        {
            InitializeComponent();
        }
        int IDServicio = 0;
        int EstadoGuarda = 1;
        private void FormatoServicio()
        {
            if (DGVServicio.Columns.Count >= 4) // Verifica si hay al menos 6 columnas
            {
                DGVServicio.Columns[0].Width = 100;
                DGVServicio.Columns[0].HeaderText = "IDServico";
                DGVServicio.Columns[1].Width = 100;
                DGVServicio.Columns[1].HeaderText = "Descripcion";
                DGVServicio.Columns[2].Width = 100;
                DGVServicio.Columns[2].HeaderText = "Monto";
              

            }
            

        }
        private void ListadoServicio(string tTexto)
        {
            try
            {
                DGVServicio.DataSource = BLServicio.ListadoSE(tTexto);
                this.FormatoServicio();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
            //datasource nos dice de donde vamos a consumir los datos
        }
        private void BotonesServicio(bool LEstado)
        {
            this.BTGuardarServicio.Enabled = LEstado;
            this.BTBuscarServicio.Enabled = LEstado;
            this.BTModificarServicio.Enabled = LEstado;
            this.BTCancelarServicio.Enabled = LEstado;
            this.BTEliminarServicio.Enabled = LEstado;
        }
        private void SeleccionaServicio()
        {
            //convertir dato de string a un valor 
            if (string.IsNullOrEmpty(Convert.ToString(DGVServicio.CurrentRow.Cells["IDServicio"].Value)))
            {
                MessageBox.Show("No hay datos que mostar", "Aviso del Sistema", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
            else
            {
                //convertir el id del cliente de la celda seleccionada de string a entero
                this.IDServicio = Convert.ToInt32(DGVServicio.CurrentRow.Cells["IDTrabajador"].Value);
                TBDescripcionServicio.Text = Convert.ToString(DGVServicio.CurrentRow.Cells["Descripcion"].Value);
                TBPrecioServicio.Text = Convert.ToString(DGVServicio.CurrentRow.Cells["Monto"].Value);
                
            }
        }
        private void ActualizarServicio()
        {
            // Aquí puedes utilizar el método GuardarCliente o crear un método específico para actualización.
            // Asumiendo que `GuardarCliente` se usa para ambos casos:
            GuardarServicio();
            this.ListadoServicio("%");
        }
        private void ModificarServicio()
        {
            EstadoGuarda = 2; // Indica que se trata de una actualización

            // Verificar si hay una fila seleccionada en el DataGridView
            if (DGVServicio.CurrentRow != null)
            {
                // Obtener el IDCliente de la fila seleccionada
                this.IDServicio = Convert.ToInt32(DGVServicio.CurrentRow.Cells["IDServicio"].Value);

                // Poblar los campos con los datos actuales del cliente
                TBDescripcionServicio.Text = Convert.ToString(DGVServicio.CurrentRow.Cells["Descripcion"].Value);
                TBPrecioServicio.Text = Convert.ToString(DGVServicio.CurrentRow.Cells["Monto"].Value);



                // Establecer el enfoque en el primer campo editable
                TBDescripcionServicio.Focus();
            }
            else
            {
                MessageBox.Show("Seleccione un Trabaajador para modificar.", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        private void GuardarServicio()
        {
            // Verificación de campos vacíos en una sola condición
            if (string.IsNullOrWhiteSpace(TBDescripcionServicio.Text) ||
                string.IsNullOrWhiteSpace(TBPrecioServicio.Text))
            {
                MessageBox.Show("Todos los campos son requeridos(*)", "Aviso del Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return; // Para detener la ejecución si algún campo está vacío
            }

            ETServicio eTServicio = new ETServicio();
            String Rpta = "";

            // Asignación de valores cliente
            eTServicio.IDServicio = this.IDServicio;
            eTServicio.Descripcion = TBDescripcionServicio.Text.Trim();
            eTServicio.Monto = float.Parse(TBPrecioServicio.Text.Trim());



            try
            {
                Rpta = BLServicio.GuardarSE(EstadoGuarda, eTServicio);

                if (Rpta == "OK")
                {
                    MessageBox.Show("Los datos se han registrado", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reinicio de valores
                    EstadoGuarda = 0;
                    this.BotonesServicio(true);
                    TBDescripcionServicio.Text = "";
                    TBPrecioServicio.Text = "";
                    
                    this.IDServicio= 0;
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
        private void FrmInicio_Load(object sender, EventArgs e)
        {

            this.ListadoServicio("%");

        }
        //inician los botones PELIGRO!!!
        private void BTGuardarServicio_Click(object sender, EventArgs e)
        {
            if (EstadoGuarda == 2)
            {
                // Actualización de Trabajador
                ActualizarServicio();
            }
            else if (EstadoGuarda == 1)
            {
                // Inserción de nuevo Trabajador
                GuardarServicio();
            }
            else
            {
                MessageBox.Show("El estado de guardado no está definido.", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BTEliminarServicio_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Convert.ToString(DGVServicio.CurrentRow.Cells["IDServicio"].Value)))
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
                    //convertir a int
                    this.IDServicio = Convert.ToInt32(DGVServicio.CurrentRow.Cells["IDServicio"].Value);
                    Rpta = BLServicio.EliminaSE(this.IDServicio);

                    if (Rpta.Equals("OK"))
                    {
                        this.IDServicio = 0;
                        MessageBox.Show("Registro Eliminado", "Aviso del Sistema", MessageBoxButtons.YesNoCancel,
                         MessageBoxIcon.Exclamation);

                    }
                }

            }
        }

        private void BTModificarServicio_Click(object sender, EventArgs e)
        {
            ModificarServicio();
        }

        private void BTBuscarServicio_Click(object sender, EventArgs e)
        {
            this.ListadoServicio(TBBuscarServicio.Text.Trim());
        }

        private void BTCancelarServicio_Click(object sender, EventArgs e)
        {
            this.BotonesServicio(true);
            TBDescripcionServicio.Text = "";
            
            TBPrecioServicio.Text = "";
            
        }

    }
}
