using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Summary description for CarrierInfo
/// </summary>
[DataContract()]
public class CarrierInfo
{
    public CarrierInfo()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    [DataMember()]
    public bool IsPass { get; set; }
    [DataMember()]
    public string Reason { get; set; }
    [DataMember()]
    public string LoadCarrierNo { get; set; }
    [DataMember()]
    public Status LoadCarrier { get; set; }
    [DataMember()]
    public string TransferCarrierNo { get; set; }
    [DataMember()]
    public Status TransferCarrier { get; set; }
    [DataMember()]
    public string UnloadCarrierNo { get; set; }
    [DataMember()]
    public Status UnloadCarrier { get; set; }
    [DataMember()]
    public string RegisterCarrierNo { get; set; }
    [DataMember()]
    public Status RegisterCarrier { get; set; }
    [DataMember()]
    public Status EnabledControlCarrier { get; set; }
    [DataMember()]
    public Status InControlCarrier { get; set; }

    [DataContract()]
    public enum Status
    {
        [EnumMember()]
        No_Use,
        [EnumMember()]
        Use
    }
}