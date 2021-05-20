using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EndLotEvenArgs
/// </summary>
public class EndLotEvenArgs
{
    public EndLotEvenArgs(string functionName,Licenser licenser)
    {
        //
        // TODO: Add constructor logic here
        //
        IsCheckLicenser = licenser;
        FunctionName = functionName;
        c_CarrierInfo = new CarrierInfo();
    }
    public string LotNo { get; set; }
    public string MachineNo { get; set; }
    public string OperatorNo { get; set; }
    public string Process { get; set; }
    public string LayerNo { get; set; }
    public int Good { get; set; }
    public int Ng { get; set; }
    public int Frame_Pass { get; set; }
    public int Frame_Fail { get; set; }
    public string FunctionName { get;private set; }
    private Licenser c_IsCheckLicenser;

    public Licenser IsCheckLicenser
    {
        get { return c_IsCheckLicenser; }
        private set { c_IsCheckLicenser = value; }
    }

    private string c_MachineOven;

    public string MachineOven
    {
        get { return c_MachineOven; }
        set { c_MachineOven = value; }
    }
    // public CarrierInfo CarrierInfo { get; set; }
    private CarrierInfo c_CarrierInfo;

    public CarrierInfo CarrierInfo
    {
        get { return c_CarrierInfo; }
        set { c_CarrierInfo = value; }
    }
    private int c_IsOnline;
    public int IsOnline
    {
        get { return c_IsOnline; }
        set { c_IsOnline = value; }
    }
    private int? C_PNashi;
    public int? PNashi
    {
        get { return C_PNashi; }
        set { C_PNashi = value; }
    }
    private int? C_PNashi_Scrap;
    public int? PNashi_Scrap
    {
        get { return C_PNashi_Scrap; }
        set { C_PNashi_Scrap = value; }
    }
    private int? c_Front_Ng;
    public int? Front_Ng
    {
        get { return c_Front_Ng; }
        set { c_Front_Ng = value; }
    }
    private int? c_Front_Ng_Scrap;
    public int? Front_Ng_Scrap
    {
        get { return c_Front_Ng_Scrap; }
        set { c_Front_Ng_Scrap = value; }
    }
    private int? c_MarkerNg;
    public int? MarkerNg
    {
        get { return c_MarkerNg; }
        set { c_MarkerNg = value; }
    }
    private int? c_MarkerNg_Scrap;
    public int? MarkerNg_Scrap
    {
        get { return c_MarkerNg_Scrap; }
        set { c_MarkerNg_Scrap = value; }
    }
    private int? c_CutFrame;
    public int? CutFrame
    {
        get { return c_CutFrame; }
        set { c_CutFrame = value; }
    }
}