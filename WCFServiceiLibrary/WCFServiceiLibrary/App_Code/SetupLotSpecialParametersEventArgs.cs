using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Summary description for SetupLotSpecialParametersEventArgs
/// </summary>
[DataContract()]
public class SetupLotSpecialParametersEventArgs
{
    public SetupLotSpecialParametersEventArgs()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    private string c_LayerNoApcs = "";
    [DataMember()]
    public string LayerNoApcs
    {
        get { return c_LayerNoApcs; }
        set { c_LayerNoApcs = value; }
    }
    private int c_FrameIn = 0;
    [DataMember()]
    public int FrameIn
    {
        get { return c_FrameIn; }
        set { c_FrameIn = value; }
    }

    private RunMode c_RunModeApcs = RunMode.Normal;
    [DataMember()]
    public RunMode RunModeApcs
    {
        get { return c_RunModeApcs; }
        set { c_RunModeApcs = value; }
    }
    private string c_McNoOvenApcs = "";
    [DataMember()]
    public string McNoOvenApcs
    {
        get { return c_McNoOvenApcs; }
        set { c_McNoOvenApcs = value; }
    }
    private bool c_IsOffline = true;
    [DataMember()]
    public bool IsOffline
    {
        get { return c_IsOffline; }
        set { c_IsOffline = value; }
    }
}