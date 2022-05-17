package com.example.anticovid;

public class ListHospitals {

    private String Name;
    private String Address;
    private String Phone;
    private String Lat;
    private  String Long;

    public ListHospitals(String Name, String Address, String Phone, String Lat, String Long) {
        this.Name = Name;
        this.Address = Address;
        this.Phone = Phone;
        this.Lat = Lat;
        this.Long = Long;

    }
    public String getName() {return Name; }
    public String getAddress() {return Address; }
    public String getPhone() { return Phone; }
}
