using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Summary description for StartLotSpecialParametersEventArgs
/// </summary>
[DataContract()]
public class StartLotSpecialParametersEventArgs
{
    public StartLotSpecialParametersEventArgs()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    private string c_McNoOvenApcs = "";
    [DataMember()]
    public string McNoOvenApcs
    {
        get { return c_McNoOvenApcs; }
        set { c_McNoOvenApcs = value; }
    }
    private RunMode c_RunModeApcs = RunMode.Normal;
    [DataMember()]
    public RunMode RunModeApcs
    {
        get { return c_RunModeApcs; }
        set { c_RunModeApcs = value; }
    }
    //Default value = false (WCF)
    private bool c_IsOffline;
    [DataMember()]
    public bool IsOffline
    {
        get { return c_IsOffline; }
        set { c_IsOffline = value; }
    }
}