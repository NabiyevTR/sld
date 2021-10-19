using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq.Expressions;
using System.Timers;
using static SLD.Extensions;
using Autodesk.Revit.DB;

namespace SLD
{
    public partial class BoardTable : System.Windows.Forms.Form
    {


        public Panel panel { get; set; }

        bool onSheet;
        bool startUpdate = false;
        private string breakerNumber;
        private string owner;



        public BoardTable(Panel panel, bool onSheet, List<string> tbNames = null)
        {
            InitializeComponent();

            this.onSheet = onSheet;

            if (onSheet)
            {
                tbName.DataSource = tbNames;
                if (tbNames.Count() > 0)
                {
                    if (tbNames.Contains(panel.titleBlockName))
                    {
                        tbName.Text = panel.titleBlockName;
                    }
                    else
                    {
                        if (tbNames.Contains(Properties.Settings.Default.set_lastTitleBlock))
                        {
                            tbName.Text = Properties.Settings.Default.set_lastTitleBlock;
                        }
                        else
                        {
                            tbName.Text = tbNames[0];
                        }
                    }
                    tbName.Visible = true;
                    tbLabel.Visible = true;
                }
                else
                {
                    tbName.Visible = false;
                    tbLabel.Visible = false;
                }

            }
            else
            {
                tbName.Visible = false;
                tbLabel.Visible = false;
            }

            this.panel = panel;

            PanelName.Text = panel.name;
            panelPhase.Text = panel.phase;
            cbNominalMin.Text = panel.minCircuitBreaker.ToString("F0");
            owner = panel.ownerPanel;
            breakerNumber = panel.breakerNumber;


            instPowerA.Text = panel.loadA.ToString("F1");
            instPowerB.Text = panel.loadB.ToString("F1");
            instPowerC.Text = panel.loadC.ToString("F1");
            instPower.Text = panel.load.ToString("F1");

            ratedPowerA.Text = panel.ratedLoadA.ToString("F1");
            ratedPowerB.Text = panel.ratedLoadB.ToString("F1");
            ratedPowerC.Text = panel.ratedLoadC.ToString("F1");
            ratedPower.Text = panel.ratedLoad.ToString("F1");

            ratedCurrentA.Text = panel.currentA.ToString("F1");
            ratedCurrentB.Text = panel.currentB.ToString("F1");
            ratedCurrentC.Text = panel.currentC.ToString("F1");
            ratedCurrent.Text = panel.current.ToString("F1");

            fullPowerA.Text = panel.fullPowerA.ToString("F1");
            fullPowerB.Text = panel.fullPowerB.ToString("F1");
            fullPowerC.Text = panel.fullPowerC.ToString("F1");
            fullPower.Text = panel.fullPower.ToString("F1");

            powFactor.Text = panel.powerFactor.ToString("F2");
            demFactor.Text = panel.demandPanelFactor.ToString("F2");
            nonsymmetric.Text = panel.nonSymmetry.ToString("F0") + "%";

            cableSafeFactor.Value = panel.cableReserve;
            if (panel.reserve != 0)
            {
                reserve.Value = panel.reserve;
            }



            //feeder

            pCable.Text = panel.cableType + " " + panel.coreQuantityAndCrossSection;

            pCurrentSafeFactor.Value = 5;

            // Подумать как сделать, чтобы значение обновлялось

            try
            {
                pUpBreakerReleaseCurrent.Value = Convert.ToDecimal(panel.breakerReleaseCurrent);
            }
            catch
            {
                pUpBreakerReleaseCurrent.Value = 6300;
            }

            try
            {
                pBreakerReleaseCurrent.Value = Convert.ToDecimal(panel.breakerReleaseCurrent);
            }
            catch
            {
                pBreakerReleaseCurrent.Value = 6300;
            }

            pBreakerNominalCurrent.Text = panel.breakerNominalCurrent;
            pCableType.Text = panel.cableType;
            pCoreMaterial.Text = panel.coreMaterial;
            pInsulationMaterial.Text = panel.insulationMaterial;
            pStandart.Text = panel.standart;
            pCablingType.Text = panel.cablingType;

            if (panel.cableDeratingFactor > 1 || panel.cableDeratingFactor < 0)
            {
                pCableDeratingFactor.Value = 1;
            }
            else
            {
                pCableDeratingFactor.Value = (decimal)panel.cableDeratingFactor;
            }

            pMaxCrossSection.Text = panel.maxCrossSection;
            pCoreQuantity.Text = panel.coreQuantity;
            pNeutralWire.Text = panel.neutralWire;

            if (panel.ratedLength > 999)
            {
                pCableLength.Value = 999;
            }
            else
            {
                pCableLength.Value = (decimal)panel.ratedLength;
            }

            pMaxVoltageDrop.Value = 2.5M;
            pVoltageDrop.Text = panel.voltageDrop.ToString();

            this.panel = panel;

            foreach (Circuit circuit in panel.circuits)
            {
                dataTable.Rows.Add(
                circuit.breakerNumber,
                circuit.description,
                circuit.rooms,
                circuit.phase,
                circuit.loadA, //Проверить если ниже был применен коэффициент спроса
                circuit.loadB,
                circuit.loadC,
                circuit.powerFactor,
                circuit.demandPanelFactor,
                circuit.demandCircuitFactor,
                circuit.currentA,
                circuit.currentB,
                circuit.currentC,
                circuit.breakerSafetyFactor,
                circuit.breakerNominalCurrent,
                circuit.breakerReleaseCurrent,
                circuit.diffReleaseCurrent,
                circuit.contactor,
                circuit.coreMaterial,
                circuit.insulationMaterial,
                circuit.standart,
                circuit.cablingType,
                circuit.coreQuantity,
                circuit.neutralWire,
                circuit.maxCrossSection,
                circuit.cableDeratingFactor,
                circuit.cableQuantity,
                circuit.cableType,
                circuit.coreQuantityAndCrossSection,
                circuit.ratedLength,
                circuit.maxLength * (1 + CTDbl(cableSafeFactor.Value.ToString()) / 100),
                circuit.totalLength * (1 + CTDbl(cableSafeFactor.Value.ToString()) / 100),
                circuit.maxVoltageDrop * (1 + CTDbl(cableSafeFactor.Value.ToString()) / 100),
                circuit.voltageDrop,
                circuit.uid
                );
            }
        }

        // Записываем всю информацию в Panel
        private void dCreate_Click(object sender, EventArgs e)
        {
            List<Circuit> circuits = new List<Circuit>();

            Document doc = App.docG;

            foreach (DataGridViewRow row in dataTable.Rows)
            {
                Circuit circuit = new Circuit(row);
                circuits.Add(circuit);

                // Add if not null

                Element docCircuit = doc.GetElement(circuit.uid);
                Storage circuitStorage = new Storage(docCircuit);
                circuitStorage.Write(circuit);

            }

            this.panel.name = PanelName.Text;
            this.panel.panelName = this.panel.name;
            this.panel.circuits = circuits;
            this.panel.reserve = (int)reserve.Value;
            this.panel.load = CTDbl(instPower.Text);
            this.panel.loadA = CTDbl(instPowerA.Text);
            this.panel.loadB = CTDbl(instPowerB.Text);
            this.panel.loadC = CTDbl(instPowerC.Text);
            this.panel.ratedLoad = CTDbl(ratedPower.Text);
            this.panel.ratedLoadA = CTDbl(ratedPowerA.Text);
            this.panel.ratedLoadB = CTDbl(ratedPowerB.Text);
            this.panel.ratedLoadC = CTDbl(ratedPowerC.Text);
            this.panel.current = CTDbl(ratedCurrent.Text);
            this.panel.currentA = CTDbl(ratedCurrentA.Text);
            this.panel.currentB = CTDbl(ratedCurrentB.Text);
            this.panel.currentC = CTDbl(ratedCurrentC.Text);
            this.panel.fullPower = CTDbl(fullPower.Text);
            this.panel.fullPowerA = CTDbl(fullPowerA.Text);
            this.panel.fullPowerB = CTDbl(fullPowerB.Text);
            this.panel.fullPowerC = CTDbl(fullPowerC.Text);
            this.panel.powerFactor = CTDbl(powFactor.Text);
            this.panel.demandPanelFactor = CTDbl(demFactor.Text);
            this.panel.phase = panelPhase.Text;
            this.panel.breakerNominalCurrent = pBreakerNominalCurrent.Text;
            this.panel.breakerReleaseCurrent = pBreakerReleaseCurrent.Value.ToString();
            this.panel.cableType = pCableType.Text;
            this.panel.coreMaterial = pCoreMaterial.Text;
            this.panel.insulationMaterial = pInsulationMaterial.Text;
            this.panel.standart = pStandart.Text;
            this.panel.cablingType = pCablingType.Text;
            this.panel.cableDeratingFactor = CTDbl(pCableDeratingFactor.Value.ToString());
            this.panel.maxCrossSection = pMaxCrossSection.Text;
            this.panel.coreQuantity = pCoreQuantity.Text;
            this.panel.neutralWire = pNeutralWire.Text;
            this.panel.ratedLength = CTDbl(pCableLength.Value.ToString());
            this.panel.maxLength = this.panel.ratedLength;
            this.panel.totalLength = this.panel.ratedLength;
            this.panel.maxVoltageDrop = CTDbl(pMaxVoltageDrop.Value.ToString());
            this.panel.voltageDrop = CTDbl(pVoltageDrop.Text);
            this.panel.coreQuantityAndCrossSection = panel.coreQuantityAndCrossSection;
            this.panel.titleBlockName = tbName.Text;
            this.panel.cableReserve = (int)cableSafeFactor.Value;
            this.panel.minCircuitBreaker = CTDbl(cbNominalMin.Text);

            if (this.panel.phase == "3Ф")
            {
                this.panel.voltage = 380.ToString();
            }
            else
            {
                this.panel.voltage = 220.ToString();
            }

            this.panel.voltage = panel.voltage;

            this.panel.ownerPanel = owner;
            this.panel.breakerNumber = breakerNumber;

            Element docPanel = doc.GetElement(panel.puid);

            Storage panelStorage = new Storage(docPanel);
            panelStorage.Write(panel);

            if (onSheet)
            {
                Properties.Settings.Default.set_lastTitleBlock = tbName.Text;
                Properties.Settings.Default.Save();

            }
        }

        private void dataTable_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            dataTable.Rows[e.RowIndex].ErrorText = "";


            List<int> valCols;
            double dblCellValue;
            int intCellValue;


            /*  //Замена пустых строк нулями
              valCols = new List<int>()  {
                                          Parameters.DGV_LOADA_COL,
                                          Parameters.DGV_LOADB_COL,
                                          Parameters.DGV_LOADC_COL };

              if (valCols.Contains(e.ColumnIndex) && dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == "")
              {
                  dblCellValue = 0;
                  dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dblCellValue.ToString("F2");
              }
              */

            //Проверка десятичных чисел
            valCols = new List<int>() { Parameters.DGV_LOADA_COL,
                                        Parameters.DGV_LOADB_COL,
                                        Parameters.DGV_LOADC_COL,
                                        Parameters.DGV_PF_COL,
                                        Parameters.DGV_KCP_COL,
                                        Parameters.DGV_KCC_COL,
                                        Parameters.DGV_CURRENTA_COL,
                                        Parameters.DGV_CURRENTB_COL,
                                        Parameters.DGV_CURRENTC_COL,
                                        Parameters.DGV_SAFEFACTOR_COL,
                                        Parameters.DGV_CBRELEASE_COL,
                                        Parameters.DGV_CABLEDERATINGFACTOR_COL,
                                        Parameters.DGV_RATEDLENGTH_COL,
                                        Parameters.DGV_MAXLENGTH_COL,
                                        Parameters.DGV_TOTALLENGT_COL,
                                        Parameters.DGV_MAXVOLTAGEDROP_COL,
                                        Parameters.DGV_VOLTAGEDROP_COL
                                        };






            if (valCols.Contains(e.ColumnIndex))
            {
                if (dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    if (!double.TryParse(e.FormattedValue.ToString(), out dblCellValue))
                    {
                        e.Cancel = true;
                        TaskDialog.Show("Ошибка", "Введите число");
                    }
                }
            }

            //Проверка целых чисел
            valCols = new List<int>() {
                                        Parameters.DGV_LINESQUANTITY_COL
                                       };


            if (valCols.Contains(e.ColumnIndex))
            {
                if (dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    if (!int.TryParse(e.FormattedValue.ToString(), out intCellValue))
                    {
                        e.Cancel = true;
                        TaskDialog.Show("Ошибка", "Введите целое число");
                    }
                    else
                    {

                    }

                }
            }

            //Проверка коэффициентов: должны быть не более 1
            valCols = new List<int>() {
                                        Parameters.DGV_PF_COL,
                                        Parameters.DGV_KCP_COL,
                                        Parameters.DGV_KCC_COL,
                                        Parameters.DGV_CABLEDERATINGFACTOR_COL
                                       };


            if (valCols.Contains(e.ColumnIndex))
            {
                if (dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {

                    if (double.TryParse(e.FormattedValue.ToString(), out dblCellValue))
                    {
                        if (dblCellValue > 1 || dblCellValue < 0)
                        {
                            e.Cancel = true;
                            TaskDialog.Show("Ошибка", "Значение коэффицента должно лежать в диапазоне от 0 до 1!");
                        }
                    }
                }
            }

            //Проверка коэффициентов: должны быть более 1
            valCols = new List<int>() {
                                        Parameters.DGV_SAFEFACTOR_COL
                                       };


            if (valCols.Contains(e.ColumnIndex))
            {
                if (dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {

                    if (double.TryParse(e.FormattedValue.ToString(), out dblCellValue))
                    {
                        if (dblCellValue < 1)
                        {
                            e.Cancel = true;
                            TaskDialog.Show("Ошибка", "Значение коэффицента должно быть больше либо равно 1!");
                        }
                    }
                }
            }


            // Поверка выключателя
            valCols = new List<int>() {
                                        Parameters.DGV_CBNOMINAL_COL,
                                        Parameters.DGV_CBRELEASE_COL
                                       };
            if (valCols.Contains(e.ColumnIndex))
            {
                if (dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    if (double.TryParse(e.FormattedValue.ToString(), out dblCellValue))
                    {

                        DataGridViewRow row = dataTable.Rows[e.RowIndex];

                        List<double> currents = new List<double>
                        {
                        CTDbl(row.Cells[Parameters.DGV_CURRENTA_COL].Value.ToString()),
                        CTDbl(row.Cells[Parameters.DGV_CURRENTB_COL].Value.ToString()),
                        CTDbl(row.Cells[Parameters.DGV_CURRENTC_COL].Value.ToString())
                        };

                        if (dblCellValue < currents.Max())
                        {
                            e.Cancel = true;
                            TaskDialog.Show("Ошибка", "Номинальный ток выключателя и расцепителя должен быть больше расчетного тока в наиболее загруженной фазе!");
                        }
                    }
                }


            }




            // Проверка тока расцепителя
            valCols = new List<int>() {
                                        Parameters.DGV_CBRELEASE_COL
                                       };


            if (valCols.Contains(e.ColumnIndex))
            {
                if (dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {

                    if (double.TryParse(e.FormattedValue.ToString(), out dblCellValue))
                    {
                        if (dblCellValue >
                            CTDbl(dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CBNOMINAL_COL].Value.ToString()))
                        {
                            e.Cancel = true;
                            TaskDialog.Show("Ошибка", "Ток расцепителя должен быть меньше номинального тока выключателя!");
                        }
                    }
                }
            }
        }



        private void dataTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            double dblCellValue;
            int intCellValue;

            List<int> dblCells;



            //Фаза
            DataGridViewRow row = dataTable.Rows[e.RowIndex];

            string phase = row.Cells[Parameters.DGV_PHASE_COL].Value.ToString();
            double loadA = CTDbl(row.Cells[Parameters.DGV_LOADA_COL].Value.ToString());
            double loadB = CTDbl(row.Cells[Parameters.DGV_LOADB_COL].Value.ToString());
            double loadC = CTDbl(row.Cells[Parameters.DGV_LOADC_COL].Value.ToString());
            double currentA = CTDbl(row.Cells[Parameters.DGV_CURRENTA_COL].Value.ToString());
            double currentB = CTDbl(row.Cells[Parameters.DGV_CURRENTB_COL].Value.ToString());
            double currentC = CTDbl(row.Cells[Parameters.DGV_CURRENTC_COL].Value.ToString());



            List<double> loads = new List<double>() { loadA, loadB, loadC };





            //Проверка десятичных данных
            dblCells = new List<int>()  {
                                        Parameters.DGV_LOADA_COL,
                                        Parameters.DGV_LOADB_COL,
                                        Parameters.DGV_LOADC_COL,
                                        Parameters.DGV_PF_COL,
                                        Parameters.DGV_KCP_COL,
                                        Parameters.DGV_KCC_COL,
                                        Parameters.DGV_CURRENTA_COL,
                                        Parameters.DGV_CURRENTB_COL,
                                        Parameters.DGV_CURRENTC_COL,
                                        Parameters.DGV_SAFEFACTOR_COL,
                                        Parameters.DGV_CBRELEASE_COL,
                                        Parameters.DGV_CABLEDERATINGFACTOR_COL,
                                        Parameters.DGV_MAXVOLTAGEDROP_COL,
                                        Parameters.DGV_VOLTAGEDROP_COL
                                        };

            if (dblCells.Contains(e.ColumnIndex) && e.Value != null && double.TryParse(e.Value.ToString(), out dblCellValue))
            {
                e.Value = dblCellValue.ToString("F2");
            }

            dblCells = new List<int>() {
                                        Parameters.DGV_RATEDLENGTH_COL,
                                        Parameters.DGV_MAXLENGTH_COL,
                                        Parameters.DGV_TOTALLENGT_COL
                                       };
            if (dblCells.Contains(e.ColumnIndex) && e.Value != null && double.TryParse(e.Value.ToString(), out dblCellValue))
            {
                e.Value = dblCellValue.ToString("F0");
            }

            //Проверка целых чисел

            dblCells = new List<int>() {
                                        Parameters.DGV_LINESQUANTITY_COL
                                        };

            if (dblCells.Contains(e.ColumnIndex) && e.Value != null && int.TryParse(e.Value.ToString(), out intCellValue))
            {
                e.Value = intCellValue.ToString("F0");
            }


            //Проверка фазы 

            dblCells = new List<int>() {
                                        Parameters.DGV_PHASE_COL
                                        };
            if (dblCells.Contains(e.ColumnIndex))
            {

                if (phase == "A")
                {
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADA_COL].ReadOnly = false;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADB_COL].ReadOnly = true;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADC_COL].ReadOnly = true;

                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADA_COL].Style.ForeColor = System.Drawing.Color.Black;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADB_COL].Style.ForeColor = System.Drawing.Color.Gray;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADC_COL].Style.ForeColor = System.Drawing.Color.Gray;

                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADA_COL].Value = loads.Max();
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADB_COL].Value = 0;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADC_COL].Value = 0;
                }

                if (phase == "B")
                {
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADA_COL].ReadOnly = true;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADB_COL].ReadOnly = false;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADC_COL].ReadOnly = true;

                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADA_COL].Style.ForeColor = System.Drawing.Color.Gray;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADB_COL].Style.ForeColor = System.Drawing.Color.Black;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADC_COL].Style.ForeColor = System.Drawing.Color.Gray;

                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADA_COL].Value = 0;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADB_COL].Value = loads.Max();
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADC_COL].Value = 0;
                }

                if (phase == "C")
                {
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADA_COL].ReadOnly = true;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADB_COL].ReadOnly = true;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADC_COL].ReadOnly = false;

                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADA_COL].Style.ForeColor = System.Drawing.Color.Gray;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADB_COL].Style.ForeColor = System.Drawing.Color.Gray;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADC_COL].Style.ForeColor = System.Drawing.Color.Black;

                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADA_COL].Value = 0;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADB_COL].Value = 0;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADC_COL].Value = loads.Max();
                }

                if (phase == "3Ф")
                {
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADA_COL].ReadOnly = false;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADB_COL].ReadOnly = false;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADC_COL].ReadOnly = false;

                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADA_COL].Style.ForeColor = System.Drawing.Color.Black;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADB_COL].Style.ForeColor = System.Drawing.Color.Black;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_LOADC_COL].Style.ForeColor = System.Drawing.Color.Black;
                }


            }



            // Метод выбора кабеля
            List<int> Cols = new List<int>() {
                                        Parameters.DGV_STANDART_COL
                                       };
            if (Cols.Contains(e.ColumnIndex))
            {
                if (dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_STANDART_COL].Value.ToString() == Parameters.STANDART_MANUAL)
                {
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CABLINGTYPE_COL].Style.ForeColor = System.Drawing.Color.Gray;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CABLINGTYPE_COL].ReadOnly = true;
                }
                else
                {
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CABLINGTYPE_COL].Style.ForeColor = System.Drawing.Color.Black;
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CABLINGTYPE_COL].ReadOnly = false;
                }
            }



















        }

        private void dataTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex > -1)
            {

                DataGridViewRow row = dataTable.Rows[e.RowIndex];

                string phase = row.Cells[Parameters.DGV_PHASE_COL].Value.ToString();
                double loadA = CTDbl(row.Cells[Parameters.DGV_LOADA_COL].Value.ToString());
                double loadB = CTDbl(row.Cells[Parameters.DGV_LOADB_COL].Value.ToString());
                double loadC = CTDbl(row.Cells[Parameters.DGV_LOADC_COL].Value.ToString());
                double pf = CTDbl(row.Cells[Parameters.DGV_PF_COL].Value.ToString());
                double kcp = CTDbl(row.Cells[Parameters.DGV_KCP_COL].Value.ToString());
                double kcc = CTDbl(row.Cells[Parameters.DGV_KCC_COL].Value.ToString());
                double currentA = CTDbl(row.Cells[Parameters.DGV_CURRENTA_COL].Value.ToString());
                double currentB = CTDbl(row.Cells[Parameters.DGV_CURRENTB_COL].Value.ToString());
                double currentC = CTDbl(row.Cells[Parameters.DGV_CURRENTC_COL].Value.ToString());
                double safeFactor = CTDbl(row.Cells[Parameters.DGV_SAFEFACTOR_COL].Value.ToString());
                double cbRelease = CTDbl(row.Cells[Parameters.DGV_CBRELEASE_COL].Value.ToString());
                string standart = row.Cells[Parameters.DGV_STANDART_COL].Value?.ToString();
                string coreMaterial = row.Cells[Parameters.DGV_COREMATERIAL_COL].Value?.ToString();
                string insulationMaterial = row.Cells[Parameters.DGV_INSULATIONMATERIAL_COL].Value?.ToString();
                string singleMultiCore = row.Cells[Parameters.DGV_SINGLEMULTICORE_COL].Value?.ToString();
                string neutral = row.Cells[Parameters.DGV_N_COL].Value?.ToString();
                string cablingType = row.Cells[Parameters.DGV_CABLINGTYPE_COL].Value?.ToString();
                double cableDeratingFactor = CTDbl(row.Cells[Parameters.DGV_CABLEDERATINGFACTOR_COL].Value?.ToString());
                double maxCrossection = CTDbl(row.Cells[Parameters.DGV_MAXCROSSSECTION_COL].Value?.ToString());







                List<double> loads = new List<double>() { loadA, loadB, loadC };
                List<double> currents = new List<double>() { currentA, currentB, currentC };

                // расчетный ток
                if (e.ColumnIndex == Parameters.DGV_LOADA_COL)
                {
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CURRENTA_COL].Value = ElectricUtil.pnCurrent(loadA, Parameters.phaseVoltage, pf, kcc);
                }

                if (e.ColumnIndex == Parameters.DGV_LOADB_COL)
                {
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CURRENTB_COL].Value = ElectricUtil.pnCurrent(loadB, Parameters.phaseVoltage, pf, kcc);
                }

                if (e.ColumnIndex == Parameters.DGV_LOADC_COL)
                {
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CURRENTC_COL].Value = ElectricUtil.pnCurrent(loadC, Parameters.phaseVoltage, pf, kcc);
                }

                if (e.ColumnIndex == Parameters.DGV_KCC_COL || e.ColumnIndex == Parameters.DGV_PF_COL)
                {
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CURRENTA_COL].Value = ElectricUtil.pnCurrent(loadA, Parameters.phaseVoltage, pf, kcc);
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CURRENTB_COL].Value = ElectricUtil.pnCurrent(loadB, Parameters.phaseVoltage, pf, kcc);
                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CURRENTC_COL].Value = ElectricUtil.pnCurrent(loadC, Parameters.phaseVoltage, pf, kcc);
                }

                // выбор выключателя и расцепителя
                if (e.ColumnIndex == Parameters.DGV_CURRENTA_COL ||
                    e.ColumnIndex == Parameters.DGV_CURRENTB_COL ||
                    e.ColumnIndex == Parameters.DGV_CURRENTC_COL ||
                    e.ColumnIndex == Parameters.DGV_SAFEFACTOR_COL)
                {
                    double currentSel = currents.Max();
                    double currentMin = 10;
                    if (cbNominalMin.Text != "-")
                    {
                        currentMin = int.Parse(cbNominalMin.Text);
                    }

                    double nominal = ElectricUtil.GetBreakerNominalCurrent(currentSel, safeFactor, currentMin);

                    if (CTDbl(row.Cells[Parameters.DGV_CBNOMINAL_COL].Value.ToString()) != nominal)
                    {
                        if (CTDbl(row.Cells[Parameters.DGV_CBNOMINAL_COL].Value.ToString()) ==
                                   CTDbl(row.Cells[Parameters.DGV_CBRELEASE_COL].Value.ToString()))
                        {
                            dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CBRELEASE_COL].Value = nominal.ToString("F0");
                        }
                        dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CBNOMINAL_COL].Value = nominal.ToString("F0");
                    }
                }

                // проверям расцепитель
                if (e.ColumnIndex == Parameters.DGV_CBNOMINAL_COL)
                {
                    double nominal = CTDbl(dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CBNOMINAL_COL].Value.ToString());

                    /*  if (CTDbl(row.Cells[Parameters.DGV_CBNOMINAL_COL].Value.ToString()) ==
                                      CTDbl(row.Cells[Parameters.DGV_CBRELEASE_COL].Value.ToString()))
                      {
                          dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CBRELEASE_COL].Value = row.Cells[Parameters.DGV_CBNOMINAL_COL].Value.ToString();
                      }

                      if (nominal < CTDbl(row.Cells[Parameters.DGV_CBRELEASE_COL].Value.ToString()))
                      {
                          dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CBRELEASE_COL].Value = row.Cells[Parameters.DGV_CBNOMINAL_COL].Value.ToString();
                      } */

                    dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CBRELEASE_COL].Value = row.Cells[Parameters.DGV_CBNOMINAL_COL].Value.ToString();

                }

                // проверка способов прокладки

                if (e.ColumnIndex == Parameters.DGV_STANDART_COL)
                {


                    if (standart == Parameters.STANDART_GOST50517)
                    {
                        DataGridViewComboBoxCell cbCell = (DataGridViewComboBoxCell)dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CABLINGTYPE_COL];

                        cbCell.Items.Add("A1");
                        cbCell.Items.Add("A2");
                        cbCell.Items.Add("B1");
                        cbCell.Items.Add("B2");
                        cbCell.Items.Add("C");
                        cbCell.Items.Add("D1");
                        cbCell.Items.Add("D2");
                        dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CABLINGTYPE_COL].Value = "A1";
                        try
                        {
                            cbCell.Items.Remove("на воздухе");
                            cbCell.Items.Remove("в земле");
                        }
                        catch
                        {

                        }
                        try
                        {
                            cbCell.Items.Remove(" ");
                        }
                        catch
                        {

                        }
                    }

                    if (standart == Parameters.STANDART_GOST31996)
                    {
                        DataGridViewComboBoxCell cbCell = (DataGridViewComboBoxCell)dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CABLINGTYPE_COL];
                        cbCell.Items.Add("на воздухе");
                        cbCell.Items.Add("в земле");
                        dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CABLINGTYPE_COL].Value = "на воздухе";
                        try
                        {
                            cbCell.Items.Remove("A1");
                            cbCell.Items.Remove("A2");
                            cbCell.Items.Remove("B1");
                            cbCell.Items.Remove("B2");
                            cbCell.Items.Remove("C");
                            cbCell.Items.Remove("D1");
                            cbCell.Items.Remove("D2");
                        }
                        catch
                        {

                        }
                        try
                        {
                            cbCell.Items.Remove(" ");
                        }
                        catch
                        {

                        }
                    }
                    if (standart == Parameters.STANDART_MANUAL)
                    {
                        DataGridViewComboBoxCell cbCell = (DataGridViewComboBoxCell)dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CABLINGTYPE_COL];

                        cbCell.Items.Add(" ");
                        dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_CABLINGTYPE_COL].Value = " ";

                        try
                        {
                            cbCell.Items.Remove("A1");
                            cbCell.Items.Remove("A2");
                            cbCell.Items.Remove("B1");
                            cbCell.Items.Remove("B2");
                            cbCell.Items.Remove("C");
                            cbCell.Items.Remove("D1");
                            cbCell.Items.Remove("D2");
                        }
                        catch
                        {

                        }
                        try
                        {
                            cbCell.Items.Remove("на воздухе");
                            cbCell.Items.Remove("в земле");
                        }
                        catch
                        {

                        }
                    }
                }


                if (e.ColumnIndex == Parameters.DGV_PHASE_COL
                    && dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != panelPhase.Text
                    && panelPhase.Text != "3Ф")
                {
                    panelPhase.Text = "3Ф";
                }

                if (e.ColumnIndex == Parameters.DGV_CBNOMINAL_COL
                    && dataTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != cbNominalMin.Text)
                {
                    cbNominalMin.Text = "-";
                }



                //выбираем кабель и считаем потери
                if (row.Cells[Parameters.DGV_STANDART_COL].Value.ToString() != Parameters.STANDART_MANUAL)
                {
                    if (
                        e.ColumnIndex == Parameters.DGV_PHASE_COL ||
                        e.ColumnIndex == Parameters.DGV_CURRENTA_COL ||
                        e.ColumnIndex == Parameters.DGV_CURRENTB_COL ||
                        e.ColumnIndex == Parameters.DGV_CURRENTC_COL ||
                        e.ColumnIndex == Parameters.DGV_CBRELEASE_COL ||
                        e.ColumnIndex == Parameters.DGV_COREMATERIAL_COL ||
                        e.ColumnIndex == Parameters.DGV_INSULATIONMATERIAL_COL ||
                        e.ColumnIndex == Parameters.DGV_STANDART_COL ||
                        e.ColumnIndex == Parameters.DGV_SINGLEMULTICORE_COL ||
                        e.ColumnIndex == Parameters.DGV_N_COL ||
                        e.ColumnIndex == Parameters.DGV_CABLINGTYPE_COL ||
                        e.ColumnIndex == Parameters.DGV_MAXCROSSSECTION_COL ||
                        e.ColumnIndex == Parameters.DGV_CABLEDERATINGFACTOR_COL ||
                        e.ColumnIndex == Parameters.DGV_RATEDLENGTH_COL ||
                        e.ColumnIndex == Parameters.DGV_MAXVOLTAGEDROP_COL)
                    {
                        row = dataTable.Rows[e.RowIndex];
                        cbRelease = CTDbl(row.Cells[Parameters.DGV_CBRELEASE_COL].Value.ToString());
                        standart = row.Cells[Parameters.DGV_STANDART_COL].Value?.ToString();
                        coreMaterial = row.Cells[Parameters.DGV_COREMATERIAL_COL].Value?.ToString();
                        insulationMaterial = row.Cells[Parameters.DGV_INSULATIONMATERIAL_COL].Value?.ToString();
                        singleMultiCore = row.Cells[Parameters.DGV_SINGLEMULTICORE_COL].Value?.ToString();
                        neutral = row.Cells[Parameters.DGV_N_COL].Value?.ToString();
                        cablingType = row.Cells[Parameters.DGV_CABLINGTYPE_COL].Value?.ToString();
                        currentA = CTDbl(row.Cells[Parameters.DGV_CURRENTA_COL].Value?.ToString());
                        currentB = CTDbl(row.Cells[Parameters.DGV_CURRENTB_COL].Value?.ToString());
                        currentC = CTDbl(row.Cells[Parameters.DGV_CURRENTC_COL].Value?.ToString());
                        //temporary
                        if (cablingType == null)
                        {
                            cablingType = "A1";
                        }
                        cableDeratingFactor = CTDbl(row.Cells[Parameters.DGV_CABLEDERATINGFACTOR_COL].Value?.ToString());
                        maxCrossection = CTDbl(row.Cells[Parameters.DGV_MAXCROSSSECTION_COL].Value?.ToString());
                        double maxVoltageDrop = CTDbl(row.Cells[Parameters.DGV_MAXVOLTAGEDROP_COL].Value?.ToString());
                        double ratedLength = CTDbl(row.Cells[Parameters.DGV_RATEDLENGTH_COL].Value?.ToString());

                        bool is3ph;
                        if (row.Cells[Parameters.DGV_PHASE_COL].Value.ToString() == "3Ф")
                        {
                            is3ph = true;
                        }
                        else
                        {
                            is3ph = false;
                        }


                        List<string> data = ElectricUtil.GetCable(phase,
                                                              cbRelease,
                                                              currentA,
                                                              currentB,
                                                              currentC,
                                                              coreMaterial,
                                                              insulationMaterial,
                                                              standart,
                                                              singleMultiCore,
                                                              neutral,
                                                              cablingType,
                                                              cableDeratingFactor,
                                                              maxCrossection,
                                                              maxVoltageDrop,
                                                              ratedLength
                                                              );

                        dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_COREQUANTITYANDSECTION_COL].Value = data[0];

                        dataTable.Rows[e.RowIndex].Cells[Parameters.DGV_VOLTAGEDROP_COL].Value = ElectricUtil.getDropVoltage(
                                                                                         CTDbl(row.Cells[Parameters.DGV_CURRENTA_COL].Value.ToString()),
                                                                                         CTDbl(row.Cells[Parameters.DGV_CURRENTB_COL].Value.ToString()),
                                                                                         CTDbl(row.Cells[Parameters.DGV_CURRENTC_COL].Value.ToString()),
                                                                                         is3ph,
                                                                                         coreMaterial,
                                                                                         data[1],
                                                                                         data[2],
                                                                                         data[3],
                                                                                         data[4],
                                                                                         CTDbl(row.Cells[Parameters.DGV_RATEDLENGTH_COL].Value.ToString())
                                                                                                                         ).ToString("F2");



                    }
                }

                // Шапка
                if (
                        e.ColumnIndex == Parameters.DGV_LOADA_COL ||
                        e.ColumnIndex == Parameters.DGV_LOADB_COL ||
                        e.ColumnIndex == Parameters.DGV_LOADC_COL ||
                        e.ColumnIndex == Parameters.DGV_KCC_COL ||
                        e.ColumnIndex == Parameters.DGV_KCP_COL ||
                         e.ColumnIndex == Parameters.DGV_PF_COL
                        )
                {
                    instPower.Text = ElectricUtil.GetInstalledPower(dataTable).ToString("F1");
                    instPowerA.Text = ElectricUtil.GetInstalledPowerPhA(dataTable).ToString("F1");
                    instPowerB.Text = ElectricUtil.GetInstalledPowerPhB(dataTable).ToString("F1");
                    instPowerC.Text = ElectricUtil.GetInstalledPowerPhC(dataTable).ToString("F1");

                    ratedPower.Text = ElectricUtil.GetRatedPower(dataTable).ToString("F1");
                    ratedPowerA.Text = ElectricUtil.GetRatedPowerPhA(dataTable).ToString("F1");
                    ratedPowerB.Text = ElectricUtil.GetRatedPowerPhB(dataTable).ToString("F1");
                    ratedPowerC.Text = ElectricUtil.GetRatedPowerPhC(dataTable).ToString("F1");

                    ratedCurrent.Text = ElectricUtil.GetRatedCurrent(dataTable).ToString("F1");
                    ratedCurrentA.Text = ElectricUtil.GetRatedCurrentPhA(dataTable).ToString("F1");
                    ratedCurrentB.Text = ElectricUtil.GetRatedCurrentPhB(dataTable).ToString("F1");
                    ratedCurrentC.Text = ElectricUtil.GetRatedCurrentPhC(dataTable).ToString("F1");

                    fullPower.Text = ElectricUtil.GetFullPower(dataTable).ToString("F1");
                    fullPowerA.Text = ElectricUtil.GetFullPowerPhA(dataTable).ToString("F1");
                    fullPowerB.Text = ElectricUtil.GetFullPowerPhB(dataTable).ToString("F1");
                    fullPowerC.Text = ElectricUtil.GetFullPowerPhC(dataTable).ToString("F1");

                    demFactor.Text = ElectricUtil.GetDemandFactor(dataTable).ToString("F2");
                    powFactor.Text = ElectricUtil.GetPowerFactor(dataTable).ToString("F2");

                    List<double> fullPowers = new List<double>() {
                        ElectricUtil.GetFullPowerPhA(dataTable),
                        ElectricUtil.GetFullPowerPhB(dataTable),
                        ElectricUtil.GetFullPowerPhC(dataTable)
                    };

                    if (fullPowers.Max() == 0 && fullPowers.Min() == 0)
                    {
                        nonsymmetric.Text = "0%";
                    }
                    else
                    {
                        if (fullPowers.Min() == 0)
                        {
                            nonsymmetric.Text = "100%";
                        }
                        else
                        {
                            nonsymmetric.Text = (100 * (fullPowers.Max() - fullPowers.Min()) / fullPowers.Max()).ToString("F0") + "%";
                        }
                    }
                }


                startUpdate = true;

                /*  if (e.ColumnIndex == Parameters.DGV_PHASE_COL)
                  {
                      DataGridViewCheckBoxCell checkCell =
                          (DataGridViewCheckBoxCell)dataTable.
                          Rows[e.RowIndex].Cells[e.ColumnIndex];


                      dataTable.Invalidate(); 
                  } */
            }





        }

        private void panelPhase_TextChanged(object sender, EventArgs e)
        {
            if (panelPhase.Text != "3Ф")
            {
                foreach (DataGridViewRow row in dataTable.Rows)
                {
                    row.Cells[Parameters.DGV_PHASE_COL].Value = panelPhase.Text;
                }

            }

            if (startUpdate)
            {
                UpdateInputCable();
            }

        }

        private void cbNominalMin_TextChanged(object sender, EventArgs e)
        {
            if (cbNominalMin.Text != "-")
            {
                int valMin = int.Parse(cbNominalMin.Text);

                if (startUpdate && valMin > Convert.ToDecimal(pBreakerNominalCurrent.Text))
                {
                    pBreakerNominalCurrent.Text = valMin.ToString("F0");
                }

                foreach (DataGridViewRow row in dataTable.Rows)
                {
                    List<double> currents = new List<double>
                    {
                        CTDbl(row.Cells[Parameters.DGV_CURRENTA_COL].Value.ToString()),
                        CTDbl(row.Cells[Parameters.DGV_CURRENTB_COL].Value.ToString()),
                        CTDbl(row.Cells[Parameters.DGV_CURRENTC_COL].Value.ToString())
                    };
                    double current = currents.Max();
                    double safeFactor = CTDbl(row.Cells[Parameters.DGV_SAFEFACTOR_COL].Value.ToString());
                    double nominal = ElectricUtil.GetBreakerNominalCurrent(currents.Max(), safeFactor, valMin);
                    try
                    {
                        if (CTDbl(row.Cells[Parameters.DGV_CBNOMINAL_COL].Value.ToString()) < nominal)
                        {
                            row.Cells[Parameters.DGV_CBNOMINAL_COL].Value = nominal.ToString();
                        }

                    }
                    catch
                    {
                    }


                }

                if (startUpdate) { UpdateInputCable(); }
            }
        }

        private void dataTable_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                dataTable.ClearSelection();
                dataTable.RefreshEdit();
                try
                {
                    dataTable.CurrentCell.Selected = false;
                }
                catch
                {

                }
                dataTable.EndEdit();
                dataTable.CurrentCell = null;


                int currentMouseOverCol = dataTable.HitTest(e.X, e.Y).ColumnIndex;
                int currentMouseOverRow = dataTable.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverCol >= 0
                    && currentMouseOverRow >= 0
                    )
                    if (!dataTable.Rows[currentMouseOverRow].Cells[currentMouseOverCol].ReadOnly)
                    {

                        dataTable.Rows[currentMouseOverRow].Cells[currentMouseOverCol].Selected = true;

                        ContextMenu m = new ContextMenu();
                        MenuItem m1 = new MenuItem("Распространить на все строки");
                        m1.Tag = new object[] { currentMouseOverRow, currentMouseOverCol };
                        m1.Click += m1_Click;
                        m.MenuItems.Add(m1);

                        if (currentMouseOverCol == Parameters.DGV_LOADA_COL ||
                            currentMouseOverCol == Parameters.DGV_LOADB_COL ||
                            currentMouseOverCol == Parameters.DGV_LOADC_COL)
                        {
                            MenuItem m2 = new MenuItem("Разбить по фазам");
                            m2.Click += m2_Click;
                            m.MenuItems.Add(m2);
                            m2.Tag = new object[] { currentMouseOverRow, currentMouseOverCol };

                            MenuItem m3 = new MenuItem("Копировать по фазам");
                            m3.Click += m3_Click;
                            m.MenuItems.Add(m3);
                            m3.Tag = new object[] { currentMouseOverRow, currentMouseOverCol };
                        }
                        m.Show(dataTable, new System.Drawing.Point(e.X, e.Y));
                    }
            }
        }

        void m1_Click(object sender, EventArgs e)
        {
            int row = (int)((object[])((MenuItem)sender).Tag)[0];
            int col = (int)((object[])((MenuItem)sender).Tag)[1];

            var val = dataTable.Rows[row].Cells[col].Value;

            for (int i = 0; i < dataTable.RowCount; i++)
            {
                dataTable.Rows[i].Cells[col].Value = val;
            }
        }

        void m2_Click(object sender, EventArgs e)
        {
            int row = (int)((object[])((MenuItem)sender).Tag)[0];
            int col = (int)((object[])((MenuItem)sender).Tag)[1];

            double val = 0;


            if (double.TryParse(dataTable.Rows[row].Cells[col].Value.ToString(), out val))
            {
                dataTable.Rows[row].Cells[Parameters.DGV_PHASE_COL].Value = "3Ф";

                dataTable.Rows[row].Cells[Parameters.DGV_LOADA_COL].Value = val / 3;
                dataTable.Rows[row].Cells[Parameters.DGV_LOADB_COL].Value = val / 3;
                dataTable.Rows[row].Cells[Parameters.DGV_LOADC_COL].Value = val / 3;
            }
        }

        void m3_Click(object sender, EventArgs e)
        {
            int row = (int)((object[])((MenuItem)sender).Tag)[0];
            int col = (int)((object[])((MenuItem)sender).Tag)[1];

            double val = 0;


            if (double.TryParse(dataTable.Rows[row].Cells[col].Value.ToString(), out val))
            {
                dataTable.Rows[row].Cells[Parameters.DGV_PHASE_COL].Value = "3Ф";

                dataTable.Rows[row].Cells[Parameters.DGV_LOADA_COL].Value = val;
                dataTable.Rows[row].Cells[Parameters.DGV_LOADB_COL].Value = val;
                dataTable.Rows[row].Cells[Parameters.DGV_LOADC_COL].Value = val;
            }
        }




        private void dataTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dataTable.ClearSelection();
        }

        private void dataTable_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            List<int> checkBoxCol = new List<int>
            {
                Parameters.DGV_PHASE_COL,
                Parameters.DGV_CBNOMINAL_COL,
                Parameters.DGV_DIFF_COL,
                Parameters.DGV_COREMATERIAL_COL,
                Parameters.DGV_INSULATIONMATERIAL_COL,
                Parameters.DGV_STANDART_COL,
                Parameters.DGV_CABLINGTYPE_COL,
                Parameters.DGV_SINGLEMULTICORE_COL,
                Parameters.DGV_MAXCROSSSECTION_COL,
                Parameters.DGV_N_COL
            };


            if (dataTable.IsCurrentCellDirty &&
               checkBoxCol.Contains(dataTable.CurrentCellAddress.X)
                )
            {
                dataTable.CommitEdit(DataGridViewDataErrorContexts.Commit);
                dataTable.EndEdit();
                try
                {
                    dataTable.CurrentCell = null;
                }
                catch
                {

                }

            }
        }



        private void cableSafeFactorChanged(object sender, EventArgs e)
        {

            int circuitCount = panel.circuits.Count();

            pCableLength.Text = Convert.ToString(
                (1 + cableSafeFactor.Value / 100) * Convert.ToDecimal(panel.ratedLength)
                );

            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < circuitCount; i++)
                {
                    DataGridViewRow row = dataTable.Rows[i];
                    row.Cells[Parameters.DGV_RATEDLENGTH_COL].Value = panel.circuits[i].ratedLength * (1 + CTDbl(cableSafeFactor.Value.ToString()) / 100);
                    row.Cells[Parameters.DGV_MAXLENGTH_COL].Value = panel.circuits[i].maxLength * (1 + CTDbl(cableSafeFactor.Value.ToString()) / 100);
                    row.Cells[Parameters.DGV_TOTALLENGT_COL].Value = panel.circuits[i].totalLength * (1 + CTDbl(cableSafeFactor.Value.ToString()) / 100);
                }
            }

        }


        private void pStandart_TextChanged(object sender, EventArgs e)
        {
            if (pStandart.Text == Parameters.STANDART_GOST31996)
            {
                pCablingType.Items.Clear();
                pCablingType.Items.Add("на воздухе");
                pCablingType.Items.Add("в земле");
                pCablingType.Text = "на воздухе";

            }
            if (pStandart.Text == Parameters.STANDART_GOST50517)
            {
                pCablingType.Items.Clear();
                pCablingType.Items.Add("A1");
                pCablingType.Items.Add("A2");
                pCablingType.Items.Add("B1");
                pCablingType.Items.Add("B2");
                pCablingType.Items.Add("C");
                pCablingType.Items.Add("D1");
                pCablingType.Items.Add("D2");
                pCablingType.Text = "A1";


            }
        }

        private void pBreakerNominalCurrent_SelectedValueChanged(object sender, EventArgs e)
        {
            pBreakerReleaseCurrent.Maximum = Convert.ToDecimal(pBreakerNominalCurrent.Text);

            try
            {
                pBreakerReleaseCurrent.Value = Convert.ToDecimal(pBreakerNominalCurrent.Text);
            }
            catch
            {

            }
            if (pUpBreakerReleaseCurrent.Value < pBreakerReleaseCurrent.Value)
            {
                pUpBreakerReleaseCurrent.Value = pBreakerReleaseCurrent.Value;
            }


        }

        public void UpdateInputCable()
        {

            string breakerNominalCurrent = ElectricUtil.GetBreakerNominalCurrent(
                                                 CTDbl(ratedCurrent.Text),
                                                 CTDbl((pCurrentSafeFactor.Value / 100 + 1).ToString()),
                                                 CTDbl(cbNominalMin.Text)).ToString();

            /*if (CTDbl(pBreakerNominalCurrent.Text) < CTDbl(breakerNominalCurrent))
            {
                pBreakerNominalCurrent.Text = breakerNominalCurrent;
            }

            if (pBreakerReleaseCurrent.Value < Convert.ToDecimal(ratedCurrent.Text, CultureInfo.InvariantCulture))
            {
                pBreakerReleaseCurrent.Value = Convert.ToDecimal(pBreakerNominalCurrent.Text, CultureInfo.InvariantCulture);
            }
            */

            if (pUpBreakerReleaseCurrent.Value < pBreakerReleaseCurrent.Value)
            {
                pUpBreakerReleaseCurrent.Value = pBreakerReleaseCurrent.Value;
            }


            List<string> data
             = ElectricUtil.GetCable(
               panelPhase.Text,
               CTDbl(pUpBreakerReleaseCurrent.Value.ToString()),
               CTDbl(ratedCurrentA.Text),
               CTDbl(ratedCurrentB.Text),
               CTDbl(ratedCurrentC.Text),
               pCoreMaterial.Text,
               pInsulationMaterial.Text,
               pStandart.Text,
               pCoreQuantity.Text,
               pNeutralWire.Text,
               pCablingType.Text,
               CTDbl(pCableDeratingFactor.Value.ToString()),
               CTDbl(pMaxCrossSection.Text.ToString()),
               CTDbl(pMaxVoltageDrop.Value.ToString()),
               CTDbl(pCableLength.Value.ToString()));

            panel.coreQuantityAndCrossSection = data[0];


            pCable.Text = pCableType.Text + " " + data[0];




            //cable, cablesPhQuantity, cablesNQuantity, corePhSection, coreNSection, coreQuantity

            bool is3ph;
            if (panelPhase.Text == "3Ф")
            {
                is3ph = true;
            }
            else
            {
                is3ph = false;
            }

            panel.voltageDrop = ElectricUtil.getDropVoltage(CTDbl(ratedCurrentA.Text),
                                                            CTDbl(ratedCurrentB.Text),
                                                            CTDbl(ratedCurrentC.Text),
                                                            is3ph,
                                                            pCoreMaterial.Text,
                                                            data[1],
                                                            data[2],
                                                            data[3],
                                                            data[4],
                                                            CTDbl(pCableLength.Value.ToString())
                                                                                       );
            pVoltageDrop.Text = panel.voltageDrop.ToString("F2");
        }

        private void pCableType_TextChanged(object sender, EventArgs e)
        {
            if (startUpdate)
            {
                panel.cableType = pCableType.Text;
                pCable.Text = panel.cableType + " " + panel.coreQuantityAndCrossSection;
            }

        }

        private void pCurrentSafeFactor_ValueChanged(object sender, EventArgs e)
        {
            if (startUpdate)
            {
                pBreakerNominalCurrent.Text = ElectricUtil.GetBreakerNominalCurrent(
                                                  CTDbl(ratedCurrent.Text),
                                                  CTDbl((pCurrentSafeFactor.Value / 100 + 1).ToString()),
                                                  CTDbl(cbNominalMin.Text)).ToString();
            }
        }

        private void pUpBreakerReleaseCurrent_ValueChanged(object sender, EventArgs e)
        {
            if (startUpdate)
            {
                //pCurrentSafeFactor.Value = (int)(ratedCurrent.Text.ToDec() / pBreakerReleaseCurrent.Value);
                UpdateInputCable();
            }
        }

        private void pCoreMaterial_TextChanged(object sender, EventArgs e)
        {
            if (startUpdate)
            {
                UpdateInputCable();
            }
        }

        private void pInsulationMaterial_TextChanged(object sender, EventArgs e)
        {
            if (startUpdate)
            {
                UpdateInputCable();
            }
        }



        private void pCablingType_TextChanged(object sender, EventArgs e)
        {
            /*  if (startUpdate)
              {
                  UpdateInputCable();
              } */
        }

        private void pCablingType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (startUpdate)
            {
                UpdateInputCable();
            }
        }

        private void pCableDeratingFactor_ValueChanged(object sender, EventArgs e)
        {
            if (startUpdate)
            {
                UpdateInputCable();
            }
        }

        private void pMaxCrossSection_TextChanged(object sender, EventArgs e)
        {
            if (startUpdate)
            {
                UpdateInputCable();
            }
        }

        private void pCableQuantity_ValueChanged(object sender, EventArgs e)
        {
            if (startUpdate)
            {
                UpdateInputCable();
            }
        }

        private void pCoreQuantity_TextChanged(object sender, EventArgs e)
        {
            if (startUpdate)
            {
                if (startUpdate)
                {
                    UpdateInputCable();
                }
            }
        }

        private void pNeutralWire_TextChanged(object sender, EventArgs e)
        {
            if (startUpdate)
            {
                UpdateInputCable();
            }
        }

        private void pCableLength_ValueChanged(object sender, EventArgs e)
        {
            if (startUpdate)
            {
                UpdateInputCable();
            }
        }

        private void pMaxVoltageDrop_ValueChanged(object sender, EventArgs e)
        {
            if (startUpdate)
            {
                UpdateInputCable();
            }
        }

        private void ratedCurrent_TextChanged(object sender, EventArgs e)
        {
            if (startUpdate)
            {
                pBreakerReleaseCurrent.Minimum = ratedCurrent.Text.ToDec();
                pUpBreakerReleaseCurrent.Minimum = ratedCurrent.Text.ToDec();
                UpdateInputCable();
            }

        }

        private void phasing_Click(object sender, EventArgs e)
        {
            string temp = "C";

            foreach (DataGridViewRow row in dataTable.Rows)
            {
                if (row.Cells[Parameters.DGV_PHASE_COL].Value.ToString() != "3Ф")
                {
                    switch (temp)
                    {
                        case "A":
                            row.Cells[Parameters.DGV_PHASE_COL].Value = "B";
                            temp = "B";
                            break;
                        case "B":
                            row.Cells[Parameters.DGV_PHASE_COL].Value = "C";
                            temp = "C";
                            break;
                        case "C":
                            row.Cells[Parameters.DGV_PHASE_COL].Value = "A";
                            temp = "A";
                            break;

                    }
                }

            }

            temp = "C";

            foreach (DataGridViewRow row in dataTable.Rows)
            {
                if (row.Cells[Parameters.DGV_PHASE_COL].Value.ToString() != "3Ф")
                {
                    switch (temp)
                    {
                        case "A":
                            row.Cells[Parameters.DGV_PHASE_COL].Value = "B";
                            temp = "B";
                            break;
                        case "B":
                            row.Cells[Parameters.DGV_PHASE_COL].Value = "C";
                            temp = "C";
                            break;
                        case "C":
                            row.Cells[Parameters.DGV_PHASE_COL].Value = "A";
                            temp = "A";
                            break;

                    }
                }

            }


        }

        private void pBreakerReleaseCurrent_ValueChanged(object sender, EventArgs e)
        {
            pUpBreakerReleaseCurrent.Minimum = pBreakerReleaseCurrent.Value;
            //   pCurrentSafeFactor.Value = (int)(ratedCurrent.Text.ToDec() / pBreakerReleaseCurrent.Value);
        }


    }
}
