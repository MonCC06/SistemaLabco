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
        int IDTrabajador = 0;
        int EstadoGuarda = 1;
        private void FormatoTrabajdor()
        {
            if (DgvCliente.Columns.Count >= 6) // Verifica si hay al menos 6 columnas
            {
                DgvTrabajador.Columns[0].Width = 100;
                DgvTrabajador.Columns[0].HeaderText = "IDTrabajador";
                DgvTrabajador.Columns[1].Width = 100;
                DgvTrabajador.Columns[1].HeaderText = "Nombre";
                DgvTrabajador.Columns[2].Width = 100;
                DgvTrabajador.Columns[2].HeaderText = "Cedula";
                DgvTrabajador.Columns[3].Width = 100;
                DgvTrabajador.Columns[3].HeaderText = "Telefono";
                DgvTrabajador.Columns[4].Width = 100;
                DgvTrabajador.Columns[4].HeaderText = "Correo";
                
            }

        }
        private void ListadoTrabajador(string tTexto)
        {
            try
            {
                DgvTrabajador.DataSource = BLTrabajador.ListadoTR(tTexto);
                this.FormatoTrabajdor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
            //datasource nos dice de donde vamos a consumir los datos
        }
        private void BotonesTrabajador(bool LEstado)
        {
            this.btnGuardarNuevoTrabajador.Enabled = LEstado;
            this.btnBuscarTrabajador.Enabled = LEstado;
            this.btnModficarTrabajador.Enabled = LEstado;
            this.btnCancelarNuevoTrabajador.Enabled = LEstado;
            this.btnEliminarTrabajador.Enabled = LEstado;
        }
        private void SeleccionaTrabajador()
        {
            //convertir dato de string a un valor 
            if (string.IsNullOrEmpty(Convert.ToString(DgvTrabajador.CurrentRow.Cells["IDTrabajador"].Value)))
            {
                MessageBox.Show("No hay datos que mostar", "Aviso del Sistema", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
            else
            {
                //convertir el id del cliente de la celda seleccionada de string a entero
                this.IDTrabajador = Convert.ToInt32(DgvTrabajador.CurrentRow.Cells["IDTrabajador"].Value);
                txtNuevoNombreTrabajador.Text = Convert.ToString(DgvTrabajador.CurrentRow.Cells["Nombre"].Value);
                txtNuevoCedulaTrabajador.Text = Convert.ToString(DgvTrabajador.CurrentRow.Cells["Cedula"].Value);
                txtNuevoCorreoTrabajador.Text = Convert.ToString(DgvTrabajador.CurrentRow.Cells["Correo"].Value);
                txtNuevoTelefonoTrabajador.Text = Convert.ToString(DgvTrabajador.CurrentRow.Cells["Telefono"].Value);
            }
        }
        private void ActualizarTrabajador()
        {
            // Aquí puedes utilizar el método GuardarCliente o crear un método específico para actualización.
            // Asumiendo que `GuardarCliente` se usa para ambos casos:
            GuardarTrabajador();
            this.ListadoTrabajador("%");
        }
        private void ModificarTrabajador()
        {
            EstadoGuarda = 2; // Indica que se trata de una actualización

            // Verificar si hay una fila seleccionada en el DataGridView
            if (DgvTrabajador.CurrentRow != null)
            {
                // Obtener el IDCliente de la fila seleccionada
                this.IDTrabajador = Convert.ToInt32(DgvTrabajador.CurrentRow.Cells["IDTrabajador"].Value);

                // Poblar los campos con los datos actuales del cliente
                txtNuevoNombreTrabajador.Text = Convert.ToString(DgvTrabajador.CurrentRow.Cells["Nombre"].Value);
                txtNuevoCedulaTrabajador.Text = Convert.ToString(DgvTrabajador.CurrentRow.Cells["Cedula"].Value);
                txtNuevoCorreoTrabajador.Text = Convert.ToString(DgvTrabajador.CurrentRow.Cells["Correo"].Value);
                txtNuevoTelefonoTrabajador.Text = Convert.ToString(DgvTrabajador.CurrentRow.Cells["Telefono"].Value);


                // Establecer el enfoque en el primer campo editable
                txtNuevoNombreTrabajador.Focus();
            }
            else
            {
                MessageBox.Show("Seleccione un Trabaajador para modificar.", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void GuardarTrabajador()
        {
            // Verificación de campos vacíos en una sola condición
            if (string.IsNullOrWhiteSpace(txtNuevoNombreTrabajador.Text) ||
                string.IsNullOrWhiteSpace(txtNuevoCedulaTrabajador.Text) ||
                string.IsNullOrWhiteSpace(txtNuevoCorreoTrabajador.Text) ||
                string.IsNullOrWhiteSpace(txtNuevoTelefonoTrabajador.Text))
            {
                MessageBox.Show("Todos los campos son requeridos(*)", "Aviso del Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return; // Para detener la ejecución si algún campo está vacío
            }

            ETTrabajador eTTrabajador = new ETTrabajador();
            String Rpta = "";

            // Asignación de valores cliente
            eTTrabajador.IDTrabajador = this.IDTrabajador;
            eTTrabajador.Nombre = txtNuevoNombreTrabajador.Text.Trim();
            eTTrabajador.Cedula = txtNuevoCedulaTrabajador.Text.Trim();
            eTTrabajador.Correo = txtNuevoCorreoTrabajador.Text.Trim();
            eTTrabajador.Telefono = txtNuevoTelefonoTrabajador.Text.Trim();

            try
            {
                Rpta = BLTrabajador.GuardarTR(EstadoGuarda, eTTrabajador);

                if (Rpta == "OK")
                {
                    MessageBox.Show("Los datos se han registrado", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reinicio de valores
                    EstadoGuarda = 0;
                    this.BotonesTrabajador(true);
                    txtNuevoNombreTrabajador.Text = "";
                    txtNuevoCedulaTrabajador.Text = "";
                    txtNuevoCorreoTrabajador.Text = "";
                    txtNuevoTelefonoTrabajador.Text = "";
                    this.IDTrabajador = 0;
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

            this.ListadoTrabajador("%");

        }
        //aqui empizan los botones cuidado!!!!!!!!!!!!!!!!
        private void btnGuardarNuevoTrabajador_Click(object sender, EventArgs e)
        {
            if (EstadoGuarda == 2)
            {
                // Actualización de Trabajador
                ActualizarTrabajador();
            }
            else if (EstadoGuarda == 1)
            {
                // Inserción de nuevo Trabajador
                GuardarTrabajador();
            }
            else
            {
                MessageBox.Show("El estado de guardado no está definido.", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminarTrabajador_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Convert.ToString(DgvTrabajador.CurrentRow.Cells["IDTrabajador"].Value)))
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
                    this.IDTrabajador = Convert.ToInt32(DgvTrabajador.CurrentRow.Cells["IDTrabajador"].Value);
                    Rpta = BLTrabajador.EliminaTR(this.IDTrabajador);

                    if (Rpta.Equals("OK"))
                    {
                        this.IDTrabajador = 0;
                        MessageBox.Show("Registro Eliminado", "Aviso del Sistema", MessageBoxButtons.YesNoCancel,
                         MessageBoxIcon.Exclamation);

                    }
                }

            }
        }

        private void btnModficarTrabajador_Click(object sender, EventArgs e)
        {
            ModificarTrabajador();
        }

        private void btnBuscarTrabajador_Click(object sender, EventArgs e)
        {
            this.ListadoTrabajador(txtBuscarTrabajador.Text.Trim());
        }

        private void btnCancelarNuevoTrabajador_Click(object sender, EventArgs e)
        {
            this.BotonesTrabajador(true);
            txtNuevoNombreTrabajador.Text = "";
            txtNuevoCedulaTrabajador.Text = "";
            txtNuevoCorreoTrabajador.Text = "";
            txtNuevoTelefonoTrabajador.Text = "";
        }

        private void ckbNombreTrabajador_CheckedChanged(object sender, EventArgs e)
        {
            this.ListadoTrabajador(txtBuscarTrabajador.Text.Trim());
        }

        private void ckbCedulaTrabajador_CheckedChanged(object sender, EventArgs e)
        {
            this.ListadoTrabajador(txtBuscarTrabajador.Text.Trim());
        }
    }
}
