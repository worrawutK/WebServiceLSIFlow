using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Summary description for LotInformation
/// </summary>
[DataContract()]
public class LotInformation
{
    public LotInformation()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    [DataMember()]
    public LotTypeState LotType { get; set; }
    [DataMember()]
    public int LotId { get; set; }
    [DataMember()]
    public string LotNo { get; set; }
    [DataMember()]
    public string DeviceName { get; set; }
    [DataMember()]
    public string PackageName { get; set; }
    [DataMember()]
    public string JobName { get; set; }
    [DataMember()]
    public int PassQty { get; set; }
    [DataMember()]
    public int FailQty { get; set; }

    [DataContract()]
    public enum LotTypeState
    {
        [EnumMember()]
        Apcs,
        [EnumMember()]
        ApcsPro,
        [EnumMember()]
        Unknown
    }
}