using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLD
{
    public class Parameters
    {
        //
        Guid INFO_ASSEMBLYGUID = new Guid("321044f7-b0b2-4b1c-af18-e71a19252be0");

        //Отрисовка схем
        public static double DRAW_LINEOUT_WIDTH = 25;
        public static double DRAW_MARGIN_LEFT = 20;
        public static double DRAW_MARGIN_RIGHT = 5;
        public static double DRAW_MARGIN_BOTTOM = 65;
        public static double DRAW_MARGIN_TOP = 65;

        public static double DRAW_TABLE_MARGIN_LEFT = 0;
        public static double DRAW_TABLE_MARGIN_RIGHT = 10;
        public static double DRAW_TABLE_WIDTH = 40;
        public static double DRAW_SHEET_HEIGTH = 297;
        public static double DRAW_SHEET_WIDTH = 420;

        public static string DRAW_SHEET_NAME = "Схема электрическая однолинейная";

        public static string DRAW_TITLEBLOCK_NAME = "SLD A3A";



        //ЭЛЕКТРИЧЕСКИЕ ПАРМАТЕРЫ
        public static double phaseVoltage = 220;

        // ПАРАМЕТРЫ СЕМЕЙСТВА

        public static string OLD_LABEL = "Маркировка";
        public static string OLD_INSTALLED_POWER = "Установленная мощность";
        public static string OLD_CURRENT = "Расчетный ток";
        public static string OLD_VOLTAGE = "Напряжение";
        public static string OLD_TEXT = "Наименование";
        public static string OLD_NUMBER = "Маркировка выключателя";

        //


        //СТАНДАРТ ВЫБОРА КАБЕЛЯ
        public static string STANDART_GOST50517 = "ГОСТ Р 50571";
        public static string STANDART_GOST31996 = "ГОСТ 31996";
        public static string STANDART_MANUAL = "Ручной ввод";

        //СТОЛБЦЫ В DGV
        public static int DGV_NUMBER_COL = 0;
        public static int DGV_DESCRIPTION_COL = 1;
        public static int DGV_ROOMS_COL = 2;
        public static int DGV_PHASE_COL = 3;
        public static int DGV_LOADA_COL = 4;
        public static int DGV_LOADB_COL = 5;
        public static int DGV_LOADC_COL = 6;
        public static int DGV_PF_COL = 7;
        public static int DGV_KCP_COL = 8;
        public static int DGV_KCC_COL = 9;
        public static int DGV_CURRENTA_COL = 10;
        public static int DGV_CURRENTB_COL = 11;
        public static int DGV_CURRENTC_COL = 12;
        public static int DGV_SAFEFACTOR_COL = 13;
        public static int DGV_CBNOMINAL_COL = 14;
        public static int DGV_CBRELEASE_COL = 15;
        public static int DGV_DIFF_COL = 16;
        public static int DGV_CONTACTOR_COL = 17;
        public static int DGV_COREMATERIAL_COL = 18;
        public static int DGV_INSULATIONMATERIAL_COL = 19;
        public static int DGV_STANDART_COL = 20;
        public static int DGV_CABLINGTYPE_COL = 21;
        public static int DGV_SINGLEMULTICORE_COL = 22;
        public static int DGV_N_COL = 23;
        public static int DGV_MAXCROSSSECTION_COL = 24;
        public static int DGV_CABLEDERATINGFACTOR_COL = 25;
        public static int DGV_LINESQUANTITY_COL = 26;
        public static int DGV_CABLETYPE_COL = 27;
        public static int DGV_COREQUANTITYANDSECTION_COL = 28;
        public static int DGV_RATEDLENGTH_COL = 29;
        public static int DGV_MAXLENGTH_COL = 30;
        public static int DGV_TOTALLENGT_COL = 31;
        public static int DGV_MAXVOLTAGEDROP_COL = 32;
        public static int DGV_VOLTAGEDROP_COL = 33;
        public static int DGV_UID_COL = 34;

        //ПАРАМЕТРЫ СЕМЕЙСТВА SLDLINEOUT

        public static string RVT_LINEOUT_FAMILYNAME = "SLD_SLD_Line_Out";
        public static string RVT_LINEOUT_TYPENAME_BREAKER = "SLD_АВ";
        public static string RVT_LINEOUT_TYPENAME_BREAKERCONTACTOR = "SLD_АВ+Контактор";
        public static string RVT_LINEOUT_TYPENAME_BREAKERDIFF = "SLD_АВ+Дифф.защита";
        public static string RVT_LINEOUT_TYPENAME_BREAKERDIFFCONTACTOR = "SLD_АВ+Дифф.защита+Контактор";
        public const string RVT_LINEOUT_IS3PH = "3 фазы";
        public const string RVT_LINEOUT_ISN = "N";
        public const string RVT_LINEOUT_ISDIFF = "Дифф. защита";
        public const string RVT_LINEOUT_ISCONTACTOR = "Контактор";
        public const string RVT_LINEOUT_SHOWRELEASE = "Показывать ток расцепителя";
        public const string RVT_LINEOUT_NUMBER = "Номер группы";
        public const string RVT_LINEOUT_PANELNAME = "Имя щита";
        public const string RVT_LINEOUT_BREAKERANNOTATION = "Маркировка выключателя";
        public const string RVT_LINEOUT_DESCRIPTION = "Наименование";
        public const string RVT_LINEOUT_PHASE = "Фаза";
        public const string RVT_LINEOUT_VOLTAGE = "Напряжение";
        public const string RVT_LINEOUT_INSTALLEDPOWER = "Py кВт";
        public const string RVT_LINEOUT_INSTALLEDPOWERPHASEA = "Py Фаза А кВт";
        public const string RVT_LINEOUT_INSTALLEDPOWERPHASEB = "Py Фаза В кВт";
        public const string RVT_LINEOUT_INSTALLEDPOWERPHASEC = "Py Фаза С кВт";
        public const string RVT_LINEOUT_PF = "Коэффициент мощности";
        public const string RVT_LINEOUT_KCC = "Коэффициент спроса Группа";
        public const string RVT_LINEOUT_KCP = "Коэффициент спроса Щит";
        public const string RVT_LINEOUT_RATEDPOWER = "Pр кВт";
        public const string RVT_LINEOUT_RATEDPOWER_PHASEA = "Pр Фаза А кВт ";
        public const string RVT_LINEOUT_RATEDPOWER_PHASEB = "Pр Фаза В кВт";
        public const string RVT_LINEOUT_RATEDPOWER_PHASEC = "Pр Фаза С кВт";
        public const string RVT_LINEOUT_RATEDCURRENT_PHASEA = "Iр Фаза А А";
        public const string RVT_LINEOUT_RATEDCURRENT_PHASEB = "Iр Фаза B А";
        public const string RVT_LINEOUT_RATEDCURRENT_PHASEC = "Iр Фаза C А";
        public const string RVT_LINEOUT_RATEDCURRENT = "Iр, А";
        public const string RVT_LINEOUT_BREAKERMODEL = "Модель выключателя";
        public const string RVT_LINEOUT_BREAKERNOMINALCURRRENT = "Номинальный ток выключателя А";
        public const string RVT_LINEOUT_BTREAKERRELEASECURRENT = "Ток расцепителя выключателя А";
        public const string RVT_LINEOUT_BREAKERTYPE = "Характеристика выключателя";
        public const string RVT_LINEOUT_DIFFPROTECTIONRELEASE = "Уставка дифф. защиты мА";
        public const string RVT_LINEOUT_CONTACTORANNOTATION = "Маркировка контактора";
        public const string RVT_LINEOUT_CONTACTORMODEL = "Модель контактора";
        public const string RVT_LINEOUT_CONTACTORNOMINALCURRENT = "Номинальный ток контактора А";
        public const string RVT_LINEOUT_ONPLANNUMBER = "Номер по плану";
        public const string RVT_LINEOUT_CABLETYPE = "Тип кабеля";
        public const string RVT_LINEOUT_MULTIORSINGLECORE = "Одножильный/многожильный";
        public const string RVT_LINEOUT_COREMATERIAL = "Материал жилы";
        public const string RVT_LINEOUT_INSULATIONMATERIAL = "Материал изоляции";
        public const string RVT_LINEOUT_COREQUANTITYANDCROSSSECTION = "Количество жил и сечение";
        public const string RVT_LINEOUT_RATEDLENGTH = "Расчетная длина";
        public const string RVT_LINEOUT_TOTALLENGTH = "Суммарная длина";
        public const string RVT_LINEOUT_MAXLENGTH = "Длина до самой дальной точки";
        public const string RVT_LINEOUT_UGO = "Условное обозначение";
        public const string RVT_LINEOUT_STANDART = "Норматив";
        public const string RVT_LINEOUT_CABLINGTYPE = "Способ прокладки";
        public const string RVT_LINEOUT_MAXVOLTAGEDROP = "Максимально допустимые потери %";
        public const string RVT_LINEOUT_VOLTAGEDROP = "Потери";

        //ПАРАМЕТРЫ СЕМЕЙСТВА SLDLINEIN

        public static string RVT_LINEIN_FAMILYNAME = "SLD_SLD_Line_In";
        public static string RVT_LINEIN_TYPENAME_BREAKER = "SLD_Ввод АВ";
        public static string RVT_LINEIN_TYPENAME_BREAKERCONTACTOR = "SLD_Ввод АВ+Контактор";
        public static string RVT_LINEIN_TYPENAME_BREAKERDIFF = "SLD_Ввод АВ+Дифф.защита";
        public static string RVT_LINEIN_TYPENAME_BREAKERDIFFCONTACTOR = "SLD_Ввод АВ+Дифф.защита+Контактор";

        public const string RVT_LINEIN_SOURCE = "Источник питания";
        public const string RVT_LINEIN_IS3PH = "3 фазы";
        public const string RVT_LINEIN_ISN = "N";
        public const string RVT_LINEIN_ISDIFF = "Дифф. защита";
        public const string RVT_LINEIN_ISCONTACTOR = "Контактор";
        public const string RVT_LINEIN_SHOWRELEASE = "Показывать ток расцепителя";
        public const string RVT_LINEIN_NUMBER = "Номер группы";
        public const string RVT_LINEIN_PANELNAME = "Имя щита";
        public const string RVT_LINEIN_BREAKERANNOTATION = "Маркировка выключателя";
        public const string RVT_LINEIN_DESCRIPTION = "Наименование";
        public const string RVT_LINEIN_PHASE = "Фаза";
        public const string RVT_LINEIN_VOLTAGE = "Напряжение";
        public const string RVT_LINEIN_INSTALLEDPOWER = "Py кВт";
        public const string RVT_LINEIN_INSTALLEDPOWERPHASEA = "Py Фаза А кВт";
        public const string RVT_LINEIN_INSTALLEDPOWERPHASEB = "Py Фаза В кВт";
        public const string RVT_LINEIN_INSTALLEDPOWERPHASEC = "Py Фаза С кВт";
        public const string RVT_LINEIN_PF = "Коэффициент мощности";
        public const string RVT_LINEIN_KCC = "Коэффициент спроса Группа";
        public const string RVT_LINEIN_KCP = "Коэффициент спроса Щит";
        public const string RVT_LINEIN_RATEDPOWER = "Pр кВт";
        public const string RVT_LINEIN_RATEDPOWER_PHASEA = "Pр Фаза А кВт ";
        public const string RVT_LINEIN_RATEDPOWER_PHASEB = "Pр Фаза В кВт";
        public const string RVT_LINEIN_RATEDPOWER_PHASEC = "Pр Фаза С кВт";
        public const string RVT_LINEIN_RATEDCURRENT_PHASEA = "Iр Фаза А А";
        public const string RVT_LINEIN_RATEDCURRENT_PHASEB = "Iр Фаза B А";
        public const string RVT_LINEIN_RATEDCURRENT_PHASEC = "Iр Фаза C А";
        public const string RVT_LINEIN_RATEDCURRENT = "Iр, А";
        public const string RVT_LINEIN_BREAKERMODEL = "Модель выключателя";
        public const string RVT_LINEIN_BREAKERNOMINALCURRRENT = "Номинальный ток выключателя А";
        public const string RVT_LINEIN_BTREAKERRELEASECURRENT = "Ток расцепителя выключателя А";
        public const string RVT_LINEIN_BREAKERTYPE = "Характеристика выключателя";
        public const string RVT_LINEIN_DIFFPROTECTIONRELEASE = "Уставка дифф. защиты мА";
        public const string RVT_LINEIN_CONTACTORANNOTATION = "Маркировка контактора";
        public const string RVT_LINEIN_CONTACTORMODEL = "Модель контактора";
        public const string RVT_LINEIN_CONTACTORNOMINALCURRENT = "Номинальный ток контактора А";
        public const string RVT_LINEIN_ONPLANNUMBER = "Номер по плану";
        public const string RVT_LINEIN_CABLETYPE = "Тип кабеля";
        public const string RVT_LINEIN_MULTIORSINGLECORE = "Одножильный/многожильный";
        public const string RVT_LINEIN_COREMATERIAL = "Материал жилы";
        public const string RVT_LINEIN_INSULATIONMATERIAL = "Материал изоляции";
        public const string RVT_LINEIN_COREQUANTITYANDCROSSSECTION = "Количество жил и сечение";
        public const string RVT_LINEIN_RATEDLENGTH = "Расчетная длина";
        public const string RVT_LINEIN_TOTALLENGTH = "Суммарная длина";
        public const string RVT_LINEIN_MAXLENGTH = "Длина до самой дальной точки";
        public const string RVT_LINEIN_UGO = "Условное обозначение";
        public const string RVT_LINEIN_STANDART = "Норматив";
        public const string RVT_LINEIN_CABLINGTYPE = "Способ прокладки";
        public const string RVT_LINEIN_MAXVOLTAGEDROP = "Максимально допустимые потери %";
        public const string RVT_LINEIN_VOLTAGEDROP = "Потери";

        //ПАРАМЕТРЫ СЕМЕЙСТВА SLDRESERVE
        public static string RVT_RESERVE_FAMILYNAME = "SLD_Reserve";
        public static string RVT_RESERVE_TYPENAME_BREAKER = "SLD_Резерв АВ";
        public static string RVT_RESERVE_TYPENAME_BREAKERCONTACTOR = "SLD_Резерв АВ+Контактор";
        public static string RVT_RESERVE_TYPENAME_BREAKERDIFF = "SLD_Резерв АВ+Дифф.защита";
        public static string RVT_RESERVE_TYPENAME_BREAKERDIFFCONTACTOR = "SLD_Резерв АВ+Дифф.защита+Контактор";
        public const string RVT_RESERVE_IS3PH = "3 фазы";
        public const string RVT_RESERVE_ISN = "N";
        public const string RVT_RESERVE_ISDIFF = "Дифф. защита";
        public const string RVT_RESERVE_ISCONTACTOR = "Контактор";
        public const string RVT_RESERVE_SHOWRELEASE = "Показывать ток расцепителя";
        public const string RVT_RESERVE_NUMBER = "Номер группы";
        public const string RVT_RESERVE_PANELNAME = "Имя щита";
        public const string RVT_RESERVE_BREAKERANNOTATION = "Маркировка выключателя";
        public const string RVT_RESERVE_DESCRIPTION = "Наименование";
        public const string RVT_RESERVE_PHASE = "Фаза";
        public const string RVT_RESERVE_BREAKERMODEL = "Модель выключателя";
        public const string RVT_RESERVE_BREAKERNOMINALCURRRENT = "Номинальный ток выключателя А";
        public const string RVT_RESERVE_BTREAKERRELEASECURRENT = "Ток расцепителя выключателя А";
        public const string RVT_RESERVE_BREAKERTYPE = "Характеристика выключателя";
        public const string RVT_RESERVE_DIFFPROTECTIONRELEASE = "Уставка дифф. защиты мА";
        public const string RVT_RESERVE_CONTACTORANNOTATION = "Маркировка контактора";
        public const string RVT_RESERVE_CONTACTORMODEL = "Модель контактора";
        public const string RVT_RESERVE_CONTACTORNOMINALCURRENT = "Номинальный ток контактора А";

        //ПАРАМЕТРЫ СЕМЕЙСТВА SLDBUSSTART
        public const string RVT_BUSSTART_FAMILYNAME = "SLD_Bus_Start";
        public const string RVT_BUSSTART_PHASE = "Фаза";

        //ПАРАМЕТРЫ СЕМЕЙСТВА SLDBUSFINISH
        public const string RVT_BUSSFINISH_FAMILYNAME = "SLD_Bus_Finish";

        //ПАРАМЕТРЫ СЕМЕЙСТВА SLDLOADSYMBOL
        public const string RVT_LOADSYMBOL_FAMILYNAME = "SLD_Load_Symbol";
        public const string RVT_LOADSYMBOL_TYPENAME_SOCKET = "SLD_Розетка";
        public const string RVT_LOADSYMBOL_TYPENAME_LF = "SLD_Светильник";
        public const string RVT_LOADSYMBOL_TYPENAME_OUT = "SLD_Кабельный вывод";

        //ПАРАМЕТРЫ СЕМЕЙСТВА SLDPANELINFORMATION
        public const string RVT_PANELINFORMATION_FAMILYNAME = "SLD_Panel_Information";

        public const string RVT_PANELINFORMATION_PANELNAME = "Имя щита";
        public const string RVT_PANELINFORMATION_PF = "Коэффициент мощности";
        public const string RVT_PANELINFORMATION_FULLPOWER = "Полная мощность";
        public const string RVT_PANELINFORMATION_RATEDPOWER = "Расчетная мощность";
        public const string RVT_PANELINFORMATION_RATEDCURRENT = "Расчетный ток";
        public const string RVT_PANELINFORMATION_INSTALLEDPOWER = "Установленная мощность";
        public const string RVT_PANELINFORMATION_DEMANDFACTOR = "Коэффициент спроса";



        //ПАРАМЕТРЫ СЕМЕЙСТВА SLDPANELINFORMATION
        public const string RVT_TABLE_FAMILYNAME = "SLD_Table";


        //DATA STORAGE

        //Identification
        public static Guid STRG_PANEL_GUID_2_01 = new Guid("60D07EEA-1710-43AE-AC2E-FFEA8D024807");
        public static Guid STRG_CIRCUIT_GUID_2_01 = new Guid("927D082D-6146-4DEF-BC5E-535832F17D0C");
        public static Guid STRG_GUID_2_01 = new Guid("9C006CC1-ECF1-4E57-82B0-F320DC35AEA6");

        public static string STRG_SCHEMANAME_2_01 = "STRG_SCHEMANAME_2_01";
        // Circuit parameters

        public const string STRG_PANELUID = "STRG_PANELUID";
        public const string STRG_CIRCUITUID = "STRG_CIRCUITID";
        public const string STRG_ANNOTATIONSYMBOL = "STRG_ANNOTATIONSYMBOL";

        public const string STRG_RVTWIRESIZE = "STRG_RVTWIRESIZE";
        public const string STRG_VOLTAGE = "STRG_VOLTAGE";
        public const string STRG_POLESNUMBER = "STRG_POLESNUMBER";

        public const string STRG_BREAKERNUMBER = "STRG_BREAKERNUMBER";
        public const string STRG_NUMBER = "STRG_NUMBER";
        public const string STRG_FULLNUMBER = "STRG_FULLNUMBER";

        public const string STRG_DESCRIPTION = "STRG_DESCRIPTION";
        public const string STRG_ROOMS = "STRG_ROOMS";
        public const string STRG_TYPE = "STRG_TYPE";
        public const string STRG_LOADA = "STRG_LOADA";
        public const string STRG_LOADB = "STRG_LOADB";
        public const string STRG_LOADC = "STRG_LOADC";
        public const string STRG_LOAD = "STRG_LOAD";
        public const string STRG_RATEDLOADA = "STRG_RATEDLOADA";
        public const string STRG_RATEDLOADB = "STRG_RATEDLOADB";
        public const string STRG_RATEDLOADC = "STRG_RATEDLOADC";
        public const string STRG_RATEDLOAD = "STRG_RATEDLOAD";
        public const string STRG_CURRENTA = "STRG_CURRENTA";
        public const string STRG_CURRENTB = "STRG_CURRENTB";
        public const string STRG_CURRENTC = "STRG_CURRENTC";
        public const string STRG_CURRENT = "STRG_CURRENT";
        public const string STRG_FULLPOWERA = "STRG_FULLPOWERA";
        public const string STRG_FULLPOWERB = "STRG_FULLPOWERB";
        public const string STRG_FULLPOWERC = "STRG_FULLPOWERC";
        public const string STRG_FULLPOWER = "STRG_FULLPOWER";

        public const string STRG_MAXCURRENT = "STRG_MAXCURRENT";
        /*not realized*/
        public const string STRG_CURRENTS = "STRG_CURRENTS";
        public const string STRG_DEMANDPANELFACROR = "STRG_DEMANDPANELFACROR";
        public const string STRG_DEMANDCIRCUITFACTOR = "STRG_DEMANDCIRCUITFACTOR";
        public const string STRG_POWERFACTOR = "STRG_POWERFACTOR";
        public const string STRG_PHASE = "STRG_PHASE";

        public const string STRG_BREAKERNOMINALCURRENT = "STRG_BREAKERNOMINALCURRENT";
        public const string STRG_BREAKERRELEASECURRENT = "STRG_BREAKERRELEASECURRENT";
        public const string STRG_UPBREAKERRELEASECURRENT = "STRG_UPBREAKERRELEASECURRENT";
        public const string STRG_BRAKERSAFETYFACTOR = "STRG_BRAKERSAFETYFACTOR";

        public const string STRG_WIREINSULATION = "STRG_WIREINSULATION";
        public const string STRG_INSULLATIONMATERIAL = "STRG_INSULLATIONMATERIAL";
        public const string STRG_RATEDLENGTH = "STRG_RATEDLENGT";
        public const string STRG_MAXLENGTH = "STRG_MAXLENGT";
        public const string STRG_TOTALLENGTH = "STRG_TOTALLENGTH";
        public const string STRG_COREQUANTITY = "STRG_COREQUANTITY";
        public const string STRG_CABLINGTYPE = "STRG_CABLINGTYPE";
        public const string STRG_COREQUANTITYANDCROSSSECTION = "STRG_COREQUANTITYANDCROSSSECTION";
        public const string STRG_CONTACTOR = "STRG_CONTACTOR";
        public const string STRG_DIFFRELEASECURRENT = "STRG_DIFFRELEASECURRENT";
        public const string STRG_NEUTRALWIRE = "STRG_NEUTRALWIRE";
        public const string STRG_CABLEDERATINGFACTOR = "STRG_CABLEDERATINGFACTOR";
        public const string STRG_CABLESAFEFACTOR = "STRG_CABLESAFEFACTOR";
        public const string STRG_CABLEQUANTITY = "STRG_CABLEQUANTITY";
        public const string STRG_CABLETYPE = "STRG_CABLETYPE";
        public const string STRG_COREMATERIAL = "STRG_COREMATERIAL";
        public const string STRG_WIRETYPE = "STRG_WIRETYPE";
        public const string STRG_STANDART = "STRG_STANDART";
        public const string STRG_MAXCROSSSECTION = "STRG_MAXCROSSSECTION";

        public const string STRG_VOLTAGEDROP = "STRG_VOLTAGEDROP";
        public const string STRG_MAXVOLTAGEDROP = "STRG_MAXVOLTAGEDROP";
        public const string STRG_ROOMSFROMLINK = "STRG_ROOMSFROMLINK";


        //Panel properties
        public const string STRG_RESERVEQUANTITY = "STRG_RESERVEQUANTITY";
        public const string STRG_RESERVEPHASES = "STRG_RESERVEPHASES";
        public const string STRG_CIRCUITPREFIX = "STRG_CIRCUITPREFIX";
        public const string STRG_CIRCUITSEPARATOR = "STRG_CIRCUITSEPARATOR";
        public const string STRG_CIRCUITNAMING = "STRG_CIRCUITNAMEING";
        public const string STRG_OWNERPANELUID = "STRG_OWNERPANELUID";
        public const string STRG_NONSYMMETRY = "STRG_NONSYMMETRY";
        public const string STRG_CABLERESERVE = "STRG_CABLERESERVE";
        public const string STRG_MINCIRCUITBREAKER = "STRG_MINCIRCUITBREAKER";
        public const string STRG_TITLEBLOCKNAME = "STRG_TITLEBLOCKNAME";





        //Reserve parameters
        public const string STRG_RESERVE_DOUBLE_1 = "STRG_RESERVE_DOUBLE_1";
        public const string STRG_RESERVE_DOUBLE_2 = "STRG_RESERVE_DOUBLE_2";
        public const string STRG_RESERVE_DOUBLE_3 = "STRG_RESERVE_DOUBLE_3";
        public const string STRG_RESERVE_DOUBLE_4 = "STRG_RESERVE_DOUBLE_4";
        public const string STRG_RESERVE_DOUBLE_5 = "STRG_RESERVE_DOUBLE_5";
        public const string STRG_RESERVE_DOUBLE_6 = "STRG_RESERVE_DOUBLE_6";
        public const string STRG_RESERVE_DOUBLE_7 = "STRG_RESERVE_DOUBLE_7";
        public const string STRG_RESERVE_DOUBLE_8 = "STRG_RESERVE_DOUBLE_8";
        public const string STRG_RESERVE_DOUBLE_9 = "STRG_RESERVE_DOUBLE_9";
        public const string STRG_RESERVE_DOUBLE_10 = "STRG_RESERVE_DOUBLE_10";
        public const string STRG_RESERVE_DOUBLE_11 = "STRG_RESERVE_DOUBLE_11";
        public const string STRG_RESERVE_DOUBLE_12 = "STRG_RESERVE_DOUBLE_12";
        public const string STRG_RESERVE_DOUBLE_13 = "STRG_RESERVE_DOUBLE_13";
        public const string STRG_RESERVE_DOUBLE_14 = "STRG_RESERVE_DOUBLE_14";
        public const string STRG_RESERVE_DOUBLE_15 = "STRG_RESERVE_DOUBLE_15";
        public const string STRG_RESERVE_DOUBLE_16 = "STRG_RESERVE_DOUBLE_16";
        public const string STRG_RESERVE_DOUBLE_17 = "STRG_RESERVE_DOUBLE_17";
        public const string STRG_RESERVE_DOUBLE_18 = "STRG_RESERVE_DOUBLE_18";
        public const string STRG_RESERVE_DOUBLE_19 = "STRG_RESERVE_DOUBLE_19";
        public const string STRG_RESERVE_DOUBLE_20 = "STRG_RESERVE_DOUBLE_20";

        public const string STRG_RESERVE_STRING_1 = "STRG_RESERVE_STRING_1";
        public const string STRG_RESERVE_STRING_2 = "STRG_RESERVE_STRING_2";
        public const string STRG_RESERVE_STRING_3 = "STRG_RESERVE_STRING_3";
        public const string STRG_RESERVE_STRING_4 = "STRG_RESERVE_STRING_4";
        public const string STRG_RESERVE_STRING_5 = "STRG_RESERVE_STRING_5";
        public const string STRG_RESERVE_STRING_6 = "STRG_RESERVE_STRING_6";
        public const string STRG_RESERVE_STRING_7 = "STRG_RESERVE_STRING_7";
        public const string STRG_RESERVE_STRING_8 = "STRG_RESERVE_STRING_8";
        public const string STRG_RESERVE_STRING_9 = "STRG_RESERVE_STRING_9";
        public const string STRG_RESERVE_STRING_10 = "STRG_RESERVE_STRING_10";
        public const string STRG_RESERVE_STRING_11 = "STRG_RESERVE_STRING_11";
        public const string STRG_RESERVE_STRING_12 = "STRG_RESERVE_STRING_12";
        public const string STRG_RESERVE_STRING_13 = "STRG_RESERVE_STRING_13";
        public const string STRG_RESERVE_STRING_14 = "STRG_RESERVE_STRING_14";
        public const string STRG_RESERVE_STRING_15 = "STRG_RESERVE_STRING_15";
        public const string STRG_RESERVE_STRING_16 = "STRG_RESERVE_STRING_16";
        public const string STRG_RESERVE_STRING_17 = "STRG_RESERVE_STRING_17";
        public const string STRG_RESERVE_STRING_18 = "STRG_RESERVE_STRING_18";
        public const string STRG_RESERVE_STRING_19 = "STRG_RESERVE_STRING_19";
        public const string STRG_RESERVE_STRING_20 = "STRG_RESERVE_STRING_20";

        public const string STRG_RESERVE_BOOL_1 = "STRG_RESERVE_BOOL_1";
        public const string STRG_RESERVE_BOOL_2 = "STRG_RESERVE_BOOL_2";
        public const string STRG_RESERVE_BOOL_3 = "STRG_RESERVE_BOOL_3";
        public const string STRG_RESERVE_BOOL_4 = "STRG_RESERVE_BOOL_4";
        public const string STRG_RESERVE_BOOL_5 = "STRG_RESERVE_BOOL_5";

        public static Guid BIND_GUID_2_01 = new Guid("92DFBEFE-6358-4717-B69F-5AF1EFD21EF2");

        public static string BIND_SCHEMANAME_2_01 = "BIND_SCHEMANAME_2_01";

        public const string BIND_ISVALID = "BIND_ISVALID";
        public const string BIND_LINKEDITEMUID = "BIND_LINKEDITEMUID";
        public const string BIND_THISITEMUID = "BIND_THISITEMUID";
        public const string BIND_TYPE = "BIND_TYPE";
        public const string BIND_DATETIME = "BIND_DATETIME";



        public const string BIND_RESERVE_DOUBLE_1 = "BIND_RESERVE_DOUBLE_1";
        public const string BIND_RESERVE_DOUBLE_2 = "BIND_RESERVE_DOUBLE_2";
        public const string BIND_RESERVE_DOUBLE_3 = "BIND_RESERVE_DOUBLE_3";
        public const string BIND_RESERVE_DOUBLE_4 = "BIND_RESERVE_DOUBLE_4";
        public const string BIND_RESERVE_DOUBLE_5 = "BIND_RESERVE_DOUBLE_5";

        public const string BIND_RESERVE_INT_1 = "BIND_RESERVE_INT_1";
        public const string BIND_RESERVE_INT_2 = "BIND_RESERVE_INT_2";
        public const string BIND_RESERVE_INT_3 = "BIND_RESERVE_INT_3";
        public const string BIND_RESERVE_INT_4 = "BIND_RESERVE_INT_4";
        public const string BIND_RESERVE_INT_5 = "BIND_RESERVE_INT_5";

        public const string BIND_RESERVE_STRING_1 = "BIND_RESERVE_STRING_1";
        public const string BIND_RESERVE_STRING_2 = "BIND_RESERVE_STRING_2";
        public const string BIND_RESERVE_STRING_3 = "BIND_RESERVE_STRING_3";
        public const string BIND_RESERVE_STRING_4 = "BIND_RESERVE_STRING_4";
        public const string BIND_RESERVE_STRING_5 = "BIND_RESERVE_STRING_5";

        public const string BIND_RESERVE_BOOL_1 = "BIND_RESERVE_BOOL_1";
        public const string BIND_RESERVE_BOOL_2 = "BIND_RESERVE_BOOL_2";
        public const string BIND_RESERVE_BOOL_3 = "BIND_RESERVE_BOOL_3";
        public const string BIND_RESERVE_BOOL_4 = "BIND_RESERVE_BOOL_4";
        public const string BIND_RESERVE_BOOL_5 = "BIND_RESERVE_BOOL_5";

        public const string BIND_TYPE_PANELINFORMATION = "BIND_TYPE_PANELINFORMATION";
        public const string BIND_TYPE_BUSSTART = "BIND_TYPE_BUSSTART";
        public const string BIND_TYPE_BUSFINISH = "BIND_TYPE_BUSFINISH";
        public const string BIND_TYPE_LINEIN = "BIND_TYPE_LINEIN";
        public const string BIND_TYPE_TABLE = "BIND_TYPE_TABLE";
        public const string BIND_TYPE_LINEOUT = "BIND_TYPE_LINEOUT";
        public const string BIND_TYPE_UGO = "BIND_TYPE_UGO";
        public const string BIND_TYPE_RESERVE = "BIND_TYPE_RESERVE";
        public const string BIND_TYPE_SHEET = "BIND_TYPE_SHEET";
        public const string BIND_TYPE_DRAFTVIEW = "BIND_TYPE_DRAFTVIEW";

        //КАБЕЛЬНЫЙ ЖУРНАЛ
        public const string RVT_CABLELIST_HEADER_FAMILYNAME = "";
        public const string RVT_CABLELIST_HEADER_TYPENAME = "";
        public const string RVT_CABLELIST_ROW_FAMILYNAME = "";
        public const string RVT_CABLELIST_ROW_TYPENAME = "";


        public const string RVT_CABLELIST_ROW_MARK = "Обозначение";
        public const string RVT_CABLELIST_ROW_START = "Начало";
        public const string RVT_CABLELIST_ROW_FINISH = "Конец";
        public const string RVT_CABLELIST_ROW_PROJECTSECTOR = "Участок";
        public const string RVT_CABLELIST_ROW_PROJECTCABLEMARK = "Марка (по проекту)";
        public const string RVT_CABLELIST_ROW_PROJECTSECTION = "Количество и сечение (по проекту)";
        public const string RVT_CABLELIST_ROW_PROJECTLENGTH = "Длина (по проекту)";
        public const string RVT_CABLELIST_ROW_REALCABLEMARK = "Длина по проекту (проложено)";
        public const string RVT_CABLELIST_ROW_REALECTSECTION = "Марка проложено (проложено)";
        public const string RVT_CABLELIST_ROW_REALECLENGTH = "Количество и сечение (проложено)";

        //
        //
        //
        //
        //
        //
    }
}
