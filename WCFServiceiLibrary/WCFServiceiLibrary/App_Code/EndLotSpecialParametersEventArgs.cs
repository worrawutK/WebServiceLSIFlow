using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Summary description for EndLotSpecialParametersEventArgs
/// </summary>
[DataContract()]
public class EndLotSpecialParametersEventArgs
{
    public EndLotSpecialParametersEventArgs()
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
    private bool c_IsOffline = true;
    [DataMember()]
    public bool IsOffline
    {
        get { return c_IsOffline; }
        set { c_IsOffline = value; }
    }
}