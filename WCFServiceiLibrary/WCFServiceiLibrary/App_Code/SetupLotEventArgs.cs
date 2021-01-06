using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SetupEventArgs
/// </summary>
public class SetupLotEventArgs
{
    public SetupLotEventArgs()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string LotNo { get; set; }
    public string MachineNo { get; set; }
    public string OperatorNo { get; set; }
    public string Process { get; set; }
    public string LayerNo { get; set; }

    private RunMode c_RunMode;

    public RunMode RunMode
    {
        get { return c_RunMode; }
        set { c_RunMode = value; }
    }

    public string FunctionName { get; set; }

    private Licenser c_IsCheckLicenser;

    public Licenser IsCheckLicenser
    {
        get { return c_IsCheckLicenser; }
        set { c_IsCheckLicenser = value; }
    }

    private string c_MachineOven = "";

    public string MachineOven
    {
        get { return c_MachineOven; }
        set { c_MachineOven = value; }
    }

    public int FrameIn { get; set; }

    public CarrierInfo CarrierInfo { get; set; }

    private int c_IsOnline;
    public int IsOnline
    {
        get { return c_IsOnline; }
        set { c_IsOnline = value; }
    }
}