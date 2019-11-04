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
    private Licenser c_IsCheckLicenser = Licenser.Check;

    public Licenser IsCheckLicenser
    {
        get { return c_IsCheckLicenser; }
        private set { c_IsCheckLicenser = value; }
    }

    private string c_MachineOven = "";

    public string MachineOven
    {
        get { return c_MachineOven; }
        set { c_MachineOven = value; }
    }
    public CarrierInfo CarrierInfo { get; set; }

}