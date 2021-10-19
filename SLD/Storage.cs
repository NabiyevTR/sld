using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SLD.Parameters;

namespace SLD
{
    public class Storage
    {
        ///errors
        double dblError = 0.0005;

        Element e { get; set; }
        string euid { get; set; }

        Schema schema;
        Entity entity;

        bool isCircuit { get; set; }


        public Storage(Element e)
        {
            this.e = e;
            this.euid = e.UniqueId;
        }

        public void Write(Circuit c)
        {
            WriteCircuit_2_01(c);
        }

        public void Write(Panel p)
        {
            WritePanel_2_01(p);
        }

        public void UpDate(Circuit c)
        {
            UpdateCircuit_2_01(c);
        }

        public void UpDate(Panel p)
        {
            UpdatePanel_2_01(p);
        }

        public Circuit ReadCircuit(Element e)
        {
            return GetCircuitlData_2_01(e);
        }

        public Panel Read()
        {
            try
            {
                Panel p = GetPanelData_2_01(e);

                List<Element> circuits = Util.getSystemsByPanel(e);
                List<Circuit> cs = new List<Circuit>();

                foreach (Element circuit in circuits)
                {
                    Circuit c = GetCircuitlData_2_01(circuit);
                    cs.Add(c);
                }

                try
                {
                    cs = cs.OrderBy(o => int.Parse(o.number)).ToList();
                }
                catch
                {

                }
                p.circuits = cs;
                return p;
            }

            catch
            {
                return null;
            }

        }

        Schema NewShema_2_01()
        {
            using (Transaction tr = new Transaction(e.Document, "Create new schema for Revit SLD"))
            {
                tr.Start();
                SchemaBuilder schemaBuilder = new SchemaBuilder(STRG_GUID_2_01);
                schemaBuilder.SetReadAccessLevel(AccessLevel.Public);

                #region create fields

                //Create fields
                FieldBuilder panelUID = schemaBuilder.AddSimpleField(STRG_PANELUID, typeof(string));
                //panelUID.SetDocumentation(STRG_PANELUID);

                FieldBuilder circuitUID = schemaBuilder.AddSimpleField(STRG_CIRCUITUID, typeof(string));
                //circuitUID.SetDocumentation(STRG_CIRCUITUID);

                FieldBuilder annotationSymbol = schemaBuilder.AddSimpleField(STRG_ANNOTATIONSYMBOL, typeof(string));
                //annotationSymbol.SetDocumentation(STRG_ANNOTATIONSYMBOL);

                FieldBuilder rvtWireSize = schemaBuilder.AddSimpleField(STRG_RVTWIRESIZE, typeof(string));
                //rvtWireSize.SetDocumentation(STRG_RVTWIRESIZE);

                FieldBuilder voltage = schemaBuilder.AddSimpleField(STRG_VOLTAGE, typeof(string));
                //voltage.SetUnitType(UnitType.UT_Electrical_Potential);
                //voltage.SetDocumentation(STRG_VOLTAGE);

                FieldBuilder polesNumber = schemaBuilder.AddSimpleField(STRG_POLESNUMBER, typeof(string));
                //polesNumber.SetDocumentation(STRG_POLESNUMBER);

                FieldBuilder breakerNumber = schemaBuilder.AddSimpleField(STRG_BREAKERNUMBER, typeof(string));
                //breakerNumber.SetDocumentation(STRG_BREAKERNUMBER);

                FieldBuilder number = schemaBuilder.AddSimpleField(STRG_NUMBER, typeof(string));
                //number.SetDocumentation(STRG_NUMBER);

                FieldBuilder fullNumber = schemaBuilder.AddSimpleField(STRG_FULLNUMBER, typeof(string));
                //fullNumber.SetDocumentation(STRG_FULLNUMBER);

                FieldBuilder description = schemaBuilder.AddSimpleField(STRG_DESCRIPTION, typeof(string));
                //description.SetDocumentation(STRG_DESCRIPTION);                

                FieldBuilder rooms = schemaBuilder.AddSimpleField(STRG_ROOMS, typeof(string));
                //rooms.SetDocumentation(STRG_ROOMS);

                FieldBuilder type = schemaBuilder.AddSimpleField(STRG_TYPE, typeof(string));
                //type.SetDocumentation(STRG_TYPE);

                FieldBuilder loadA = schemaBuilder.AddSimpleField(STRG_LOADA, typeof(double));
                loadA.SetUnitType(UnitType.UT_Custom);
                //loadA.SetUnitType(UnitType.UT_Electrical_Power);
                //loadA.SetDocumentation(STRG_LOADA);

                FieldBuilder loadB = schemaBuilder.AddSimpleField(STRG_LOADB, typeof(double));
                loadB.SetUnitType(UnitType.UT_Custom);
                //loadB.SetUnitType(UnitType.UT_Electrical_Power);
                //loadB.SetDocumentation(STRG_LOADB);

                FieldBuilder loadC = schemaBuilder.AddSimpleField(STRG_LOADC, typeof(double));
                loadC.SetUnitType(UnitType.UT_Custom);
                //loadC.SetUnitType(UnitType.UT_Electrical_Power);
                //loadC.SetDocumentation(STRG_LOADC);

                FieldBuilder load = schemaBuilder.AddSimpleField(STRG_LOAD, typeof(double));
                load.SetUnitType(UnitType.UT_Custom);
                //load.SetUnitType(UnitType.UT_Electrical_Power);
                //load.SetDocumentation(STRG_LOAD);

                FieldBuilder ratedloadA = schemaBuilder.AddSimpleField(STRG_RATEDLOADA, typeof(double));
                ratedloadA.SetUnitType(UnitType.UT_Custom);
                //ratedloadA.SetUnitType(UnitType.UT_Electrical_Power);
                //ratedloadA.SetDocumentation(STRG_RATEDLOADA);

                FieldBuilder ratedloadB = schemaBuilder.AddSimpleField(STRG_RATEDLOADB, typeof(double));
                ratedloadB.SetUnitType(UnitType.UT_Custom);
                //ratedloadB.SetDocumentation(STRG_RATEDLOADB);

                FieldBuilder ratedloadC = schemaBuilder.AddSimpleField(STRG_RATEDLOADC, typeof(double));
                ratedloadC.SetUnitType(UnitType.UT_Custom);
                //ratedloadC.SetUnitType(UnitType.UT_Electrical_Power);
                //ratedloadC.SetDocumentation(STRG_RATEDLOADC);

                FieldBuilder ratedload = schemaBuilder.AddSimpleField(STRG_RATEDLOAD, typeof(double));
                ratedload.SetUnitType(UnitType.UT_Custom);
                //ratedload.SetDocumentation(STRG_RATEDLOAD);

                FieldBuilder currentA = schemaBuilder.AddSimpleField(STRG_CURRENTA, typeof(double));
                currentA.SetUnitType(UnitType.UT_Custom);
                // currentA.SetDocumentation(STRG_CURRENTA);

                FieldBuilder currentB = schemaBuilder.AddSimpleField(STRG_CURRENTB, typeof(double));
                currentB.SetUnitType(UnitType.UT_Custom);
                //currentB.SetDocumentation(STRG_CURRENTB);

                FieldBuilder currentC = schemaBuilder.AddSimpleField(STRG_CURRENTC, typeof(double));
                currentC.SetUnitType(UnitType.UT_Custom);
                // currentC.SetDocumentation(STRG_CURRENTC);

                FieldBuilder current = schemaBuilder.AddSimpleField(STRG_CURRENT, typeof(double));
                current.SetUnitType(UnitType.UT_Custom);
                // current.SetDocumentation(STRG_CURRENT);

                FieldBuilder fullPowerA = schemaBuilder.AddSimpleField(STRG_FULLPOWERA, typeof(double));
                fullPowerA.SetUnitType(UnitType.UT_Custom);
                // fullPowerA.SetDocumentation(STRG_FULLPOWERA);

                FieldBuilder fullPowerB = schemaBuilder.AddSimpleField(STRG_FULLPOWERB, typeof(double));
                fullPowerB.SetUnitType(UnitType.UT_Custom);
                //fullPowerB.SetDocumentation(STRG_FULLPOWERB);

                FieldBuilder fullPowerC = schemaBuilder.AddSimpleField(STRG_FULLPOWERC, typeof(double));
                fullPowerC.SetUnitType(UnitType.UT_Custom);
                //fullPowerC.SetDocumentation(STRG_FULLPOWERC);

                FieldBuilder fullPower = schemaBuilder.AddSimpleField(STRG_FULLPOWER, typeof(double));
                fullPower.SetUnitType(UnitType.UT_Custom);
                //fullPower.SetDocumentation(STRG_FULLPOWER);

                FieldBuilder maxCurrent = schemaBuilder.AddSimpleField(STRG_MAXCURRENT, typeof(double));
                maxCurrent.SetUnitType(UnitType.UT_Custom);
                //maxCurrent.SetDocumentation(STRG_MAXCURRENT);

                FieldBuilder demandPanelFactor = schemaBuilder.AddSimpleField(STRG_DEMANDPANELFACROR, typeof(double));
                demandPanelFactor.SetUnitType(UnitType.UT_Custom);
                //demandPanelFactor.SetDocumentation(STRG_DEMANDPANELFACROR);

                FieldBuilder demandCircuitFactor = schemaBuilder.AddSimpleField(STRG_DEMANDCIRCUITFACTOR, typeof(double));
                demandCircuitFactor.SetUnitType(UnitType.UT_Custom);
                //demandCircuitFactor.SetDocumentation(STRG_DEMANDCIRCUITFACTOR);

                FieldBuilder powerFactor = schemaBuilder.AddSimpleField(STRG_POWERFACTOR, typeof(double));
                powerFactor.SetUnitType(UnitType.UT_Custom);
                //powerFactor.SetDocumentation(STRG_POWERFACTOR);

                FieldBuilder phase = schemaBuilder.AddSimpleField(STRG_PHASE, typeof(string));
                //phase.SetDocumentation(STRG_PHASE);

                FieldBuilder breakerNominalCurrent = schemaBuilder.AddSimpleField(STRG_BREAKERNOMINALCURRENT, typeof(string));
                //breakerNominalCurrent.SetDocumentation(STRG_BREAKERNOMINALCURRENT);

                FieldBuilder breakerReleaseCurrent = schemaBuilder.AddSimpleField(STRG_BREAKERRELEASECURRENT, typeof(string));
                //breakerReleaseCurrent.SetDocumentation(STRG_BREAKERRELEASECURRENT);

                FieldBuilder upBreakerReleaseCurrent = schemaBuilder.AddSimpleField(STRG_UPBREAKERRELEASECURRENT, typeof(string));
                //upBreakerReleaseCurrent.SetDocumentation(STRG_UPBREAKERRELEASECURRENT);

                FieldBuilder breakerSafetyFactor = schemaBuilder.AddSimpleField(STRG_BRAKERSAFETYFACTOR, typeof(double));
                breakerSafetyFactor.SetUnitType(UnitType.UT_Custom);
                //breakerSafetyFactor.SetDocumentation(STRG_BRAKERSAFETYFACTOR);

                FieldBuilder wireInsulation = schemaBuilder.AddSimpleField(STRG_WIREINSULATION, typeof(string));
                //wireInsulation.SetDocumentation(STRG_WIREINSULATION);

                FieldBuilder insulationMaterial = schemaBuilder.AddSimpleField(STRG_INSULLATIONMATERIAL, typeof(string));
                //insulationMaterial.SetDocumentation(STRG_INSULLATIONMATERIAL);

                FieldBuilder ratedLength = schemaBuilder.AddSimpleField(STRG_RATEDLENGTH, typeof(double));
                ratedLength.SetUnitType(UnitType.UT_Custom);
                //ratedLength.SetDocumentation(STRG_RATEDLENGTH);

                FieldBuilder maxLength = schemaBuilder.AddSimpleField(STRG_MAXLENGTH, typeof(double));
                maxLength.SetUnitType(UnitType.UT_Custom);
                //maxLength.SetDocumentation(STRG_MAXLENGTH);

                FieldBuilder totalLength = schemaBuilder.AddSimpleField(STRG_TOTALLENGTH, typeof(double));
                totalLength.SetUnitType(UnitType.UT_Custom);
                //totalLength.SetDocumentation(STRG_TOTALLENGTH);

                FieldBuilder coreQuantity = schemaBuilder.AddSimpleField(STRG_COREQUANTITY, typeof(string));
                //coreQuantity.SetUnitType(UnitType.UT_Custom);
                //coreQuantity.SetDocumentation(STRG_COREQUANTITY);

                FieldBuilder cablingType = schemaBuilder.AddSimpleField(STRG_CABLINGTYPE, typeof(string));
                //cablingType.SetDocumentation(STRG_CABLINGTYPE);

                FieldBuilder coresQuantityAndCrossSection = schemaBuilder.AddSimpleField(STRG_COREQUANTITYANDCROSSSECTION, typeof(string));
                //coresQuantityAndCrossSection.SetDocumentation(STRG_COREQUANTITYANDCROSSSECTION);

                FieldBuilder contactor = schemaBuilder.AddSimpleField(STRG_CONTACTOR, typeof(bool));
                //contactor.SetDocumentation(STRG_CONTACTOR);

                FieldBuilder diffreleasecurrent = schemaBuilder.AddSimpleField(STRG_DIFFRELEASECURRENT, typeof(string));
                //contactor.SetDocumentation(STRG_CONTACTOR);

                FieldBuilder neutralWire = schemaBuilder.AddSimpleField(STRG_NEUTRALWIRE, typeof(string));
                //neutralWire.SetDocumentation(STRG_NEUTRALWIRE);

                FieldBuilder cableDeratingFactor = schemaBuilder.AddSimpleField(STRG_CABLEDERATINGFACTOR, typeof(double));
                cableDeratingFactor.SetUnitType(UnitType.UT_Custom);
                //cableDeratingFactor.SetDocumentation(STRG_CABLESAFEFACTOR);

                FieldBuilder cableSafeFactor = schemaBuilder.AddSimpleField(STRG_CABLESAFEFACTOR, typeof(double));
                cableSafeFactor.SetUnitType(UnitType.UT_Custom);
                //cableSafeFactor.SetDocumentation(STRG_CABLESAFEFACTOR);

                FieldBuilder cableQuantity = schemaBuilder.AddSimpleField(STRG_CABLEQUANTITY, typeof(int));
                //cableQuantity.SetUnitType(UnitType.UT_Custom);
                //cableQuantity.SetDocumentation(STRG_CABLEQUANTITY);

                FieldBuilder cableType = schemaBuilder.AddSimpleField(STRG_CABLETYPE, typeof(string));
                //cableType.SetDocumentation(STRG_CABLETYPE);

                FieldBuilder coreMaterial = schemaBuilder.AddSimpleField(STRG_COREMATERIAL, typeof(string));
                //coreMaterial.SetDocumentation(STRG_COREMATERIAL);

                FieldBuilder wireType = schemaBuilder.AddSimpleField(STRG_WIRETYPE, typeof(string));
                //wireType.SetDocumentation(STRG_WIRETYPE);

                FieldBuilder standart = schemaBuilder.AddSimpleField(STRG_STANDART, typeof(string));
                //standart.SetDocumentation(STRG_STANDART);

                FieldBuilder cableReserve = schemaBuilder.AddSimpleField(STRG_CABLERESERVE, typeof(int));
                //cableQuantity.SetDocumentation(STRG_CABLERESERVE);

                FieldBuilder maxCrossSection = schemaBuilder.AddSimpleField(STRG_MAXCROSSSECTION, typeof(string));
                //maxCrossSection.SetDocumentation(STRG_MAXCROSSSECTION);

                FieldBuilder voltageDrop = schemaBuilder.AddSimpleField(STRG_VOLTAGEDROP, typeof(double));
                voltageDrop.SetUnitType(UnitType.UT_Custom);
                //voltageDrop.SetDocumentation(STRG_VOLTAGEDROP);

                FieldBuilder maxVoltageDrop = schemaBuilder.AddSimpleField(STRG_MAXVOLTAGEDROP, typeof(double));
                maxVoltageDrop.SetUnitType(UnitType.UT_Custom);
                //maxVoltageDrop.SetDocumentation(STRG_MAXVOLTAGEDROP);

                FieldBuilder roomsFromLink = schemaBuilder.AddSimpleField(STRG_ROOMSFROMLINK, typeof(bool));
                //roomsFromLink.SetDocumentation(STRG_ROOMSFROMLINK);

                FieldBuilder reserveQuantity = schemaBuilder.AddSimpleField(STRG_RESERVEQUANTITY, typeof(int));
                //reserveQuantity.SetUnitType(UnitType.UT_Custom);
                //reserveQuantity.SetDocumentation(STRG_RESERVEQUANTITY);

                FieldBuilder reservePhases = schemaBuilder.AddSimpleField(STRG_RESERVEPHASES, typeof(string)); //?
                                                                                                               //reservePhases.SetDocumentation(STRG_RESERVEPHASES);

                FieldBuilder circuitPrefix = schemaBuilder.AddSimpleField(STRG_CIRCUITPREFIX, typeof(string));
                //circuitPrefix.SetDocumentation(STRG_CIRCUITPREFIX);

                FieldBuilder circuitSeparator = schemaBuilder.AddSimpleField(STRG_CIRCUITSEPARATOR, typeof(string));
                //circuitSeparator.SetDocumentation(STRG_CIRCUITSEPARATOR);

                FieldBuilder circuitNaming = schemaBuilder.AddSimpleField(STRG_CIRCUITNAMING, typeof(int));
                //circuitNameing.SetDocumentation(STRG_CIRCUITNAMEING);

                FieldBuilder ownerPanelUID = schemaBuilder.AddSimpleField(STRG_OWNERPANELUID, typeof(string));
                //ownerPanelUID.SetDocumentation(STRG_OWNERPANELUID);

                FieldBuilder nonSymmetry = schemaBuilder.AddSimpleField(STRG_NONSYMMETRY, typeof(double));
                nonSymmetry.SetUnitType(UnitType.UT_Custom);
                //nonSymmetry.SetDocumentation(STRG_NONSYMMETRY);

                FieldBuilder minCircuitBreaker = schemaBuilder.AddSimpleField(STRG_MINCIRCUITBREAKER, typeof(double));
                minCircuitBreaker.SetUnitType(UnitType.UT_Custom);
                //nonSymmetry.SetDocumentation(STRG_MINCIRCUITBREAKER);

                FieldBuilder titleBlockName = schemaBuilder.AddSimpleField(STRG_TITLEBLOCKNAME, typeof(string));
                //ownerPanelUID.SetDocumentation(STRG_TITLEBLOCKNAME);

                FieldBuilder reserveDouble1 = schemaBuilder.AddSimpleField(STRG_RESERVE_DOUBLE_1, typeof(double));
                reserveDouble1.SetUnitType(UnitType.UT_Custom);
                //reserveDouble1.SetDocumentation(STRG_RESERVE_DOUBLE_1);

                FieldBuilder reserveDouble2 = schemaBuilder.AddSimpleField(STRG_RESERVE_DOUBLE_2, typeof(double));
                reserveDouble2.SetUnitType(UnitType.UT_Custom);
                //reserveDouble2.SetDocumentation(STRG_RESERVE_DOUBLE_2);

                FieldBuilder reserveDouble3 = schemaBuilder.AddSimpleField(STRG_RESERVE_DOUBLE_3, typeof(double));
                reserveDouble3.SetUnitType(UnitType.UT_Custom);
                //reserveDouble3.SetDocumentation(STRG_RESERVE_DOUBLE_3);

                FieldBuilder reserveDouble4 = schemaBuilder.AddSimpleField(STRG_RESERVE_DOUBLE_4, typeof(double));
                reserveDouble4.SetUnitType(UnitType.UT_Custom);
                //reserveDouble4.SetDocumentation(STRG_RESERVE_DOUBLE_4);

                FieldBuilder reserveDouble5 = schemaBuilder.AddSimpleField(STRG_RESERVE_DOUBLE_5, typeof(double));
                reserveDouble5.SetUnitType(UnitType.UT_Custom);
                //reserveDouble5.SetDocumentation(STRG_RESERVE_DOUBLE_5);

                FieldBuilder reserveDouble6 = schemaBuilder.AddSimpleField(STRG_RESERVE_DOUBLE_6, typeof(double));
                reserveDouble6.SetUnitType(UnitType.UT_Custom);
                //reserveDouble6.SetDocumentation(STRG_RESERVE_DOUBLE_6);

                FieldBuilder reserveDouble7 = schemaBuilder.AddSimpleField(STRG_RESERVE_DOUBLE_7, typeof(double));
                reserveDouble7.SetUnitType(UnitType.UT_Custom);
                //reserveDouble7.SetDocumentation(STRG_RESERVE_DOUBLE_7);

                FieldBuilder reserveDouble8 = schemaBuilder.AddSimpleField(STRG_RESERVE_DOUBLE_8, typeof(double));
                reserveDouble8.SetUnitType(UnitType.UT_Custom);
                //reserveDouble8.SetDocumentation(STRG_RESERVE_DOUBLE_8);

                FieldBuilder reserveDouble9 = schemaBuilder.AddSimpleField(STRG_RESERVE_DOUBLE_9, typeof(double));
                reserveDouble9.SetUnitType(UnitType.UT_Custom);
                //reserveDouble9.SetDocumentation(STRG_RESERVE_DOUBLE_9);

                FieldBuilder reserveDouble10 = schemaBuilder.AddSimpleField(STRG_RESERVE_DOUBLE_10, typeof(double));
                reserveDouble10.SetUnitType(UnitType.UT_Custom);
                //reserveDouble10.SetDocumentation(STRG_RESERVE_DOUBLE_10);

                FieldBuilder reserveDouble11 = schemaBuilder.AddSimpleField(STRG_RESERVE_DOUBLE_11, typeof(double));
                reserveDouble11.SetUnitType(UnitType.UT_Custom);
                //reserveDouble11.SetDocumentation(STRG_RESERVE_DOUBLE_11);

                FieldBuilder reserveDouble12 = schemaBuilder.AddSimpleField(STRG_RESERVE_DOUBLE_12, typeof(double));
                reserveDouble12.SetUnitType(UnitType.UT_Custom);
                //reserveDouble12.SetDocumentation(STRG_RESERVE_DOUBLE_12);

                FieldBuilder reserveDouble13 = schemaBuilder.AddSimpleField(STRG_RESERVE_DOUBLE_13, typeof(double));
                reserveDouble13.SetUnitType(UnitType.UT_Custom);
                //reserveDouble13.SetDocumentation(STRG_RESERVE_DOUBLE_13);

                FieldBuilder reserveDouble14 = schemaBuilder.AddSimpleField(STRG_RESERVE_DOUBLE_14, typeof(double));
                reserveDouble14.SetUnitType(UnitType.UT_Custom);
                //reserveDouble14.SetDocumentation(STRG_RESERVE_DOUBLE_14);

                FieldBuilder reserveDouble15 = schemaBuilder.AddSimpleField(STRG_RESERVE_DOUBLE_15, typeof(double));
                reserveDouble15.SetUnitType(UnitType.UT_Custom);
                //reserveDouble15.SetDocumentation(STRG_RESERVE_DOUBLE_15);

                FieldBuilder reserveDouble16 = schemaBuilder.AddSimpleField(STRG_RESERVE_DOUBLE_16, typeof(double));
                reserveDouble16.SetUnitType(UnitType.UT_Custom);
                //reserveDouble16.SetDocumentation(STRG_RESERVE_DOUBLE_16);

                FieldBuilder reserveDouble17 = schemaBuilder.AddSimpleField(STRG_RESERVE_DOUBLE_17, typeof(double));
                reserveDouble17.SetUnitType(UnitType.UT_Custom);
                //reserveDouble17.SetDocumentation(STRG_RESERVE_DOUBLE_17);

                FieldBuilder reserveDouble18 = schemaBuilder.AddSimpleField(STRG_RESERVE_DOUBLE_18, typeof(double));
                reserveDouble18.SetUnitType(UnitType.UT_Custom);
                //reserveDouble18.SetDocumentation(STRG_RESERVE_DOUBLE_18);

                FieldBuilder reserveDouble19 = schemaBuilder.AddSimpleField(STRG_RESERVE_DOUBLE_19, typeof(double));
                reserveDouble19.SetUnitType(UnitType.UT_Custom);
                //reserveDouble19.SetDocumentation(STRG_RESERVE_DOUBLE_19);

                FieldBuilder reserveDouble20 = schemaBuilder.AddSimpleField(STRG_RESERVE_DOUBLE_20, typeof(double));
                reserveDouble20.SetUnitType(UnitType.UT_Custom);
                //reserveDouble20.SetDocumentation(STRG_RESERVE_DOUBLE_20);

                FieldBuilder reserveString1 = schemaBuilder.AddSimpleField(STRG_RESERVE_STRING_1, typeof(string));
                //reserveString1.SetDocumentation(STRG_RESERVE_STRING_1);

                FieldBuilder reserveString2 = schemaBuilder.AddSimpleField(STRG_RESERVE_STRING_2, typeof(string));
                //reserveString2.SetDocumentation(STRG_RESERVE_STRING_2);

                FieldBuilder reserveString3 = schemaBuilder.AddSimpleField(STRG_RESERVE_STRING_3, typeof(string));
                //reserveString3.SetDocumentation(STRG_RESERVE_STRING_3);

                FieldBuilder reserveString4 = schemaBuilder.AddSimpleField(STRG_RESERVE_STRING_4, typeof(string));
                //reserveString4.SetDocumentation(STRG_RESERVE_STRING_4);

                FieldBuilder reserveString5 = schemaBuilder.AddSimpleField(STRG_RESERVE_STRING_5, typeof(string));
                //reserveString5.SetDocumentation(STRG_RESERVE_STRING_5);

                FieldBuilder reserveString6 = schemaBuilder.AddSimpleField(STRG_RESERVE_STRING_6, typeof(string));
                //reserveString6.SetDocumentation(STRG_RESERVE_STRING_6);

                FieldBuilder reserveString7 = schemaBuilder.AddSimpleField(STRG_RESERVE_STRING_7, typeof(string));
                //reserveString7.SetDocumentation(STRG_RESERVE_STRING_7);

                FieldBuilder reserveString8 = schemaBuilder.AddSimpleField(STRG_RESERVE_STRING_8, typeof(string));
                //reserveString8.SetDocumentation(STRG_RESERVE_STRING_8);

                FieldBuilder reserveString9 = schemaBuilder.AddSimpleField(STRG_RESERVE_STRING_9, typeof(string));
                //reserveString9.SetDocumentation(STRG_RESERVE_STRING_9);

                FieldBuilder reserveString10 = schemaBuilder.AddSimpleField(STRG_RESERVE_STRING_10, typeof(string));
                //reserveString10.SetDocumentation(STRG_RESERVE_STRING_10);

                FieldBuilder reserveString11 = schemaBuilder.AddSimpleField(STRG_RESERVE_STRING_11, typeof(string));
                //reserveString11.SetDocumentation(STRG_RESERVE_STRING_11);

                FieldBuilder reserveString12 = schemaBuilder.AddSimpleField(STRG_RESERVE_STRING_12, typeof(string));
                //reserveString12.SetDocumentation(STRG_RESERVE_STRING_12);

                FieldBuilder reserveString13 = schemaBuilder.AddSimpleField(STRG_RESERVE_STRING_13, typeof(string));
                //reserveString13.SetDocumentation(STRG_RESERVE_STRING_13);

                FieldBuilder reserveString14 = schemaBuilder.AddSimpleField(STRG_RESERVE_STRING_14, typeof(string));
                //reserveString14.SetDocumentation(STRG_RESERVE_STRING_14);

                FieldBuilder reserveString15 = schemaBuilder.AddSimpleField(STRG_RESERVE_STRING_15, typeof(string));
                //reserveString15.SetDocumentation(STRG_RESERVE_STRING_15);

                FieldBuilder reserveString16 = schemaBuilder.AddSimpleField(STRG_RESERVE_STRING_16, typeof(string));
                //reserveString16.SetDocumentation(STRG_RESERVE_STRING_16);

                FieldBuilder reserveString17 = schemaBuilder.AddSimpleField(STRG_RESERVE_STRING_17, typeof(string));
                //reserveString17.SetDocumentation(STRG_RESERVE_STRING_17);

                FieldBuilder reserveString18 = schemaBuilder.AddSimpleField(STRG_RESERVE_STRING_18, typeof(string));
                //reserveString18.SetDocumentation(STRG_RESERVE_STRING_18);

                FieldBuilder reserveString19 = schemaBuilder.AddSimpleField(STRG_RESERVE_STRING_19, typeof(string));
                //reserveString19.SetDocumentation(STRG_RESERVE_STRING_19);

                FieldBuilder reserveString20 = schemaBuilder.AddSimpleField(STRG_RESERVE_STRING_20, typeof(string));
                //reserveString20.SetDocumentation(STRG_RESERVE_STRING_20);

                FieldBuilder reserveBool1 = schemaBuilder.AddSimpleField(STRG_RESERVE_BOOL_1, typeof(bool));
                //reserveBool1.SetDocumentation(STRG_RESERVE_BOOL_1);

                FieldBuilder reserveBool2 = schemaBuilder.AddSimpleField(STRG_RESERVE_BOOL_2, typeof(bool));
                //reserveBool2.SetDocumentation(STRG_RESERVE_BOOL_2);

                FieldBuilder reserveBool3 = schemaBuilder.AddSimpleField(STRG_RESERVE_BOOL_3, typeof(bool));
                //reserveBool3.SetDocumentation(STRG_RESERVE_BOOL_3);

                FieldBuilder reserveBool4 = schemaBuilder.AddSimpleField(STRG_RESERVE_BOOL_4, typeof(bool));
                //reserveBool4.SetDocumentation(STRG_RESERVE_BOOL_4);

                FieldBuilder reserveBool5 = schemaBuilder.AddSimpleField(STRG_RESERVE_BOOL_5, typeof(bool));
                //reserveBool5.SetDocumentation(STRG_RESERVE_BOOL_5);


                #endregion

                schemaBuilder.SetSchemaName(STRG_SCHEMANAME_2_01);
                Schema schema = schemaBuilder.Finish();
                tr.Commit();
                return schema;
            }


        }

        void WritePanel_2_01(Panel p)
        {
            IList<Guid> guids = e.GetEntitySchemaGuids();

            schema = Schema.Lookup(STRG_GUID_2_01);

            if (schema == null)
            {
                try
                {
                    schema = NewShema_2_01();
                }
                catch
                {
                    return;
                }
            }

            using (Transaction tr = new Transaction(e.Document, "Update Panel Data for Revit SLD"))
            {
                tr.Start();

                entity = new Entity(schema);

                SetData(STRG_PANELUID, p.puid);
                SetData(STRG_CIRCUITUID, p.uid);
                SetData(STRG_RVTWIRESIZE, p.rvtWireSize);
                SetData(STRG_VOLTAGE, p.voltage);
                SetData(STRG_POLESNUMBER, p.polesNumber);
                SetData(STRG_BREAKERNUMBER, p.breakerNumber);
                SetData(STRG_NUMBER, p.number);
                SetData(STRG_FULLNUMBER, p.fullNumber);
                SetData(STRG_DESCRIPTION, p.description);
                SetData(STRG_ROOMS, p.rooms);
                SetData(STRG_TYPE, p.type);
                SetData(STRG_LOADA, p.loadA);
                SetData(STRG_LOADB, p.loadB);
                SetData(STRG_LOADC, p.loadC);
                SetData(STRG_LOAD, p.load);
                SetData(STRG_RATEDLOADA, p.ratedLoadA);
                SetData(STRG_RATEDLOADB, p.ratedLoadB);
                SetData(STRG_RATEDLOADC, p.ratedLoadC);
                SetData(STRG_RATEDLOAD, p.ratedLoad);
                SetData(STRG_CURRENTA, p.currentA);
                SetData(STRG_CURRENTB, p.currentB);
                SetData(STRG_CURRENTC, p.currentC);
                SetData(STRG_CURRENT, p.current);
                SetData(STRG_FULLPOWERA, p.fullPowerA);
                SetData(STRG_FULLPOWERB, p.fullPowerB);
                SetData(STRG_FULLPOWERC, p.fullPowerC);
                SetData(STRG_FULLPOWER, p.fullPower);
                SetData(STRG_MAXCURRENT, p.maxCurrent);
                SetData(STRG_DEMANDPANELFACROR, p.demandPanelFactor);
                SetData(STRG_DEMANDCIRCUITFACTOR, p.demandCircuitFactor);
                SetData(STRG_POWERFACTOR, p.powerFactor);
                SetData(STRG_PHASE, p.phase);
                SetData(STRG_BREAKERNOMINALCURRENT, p.breakerNominalCurrent);
                SetData(STRG_BREAKERRELEASECURRENT, p.breakerReleaseCurrent);
                SetData(STRG_BRAKERSAFETYFACTOR, p.breakerSafetyFactor);
                SetData(STRG_WIREINSULATION, p.wireInsulation);
                SetData(STRG_INSULLATIONMATERIAL, p.insulationMaterial);
                SetData(STRG_RATEDLENGTH, p.ratedLength);
                SetData(STRG_MAXLENGTH, p.maxLength);
                SetData(STRG_TOTALLENGTH, p.totalLength);
                SetData(STRG_CABLERESERVE, p.cableReserve);
                SetData(STRG_COREQUANTITY, p.coreQuantity);
                SetData(STRG_CABLINGTYPE, p.cablingType);
                SetData(STRG_COREQUANTITYANDCROSSSECTION, p.coreQuantityAndCrossSection);
                SetData(STRG_NEUTRALWIRE, p.neutralWire);
                SetData(STRG_CONTACTOR, p.contactor);
                SetData(STRG_DIFFRELEASECURRENT, p.diffReleaseCurrent);
                SetData(STRG_CABLEDERATINGFACTOR, p.cableDeratingFactor);
                SetData(STRG_CABLESAFEFACTOR, p.cableSafeFactor);
                SetData(STRG_CABLEQUANTITY, p.cableQuantity);
                SetData(STRG_CABLETYPE, p.cableType);
                SetData(STRG_COREMATERIAL, p.coreMaterial);
                SetData(STRG_WIRETYPE, p.wireType);
                SetData(STRG_STANDART, p.standart);
                SetData(STRG_MAXCROSSSECTION, p.maxCrossSection);
                SetData(STRG_VOLTAGEDROP, p.voltageDrop);
                SetData(STRG_MAXVOLTAGEDROP, p.maxVoltageDrop);
                SetData(STRG_ROOMSFROMLINK, p.roomsFromLinkedFile);
                //SetData(ref entity, STRG_UPBREAKERRELEASECURRENT, p.cuur);
                SetData(STRG_RESERVEQUANTITY, p.reserve);
                SetData(STRG_RESERVEPHASES, p.reservePhases);
                SetData(STRG_CIRCUITPREFIX, p.circuitPrefix);
                SetData(STRG_CIRCUITSEPARATOR, p.circuitSeparator);
                SetData(STRG_CIRCUITNAMING, p.circuitNaming);
                SetData(STRG_OWNERPANELUID, p.ownerPanel);
                SetData(STRG_NONSYMMETRY, p.nonSymmetry);
                SetData(STRG_MINCIRCUITBREAKER, p.minCircuitBreaker);
                SetData(STRG_TITLEBLOCKNAME, p.titleBlockName);


                e.SetEntity(entity);

                tr.Commit();
            }
        }

        void UpdatePanel_2_01(Panel p)
        {
            IList<Guid> guids = e.GetEntitySchemaGuids();

            schema = Schema.Lookup(STRG_GUID_2_01);

            if (schema == null)
            {
                try
                {
                    schema = NewShema_2_01();
                }
                catch
                {
                    return;
                }
            }

            using (Transaction tr = new Transaction(e.Document, "Update Panel Data for Revit SLD"))
            {
                tr.Start();

                entity = new Entity(schema);

                UpdateData(STRG_PANELUID, p.puid);
                UpdateData(STRG_CIRCUITUID, p.uid);
                UpdateData(STRG_RVTWIRESIZE, p.rvtWireSize);
                UpdateData(STRG_VOLTAGE, p.voltage);
                UpdateData(STRG_POLESNUMBER, p.polesNumber);
                UpdateData(STRG_BREAKERNUMBER, p.breakerNumber);
                UpdateData(STRG_NUMBER, p.number);
                UpdateData(STRG_FULLNUMBER, p.fullNumber);
                UpdateData(STRG_DESCRIPTION, p.description);
                UpdateData(STRG_ROOMS, p.rooms);
                UpdateData(STRG_TYPE, p.type);
                UpdateData(STRG_LOADA, p.loadA);
                UpdateData(STRG_LOADB, p.loadB);
                UpdateData(STRG_LOADC, p.loadC);
                UpdateData(STRG_LOAD, p.load);
                UpdateData(STRG_RATEDLOADA, p.ratedLoadA);
                UpdateData(STRG_RATEDLOADB, p.ratedLoadB);
                UpdateData(STRG_RATEDLOADC, p.ratedLoadC);
                UpdateData(STRG_RATEDLOAD, p.ratedLoad);
                UpdateData(STRG_CURRENTA, p.currentA);
                UpdateData(STRG_CURRENTB, p.currentB);
                UpdateData(STRG_CURRENTC, p.currentC);
                UpdateData(STRG_CURRENT, p.current);
                UpdateData(STRG_FULLPOWERA, p.fullPowerA);
                UpdateData(STRG_FULLPOWERB, p.fullPowerB);
                UpdateData(STRG_FULLPOWERC, p.fullPowerC);
                UpdateData(STRG_FULLPOWER, p.fullPower);
                UpdateData(STRG_MAXCURRENT, p.maxCurrent);
                UpdateData(STRG_DEMANDPANELFACROR, p.demandPanelFactor);
                UpdateData(STRG_DEMANDCIRCUITFACTOR, p.demandCircuitFactor);
                UpdateData(STRG_POWERFACTOR, p.powerFactor);
                UpdateData(STRG_PHASE, p.phase);
                UpdateData(STRG_BREAKERNOMINALCURRENT, p.breakerNominalCurrent);
                UpdateData(STRG_BREAKERRELEASECURRENT, p.breakerReleaseCurrent);
                UpdateData(STRG_BRAKERSAFETYFACTOR, p.breakerSafetyFactor);
                UpdateData(STRG_WIREINSULATION, p.wireInsulation);
                UpdateData(STRG_INSULLATIONMATERIAL, p.insulationMaterial);
                UpdateData(STRG_RATEDLENGTH, p.ratedLength);
                UpdateData(STRG_MAXLENGTH, p.maxLength);
                UpdateData(STRG_TOTALLENGTH, p.totalLength);
                UpdateData(STRG_CABLERESERVE, p.cableReserve);
                UpdateData(STRG_COREQUANTITY, p.coreQuantity);
                UpdateData(STRG_CABLINGTYPE, p.cablingType);
                UpdateData(STRG_COREQUANTITYANDCROSSSECTION, p.coreQuantityAndCrossSection);
                UpdateData(STRG_NEUTRALWIRE, p.neutralWire);
                UpdateData(STRG_CONTACTOR, p.contactor);
                UpdateData(STRG_DIFFRELEASECURRENT, p.diffReleaseCurrent);
                UpdateData(STRG_CABLEDERATINGFACTOR, p.cableDeratingFactor);
                UpdateData(STRG_CABLESAFEFACTOR, p.cableSafeFactor);
                UpdateData(STRG_CABLEQUANTITY, p.cableQuantity);
                UpdateData(STRG_CABLETYPE, p.cableType);
                UpdateData(STRG_COREMATERIAL, p.coreMaterial);
                UpdateData(STRG_WIRETYPE, p.wireType);
                UpdateData(STRG_STANDART, p.standart);
                UpdateData(STRG_MAXCROSSSECTION, p.maxCrossSection);
                UpdateData(STRG_VOLTAGEDROP, p.voltageDrop);
                UpdateData(STRG_MAXVOLTAGEDROP, p.maxVoltageDrop);
                UpdateData(STRG_ROOMSFROMLINK, p.roomsFromLinkedFile);
                //SetData(ref entity, STRG_UPBREAKERRELEASECURRENT, p.cuur);
                UpdateData(STRG_RESERVEQUANTITY, p.reserve);
                UpdateData(STRG_RESERVEPHASES, p.reservePhases);
                UpdateData(STRG_CIRCUITPREFIX, p.circuitPrefix);
                UpdateData(STRG_CIRCUITSEPARATOR, p.circuitSeparator);
                UpdateData(STRG_CIRCUITNAMING, p.circuitNaming);
                UpdateData(STRG_OWNERPANELUID, p.ownerPanel);
                UpdateData(STRG_NONSYMMETRY, p.nonSymmetry);
                UpdateData(STRG_MINCIRCUITBREAKER, p.minCircuitBreaker);
                UpdateData(STRG_TITLEBLOCKNAME, p.titleBlockName);

                e.SetEntity(entity);

                tr.Commit();
            }
        }


        void WriteCircuit_2_01(Circuit c)
        {
            IList<Guid> guids = e.GetEntitySchemaGuids();

            schema = Schema.Lookup(STRG_GUID_2_01);

            if (schema == null)
            {
                try
                {
                    schema = NewShema_2_01();
                }
                catch
                {
                    return;
                }
            }

            using (Transaction tr = new Transaction(e.Document, "Update Circuit Data for Revit SLD"))
            {
                tr.Start();

                entity = new Entity(schema);

                SetData(STRG_CIRCUITUID, c.uid);
                SetData(STRG_RVTWIRESIZE, c.rvtWireSize);
                SetData(STRG_VOLTAGE, c.voltage);
                SetData(STRG_POLESNUMBER, c.polesNumber);
                SetData(STRG_BREAKERNUMBER, c.breakerNumber);
                SetData(STRG_NUMBER, c.number);
                SetData(STRG_FULLNUMBER, c.fullNumber);
                SetData(STRG_DESCRIPTION, c.description);
                SetData(STRG_ROOMS, c.rooms);
                SetData(STRG_TYPE, c.type);
                SetData(STRG_LOADA, c.loadA);
                SetData(STRG_LOADB, c.loadB);
                SetData(STRG_LOADC, c.loadC);
                SetData(STRG_LOAD, c.load);
                SetData(STRG_RATEDLOADA, c.ratedLoadA);
                SetData(STRG_RATEDLOADB, c.ratedLoadB);
                SetData(STRG_RATEDLOADC, c.ratedLoadC);
                SetData(STRG_RATEDLOAD, c.ratedLoad);
                SetData(STRG_CURRENTA, c.currentA);
                SetData(STRG_CURRENTB, c.currentB);
                SetData(STRG_CURRENTC, c.currentC);
                SetData(STRG_CURRENT, c.current);
                SetData(STRG_FULLPOWERA, c.fullPowerA);
                SetData(STRG_FULLPOWERB, c.fullPowerB);
                SetData(STRG_FULLPOWERC, c.fullPowerC);
                SetData(STRG_FULLPOWER, c.fullPower);
                SetData(STRG_MAXCURRENT, c.maxCurrent);
                SetData(STRG_DEMANDPANELFACROR, c.demandPanelFactor);
                SetData(STRG_DEMANDCIRCUITFACTOR, c.demandCircuitFactor);
                SetData(STRG_POWERFACTOR, c.powerFactor);
                SetData(STRG_PHASE, c.phase);
                SetData(STRG_BREAKERNOMINALCURRENT, c.breakerNominalCurrent);
                SetData(STRG_BREAKERRELEASECURRENT, c.breakerReleaseCurrent);
                // SetData(ref entity, STRG_UPBREAKERRELEASECURRENT);
                SetData(STRG_BRAKERSAFETYFACTOR, c.breakerSafetyFactor);
                SetData(STRG_WIREINSULATION, c.wireInsulation);
                SetData(STRG_INSULLATIONMATERIAL, c.insulationMaterial);
                SetData(STRG_RATEDLENGTH, c.ratedLength);
                SetData(STRG_MAXLENGTH, c.maxLength);
                SetData(STRG_TOTALLENGTH, c.totalLength);
                SetData(STRG_COREQUANTITY, c.coreQuantity);
                SetData(STRG_CABLINGTYPE, c.cablingType);
                SetData(STRG_COREQUANTITYANDCROSSSECTION, c.coreQuantityAndCrossSection);
                SetData(STRG_NEUTRALWIRE, c.neutralWire);
                SetData(STRG_CONTACTOR, c.contactor);
                SetData(STRG_DIFFRELEASECURRENT, c.diffReleaseCurrent);
                SetData(STRG_CABLEDERATINGFACTOR, c.cableDeratingFactor);
                SetData(STRG_CABLESAFEFACTOR, c.cableSafeFactor);
                SetData(STRG_CABLEQUANTITY, c.cableQuantity);
                SetData(STRG_CABLETYPE, c.cableType);
                SetData(STRG_COREMATERIAL, c.coreMaterial);
                SetData(STRG_WIRETYPE, c.wireType);
                SetData(STRG_STANDART, c.standart);
                SetData(STRG_MAXCROSSSECTION, c.maxCrossSection);
                SetData(STRG_VOLTAGEDROP, c.voltageDrop);
                SetData(STRG_MAXVOLTAGEDROP, c.maxVoltageDrop);
                SetData(STRG_ROOMSFROMLINK, c.roomsFromLinkedFile);

                e.SetEntity(entity);

                tr.Commit();
            }
        }

        void UpdateCircuit_2_01(Circuit c)
        {
            IList<Guid> guids = e.GetEntitySchemaGuids();

            schema = Schema.Lookup(STRG_GUID_2_01);

            if (schema == null)
            {
                try
                {
                    schema = NewShema_2_01();
                }
                catch
                {
                    return;
                }
            }

            using (Transaction tr = new Transaction(e.Document, "Update Circuit Data for Revit SLD"))
            {
                tr.Start();

                entity = new Entity(schema);

                UpdateData(STRG_CIRCUITUID, c.uid);
                UpdateData(STRG_RVTWIRESIZE, c.rvtWireSize);
                UpdateData(STRG_VOLTAGE, c.voltage);
                UpdateData(STRG_POLESNUMBER, c.polesNumber);
                UpdateData(STRG_BREAKERNUMBER, c.breakerNumber);
                UpdateData(STRG_NUMBER, c.number);
                UpdateData(STRG_FULLNUMBER, c.fullNumber);
                UpdateData(STRG_DESCRIPTION, c.description);
                UpdateData(STRG_ROOMS, c.rooms);
                UpdateData(STRG_TYPE, c.type);
                UpdateData(STRG_LOADA, c.loadA);
                UpdateData(STRG_LOADB, c.loadB);
                UpdateData(STRG_LOADC, c.loadC);
                UpdateData(STRG_LOAD, c.load);
                UpdateData(STRG_RATEDLOADA, c.ratedLoadA);
                UpdateData(STRG_RATEDLOADB, c.ratedLoadB);
                UpdateData(STRG_RATEDLOADC, c.ratedLoadC);
                UpdateData(STRG_RATEDLOAD, c.ratedLoad);
                UpdateData(STRG_CURRENTA, c.currentA);
                UpdateData(STRG_CURRENTB, c.currentB);
                UpdateData(STRG_CURRENTC, c.currentC);
                UpdateData(STRG_CURRENT, c.current);
                UpdateData(STRG_FULLPOWERA, c.fullPowerA);
                UpdateData(STRG_FULLPOWERB, c.fullPowerB);
                UpdateData(STRG_FULLPOWERC, c.fullPowerC);
                UpdateData(STRG_FULLPOWER, c.fullPower);
                UpdateData(STRG_MAXCURRENT, c.maxCurrent);
                UpdateData(STRG_DEMANDPANELFACROR, c.demandPanelFactor);
                UpdateData(STRG_DEMANDCIRCUITFACTOR, c.demandCircuitFactor);
                UpdateData(STRG_POWERFACTOR, c.powerFactor);
                UpdateData(STRG_PHASE, c.phase);
                UpdateData(STRG_BREAKERNOMINALCURRENT, c.breakerNominalCurrent);
                UpdateData(STRG_BREAKERRELEASECURRENT, c.breakerReleaseCurrent);
                // SetData(ref entity, STRG_UPBREAKERRELEASECURRENT);
                UpdateData(STRG_BRAKERSAFETYFACTOR, c.breakerSafetyFactor);
                UpdateData(STRG_WIREINSULATION, c.wireInsulation);
                UpdateData(STRG_INSULLATIONMATERIAL, c.insulationMaterial);
                UpdateData(STRG_RATEDLENGTH, c.ratedLength);
                UpdateData(STRG_MAXLENGTH, c.maxLength);
                UpdateData(STRG_TOTALLENGTH, c.totalLength);
                UpdateData(STRG_COREQUANTITY, c.coreQuantity);
                UpdateData(STRG_CABLINGTYPE, c.cablingType);
                UpdateData(STRG_COREQUANTITYANDCROSSSECTION, c.coreQuantityAndCrossSection);
                UpdateData(STRG_NEUTRALWIRE, c.neutralWire);
                UpdateData(STRG_CONTACTOR, c.contactor);
                UpdateData(STRG_DIFFRELEASECURRENT, c.diffReleaseCurrent);
                UpdateData(STRG_CABLEDERATINGFACTOR, c.cableDeratingFactor);
                UpdateData(STRG_CABLESAFEFACTOR, c.cableSafeFactor);
                UpdateData(STRG_CABLEQUANTITY, c.cableQuantity);
                UpdateData(STRG_CABLETYPE, c.cableType);
                UpdateData(STRG_COREMATERIAL, c.coreMaterial);
                UpdateData(STRG_WIRETYPE, c.wireType);
                UpdateData(STRG_STANDART, c.standart);
                UpdateData(STRG_MAXCROSSSECTION, c.maxCrossSection);
                UpdateData(STRG_VOLTAGEDROP, c.voltageDrop);
                UpdateData(STRG_MAXVOLTAGEDROP, c.maxVoltageDrop);
                UpdateData(STRG_ROOMSFROMLINK, c.roomsFromLinkedFile);

                e.SetEntity(entity);

                tr.Commit();
            }
        }


        Panel GetPanelData_2_01(Element e)
        {
            Panel p = new Panel();

            IList<Guid> eGuids = e.GetEntitySchemaGuids();

            if (!eGuids.Contains(STRG_GUID_2_01))
            {
                return null;
            }

            Schema schema = Schema.Lookup(STRG_GUID_2_01);

            p.puid = GetStrData(e, schema, STRG_PANELUID);
            p.uid = GetStrData(e, schema, STRG_CIRCUITUID);
            p.rvtWireSize = GetStrData(e, schema, STRG_RVTWIRESIZE);
            p.voltage = GetStrData(e, schema, STRG_VOLTAGE);
            p.polesNumber = GetStrData(e, schema, STRG_POLESNUMBER);
            p.breakerNumber = GetStrData(e, schema, STRG_BREAKERNUMBER);
            p.number = GetStrData(e, schema, STRG_NUMBER);
            p.fullNumber = GetStrData(e, schema, STRG_FULLNUMBER);
            p.description = GetStrData(e, schema, STRG_DESCRIPTION);
            p.rooms = GetStrData(e, schema, STRG_ROOMS);
            p.type = GetStrData(e, schema, STRG_TYPE);
            p.loadA = GetDblData(e, schema, STRG_LOADA);
            p.loadB = GetDblData(e, schema, STRG_LOADB);
            p.loadC = GetDblData(e, schema, STRG_LOADC);
            p.load = GetDblData(e, schema, STRG_LOAD);
            p.ratedLoadA = GetDblData(e, schema, STRG_RATEDLOADA);
            p.ratedLoadB = GetDblData(e, schema, STRG_RATEDLOADB);
            p.ratedLoadC = GetDblData(e, schema, STRG_RATEDLOADC);
            p.ratedLoad = GetDblData(e, schema, STRG_RATEDLOAD);
            p.currentA = GetDblData(e, schema, STRG_CURRENTA);
            p.currentB = GetDblData(e, schema, STRG_CURRENTB);
            p.currentC = GetDblData(e, schema, STRG_CURRENTC);
            p.current = GetDblData(e, schema, STRG_CURRENT);
            p.fullPowerA = GetDblData(e, schema, STRG_FULLPOWERA);
            p.fullPowerB = GetDblData(e, schema, STRG_FULLPOWERB);
            p.fullPowerC = GetDblData(e, schema, STRG_FULLPOWERC);
            p.fullPower = GetDblData(e, schema, STRG_FULLPOWER);
            p.maxCurrent = GetDblData(e, schema, STRG_MAXCURRENT);
            p.demandPanelFactor = GetDblData(e, schema, STRG_DEMANDPANELFACROR);
            p.demandCircuitFactor = GetDblData(e, schema, STRG_DEMANDCIRCUITFACTOR);
            p.powerFactor = GetDblData(e, schema, STRG_POWERFACTOR);
            p.phase = GetStrData(e, schema, STRG_PHASE);
            p.breakerNominalCurrent = GetStrData(e, schema, STRG_BREAKERNOMINALCURRENT);
            p.breakerReleaseCurrent = GetStrData(e, schema, STRG_BREAKERRELEASECURRENT);
            p.breakerSafetyFactor = GetDblData(e, schema, STRG_BRAKERSAFETYFACTOR);
            p.wireInsulation = GetStrData(e, schema, STRG_WIREINSULATION);
            p.insulationMaterial = GetStrData(e, schema, STRG_INSULLATIONMATERIAL);
            p.ratedLength = GetDblData(e, schema, STRG_RATEDLENGTH);
            p.maxLength = GetDblData(e, schema, STRG_MAXLENGTH);
            p.totalLength = GetDblData(e, schema, STRG_TOTALLENGTH);
            p.coreQuantity = GetStrData(e, schema, STRG_COREQUANTITY);
            p.cablingType = GetStrData(e, schema, STRG_CABLINGTYPE);
            p.coreQuantityAndCrossSection = GetStrData(e, schema, STRG_COREQUANTITYANDCROSSSECTION);
            p.neutralWire = GetStrData(e, schema, STRG_NEUTRALWIRE);
            p.contactor = GetBoolData(e, schema, STRG_CONTACTOR);
            p.diffReleaseCurrent = GetStrData(e, schema, STRG_DIFFRELEASECURRENT);
            p.cableDeratingFactor = GetDblData(e, schema, STRG_CABLEDERATINGFACTOR);
            p.cableSafeFactor = GetDblData(e, schema, STRG_CABLESAFEFACTOR);
            p.cableQuantity = GetIntData(e, schema, STRG_CABLEQUANTITY);
            p.cableType = GetStrData(e, schema, STRG_CABLETYPE);
            p.coreMaterial = GetStrData(e, schema, STRG_COREMATERIAL);
            p.wireType = GetStrData(e, schema, STRG_WIRETYPE);
            p.standart = GetStrData(e, schema, STRG_STANDART);
            p.maxCrossSection = GetStrData(e, schema, STRG_MAXCROSSSECTION);
            p.voltageDrop = GetDblData(e, schema, STRG_VOLTAGEDROP);
            p.maxVoltageDrop = GetDblData(e, schema, STRG_MAXVOLTAGEDROP);
            p.roomsFromLinkedFile = GetBoolData(e, schema, STRG_ROOMSFROMLINK);
            p.reserve = GetIntData(e, schema, STRG_RESERVEQUANTITY);
            p.reservePhases = GetStrData(e, schema, STRG_RESERVEPHASES);
            p.circuitPrefix = GetStrData(e, schema, STRG_CIRCUITPREFIX);
            p.circuitSeparator = GetStrData(e, schema, STRG_CIRCUITSEPARATOR);
            p.circuitNaming = GetIntData(e, schema, STRG_CIRCUITNAMING);
            p.ownerPanel = GetStrData(e, schema, STRG_OWNERPANELUID);
            p.nonSymmetry = GetDblData(e, schema, STRG_NONSYMMETRY);
            p.cableReserve = GetIntData(e, schema, STRG_CABLERESERVE);
            p.minCircuitBreaker = GetDblData(e, schema, STRG_MINCIRCUITBREAKER);
            p.titleBlockName = GetStrData(e, schema, STRG_TITLEBLOCKNAME);

            try
            {
                Document doc = e.Document;
                p.name = doc.GetElement(p.puid).Name;
            }
            catch
            {

            }
            return p;

        }

        Circuit GetCircuitlData_2_01(Element e)
        {
            Circuit c = new Circuit();

            IList<Guid> eGuids = e.GetEntitySchemaGuids();

            if (!eGuids.Contains(STRG_GUID_2_01))
            {
                return null;
            }

            Schema schema = Schema.Lookup(STRG_GUID_2_01);

            c.uid = GetStrData(e, schema, STRG_CIRCUITUID);
            c.rvtWireSize = GetStrData(e, schema, STRG_RVTWIRESIZE);
            c.voltage = GetStrData(e, schema, STRG_VOLTAGE);
            c.polesNumber = GetStrData(e, schema, STRG_POLESNUMBER);
            c.breakerNumber = GetStrData(e, schema, STRG_BREAKERNUMBER);
            c.number = GetStrData(e, schema, STRG_NUMBER);
            c.fullNumber = GetStrData(e, schema, STRG_FULLNUMBER);
            c.description = GetStrData(e, schema, STRG_DESCRIPTION);
            c.rooms = GetStrData(e, schema, STRG_ROOMS);
            c.type = GetStrData(e, schema, STRG_TYPE);
            c.loadA = GetDblData(e, schema, STRG_LOADA);
            c.loadB = GetDblData(e, schema, STRG_LOADB);
            c.loadC = GetDblData(e, schema, STRG_LOADC);
            c.load = GetDblData(e, schema, STRG_LOAD);
            c.ratedLoadA = GetDblData(e, schema, STRG_RATEDLOADA);
            c.ratedLoadB = GetDblData(e, schema, STRG_RATEDLOADB);
            c.ratedLoadC = GetDblData(e, schema, STRG_RATEDLOADC);
            c.ratedLoad = GetDblData(e, schema, STRG_RATEDLOAD);
            c.currentA = GetDblData(e, schema, STRG_CURRENTA);
            c.currentB = GetDblData(e, schema, STRG_CURRENTB);
            c.currentC = GetDblData(e, schema, STRG_CURRENTC);
            c.current = GetDblData(e, schema, STRG_CURRENT);
            c.fullPowerA = GetDblData(e, schema, STRG_FULLPOWERA);
            c.fullPowerB = GetDblData(e, schema, STRG_FULLPOWERB);
            c.fullPowerC = GetDblData(e, schema, STRG_FULLPOWERC);
            c.fullPower = GetDblData(e, schema, STRG_FULLPOWER);
            c.maxCurrent = GetDblData(e, schema, STRG_MAXCURRENT);
            c.demandPanelFactor = GetDblData(e, schema, STRG_DEMANDPANELFACROR);
            c.demandCircuitFactor = GetDblData(e, schema, STRG_DEMANDCIRCUITFACTOR);
            c.powerFactor = GetDblData(e, schema, STRG_POWERFACTOR);
            c.phase = GetStrData(e, schema, STRG_PHASE);
            c.breakerNominalCurrent = GetStrData(e, schema, STRG_BREAKERNOMINALCURRENT);
            c.breakerReleaseCurrent = GetStrData(e, schema, STRG_BREAKERRELEASECURRENT);
            c.breakerSafetyFactor = GetDblData(e, schema, STRG_BRAKERSAFETYFACTOR);
            c.wireInsulation = GetStrData(e, schema, STRG_WIREINSULATION);
            c.insulationMaterial = GetStrData(e, schema, STRG_INSULLATIONMATERIAL);
            c.ratedLength = GetDblData(e, schema, STRG_RATEDLENGTH);
            c.maxLength = GetDblData(e, schema, STRG_MAXLENGTH);
            c.totalLength = GetDblData(e, schema, STRG_TOTALLENGTH);
            c.coreQuantity = GetStrData(e, schema, STRG_COREQUANTITY);
            c.cableType = GetStrData(e, schema, STRG_CABLETYPE);
            c.cablingType = GetStrData(e, schema, STRG_CABLINGTYPE);
            c.coreQuantityAndCrossSection = GetStrData(e, schema, STRG_COREQUANTITYANDCROSSSECTION);
            c.neutralWire = GetStrData(e, schema, STRG_NEUTRALWIRE);
            c.contactor = GetBoolData(e, schema, STRG_CONTACTOR);
            c.diffReleaseCurrent = GetStrData(e, schema, STRG_DIFFRELEASECURRENT);
            c.cableDeratingFactor = GetDblData(e, schema, STRG_CABLEDERATINGFACTOR);
            c.cableSafeFactor = GetDblData(e, schema, STRG_CABLESAFEFACTOR);
            c.cableQuantity = GetIntData(e, schema, STRG_CABLEQUANTITY);
            c.coreMaterial = GetStrData(e, schema, STRG_COREMATERIAL);
            c.wireType = GetStrData(e, schema, STRG_WIRETYPE);
            c.standart = GetStrData(e, schema, STRG_STANDART);
            c.maxCrossSection = GetStrData(e, schema, STRG_MAXCROSSSECTION);
            c.voltageDrop = GetDblData(e, schema, STRG_VOLTAGEDROP);
            c.maxVoltageDrop = GetDblData(e, schema, STRG_MAXVOLTAGEDROP);
            c.roomsFromLinkedFile = GetBoolData(e, schema, STRG_ROOMSFROMLINK);


            return c;
        }


        #region SetFields

        void SetData(string fieldName, double value)
        {
            try
            {
                Field fieldSpliceLocation = schema.GetField(fieldName);
                entity.Set<double>(fieldSpliceLocation, value, DisplayUnitType.DUT_CUSTOM);
            }
            catch
            {

            }
        }

        void SetData(string fieldName, string value)
        {
            try
            {
                if (value != null)
                {

                    Field fieldSpliceLocation = schema.GetField(fieldName);
                    entity.Set<string>(fieldSpliceLocation, value);
                }
            }
            catch
            {

            }
        }

        void SetData(string fieldName, int value)
        {
            try
            {
                Field fieldSpliceLocation = schema.GetField(fieldName);
                entity.Set<int>(fieldSpliceLocation, value);
            }
            catch
            {
            }
        }

        void SetData(string fieldName, bool value)
        {
            try
            {
                Field fieldSpliceLocation = schema.GetField(fieldName);
                entity.Set<bool>(fieldSpliceLocation, value);
            }
            catch
            {

            }
        }

        #endregion

        void UpdateData(string fieldName, double value)
        {
            try
            {
                Field fieldSpliceLocation = schema.GetField(fieldName);
                double storageValue = entity.Get<double>(schema.GetField(fieldName), DisplayUnitType.DUT_CUSTOM);
                if (Math.Abs(storageValue - value) > dblError)
                {
                    entity.Set<double>(fieldSpliceLocation, value, DisplayUnitType.DUT_CUSTOM);
                }
            }
            catch
            {

            }
        }

        void UpdateData(string fieldName, string value)
        {
            try
            {
                if (value != null)
                {
                    Field fieldSpliceLocation = schema.GetField(fieldName);
                    string storageValue = entity.Get<string>(schema.GetField(fieldName));
                    if (value != storageValue)
                    {
                        entity.Set<string>(fieldSpliceLocation, value);
                    }
                }
            }
            catch
            {

            }
        }

        void UpdateData(string fieldName, int value)
        {
            try
            {
                Field fieldSpliceLocation = schema.GetField(fieldName);
                int storageValue = entity.Get<int>(schema.GetField(fieldName));
                if (value != storageValue)
                {
                    entity.Set<int>(fieldSpliceLocation, value);
                }
            }
            catch
            {
            }
        }

        void UpdateData(string fieldName, bool value)
        {
            try
            {
                Field fieldSpliceLocation = schema.GetField(fieldName);
                bool storageValue = entity.Get<bool>(schema.GetField(fieldName));
                if (value != storageValue)
                {
                    entity.Set<bool>(fieldSpliceLocation, value);
                }
            }
            catch
            {

            }
        }



        #region GetFields
        double GetDblData(Element e, Schema schema, string fieldName)
        {

            if (schema == null) { return -1; }

            try
            {
                Entity retrivedEntity = e.GetEntity(schema);

                double retrivedData = retrivedEntity.Get<double>(
                    schema.GetField(fieldName), DisplayUnitType.DUT_CUSTOM);

                return retrivedData;
            }
            catch
            {
                return -1;
            }

        }

        string GetStrData(Element e, Schema schema, string fieldName)
        {

            if (schema == null) { return "-1"; }

            try
            {
                Entity retrivedEntity = e.GetEntity(schema);

                string retrivedData = retrivedEntity.Get<string>(
                    schema.GetField(fieldName));

                return retrivedData;
            }
            catch
            {
                return "-1";
            }

        }

        int GetIntData(Element e, Schema schema, string fieldName)
        {

            if (schema == null) { return -1; }

            try
            {
                Entity retrivedEntity = e.GetEntity(schema);

                int retrivedData = retrivedEntity.Get<int>(
                    schema.GetField(fieldName));

                return retrivedData;
            }
            catch
            {
                return -1;
            }

        }

        bool GetBoolData(Element e, Schema schema, string fieldName)
        {

            if (schema == null) { return false; }

            try
            {
                Entity retrivedEntity = e.GetEntity(schema);

                bool retrivedData = retrivedEntity.Get<bool>(
                    schema.GetField(fieldName));

                return retrivedData;
            }
            catch
            {
                return false;
            }

        }

        #endregion

        Schema GetShemaByGUiD_2_01()
        {
            return Schema.Lookup(STRG_GUID_2_01);
        }


        public static bool IsValid(Element e)
        {
            IList<Guid> eGuids = e.GetEntitySchemaGuids();

            if (eGuids.Contains(STRG_GUID_2_01))
            {
                try
                {
                    Schema schema = Schema.Lookup(STRG_GUID_2_01);
                    Entity entity = e.GetEntity(schema);
                    string panelUId = entity.Get<string>(schema.GetField(STRG_PANELUID));
                    if (e.UniqueId == panelUId) { return true; }
                }
                catch { return false; }                
            }
            return false;
        }
    }
}
