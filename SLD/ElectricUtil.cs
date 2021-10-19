using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SLD.Extensions;


namespace SLD
{
    public class ElectricUtil
    {
        public static double pnCurrent(double phaseLoad, double voltage, double powerFactor, double factor)
        {
            return phaseLoad * factor * 1000 / voltage / powerFactor;
        }

        public static List<string> GetCable(string phase,
                                            double cbRelease,
                                            double currentA,
                                            double currentB,
                                            double currentC,
                                            string coreMaterial,
                                            string insulationType,
                                            string gost,
                                            string oneOrMultiCore,
                                            string neutral,
                                            string cablingType,
                                            double deratingFactor,
                                            double maxCrossSection,
                                            double maxVoltageDrop,
                                            double length
                                           )
        {
            string PVC = "ПВХ";
            string XLPE = "ШП";
            string gost50571 = "ГОСТ Р 50571";
            string gost31996 = "ГОСТ 31996";
            string air = "на воздухе";
            string ground = "в земле";
            string cu = "медь";
            string al = "алюминий";
            string multiCore = "многожильный";
            string singleCore = "одножильный";



            List<string> cabSel = new List<string>();

            bool is3ph = false;
            if (phase == "3Ф") { is3ph = true; }


            if (gost == gost50571)
            {
                if (insulationType == PVC)
                {
                    if (coreMaterial == cu)
                    {
                        if (cablingType == "A1")
                        {
                            if (phase == "3Ф" || phase == "3ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Cu_3ph_A1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Cu_1ph_A1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "A2")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Cu_3ph_A2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Cu_1ph_A2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "B1")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Cu_3ph_B1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Cu_1ph_B1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "B2")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Cu_3ph_B2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Cu_1ph_B2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "C")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Cu_3ph_C, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Cu_1ph_C, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "D1")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Cu_3ph_D1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Cu_1ph_D1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "D2")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Cu_3ph_D2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Cu_1ph_D2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }

                    }
                    if (coreMaterial == al)
                    {
                        if (cablingType == "A1")
                        {
                            if (phase == "3Ф" || phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Al_3ph_A1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Al_1ph_A1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "A2")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Al_3ph_A2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Al_1ph_A2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "B1")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Al_3ph_B1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Al_1ph_B1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "B2")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Al_3ph_B2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Al_1ph_B2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "C")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Al_3ph_C, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Al_1ph_C, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "D1")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Al_3ph_D1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Al_1ph_D1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "D2")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Al_3ph_D2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_PVC_Al_1ph_D2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                    }
                }

                if (insulationType == XLPE)
                {
                    if (coreMaterial == cu)
                    {
                        if (cablingType == "A1")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Cu_3ph_A1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Cu_1ph_A1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "A2")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Cu_3ph_A2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Cu_1ph_A2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "B1")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Cu_3ph_B1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Cu_1ph_B1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "B2")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Cu_3ph_B2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Cu_1ph_B2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "C")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Cu_3ph_C, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Cu_1ph_C, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "D1")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Cu_3ph_D1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Cu_1ph_D1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "D2")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Cu_3ph_D2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Cu_1ph_D2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }

                    }


                    if (coreMaterial == al)
                    {
                        if (cablingType == "A1")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Al_3ph_A1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Al_1ph_A1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "A2")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Al_3ph_A2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Al_1ph_A2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "B1")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Al_3ph_B1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Al_1ph_B1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "B2")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Al_3ph_B2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Al_1ph_B2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "C")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Al_3ph_C, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Al_1ph_C, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "D1")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Al_3ph_D1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Al_1ph_D1, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                        if (cablingType == "D2")
                        {
                            if (phase == "3Ф")
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Al_3ph_D2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                            else
                            {
                                cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost50571_XLPE_Al_1ph_D2, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                            }
                        }
                    }
                }
            }


            if (gost == gost31996)
            {
                if (insulationType == PVC)
                {
                    if (coreMaterial == cu)
                    {
                        if (oneOrMultiCore == singleCore)
                        {
                            if (cablingType == air)
                            {
                                if (phase == "3Ф")
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_PVC_Cu_SC_Air, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                                else
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_PVC_Cu_SC_Air, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                            }
                            if (cablingType == ground)
                            {
                                if (phase == "3Ф")
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_PVC_Cu_SC_Ground, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                                else
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_PVC_Cu_SC_Ground, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                            }

                        }
                        if (oneOrMultiCore == multiCore)
                        {
                            if (cablingType == air)
                            {
                                if (phase == "3Ф")
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_PVC_Cu_MC_Air, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor * 0.93);
                                }
                                else
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_PVC_Cu_MC_Air, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                            }
                            if (cablingType == ground)
                            {
                                if (phase == "3Ф")
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_PVC_Cu_MC_Ground, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor * 0.93);
                                }
                                else
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_PVC_Cu_MC_Ground, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                            }
                        }

                    }
                    if (coreMaterial == al)
                    {
                        if (oneOrMultiCore == singleCore)
                        {
                            if (cablingType == air)
                            {
                                if (phase == "3Ф")
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_PVC_Al_SC_Air, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                                else
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_PVC_Al_SC_Air, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                            }
                            if (cablingType == ground)
                            {
                                if (phase == "3Ф")
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_PVC_Al_SC_Ground, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                                else
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_PVC_Al_SC_Ground, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                            }

                        }
                        if (oneOrMultiCore == multiCore)
                        {
                            if (cablingType == air)
                            {
                                if (phase == "3Ф")
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_PVC_Al_MC_Air, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor * 0.93);
                                }
                                else
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_PVC_Al_MC_Air, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                            }
                            if (cablingType == ground)
                            {
                                if (phase == "3Ф")
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_PVC_Al_MC_Ground, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor * 0.93);
                                }
                                else
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_PVC_Al_MC_Ground, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                            }
                        }
                    }

                }

                if (insulationType == XLPE)
                {
                    if (coreMaterial == cu)
                    {
                        if (oneOrMultiCore == singleCore)
                        {
                            if (cablingType == air)
                            {
                                if (phase == "3Ф")
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_XLPE_Cu_SC_Air, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                                else
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_XLPE_Cu_SC_Air, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                            }
                            if (cablingType == ground)
                            {
                                if (phase == "3Ф")
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_XLPE_Cu_SC_Ground, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                                else
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_XLPE_Cu_SC_Ground, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                            }

                        }
                        if (oneOrMultiCore == multiCore)
                        {
                            if (cablingType == air)
                            {
                                if (phase == "3Ф")
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_XLPE_Cu_MC_Air, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor * 0.93);
                                }
                                else
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_XLPE_Cu_MC_Air, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                            }
                            if (cablingType == ground)
                            {
                                if (phase == "3Ф")
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_XLPE_Cu_MC_Ground, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor * 0.93);
                                }
                                else
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_XLPE_Cu_MC_Ground, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                            }
                        }

                    }
                    if (coreMaterial == al)
                    {
                        if (oneOrMultiCore == singleCore)
                        {
                            if (cablingType == air)
                            {
                                if (phase == "3Ф")
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_XLPE_Al_SC_Air, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                                else
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_XLPE_Al_SC_Air, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                            }
                            if (cablingType == ground)
                            {
                                if (phase == "3Ф")
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_XLPE_Al_SC_Ground, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                                else
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_XLPE_Al_SC_Ground, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                            }

                        }
                        if (oneOrMultiCore == multiCore)
                        {
                            if (cablingType == air)
                            {
                                if (phase == "3Ф")
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_XLPE_Al_MC_Air, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor * 0.93);
                                }
                                else
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_XLPE_Al_MC_Air, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                            }
                            if (cablingType == ground)
                            {
                                if (phase == "3Ф")
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_XLPE_Al_MC_Ground, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor * 0.93);
                                }
                                else
                                {
                                    cabSel = GetCrossSectionWithMaxVoltageDrop(ElectricBase.gost31996_XLPE_Al_MC_Ground, cbRelease, currentA, currentB, currentC, is3ph, coreMaterial, maxCrossSection, maxVoltageDrop, length, deratingFactor);
                                }
                            }
                        }
                    }
                }
            }



            string cablesPhQuantity = cabSel[0];
            string corePhSection = cabSel[1];
            string cablesNQuantity = cabSel[2];
            string coreNSection = cabSel[3];
            string coreQuantity;

            /*

                        bool is3ph = false; 
                        if (phase == "3Ф") { is3ph = true};

                        double dropVoltage = getDropVoltage(
                            currentA,
                            currentB,
                            currentC,
                            is3ph,
                            coreMaterial,
                            cablesPhQuantity,
                            cablesNQuantity,
                            corePhSection,
                            coreNSection,
                            length
                            );




                        while (dropVoltage > maxDropVoltage)
                        {
                            //increase section

                            dropVoltage = getDropVoltage(
                            currentA,
                            currentB,
                            currentC,
                            is3ph,
                            coreMaterial,
                            cablesPhQuantity,
                            cablesNQuantity,
                            corePhSection,
                            coreNSection,
                            length
                            )
                        }



                */





            //cable description
            if (phase == "3Ф")
            {
                if (neutral == "без нулевого")
                {
                    coreQuantity = "4";
                }
                else
                {
                    coreQuantity = "5";
                }
            }
            else
            {
                if (neutral == "без нулевого")
                {
                    coreQuantity = "2";
                }
                else
                {
                    coreQuantity = "3";
                }
            }

            string cable = "";

            if (oneOrMultiCore == multiCore)
            {
                if (int.Parse(cablesPhQuantity) == 1)
                {
                    cable = coreQuantity + "x" + corePhSection;
                }
                else
                {
                    cable = cablesPhQuantity + "x(" + coreQuantity + "x" + corePhSection + ")";
                }
            }

            if (
                oneOrMultiCore == singleCore && (neutral == "равный фазному" || phase != "3Ф")
                )
            {
                if (int.Parse(cablesPhQuantity) == 1)
                {
                    cable = coreQuantity + "x(1x" + corePhSection + ")";
                }
                else
                {
                    cable = cablesPhQuantity + "x" + coreQuantity + "x(1x" + corePhSection + ")";
                }
            }


            if (oneOrMultiCore == singleCore && neutral == "половина фазного" && phase == "3Ф")
            {
                if (int.Parse(cablesPhQuantity) == 1)
                {
                    cable = "3x(1x" + corePhSection + ")+2x(1x" + coreNSection + ")";
                }

                else
                {
                    cable = cablesPhQuantity + "x" + "3x(1x" + corePhSection + ")+" + cablesNQuantity + "x2х(1х" + coreNSection + ")";
                }
            }




            List<string> returnList = new List<string>() { cable, cablesPhQuantity, cablesNQuantity, corePhSection, coreNSection, coreQuantity };
            return returnList;
        }

        private static List<string> GetCrossSection(Dictionary<string, double> tbl, double current, double maxCrossSection)
        {
            for (int i = 1; i <= 1000; i++)
            {
                string phaseSection = tbl.FirstOrDefault(x => x.Value >= current / i).Key;
                if (phaseSection != null && CTDbl(phaseSection) <= maxCrossSection)
                {
                    double crossSectionPh = CTDbl(phaseSection);

                    for (int k = 1; k <= 1000; k++)
                    {
                        string neutralSection = ElectricBase.cable_CrossSection.FirstOrDefault(x => x.Value >= crossSectionPh * i / 2 / k).Key;

                        if (neutralSection != null && CTDbl(neutralSection) <= maxCrossSection)
                        {
                            if (k * CTDbl(neutralSection) / 2 < 16)
                            {
                                neutralSection = 16.ToString("F0");
                            }

                            if (k * CTDbl(phaseSection) < 16)
                            {
                                neutralSection = phaseSection;
                            }


                            List<string> returnList = new List<string>() { i.ToString(), phaseSection, k.ToString(), neutralSection };
                            return returnList;
                        }
                    }
                }
            }
            List<string> error = new List<string>() { "Ошибка", "Ошибка", "Ошибка", "Ошибка" };
            return error;
        }

        private static List<string> GetCrossSectionWithMaxVoltageDrop(Dictionary<string, double> tbl,
                                                                        double cbRelease,
                                                                        double currentA,
                                                                        double currentB,
                                                                        double currentC,
                                                                        bool is3ph,
                                                                        string coreMaterial,
                                                                        double maxCrossSection,
                                                                        double maxVoltageDrop,
                                                                        double length,
                                                                        double deratingFactor)
        {


            double current = cbRelease / deratingFactor;


            for (int i = 1; i <= 1000; i++)
            {
                string phaseSection = tbl.FirstOrDefault(x => x.Value >= current / i).Key;
                vd:
                if (phaseSection != null && CTDbl(phaseSection) <= maxCrossSection)
                {
                    double crossSectionPh = CTDbl(phaseSection);

                    for (int k = 1; k <= 1000; k++)
                    {
                        string neutralSection = ElectricBase.cable_CrossSection.FirstOrDefault(x => x.Value >= crossSectionPh * i / 2 / k).Key;

                        if (neutralSection != null && CTDbl(neutralSection) <= maxCrossSection)
                        {
                            if (k * CTDbl(neutralSection) / 2 < 16)
                            {
                                neutralSection = 16.ToString("F0");
                            }

                            if (k * CTDbl(phaseSection) < 16)
                            {
                                neutralSection = phaseSection;
                            }
                            //Voltage drop
                            double voltageDrop = getDropVoltage(
                                                 currentA,
                                                 currentB,
                                                 currentC,
                                                 is3ph,
                                                 coreMaterial,
                                                 i.ToString(),
                                                 k.ToString(),
                                                 phaseSection,
                                                 neutralSection,
                                                 length
                                                );

                            if (voltageDrop > maxVoltageDrop)
                            {
                                string phaseSectionTemp = ElectricBase.cable_CrossSection.FirstOrDefault(x => x.Value > crossSectionPh).Key;
                                if (phaseSectionTemp == null)
                                {
                                    i = i + 1;
                                    phaseSectionTemp = tbl.FirstOrDefault(x => x.Value >= current / i).Key;
                                }
                                else
                                {

                                }
                                phaseSection = phaseSectionTemp;
                                goto vd;
                            }

                            List<string> returnList = new List<string>() { i.ToString(), phaseSection, k.ToString(), neutralSection };
                            return returnList;
                        }
                    }
                }
            }
            List<string> error = new List<string>() { "Ошибка", "Ошибка", "Ошибка", "Ошибка" };
            return error;
        }

        public static double GetBreakerNominalCurrent(double current, double k, double minCurrent)
        {
            double currentSel = current * k;

            for (int i = 0; i < ElectricBase.ciruitBreakerNominal.Count; i++)
            {
                if (ElectricBase.ciruitBreakerNominal[i] >= currentSel)
                {
                    if (ElectricBase.ciruitBreakerNominal[i] < minCurrent)
                    {
                        return minCurrent;
                    }
                    else
                    {
                        return ElectricBase.ciruitBreakerNominal[i];
                    }
                }
            }
            return 6300;
        }


        //Voltage drop calculation   
        public static double getDropVoltage(double currentA,
                                            double currentB,
                                            double currentC,
                                            bool is3ph,
                                            string coreMaterial,
                                            string phaseQuantity,
                                            string neutralQuantity,
                                            string phaseSection,
                                            string neutralSection,
                                            double length)
        {

            double rPh;
            double xPh;
            double rN;
            double xN;

            if (coreMaterial == "алюминий")
            {

                if (!ElectricBase.CableAluminiumResistanse.TryGetValue(phaseSection, out rPh))
                {
                    return -1;
                }
                if (!ElectricBase.CableAluminiumResistanse.TryGetValue(neutralSection, out rN))
                {
                    return -1;
                }

            }
            else
            {
                if (!ElectricBase.CableCopperResistanse.TryGetValue(phaseSection, out rPh))
                {
                    return -1;
                }
                if (!ElectricBase.CableCopperResistanse.TryGetValue(neutralSection, out rN))
                {
                    return -1;
                }
            }


            if (!ElectricBase.CableInductanse.TryGetValue(phaseSection, out xPh))
            {
                return -1;
            }


            if (!ElectricBase.CableInductanse.TryGetValue(neutralSection, out xN))
            {
                return -1;
            }

            double qPh = CTDbl(phaseQuantity);
            double qN = CTDbl(neutralQuantity);

            double zPh = Math.Sqrt(rPh * rPh + xPh * xPh) / qPh;
            double zN = Math.Sqrt(rN * rN + xN * xN) / qN;

            double phCurrent = 0;
            double nCurrent = 0;

            if (!is3ph)
            {
                phCurrent = currentA + currentB + currentC;
                nCurrent = phCurrent;
            }
            else
            {
                double pi = Math.PI;

                phCurrent = new List<double>() { currentA, currentB, currentC }.Max();

                double reCurrentA = currentA * Math.Cos(pi / 2);
                double iCurrentA = currentA * Math.Sin(pi / 2);

                double reCurrentB = currentB * Math.Cos(-pi / 6);
                double iCurrentB = currentB * Math.Sin(-pi / 6);

                double reCurrentC = currentC * Math.Cos(pi + pi / 6);
                double iCurrentC = currentC * Math.Sin(pi + pi / 6);

                double reCurrent = reCurrentA + reCurrentB + reCurrentC;
                double iCurrent = iCurrentA + iCurrentB + iCurrentC;

                nCurrent = Math.Sqrt(reCurrent * reCurrent + iCurrent * iCurrent);

            }


            double dropVoltage = (length / 1000) * (phCurrent * zPh + nCurrent * zN) * 100 / 220;



            return dropVoltage;

        }

        //Расчет итоговых показателей
        public static double GetInstalledPower(DataGridView dgv)
        {
            double installedPower = 0;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                installedPower = installedPower
                    + CTDbl(row.Cells[Parameters.DGV_LOADA_COL].Value.ToString())
                    + CTDbl(row.Cells[Parameters.DGV_LOADB_COL].Value.ToString())
                    + CTDbl(row.Cells[Parameters.DGV_LOADC_COL].Value.ToString());
            }
            return installedPower;
        }

        public static double GetInstalledPowerPhA(DataGridView dgv)
        {
            double installedPowerPhA = 0;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                installedPowerPhA = installedPowerPhA
                    + CTDbl(row.Cells[Parameters.DGV_LOADA_COL].Value.ToString());
            }
            return installedPowerPhA;
        }

        public static double GetInstalledPowerPhB(DataGridView dgv)
        {
            double installedPowerPhB = 0;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                installedPowerPhB = installedPowerPhB
                    + CTDbl(row.Cells[Parameters.DGV_LOADB_COL].Value.ToString());
            }
            return installedPowerPhB;
        }

        public static double GetInstalledPowerPhC(DataGridView dgv)
        {
            double installedPowerPhC = 0;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                installedPowerPhC = installedPowerPhC
                    + CTDbl(row.Cells[Parameters.DGV_LOADC_COL].Value.ToString());
            }
            return installedPowerPhC;
        }

        public static double GetRatedPower(DataGridView dgv)
        {
            double ratedPower = 0;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                ratedPower = ratedPower
                    + CTDbl(row.Cells[Parameters.DGV_KCC_COL].Value.ToString())
                    * CTDbl(row.Cells[Parameters.DGV_KCP_COL].Value.ToString())
                    * (
                      CTDbl(row.Cells[Parameters.DGV_LOADA_COL].Value.ToString())
                    + CTDbl(row.Cells[Parameters.DGV_LOADB_COL].Value.ToString())
                    + CTDbl(row.Cells[Parameters.DGV_LOADC_COL].Value.ToString())
                    );
            }
            return ratedPower;
        }


        public static double GetRatedPowerPhA(DataGridView dgv)
        {
            double ratedPowerPhA = 0;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                ratedPowerPhA = ratedPowerPhA
                    + CTDbl(row.Cells[Parameters.DGV_KCC_COL].Value.ToString())
                    * CTDbl(row.Cells[Parameters.DGV_KCP_COL].Value.ToString())
                    * CTDbl(row.Cells[Parameters.DGV_LOADA_COL].Value.ToString());
            }
            return ratedPowerPhA;
        }

        public static double GetRatedPowerPhB(DataGridView dgv)
        {
            double ratedPowerPhB = 0;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                ratedPowerPhB = ratedPowerPhB
                    + CTDbl(row.Cells[Parameters.DGV_KCC_COL].Value.ToString())
                    * CTDbl(row.Cells[Parameters.DGV_KCP_COL].Value.ToString())
                    * CTDbl(row.Cells[Parameters.DGV_LOADB_COL].Value.ToString());
            }
            return ratedPowerPhB;
        }

        public static double GetRatedPowerPhC(DataGridView dgv)
        {
            double ratedPowerPhC = 0;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                ratedPowerPhC = ratedPowerPhC
                    + CTDbl(row.Cells[Parameters.DGV_KCC_COL].Value.ToString())
                    * CTDbl(row.Cells[Parameters.DGV_KCP_COL].Value.ToString())
                    * CTDbl(row.Cells[Parameters.DGV_LOADC_COL].Value.ToString());
            }
            return ratedPowerPhC;
        }

        public static double GetFullPower(DataGridView dgv)
        {
            double fullPower = 0;
            double activePower = 0;
            double reactivePower = 0;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                activePower = activePower
                    + CTDbl(row.Cells[Parameters.DGV_KCC_COL].Value.ToString())
                    * CTDbl(row.Cells[Parameters.DGV_KCP_COL].Value.ToString())
                    * (
                      CTDbl(row.Cells[Parameters.DGV_LOADA_COL].Value.ToString())
                    + CTDbl(row.Cells[Parameters.DGV_LOADB_COL].Value.ToString())
                    + CTDbl(row.Cells[Parameters.DGV_LOADC_COL].Value.ToString())
                    );

                reactivePower = reactivePower
                    + CTDbl(row.Cells[Parameters.DGV_KCC_COL].Value.ToString())
                    * CTDbl(row.Cells[Parameters.DGV_KCP_COL].Value.ToString())
                    * Math.Tan(Math.Acos(CTDbl(row.Cells[Parameters.DGV_PF_COL].Value.ToString())))
                    * (
                      CTDbl(row.Cells[Parameters.DGV_LOADA_COL].Value.ToString())
                    + CTDbl(row.Cells[Parameters.DGV_LOADB_COL].Value.ToString())
                    + CTDbl(row.Cells[Parameters.DGV_LOADC_COL].Value.ToString())
                      );

                fullPower = Math.Sqrt(activePower * activePower + reactivePower * reactivePower);

            }
            return fullPower;
        }

        public static double GetFullPowerPhA(DataGridView dgv)
        {
            double fullPowerPhA = 0;
            double activePowerPhA = 0;
            double reactivePowerPhA = 0;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                activePowerPhA = activePowerPhA
                    + CTDbl(row.Cells[Parameters.DGV_KCC_COL].Value.ToString())
                    * CTDbl(row.Cells[Parameters.DGV_KCP_COL].Value.ToString())
                    * CTDbl(row.Cells[Parameters.DGV_LOADA_COL].Value.ToString());

                reactivePowerPhA = reactivePowerPhA
                    + CTDbl(row.Cells[Parameters.DGV_KCC_COL].Value.ToString())
                    * CTDbl(row.Cells[Parameters.DGV_KCP_COL].Value.ToString())
                    * Math.Tan(Math.Acos(CTDbl(row.Cells[Parameters.DGV_PF_COL].Value.ToString())))
                    * CTDbl(row.Cells[Parameters.DGV_LOADA_COL].Value.ToString());

                fullPowerPhA = Math.Sqrt(activePowerPhA * activePowerPhA + reactivePowerPhA * reactivePowerPhA);

            }
            return fullPowerPhA;
        }


        public static double GetFullPowerPhB(DataGridView dgv)
        {
            double fullPowerPhB = 0;
            double activePowerPhB = 0;
            double reactivePowerPhB = 0;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                activePowerPhB = activePowerPhB
                    + CTDbl(row.Cells[Parameters.DGV_KCC_COL].Value.ToString())
                    * CTDbl(row.Cells[Parameters.DGV_KCP_COL].Value.ToString())
                    * CTDbl(row.Cells[Parameters.DGV_LOADB_COL].Value.ToString());

                reactivePowerPhB = reactivePowerPhB
                    + CTDbl(row.Cells[Parameters.DGV_KCC_COL].Value.ToString())
                    * CTDbl(row.Cells[Parameters.DGV_KCP_COL].Value.ToString())
                    * Math.Tan(Math.Acos(CTDbl(row.Cells[Parameters.DGV_PF_COL].Value.ToString())))
                    * CTDbl(row.Cells[Parameters.DGV_LOADB_COL].Value.ToString());

                fullPowerPhB = Math.Sqrt(activePowerPhB * activePowerPhB + reactivePowerPhB * reactivePowerPhB);

            }
            return fullPowerPhB;
        }

        public static double GetFullPowerPhC(DataGridView dgv)
        {
            double fullPowerPhC = 0;
            double activePowerPhC = 0;
            double reactivePowerPhC = 0;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                activePowerPhC = activePowerPhC
                    + CTDbl(row.Cells[Parameters.DGV_KCC_COL].Value.ToString())
                    * CTDbl(row.Cells[Parameters.DGV_KCP_COL].Value.ToString())
                    * CTDbl(row.Cells[Parameters.DGV_LOADC_COL].Value.ToString());

                reactivePowerPhC = reactivePowerPhC
                    + CTDbl(row.Cells[Parameters.DGV_KCC_COL].Value.ToString())
                    * CTDbl(row.Cells[Parameters.DGV_KCP_COL].Value.ToString())
                    * Math.Tan(Math.Acos(CTDbl(row.Cells[Parameters.DGV_PF_COL].Value.ToString())))
                    * CTDbl(row.Cells[Parameters.DGV_LOADC_COL].Value.ToString());

                fullPowerPhC = Math.Sqrt(activePowerPhC * activePowerPhC + reactivePowerPhC * reactivePowerPhC);

            }
            return fullPowerPhC;
        }


        public static double GetRatedCurrentPhA(DataGridView dgv)
        {
            return GetFullPowerPhA(dgv) / 0.22;
        }

        public static double GetRatedCurrentPhB(DataGridView dgv)
        {
            return GetFullPowerPhB(dgv) / 0.22;
        }

        public static double GetRatedCurrentPhC(DataGridView dgv)
        {
            return GetFullPowerPhC(dgv) / 0.22;
        }

        public static double GetRatedCurrent(DataGridView dgv)
        {
            return new List<double>()
            {
                GetRatedCurrentPhA(dgv),
                GetRatedCurrentPhB(dgv),
                GetRatedCurrentPhC(dgv)
            }.Max();
        }



        public static double GetDemandFactor(DataGridView dgv)
        {
            double df;
            try
            {
                df = GetRatedPower(dgv) / GetInstalledPower(dgv);
            }
            catch
            {
                return 1;
            }

            if (df == double.NaN)
            {
                return 1;
            }
            return df;
        }

        public static double GetPowerFactor(DataGridView dgv)
        {
            double pf;

            try
            {
                pf = GetRatedPower(dgv) / GetFullPower(dgv);
            }
            catch
            {
                return 1;
            }

            if (pf == double.NaN)
            {
                return 1;
            }
            return pf;
        }
    }
}
