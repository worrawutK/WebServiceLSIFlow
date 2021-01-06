using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Summary description for ResultBase
/// </summary>
[DataContract()]
public class ResultBase
{
    public ResultBase()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    [DataMember()]
    public string Reason { get; set; }
    [DataMember()]
    public bool IsPass { get; set; }
    [DataMember()]
    public string ErrorNo { get; set; }

}