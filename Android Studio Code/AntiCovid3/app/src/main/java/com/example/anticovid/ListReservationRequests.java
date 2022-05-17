package com.example.anticovid;

public class ListReservationRequests {

    private String reqId;
    private String reqStatus;
    private String reqDate;
    private String userId;
    private String hosId;
    private String resId;
    private String desc;

    public ListReservationRequests(String reqId, String reqStatus, String reqDate, String userId, String hosId, String resId, String desc){
        this.reqId = reqId;
        this.reqStatus = reqStatus;
        this.reqDate = reqDate;
        this.userId = userId;
        this.hosId = hosId;
        this.resId = resId;
        this.desc = desc;

    }

    public String getReqId(){return reqId;}
    public String getReqStatus(){return reqStatus;}
    public String getReqDate(){return reqDate;}
    public String getUserId(){return userId;}
    public String getHosId() { return hosId; }
    public String getResId() { return resId; }
    public String getDesc() { return desc; }
}
