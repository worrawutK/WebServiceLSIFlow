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
    private int? c_FramePass;
    [DataMember()]
    public int? FramePass
    {
        get { return c_FramePass; }
        set { c_FramePass = value; }
    }

    private int? c_FrameFail;
    [DataMember()]
    public int? FrameFail
    {
        get { return c_FrameFail; }
        set { c_FrameFail = value; }
    }
    private int? C_PNashi;
    [DataMember()]
    public int? PNashi
    {
        get { return C_PNashi; }
        set { C_PNashi = value; }
    }
    private int? C_PNashi_Scrap;
    [DataMember()]
    public int? PNashi_Scrap
    {
        get { return C_PNashi_Scrap; }
        set { C_PNashi_Scrap = value; }
    }
    private int? c_Front_Ng;
    [DataMember()]
    public int? Front_Ng
    {
        get { return c_Front_Ng; }
        set { c_Front_Ng = value; }
    }
    private int? c_Front_Ng_Scrap;
    [DataMember()]
    public int? Front_Ng_Scrap
    {
        get { return c_Front_Ng_Scrap; }
        set { c_Front_Ng_Scrap = value; }
    }
    private int? c_MarkerNg;
    [DataMember()]
    public int? MarkerNg
    {
        get { return c_MarkerNg; }
        set { c_MarkerNg = value; }
    }
    private int? c_MarkerNg_Scrap;
    [DataMember()]
    public int? MarkerNg_Scrap
    {
        get { return c_MarkerNg_Scrap; }
        set { c_MarkerNg_Scrap = value; }
    }
    private int? c_CutFrame;
    [DataMember()]
    public int? CutFrame
    {
        get { return c_CutFrame; }
        set { c_CutFrame = value; }
    }
}