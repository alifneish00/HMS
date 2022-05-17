package com.example.anticovid;

public class User {

    private String Name;
    private String Address;
    private String Mobile;
    private String Dob;
    private String Pass;
    private Boolean isAdmin = false;

    public User (String Name, String Address, String Mobile, String Dob, String Pass, Boolean isAdmin) {
        this.Name = Name;
        this.Address = Address;
        this.Mobile = Mobile;
        this.Dob = Dob;
        this.Pass = Pass;
        this.isAdmin = isAdmin;
    }

    public  String getName(){return Name;}
    public  String getAddress(){return Address;}
    public  String getMobile(){return  Mobile;}
    public  String getDob(){return  Dob;}
    public  String getPass(){return Pass;}


}
