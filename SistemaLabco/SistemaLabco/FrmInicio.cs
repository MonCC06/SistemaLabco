using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ET;
using BL;

namespace SistemaLabco
{
    public partial class FrmInicio : Form
    {
        public FrmInicio()
        {
            InitializeComponent();
        }

        int EstadoGuarda = 1;
        int IDFactura = 0;
        int IDCliente = 0;
        int IDDetalle = 0;
        private void BtnAnularFA_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnGuardarFA_Click(object sender, EventArgs e)
        {
            if (TxtNombreCliente.Text == String.Empty ||
                TxtCedulaCliente.Text == String.Empty ||
                TxtEmailCliente.Text == String.Empty ||
                TxtTelefonoCliente.Text == String.Empty ||
                TxtPlacaVehiculoFactura.Text == String.Empty ||
                TxtAnnoVehiculoFactura.Text == String.Empty ||
                TxtDistanciaVehiculoFactura.Text == String.Empty ||
                TxtMarcaVehiculoFactura.Text == String.Empty ||
                TxtTrabajador.Text == String.Empty ||
                TxtEstadoFactura.Text == String.Empty)
            {
                MessageBox.Show("Datos sin rellenar, porfavor complete los datos", "Aviso del Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                if (CkbMillas.Checked || CkbKilometros.Checked)
                {
                    string Rpta = "";
                    ETFactura eTFactura = new ETFactura();
                    ETCliente etCliente = new ETCliente();
                    ETVehiculo etVehiculo = new ETVehiculo();
                    ETTrabajador etTrabajador = new ETTrabajador();
                    ETMarca etMarca = new ETMarca();

                    eTFactura.IDFactura = this.IDFactura;
                    eTFactura.IDCliente = this.IDCliente;
                    eTFactura.IDDetalle = this.IDDetalle;
                    etCliente.Nombre = TxtNombreCliente.Text.Trim();
                    etCliente.Cedula = TxtNombreCliente.Text.Trim();
                    etCliente.Telefono = TxtNombreCliente.Text.Trim();
                    etCliente.Correo = TxtNombreCliente.Text.Trim();
                    etVehiculo.Anno = TxtAnnoVehiculoFactura.Text.Trim();
                    etVehiculo.DistanciaRecorrida = TxtDistanciaVehiculoFactura.Text.Trim();
                    etVehiculo.Placa = TxtPlacaVehiculoFactura.Text.Trim();
                    etMarca.Nombre = TxtMarcaVehiculoFactura.Text.Trim();
                    etVehiculo.TipodeDistancia = CkbMillas.Checked;
                    etVehiculo.TipodeDistancia = CkbKilometros.Checked;
                    eTFactura.Subtotal = float.Parse(TxtSubtotal.Text.Trim());
                    eTFactura.Iva = float.Parse(TxtIVA.Text.Trim());
                    eTFactura.Total = float.Parse(TxtTotal.Text.Trim());
                    eTFactura.Estado = Convert.ToBoolean(TxtEstadoFactura.Text.Trim());
                    etTrabajador.Nombre = TxtTrabajador.Text.Trim();

                    try
                    {
                        Rpta = BLFactura.GuardarFA(EstadoGuarda, eTFactura);

                        if (Rpta == "OK")
                        {
                            MessageBox.Show("Los datos se han registrado", "Aviso del sistema", MessageBoxButtons.OK,
                           MessageBoxIcon.Information);
                        }

                        else
                        {
                            MessageBox.Show("Datos sin guardar", "Aviso del Sistema", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex) 
                    {
                        MessageBox.Show("Error inesperado: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    
                }
                else
                {
                    MessageBox.Show("CheckBox sin marcar, porfavor marquelo", "Aviso del Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                }
            }
        }

        private void BtnImprimirFA_Click(object sender, EventArgs e)
        {
            TxtCedulaCliente.ReadOnly = true;
            TxtNombreCliente.ReadOnly = true;
            TxtTelefonoCliente.ReadOnly = true;
            TxtEmailCliente.ReadOnly = true;
            TxtEstadoFactura.ReadOnly = true;
            TxtTrabajador.ReadOnly = true;
            TxtPlacaVehiculoFactura.ReadOnly = true;
            TxtMarcaVehiculoFactura.ReadOnly = true;
            TxtAnnoVehiculoFactura.ReadOnly = true;
            TxtDistanciaVehiculoFactura.ReadOnly = true;

            TxtEstadoFactura.Text = "Cancelada";
        }
    }
}
